using ApiService.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Dtos
{
    public class ProductDto
    {
         public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "stock should be more than 0")]
        public int Stock { get; set; }

        public int PointPercent { get; set; }

        public int MaxPoint { get; set; }

        public int CategoryId { get; set; }
        public bool IsValid { get; set; }
    }
}
