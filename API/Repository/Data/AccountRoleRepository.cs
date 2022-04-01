using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, string>
    {
        private readonly MyContext myContext;
        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int SetManager(SignVM signVM)
        {
            var emp = myContext.Employees.FirstOrDefault(x => x.Email == signVM.Email);
            if (emp == null)
            {
                return 0;
            }
            var ar = new AccountRole
            {
                RoleID = 2,
                NIK = emp.NIK
            };
            return Insert(ar);
        }
    }
}