using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IMenu_RoleRepository : IRepository<Menu_Role>
    {

    }
    public class Menu_RoleRepository : Repository<Menu_Role>, IMenu_RoleRepository
    {
        public Menu_RoleRepository(MyDbContext _db) : base(_db)
        {
        }
        public MyDbContext MyDbContext
        {
            get
            {
                return _db as MyDbContext;
            }
        }


    }
}