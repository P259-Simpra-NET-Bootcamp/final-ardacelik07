using AutoMapper;
using DAL.DTOs.Dtos;
using DAL.DTOs.Requests;
using DAL.DTOs.Responses;
using DAL.Models;

namespace ApiService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductDto>().ReverseMap();
            CreateMap<Order,OrderResponse>().ReverseMap();
            CreateMap<OrderDetails,OrderDetailResponse>().ReverseMap();
            CreateMap<Product,ReportProductResponse>().ReverseMap();
            CreateMap<Order, ReportOrderResponse>().ReverseMap();
            CreateMap<OrderDetails,ReportOrderDetailResponse>().ReverseMap();
            CreateMap<User, ReportUserResponse>().ReverseMap();
            CreateMap<User, GiftCardResponse>().ReverseMap();
            CreateMap<Order, ReportPreviousOrdersResponse>().ReverseMap();

        }
    }
}
