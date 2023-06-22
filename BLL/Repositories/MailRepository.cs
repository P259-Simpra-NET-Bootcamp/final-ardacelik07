using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.IRepositories;
using DAL;
using DAL.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BLL.Repositories
{
    public class MailRepository :  IMailRepository
    {


        public MailRepository()
        {


        }

        public async Task<bool> SendEmailForForgotPassword(string toAddress, string customerName, string code, string link)
        {
            try
            {
                MailAddress from = new MailAddress("celikarda812@gmail.com");
                MailAddress to = new MailAddress(toAddress);
                MailMessage msg = new MailMessage(from, to);

                msg.IsBodyHtml = true;
                msg.Subject = "şifre Sıfırlama";
                msg.Body += "<h2> Merhaba " + customerName + ", <br> <br>"
                    + "Şifre değiştirme isteğiniz alınmıştır." + "<br> <br>" +
                     " Kodunuz: "
                    + code
                    + "<br> <br>"
                    + "<a href='"+ link + "'> Buraya</a> tıklayarak şifrenizi değiştirebilirsiniz  <br> <br> ";

                SmtpClient Client = new SmtpClient();
                Client.Port = 587;
                Client.Host = "smtp.gmail.com";
                Client.EnableSsl = true;
                Client.Timeout = 10000;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.UseDefaultCredentials = false;
                Client.Credentials = new NetworkCredential("celikarda812@gmail.com", "umowdeqmuezaxmbb");
                Client.Send(msg);
                return true;

            }
            catch (Exception ex)
            {
                
                return false;
            }


        }

        public async Task<bool> SendEmailForGiftCard(string toAddress, string kupon, string amount, string expiredate)
        {
            try
            {
                MailAddress from = new MailAddress("celikarda812@gmail.com");
                MailAddress to = new MailAddress(toAddress);
                MailMessage msg = new MailMessage(from, to);

                msg.IsBodyHtml = true;
                msg.Subject = "şifre Sıfırlama";
                msg.Body += "<h2> Merhaba  <br> <br>"
                    + $"Alışverişinizde kullanmak için  {amount} TL tutarında {expiredate} tarihine kadar geçerli kupon kazandınız." + "<br> <br>" +
                     " kupon kodunuz: "
                    + kupon
                    + "<br> <br>"
                    + " iyi alışverişler ";

                SmtpClient Client = new SmtpClient();
                Client.Port = 587;
                Client.Host = "smtp.gmail.com";
                Client.EnableSsl = true;
                Client.Timeout = 10000;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.UseDefaultCredentials = false;
                Client.Credentials = new NetworkCredential("celikarda812@gmail.com", "umowdeqmuezaxmbb");
                Client.Send(msg);
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
