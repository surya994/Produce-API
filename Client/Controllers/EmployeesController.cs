using Client.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Client.Repositories.Data;

namespace Client.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository repository;
        public EmployeesController(EmployeeRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<JsonResult> GetMasterAll()
        {
            var result = await repository.GetMaster();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetMaster(string id)
        {
            var result = await repository.GetMaster(id);
            return Json(result);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
