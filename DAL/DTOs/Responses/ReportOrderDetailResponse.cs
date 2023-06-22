using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class ReportOrderDetailResponse
    {
       

        public int OrderId { get; set; }





        public int Quantity { get; set; }

        public ReportProductResponse product { get; set; }
    }
}
