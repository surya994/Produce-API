using API.Models;
using Client.Models;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginRepository repository;
        private readonly ILogger<LoginController> _logger;
        public LoginController(LoginRepository repository, ILogger<LoginController> logger)
        {
            this.repository = repository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Test([FromForm] LoginVM loginVM)
        {
            if (loginVM.Password == null || loginVM.Email == null)
            {
                TempData["message"] = "Data Tidak Boleh kosong";
                return RedirectToAction("index", "login");
            }
            else
            {
                var result = await repository.Login(loginVM);
                if (result.Status == 200)
                {
                    var token = result.TokenId;

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    var email = jwt.Claims.First(claim => claim.Type == "Email").Value;
                    HttpContext.Session.SetString("Email", email);

                    HttpContext.Session.SetString("JWToken", token);
                    return RedirectToAction("index", "admin");
                }
                else
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("index", "login");
                }
            }
            /*return Redirect("~/login?error=true");*/
            /*return RedirectToAction("index","admin");*/
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
