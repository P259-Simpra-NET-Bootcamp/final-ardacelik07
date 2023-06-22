using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepositories
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {

        List<Category> GetAllWithIncludes();

        Category    CategoryGetByIdWithIncludes(int id);
    }
}
