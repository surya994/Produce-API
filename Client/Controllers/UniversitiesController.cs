using Client.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Client.Repositories.Data;

namespace Client.Controllers
{
    public class UniversitiesController : BaseController<University, UniversityRepository, int>
    {
        private readonly UniversityRepository repository;
        public UniversitiesController(UniversityRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
