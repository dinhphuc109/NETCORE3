using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface INhaCungCapRepository : IRepository<NhaCungCap>
    {

    }
    public class NhaCungCapRepository : Repository<NhaCungCap>, INhaCungCapRepository
    {
        public NhaCungCapRepository(MyDbContext _db) : base(_db)
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