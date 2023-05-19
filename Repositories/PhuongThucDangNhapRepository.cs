using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
    public interface IPhuongThucDangNhapRepository : IRepository<PhuongThucDangNhap>
    {

    }
    public class PhuongThucDangNhapRepository : Repository<PhuongThucDangNhap>, IPhuongThucDangNhapRepository
    {
        public PhuongThucDangNhapRepository(MyDbContext _db) : base(_db)
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