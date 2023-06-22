using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Responses
{
    public class GiftCardResponse
    {

        public string Name { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string GiftCode { get; set; }

        public decimal GiftCodeAmount { get; set; }

        public DateTime GiftCodeExpireDate { get; set; }

        public int GiftCodeIsValid { get; set; }
    }
}
