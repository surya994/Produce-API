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
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpPost]
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
        }
        /*[Authorize(Roles = "Director,Manager")]*/
        [HttpGet("master/{nik}")]
        public ActionResult GetMasterData(string nik)
        {
            if (employeeRepository.Get(nik) == null)
            {
                return BadRequest(new { status = 404, message = "NIK Tidak Ditemukan" });
            }
            return Ok(new { status = 200, result = employeeRepository.GetMaster(nik), message = "Data Ditemukan" });
        }
        /*[Authorize(Roles ="Director,Manager")]*/
        [HttpGet("master")]
        public ActionResult GetMasterData()
        {
            if (employeeRepository.GetMaster() == null)
            {
                return NotFound(new { status = 404, result = employeeRepository.GetMaster(), message = "Data Tidak Ditemukan" });
            }
            return Ok(new { status = 200, result = employeeRepository.GetMaster(), message = "Data Ditemukan" });
        }
        [HttpGet("TestCORS")]
        [EnableCors("AllowOrigin")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS Berhasil");
        }
    }
}
