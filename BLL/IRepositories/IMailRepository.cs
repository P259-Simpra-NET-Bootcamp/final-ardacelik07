using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;


namespace BLL.IRepositories
{
    public interface IMailRepository
    {
        Task<bool> SendEmailForForgotPassword(String toAddress, String customerName, String code, String link);

        Task<bool> SendEmailForGiftCard(string toAddress, string kupon, string amount,string expiredate);
    }
}
