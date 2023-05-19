using System.Collections.Generic;
using NETCORE3.Data;
using NETCORE3.Infrastructure;
using NETCORE3.Repositories;

namespace NETCORE3.UOW
{
    public class UnitofWork : IUnitofWork
    {
        public IMenuRepository Menus { get; private set; }
        public ILoaiRepository Loais { get; private set; }
        public INhomRepository Nhoms { get; private set; }
        public IVatTuRepository VatTus { get; private set; }
        public INhaCungCapRepository NhaCungCaps { get; private set; }
        public ILoaiVatTuRepository LoaiVatTus { get; private set; }
        public IDonViTinhRepository DonViTinhs { get; private set; }
        public IHangRepository Hangs { get; private set; }
        public IPhanHoiRepository PhanHois { get; private set; }
        public IPhuongThucDangNhapRepository PhuongThucDangNhaps { get; private set; }
        public IChiTietHangRepository ChiTietHangs { get; private set; }
        public IChiTietNhomRepository ChiTietNhoms { get; private set; }
        public IChiTietNhomLoaiRepository ChiTietNhomLoais { get; private set; }
        public IMenu_RoleRepository Menu_Roles { get; private set; }
        public ILogRepository Logs { get; private set; }
        public IBoPhanRepository BoPhans { get; private set; }

        public ITapDoanRepository TapDoans { get; private set; }
        public IDonViRepository DonVis { get; private set; }
        public IPhongBanRepository PhongBans { get; private set; }

        private MyDbContext db;
        public UnitofWork(MyDbContext _db)
        {
            db = _db;
            Menus = new MenuRepository(db);
            Loais = new LoaiRepository(db);
            Nhoms = new NhomRepository(db);
            Hangs = new HangRepository(db);
            PhanHois = new PhanHoiRepository(db);
            ChiTietHangs = new ChiTietHangRepository(db);
            ChiTietNhoms = new ChiTietNhomRepository(db);
            ChiTietNhomLoais = new ChiTietNhomLoaiRepository(db);
            DonViTinhs = new DonViTinhRepository(db);
            LoaiVatTus = new LoaiVatTuRepository(db);
            VatTus = new VatTuRepository(db);
            
            NhaCungCaps = new NhaCungCapRepository(db);
            PhuongThucDangNhaps = new PhuongThucDangNhapRepository(db);
            Menu_Roles = new Menu_RoleRepository(db);
            Logs = new LogRepository(db);
            BoPhans = new BoPhanRepository(db);

            TapDoans = new TapDoanRepository(db);
            DonVis = new DonViRepository(db);
            PhongBans = new PhongBanRepository(db);

        }
        public void Dispose()
        {
            db.Dispose();
        }
        public int Complete()
        {
            return db.SaveChanges();
        }
  }
}