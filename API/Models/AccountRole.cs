using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("AccountRole")]
    public class AccountRole
    {
        public string NIK { get; set; }
        public int RoleID { get; set; }
        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
       

    }
}
