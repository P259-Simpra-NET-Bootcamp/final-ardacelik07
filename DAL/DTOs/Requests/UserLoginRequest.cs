using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public Guid id { get; set; }

        public string Code { get; set; }


    }
}
