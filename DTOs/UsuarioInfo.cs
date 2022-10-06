﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class UsuarioInfo
    {
        [EmailAddress]
        [Required]

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
