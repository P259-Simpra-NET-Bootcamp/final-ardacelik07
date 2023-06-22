using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Order
    {
        //order sınıfım bir nevi basket gibi davranıyor. orderlar icin ayrı bir tablo acmamayı tercih ettim.

        public int Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public decimal TotalPrice { get; set; }

        public int isValid { get; set; }

        public int isCompletedSuccesfully { get; set; }

        public decimal UsedPoint { get; set; }

        public string UsedGiftCode { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

        public string SiparişNo { get; set; }
    }
}
