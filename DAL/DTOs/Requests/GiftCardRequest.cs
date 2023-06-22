using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Requests
{
    public class GiftCardRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public decimal GiftCodeAmount { get; set; }
        [Required]
        public int activedays { get; set; }

    }
}
