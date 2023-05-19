using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IVatTuRepository : IRepository<VatTu>
    {

    }
    public class VatTuRepository : Repository<VatTu>, IVatTuRepository
    {
        public VatTuRepository(MyDbContext _db) : base(_db)
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