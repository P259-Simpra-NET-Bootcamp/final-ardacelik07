using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTOs.Dtos
{
    public class AddBalanceWithCreditCard
    {

       
        [Required]
        public int amount { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public int CVC { get; set; }


        [Required]

        public string expireDate { get; set; }



    }
}
