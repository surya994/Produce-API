using API.Base;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilingController : BaseController<Profiling, ProfilingRepository, string>
    {
        private readonly ProfilingRepository profilingRepository;
        public ProfilingController(ProfilingRepository profilingRepository) : base(profilingRepository)
        {
            this.profilingRepository = profilingRepository;
        }
    }
}
