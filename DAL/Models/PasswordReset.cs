using System;
namespace DAL.Models
{
    public class PasswordReset : BaseClass
    {
        //bu tabloda herhangi bir UI olsa idi projemde linkin gecerli olup olmadıgını kontrol etmek icindir. kontrolü eklemiştim ancak daha sonradan kaldırdım.
        public Guid UserID { get; set; }
        public DateTime EndDate { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; }
    }
}
