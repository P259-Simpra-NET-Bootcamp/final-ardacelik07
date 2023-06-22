using BLL.IRepositories;
using DAL.DTOs.Dtos;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IPaymentService
    {

        Task<string> ComplateOrder(PaymentParams param);

    }
}
