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
            if (accountRepository.GetEmail(registerVM.Email) != null)
            {
                return BadRequest(new { status = 400, message = "Email sudah Ada" });
            }
            if (accountRepository.GetPhone(registerVM.Phone) != null)
            {
                return BadRequest(new { status = 400, message = "Phone sudah Ada" });
            }
            accountRepository.Register(registerVM);
            return Ok(new { status = 200, message = "Register Berhasil" });
        }
        [HttpPut("master/update")]
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
        }
        [HttpPost("login")]
        public ActionResult Login(LoginVM loginVM)
        {
            if (accountRepository.GetEmail(loginVM.Email) == null)
            {
                return NotFound(new { status = 404, message = "Email Tidak Ditemukan" });
            }
            if (accountRepository.Login(loginVM) == 1)
            {
                var tokenid = accountRepository.GetToken(loginVM.Email);
                return Ok(new { status = 200, tokenid, message = "Login Berhasil" });
            }
            else
            {
                return Unauthorized(new { status = 401, message = "Password Salah" });
            }
        }
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
    }
 }
