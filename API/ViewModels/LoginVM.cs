using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class LoginVM
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
