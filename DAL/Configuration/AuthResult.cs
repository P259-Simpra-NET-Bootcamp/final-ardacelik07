using System;
using System.Collections.Generic;

namespace DAL.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }
       
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string UserId { get; set; }
    }
}
