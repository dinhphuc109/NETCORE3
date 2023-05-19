using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface INhomRepository : IRepository<Nhom>
    {

    }
    public class NhomRepository : Repository<Nhom>, INhomRepository
    {
        public NhomRepository(MyDbContext _db) : base(_db)
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