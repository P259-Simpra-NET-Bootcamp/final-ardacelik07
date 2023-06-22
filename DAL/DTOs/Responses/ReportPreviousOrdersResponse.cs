using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class ReportPreviousOrdersResponse
    {
        public Guid UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public int isValid { get; set; }

        public decimal UsedPoint { get; set; }

        public string UsedGiftCode { get; set; }
        public string SiparişNo { get; set; }
        public List<ReportOrderDetailResponse> OrderDetails { get; set; } = new List<ReportOrderDetailResponse>();
    }
}
