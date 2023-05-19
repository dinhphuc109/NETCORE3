using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
  public interface ILogRepository : IRepository<Log>
    {

    }
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(MyDbContext _db) : base(_db)
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