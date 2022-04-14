using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Client.Models
{
    public class LoginResponseVM
    {
        public int Status { get; set; }
        public string TokenId { get; set; }
        public string Message { get; set; }
    }
}
