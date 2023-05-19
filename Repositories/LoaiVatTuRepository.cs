using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface ILoaiVatTuRepository : IRepository<LoaiVatTu>
    {

    }
    public class LoaiVatTuRepository : Repository<LoaiVatTu>, ILoaiVatTuRepository
    {
        public LoaiVatTuRepository(MyDbContext _db) : base(_db)
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