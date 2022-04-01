using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class EducationRepository : GeneralRepository<MyContext, Education, int>
    {
        private readonly MyContext myContext;
        public EducationRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }
    }
}