using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Dtos
{
    public class OrderDto
    {



        public Guid Id { get; set; }

        public string email { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int price { get; set; }

        [Required]
        public int ProductId { get; set; }

       
    }
}
