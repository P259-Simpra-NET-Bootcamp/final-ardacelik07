using System;
using System.Threading.Tasks;
using BLL.IRepositories;

namespace BLL.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPasswordResetRepository PasswordResetRepository { get; }
        IMailRepository Mail { get; }
        IProductRepository ProductRepository  { get; }
        IOrderRepository OrderRepository { get;  }
        ICategoryRepository CategoryRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }


        // Sending the changes to database
        Task CompleteAsync();
    }
}
