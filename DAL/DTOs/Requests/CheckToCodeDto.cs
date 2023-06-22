using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Requests
{
    public class CheckToCodeDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
    }
}
