﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Dtos
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
