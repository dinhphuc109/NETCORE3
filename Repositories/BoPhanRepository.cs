using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IBoPhanRepository : IRepository<BoPhan>
    {

    }
    public class BoPhanRepository : Repository<BoPhan>, IBoPhanRepository
    {
        public BoPhanRepository(MyDbContext _db) : base(_db)
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