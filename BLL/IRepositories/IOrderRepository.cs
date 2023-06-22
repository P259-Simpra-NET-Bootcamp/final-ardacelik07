using DAL.DTOs.Dtos;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepositories
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Order CreateOrder(Guid id);

        Order GetTheCurrentOrder(Guid id);

        Order AddItemToTheOrder(OrderDto OrderDto);


        List<Order> UserPreviousOrders(Guid id);

    }
}
