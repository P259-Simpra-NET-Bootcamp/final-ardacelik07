using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public  class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }

        public decimal PointPercent { get; set; }

        public decimal MaxPoint { get; set; }

        public int CategoryId { get; set; }

        public bool IsValid { get; set; }

        public Category Category { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
