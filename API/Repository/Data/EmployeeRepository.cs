using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }
        public Employee GetEmail(string email)
        {
            return myContext.Employees.FirstOrDefault(x => x.Email == email);
        }
        public Employee GetPhone(string phone)
        {
            return myContext.Employees.FirstOrDefault(x => x.Phone == phone);
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
        public IEnumerable GetMaster()
        {
            var result = (
                from emp in myContext.Employees
                join pro in myContext.Profilings on emp.NIK equals pro.NIK into temp1
                from tmp1 in temp1.DefaultIfEmpty()
                join edu in myContext.Educations on tmp1.EducationId equals edu.Id into temp2
                from tmp2 in temp2.DefaultIfEmpty()
                join unv in myContext.Universities on tmp2.UniversityId equals unv.Id into temp3
                from tmp3 in temp3.DefaultIfEmpty()
                select new
                {
                    emp.NIK,
                    FullName = emp.FirstName + " " + emp.LastName,
                    Gender = emp.Gender.ToString(),
                    emp.BirthDate,
                    emp.Phone,
                    emp.Email,
                    emp.Salary,
                    tmp1.EducationId,
                    tmp2.Degree,
                    tmp2.GPA,
                    UniversityName = tmp3.Name
                }).ToList();
            return result;
        }
        public IEnumerable GetMaster(string nik)
        {
            var result = (
                from emp in myContext.Employees
                join pro in myContext.Profilings on emp.NIK equals pro.NIK into temp1
                from tmp1 in temp1.DefaultIfEmpty()
                join edu in myContext.Educations on tmp1.EducationId equals edu.Id into temp2
                from tmp2 in temp2.DefaultIfEmpty()
                join unv in myContext.Universities on tmp2.UniversityId equals unv.Id into temp3
                from tmp3 in temp3.DefaultIfEmpty()
                where emp.NIK == nik
                select new
                {
                    emp.NIK,
                    emp.FirstName, 
                    emp.LastName,
                    emp.Gender,
                    emp.BirthDate,
                    emp.Phone,
                    emp.Email,
                    emp.Salary,
                    tmp1.EducationId,
                    tmp2.Degree,
                    tmp2.GPA,
                    tmp2.UniversityId
                }).ToList();
            return result;
        }
        /*public IEnumerable GetMaster(string nik)
        {
            var result = (
                from emp in myContext.Employees
                join pro in myContext.Profilings on emp.NIK equals pro.NIK into temp1
                from tmp1 in temp1.DefaultIfEmpty()
                join edu in myContext.Educations on tmp1.EducationId equals edu.Id into temp2
                from tmp2 in temp2.DefaultIfEmpty()
                join unv in myContext.Universities on tmp2.UniversityId equals unv.Id into temp3
                from tmp3 in temp3.DefaultIfEmpty()
                where emp.NIK == nik
                select new
                {
                    emp.NIK,
                    FullName = emp.FirstName + " " + emp.LastName,
                    Gender = emp.Gender.ToString(),
                    emp.BirthDate,
                    emp.Phone,
                    emp.Email,
                    emp.Salary,
                    tmp1.EducationId,
                    tmp2.Degree,
                    tmp2.GPA,
                    UniversityName = tmp3.Name
                }).ToList();
            return result;
        }*/
        /*public List<MasterDataVM> GetMaster()
        {
             var result = (
                 from emp in myContext.Employees
                 join pro in myContext.Profilings on emp.NIK equals pro.NIK into temp1
                 from tmp1 in temp1.DefaultIfEmpty()
                 join edu in myContext.Educations on tmp1.EducationId equals edu.Id into temp2
                 from tmp2 in temp2.DefaultIfEmpty()
                 join unv in myContext.Universities on tmp2.UniversityId equals unv.Id into temp3
                 from tmp3 in temp3.DefaultIfEmpty()
                 select new{
                     emp.NIK,
                     FullName = emp.FirstName+" "+emp.LastName,
                     Gender = emp.Gender.ToString(),
                     emp.BirthDate,
                     emp.Phone,
                     emp.Email,
                     emp.Salary,
                     tmp1.EducationId,
                     tmp2.Degree,
                     tmp2.GPA,
                     tmp3.Name
                 }).ToList();
             List<MasterDataVM> master = new List<MasterDataVM>();
             foreach (var item in result)
             {
                 MasterDataVM temp = new MasterDataVM
                 {
                     NIK = item.NIK,
                     FullName = item.FullName,
                     Gender = item.Gender,
                     BirthDate = item.BirthDate,
                     Phone = item.Phone,
                     Email = item.Email,
                     Salary = item.Salary,
                     EducationId = item.EducationId,
                     Degree = item.Degree,
                     GPA = item.GPA,
                     UniversityName = item.Name
                 };
                 master.Add(temp);
             }
             return master;
         }*/


        /*public MasterDataVM GetMaster(string nik)
        {
            var emp = myContext.Employees.Find(nik);
            var pro = myContext.Profilings.Find(nik);
            var edu = myContext.Educations.Find(pro.EducationId);
            var uni = myContext.Universities.Find(edu.UniversityId);
            var master = new MasterDataVM
            {
                NIK = emp.NIK,
                FullName = emp.FirstName + " " + emp.LastName,
                Gender = emp.gender.ToString(),
                BirthDate = emp.BirthDate,
                Phone = emp.Phone,
                Email = emp.Email,
                Salary = emp.Salary,
                EducationId = edu.Id,
                Degree = edu.Degree,
                GPA = edu.GPA,
                UniversityName = uni.Name
            };
            return master;
        }
        public List<MasterDataVM> GetMaster()
        {
            List<MasterDataVM> master = new List<MasterDataVM>();
            var emp = myContext.Employees.ToList();
            foreach (var item in emp)
            {
                var pro = myContext.Profilings.Find(item.NIK);
                var edu = myContext.Educations.Find(pro.EducationId);
                var uni = myContext.Universities.Find(edu.UniversityId);
                MasterDataVM temp = new MasterDataVM();
                temp.NIK = item.NIK;
                temp.FullName = item.FirstName + " " + item.LastName;
                temp.Gender = item.gender.ToString();
                temp.BirthDate = item.BirthDate;
                temp.Phone = item.Phone;
                temp.Email = item.Email;
                temp.Salary = item.Salary;
                temp.EducationId = edu.Id;
                temp.Degree = edu.Degree;
                temp.GPA = edu.GPA;
                temp.UniversityName = uni.Name;
                master.Add(temp);
            }
            return master;
        }*/
    }
}
