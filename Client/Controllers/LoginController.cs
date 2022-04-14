using API.Models;
using Client.Models;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var result = await repository.Login(loginVM);
            if (result.Status == 200)
            {
                TempData["message"] = result.Message;
                return View();
            }
            else
            {
                TempData["message"] = result.Message;
                return RedirectToAction("index", "login");
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
