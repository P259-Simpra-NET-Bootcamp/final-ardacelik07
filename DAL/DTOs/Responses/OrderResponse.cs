using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

      

        public decimal TotalPrice { get; set; }

        public int isValid { get; set; }

        public string SiparişNo { get; set; }
        public List<OrderDetailResponse> OrderDetails { get; set; } = new List<OrderDetailResponse>();
    }
}

