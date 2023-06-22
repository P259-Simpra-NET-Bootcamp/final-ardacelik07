using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Requests
{
    public class StockControlRequest
    {

        public string action { get; set; }

        public int amount { get; set; }
    }
}
