using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IChiTietHangRepository : IRepository<ChiTietHang>
    {

    }
    public class ChiTietHangRepository : Repository<ChiTietHang>, IChiTietHangRepository
    {
        public ChiTietHangRepository(MyDbContext _db) : base(_db)
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