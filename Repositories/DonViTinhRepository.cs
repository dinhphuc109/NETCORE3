using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IDonViTinhRepository : IRepository<DonViTinh>
    {

    }
    public class DonViTinhRepository : Repository<DonViTinh>, IDonViTinhRepository
    {
        public DonViTinhRepository(MyDbContext _db) : base(_db)
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