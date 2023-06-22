using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class OrderDetailResponse
    {
      
        public int Id { get; set; }

        public int OrderId { get; set; }




      
        public int Quantity { get; set; }

        public int ProductId { get; set; }
     
    }
}
