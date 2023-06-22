using DAL.DTOs.Requests;
using DAL.DTOs.Responses;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface  IGiftCardService
    {
        List<GiftCardResponse> GiftCardsForAllUsers();
       Task<GiftCardResponse> CreateGiftCards(GiftCardRequest giftCardRequest);

        Task<bool> DeleteGiftCards(string email);
    }
}
