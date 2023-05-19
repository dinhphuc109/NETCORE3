using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IChiTietNhomRepository : IRepository<ChiTietNhom>
    {

    }
    public class ChiTietNhomRepository : Repository<ChiTietNhom>, IChiTietNhomRepository
    {
        public ChiTietNhomRepository(MyDbContext _db) : base(_db)
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