using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IChiTietNhomLoaiRepository : IRepository<ChiTietNhomLoai>
    {

    }
    public class ChiTietNhomLoaiRepository : Repository<ChiTietNhomLoai>, IChiTietNhomLoaiRepository
    {
        public ChiTietNhomLoaiRepository(MyDbContext _db) : base(_db)
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