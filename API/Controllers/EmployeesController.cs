using API.Base;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    /*[Authorize(Roles = "Director,Manager")]*/
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [Authorize]
        [HttpGet("master")]
        public ActionResult GetMasterData()
        {
            if (employeeRepository.GetMaster() == null)
            {
                return NotFound(employeeRepository.GetMaster());
            }
            return Ok(employeeRepository.GetMaster());
        }
        /*[Authorize(Roles = "Director,Manager")]*/
        [HttpGet("master/{nik}")]
        public ActionResult GetMasterData(string nik)
        {
            if (employeeRepository.Get(nik) == null)
            {
                return NotFound(employeeRepository.GetMaster(nik));
            }
            return Ok(employeeRepository.GetMaster(nik));
        }
        /*[HttpGet("email/{email}")]
        public ActionResult GetByEmail(string email)
        {
            if (employeeRepository.GetEmail(email) == null)
            {
                return NotFound(employeeRepository.GetEmail(email));
            }
            return Ok(employeeRepository.GetEmail(email));
        }
        [HttpGet("phone/{phone}")]
        public ActionResult GetByPhone(string phone)
        {
            if (employeeRepository.GetPhone(phone) == null)
            {
                return NotFound(employeeRepository.GetPhone(phone));
            }
            return Ok(employeeRepository.GetPhone(phone));
        }*/
        /*[HttpPost]
        public override ActionResult Post(Employee employee)
        {
            if (employeeRepository.GetEmail(employee.Email) != null)
            {
                return BadRequest(new { status = 400, message = "Email sudah Ada" });
            }
            if (employeeRepository.GetPhone(employee.Phone) != null)
            {
                return BadRequest(new { status = 400, message = "Phone sudah Ada" });
            }
            employee.NIK = employeeRepository.GenerateNIK();
            employeeRepository.Insert(employee);
            return Ok(new { status = 200, message = "Data Berhasil Ditambahkan" });
        }*/
        /*[Authorize(Roles ="Director,Manager")]*/
        /*[HttpGet("TestCORS")]
        [EnableCors("AllowOrigin")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS Berhasil");
        }*/
    }
}
