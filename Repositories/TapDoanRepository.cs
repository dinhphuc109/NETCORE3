using System;
using NETCORE3.Data;
using NETCORE3.Data.Migrations;
using NETCORE3.Infrastructure;
using NETCORE3.Models;

namespace NETCORE3.Repositories
{
	public interface ITapDoanRepository : IRepository<TapDoan>
	{

	}
    public class TapDoanRepository : Repository<TapDoan>, ITapDoanRepository
    {
        public TapDoanRepository(MyDbContext _db) : base(_db)
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

