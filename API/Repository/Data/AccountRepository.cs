using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace API.Repository
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;
        public IConfiguration configuration;
        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this.myContext = myContext;
            this.configuration = configuration;
        }
        public int Register(RegisterVM registerVM)
        {
            var emp = new Employee
            {
                NIK = GenerateNIK(),
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Gender)registerVM.Gender

            };
            myContext.Employees.Add(emp);
            myContext.SaveChanges();
            var acc = new Account
            {
                NIK = emp.NIK,
                Password = HashPassword(registerVM.Password)
            };
            myContext.Accounts.Add(acc);
            myContext.SaveChanges();

            var accrol = new AccountRole
            {
                NIK = emp.NIK,
                RoleID = 3
            };
            myContext.AccountRoles.Add(accrol);
            myContext.SaveChanges();

            var edu = new Education
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversityId = registerVM.UniversityId
            };
            myContext.Educations.Add(edu);
            myContext.SaveChanges();
            var pro = new Profiling
            {
                NIK = emp.NIK,
                EducationId = edu.Id
            };
            myContext.Profilings.Add(pro);
            myContext.SaveChanges();
            return 1;
        }
        public int Edit(UpdateMasterVM updateMasterVM)
        {
            var emp = new Employee
            {
                NIK = updateMasterVM.NIK,
                FirstName = updateMasterVM.FirstName,
                LastName = updateMasterVM.LastName,
                Phone = updateMasterVM.Phone,
                BirthDate = updateMasterVM.BirthDate,
                Salary = updateMasterVM.Salary,
                Email = updateMasterVM.Email,
                Gender = (Gender)updateMasterVM.Gender

            };
            myContext.Entry(emp).State = EntityState.Modified;
            myContext.SaveChanges();
            var edu = new Education
            {
                Id = updateMasterVM.EducationId,
                Degree = updateMasterVM.Degree,
                GPA = updateMasterVM.GPA,
                UniversityId = updateMasterVM.UniversityId
            };
            myContext.Entry(edu).State = EntityState.Modified;
            myContext.SaveChanges();
            return 1;
        }

        public int Login(LoginVM loginVM)
        {
            var emp = myContext.Employees.FirstOrDefault(x => x.Email == loginVM.Email);
            var acc = myContext.Accounts.Find(emp.NIK);
            if (ValidatePassword(loginVM.Password, acc.Password))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public int ChangePassword(ChangePassVM changePassVM)
        {
            var emp = myContext.Employees.FirstOrDefault(x => x.Email == changePassVM.Email);
            var acc = myContext.Accounts.AsNoTracking().FirstOrDefault(x => x.NIK == emp.NIK);

            if (changePassVM.OTP != acc.OTP)
            {
                return 0;
            }
            if (acc.IsUsed)
            {
                return -1;
            }
            if (DateTime.Now > acc.ExpiredToken)
            {
                return -2;
            }
            Account account = new Account
            {
                NIK = emp.NIK,
                Password = HashPassword(changePassVM.NewPass),
                IsUsed = true
            };
            myContext.Entry(account).State = EntityState.Modified;
            myContext.Entry(account).Property(x => x.OTP).IsModified = false;
            myContext.Entry(account).Property(x => x.ExpiredToken).IsModified = false;
            var result = myContext.SaveChanges();
            return result;
        }
        public string GenerateNIK()
        {
            string year = DateTime.Now.ToString("yyyy");
            var cari = myContext.Employees.OrderByDescending(x => x.NIK).FirstOrDefault(x => x.NIK.StartsWith(year));
            if (cari == null)
            {
                return year + "001";
            }
            else
            {
                int nik = Convert.ToInt32(cari.NIK);
                nik++;
                return Convert.ToString(nik);
            }
        }
        public Employee GetEmail(string email)
        {
            return myContext.Employees.FirstOrDefault(x => x.Email == email);
        }
        public Employee GetEmail(string email, string nik)
        {
            string oldEmail = myContext.Employees.AsNoTracking().FirstOrDefault(x => x.NIK == nik).Email;
            return myContext.Employees.FirstOrDefault(x => x.Email != oldEmail && x.Email == email);
        }
        public Employee GetPhone(string phone)
        {
            return myContext.Employees.FirstOrDefault(x => x.Phone == phone);
        }
        public Employee GetPhone(string phone, string nik)
        {
            string oldPhone = myContext.Employees.AsNoTracking().FirstOrDefault(x => x.NIK == nik).Phone;
            return myContext.Employees.FirstOrDefault(x => x.Phone != oldPhone && x.Phone == phone);
        }
        public int GetOTP(string email)
        {
            var emp = myContext.Employees.FirstOrDefault(x => x.Email == email);
            Random rand = new Random();
            Account account = new Account
            {
                NIK = emp.NIK,
                OTP = rand.Next(100000, 999999),
                ExpiredToken = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            myContext.Entry(account).State = EntityState.Modified;
            myContext.Entry(account).Property(x => x.Password).IsModified = false;
            myContext.SaveChanges();
            string to = email;
            string from = "test.msuryanto@gmail.com";
            MailMessage message = new MailMessage(from, to);

            string mailbody = $"Kode OTP untuk ganti password {account.OTP}, berlaku sampai {account.ExpiredToken}";
            message.Subject = "Lupa Password";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential basicCredential1 = new
            NetworkCredential("test.msuryanto@gmail.com", "testaja321");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.Send(message);
            return 1;
        }
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }
        private static bool ValidatePassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        public string GetToken(string email)
        {
            var role = GetRole(email);
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", email));
            foreach (var item in role)
            {
                claims.Add(new Claim("roles", item));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signIn
                );
            var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
            claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
            return idtoken;
        }
        public List<string> GetRole(string email)
        {
            var emp = myContext.Employees.FirstOrDefault(x => x.Email == email);
            var result = (
               from accrol in myContext.AccountRoles
               join rol in myContext.Roles on accrol.RoleID equals rol.Id 
               where accrol.NIK == emp.NIK
               select rol.Name
               ).ToList();
            return result;
        }

    }
 }