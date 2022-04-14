using Client.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Client.Repositories.Data;

namespace Client.Controllers
{
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository repository;
        public AccountsController(AccountRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]RegisterVM registerVM)
        {
            var checkEmail = await repository.GetByEmail(registerVM.Email);
            var checkPhone = await repository.GetByPhone(registerVM.Phone);
            if (checkEmail != null)
            {
                return BadRequest("Email Sudah Terdaftar");
            }
            if (checkPhone != null)
            {
                return BadRequest("Phone Sudah Terdaftar");
            }
            repository.Register(registerVM);
            return Ok("Data Berhasil Didaftarkan");
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateMasterVM updateMasterVM)
        {
            var checkEmail = await repository.GetByEmail(updateMasterVM.Email, updateMasterVM.NIK);
            var checkPhone = await repository.GetByPhone(updateMasterVM.Phone, updateMasterVM.NIK);
            if (checkEmail != null)
            {
                return BadRequest("Email Sudah Terdaftar");
            }
            if (checkPhone != null)
            {
                return BadRequest("Phone Sudah Terdaftar");
            }
            repository.Update(updateMasterVM);
            return Ok("Data Berhasil Diupdate");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
