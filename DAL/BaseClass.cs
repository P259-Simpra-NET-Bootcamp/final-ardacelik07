using System;
namespace DAL
{
    public class BaseClass
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public BaseClass()
        {
            Id = Guid.NewGuid();
            Status = 1;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

        }
    }
}
