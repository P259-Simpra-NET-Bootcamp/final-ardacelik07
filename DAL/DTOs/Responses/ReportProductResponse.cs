using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class ReportProductResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }
    }
}
