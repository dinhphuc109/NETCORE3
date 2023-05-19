using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NETCORE3.Repositories;

namespace NETCORE3.Infrastructure
{
    public interface IUnitofWork : IDisposable
    {
        IMenuRepository Menus { get; }
        ILoaiRepository Loais { get; }
        INhomRepository Nhoms { get; }
        IVatTuRepository VatTus { get; }
        ILoaiVatTuRepository LoaiVatTus { get; }
        IDonViTinhRepository DonViTinhs { get; }
        INhaCungCapRepository NhaCungCaps { get; }
        IHangRepository Hangs { get; }
        IPhanHoiRepository PhanHois { get; }
        IPhuongThucDangNhapRepository PhuongThucDangNhaps { get; }
        IChiTietHangRepository ChiTietHangs { get; }
        IChiTietNhomRepository ChiTietNhoms { get; }
        IChiTietNhomLoaiRepository ChiTietNhomLoais { get; }
        IMenu_RoleRepository Menu_Roles { get; }
        ILogRepository Logs { get; }
        IBoPhanRepository BoPhans { get; }

        ITapDoanRepository TapDoans { get; }
        IDonViRepository DonVis { get; }
        IPhongBanRepository PhongBans { get; }
    
        int Complete();
    }
}
