using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class ProfilingRepository : GeneralRepository<MyContext, Profiling, string>
    {
        private readonly MyContext myContext;
        public ProfilingRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }
    }
}