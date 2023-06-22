using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class User : BaseClass
    {
        

        public Guid? UserTypeID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public decimal balance { get; set; }
        public string AspNetUserId { get; set; }
        public string PhoneNumber { get; set; }
       
        public string Email { get; set; }

        public decimal point { get; set; }
        public string Address { get; set; }

        public string Role { get; set; }

        public List<Order> Orders { get; set; }

        public string GiftCode { get; set; } = "";

        public decimal GiftCodeAmount { get; set; }

        public DateTime GiftCodeExpireDate { get; set; } 

        public int GiftCodeIsValid { get; set; }

    }
}
