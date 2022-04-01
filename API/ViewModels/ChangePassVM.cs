using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class ChangePassVM
    {
        public string Email { get; set; }
        public int OTP { get; set; }
        public string NewPass { get; set; }
        public string ConfirmPass { get; set; }
    }
}
