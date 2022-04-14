using API.Base;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public AccountController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        [HttpPost("register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            return Ok(accountRepository.Register(registerVM));
        }
        [HttpPut("master/update")]
        public ActionResult Update(UpdateMasterVM updateMasterVM)
        {
            return Ok(accountRepository.Edit(updateMasterVM));
        }
        [HttpPost("login")]
        public ActionResult Login(LoginVM loginVM)
        {
            if (accountRepository.GetEmail(loginVM.Email) == null)
            {
                string tokenid = null;
                return NotFound(new { status = 401, tokenid, message = "Email Tidak Ditemukan" });
            }
            if (accountRepository.Login(loginVM) == 1)
            {
                var tokenid = accountRepository.GetToken(loginVM.Email);
                return Ok(new { status = 200, tokenid, message = "Login Berhasil" });
            }
            else
            {
                string tokenid = null;
                return Unauthorized(new { status = 401, tokenid, message = "Email atau Password Salah" });
            }
        }


        /*[HttpPut("master/update")]
        public ActionResult Update(UpdateMasterVM updateMasterVM)
        {
            if (accountRepository.GetEmail(updateMasterVM.Email, updateMasterVM.NIK) != null)
            {
                return BadRequest(new { status = 400, message = "Email sudah Terdaftar" });
            }
            if (accountRepository.GetPhone(updateMasterVM.Phone, updateMasterVM.NIK) != null)
            {
                return BadRequest(new { status = 400, message = "Phone sudah Terdaftar" });
            }
            accountRepository.Edit(updateMasterVM);
            return Ok(new { status = 200, message = "Data Berhasil Diupdate" });
        }*/
        



        [HttpPost("forgot")]
        public ActionResult ForgotPass(ForgotPassVM forgotPassVM)
        {
            if (accountRepository.GetEmail(forgotPassVM.Email) == null)
            {
                return NotFound(new { status = 404, message = "Email Tidak Ditemukan" });
            }
            accountRepository.GetOTP(forgotPassVM.Email);
            return Ok(new { status = 200, message = "OTP Berhasil Dikirim" });
        }
        [HttpPost("change")]
        public ActionResult ChangePass(ChangePassVM changePassVM)
        {
            if(changePassVM.NewPass != changePassVM.ConfirmPass){
                return BadRequest(new { status = 400, message = "Confirm Password Tidak Sama" });
            }
            int result = accountRepository.ChangePassword(changePassVM);
            switch (result)
            {
                case 0:
                    return BadRequest(new { status = 400, message = "OTP Salah" });
                case -1:
                    return BadRequest(new { status = 400, message = "OTP Sudah Digunakan" });
                case -2:
                    return BadRequest(new { status = 400, message = "OTP Sudah Kadaluarsa" });
                default:
                    return Ok(new { status = 200, message = "Ganti Password Berhasil" });
            }
        }
        [HttpGet("email/{email}")]
        public ActionResult GetByEmail(string email)
        {
            if (accountRepository.GetEmail(email) == null)
            {
                return NotFound(accountRepository.GetEmail(email));
            }
            return Ok(accountRepository.GetEmail(email));
        }
        [HttpGet("phone/{phone}")]
        public ActionResult GetByPhone(string phone)
        {
            if (accountRepository.GetPhone(phone) == null)
            {
                return NotFound(accountRepository.GetPhone(phone));
            }
            return Ok(accountRepository.GetPhone(phone));
        }
        [HttpGet("email/{email}/{nik}")]
        public ActionResult GetByEmail(string email, string nik)
        {
            if (accountRepository.GetEmail(email, nik) == null)
            {
                return NotFound(accountRepository.GetEmail(email, nik));
            }
            return Ok(accountRepository.GetEmail(email, nik));
        }
        [HttpGet("phone/{phone}/{nik}")]
        public ActionResult GetByPhone(string phone, string nik)
        {
            if (accountRepository.GetPhone(phone, nik) == null)
            {
                return NotFound(accountRepository.GetPhone(phone, nik));
            }
            return Ok(accountRepository.GetPhone(phone, nik));
        }
    }
 }
