using API.Base;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRoleController : BaseController<AccountRole, AccountRoleRepository, string>
    {
        private readonly AccountRoleRepository accountRoleRepository;
        public AccountRoleController(AccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
        {
            this.accountRoleRepository = accountRoleRepository;
        }
        [Authorize(Roles = "Director")]
        [HttpPost("SignManager")]
        public ActionResult SignManager(SignVM signVM)
        {
            if (accountRoleRepository.SetManager(signVM)==0)
            {
                return NotFound(new { status = 404, message = "Email Tidak Ditemukan" });
            }
            return Ok(new { status = 200, message = "Manager Telah Ditambahkan" });
        }
    }
}