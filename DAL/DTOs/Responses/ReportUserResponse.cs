using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class ReportUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public decimal balance { get; set; }

        public string Role { get; set; }

        public decimal point { get; set; }

        public string GiftCode { get; set; }

        public int GiftCodeIsValid { get; set; }
    }
}

