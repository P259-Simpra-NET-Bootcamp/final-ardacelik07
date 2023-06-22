using System;
using System.Threading.Tasks;
using BLL.IConfiguration;
using BLL.IRepositories;
using BLL.Repositories;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BLL.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<IdentityUser> _userManager;
        

        public IUserRepository UserRepository { get; private set; }
        public IPasswordResetRepository PasswordResetRepository { get; private set; }
     
        public IMailRepository Mail { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }
        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
            _userManager = userManager;
            


            UserRepository = new UserRepository(_context, _logger, _userManager);
            PasswordResetRepository = new PasswordResetRepository(_context, _logger);
            CategoryRepository= new CategoryRepository(_context, _logger);
            ProductRepository= new ProductRepository(_context, _logger);
            OrderRepository= new OrderRepository(_context, _logger);
            OrderDetailRepository = new OrderDetailRepository(_context, _logger);
            Mail = new MailRepository();
          
        }


        public async Task CompleteAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
           catch(Exception ex)
            {
                _logger.LogError(ex, "{UnitOfWork} ComplateAsync method error", typeof(UnitOfWork));

                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
