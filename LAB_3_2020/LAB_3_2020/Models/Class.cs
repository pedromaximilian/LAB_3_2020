using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3_2020.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public String Mail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Pass { get; set; }

    }
}
