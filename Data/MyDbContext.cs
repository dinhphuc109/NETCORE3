using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NETCORE3.Models;
using static NETCORE3.Data.MyDbContext;

namespace NETCORE3.Data
{
    public class MyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>,
      ApplicationUserRole, IdentityUserLogin<Guid>,
      IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
      
    {
        public class ApplicationUser : IdentityUser<Guid>
        {
            public string FullName { get; set; }
            public string MaNhanVien { get; set; }
            public string? ChucDanh { get; set; }
            public bool IsActive { get; set; }
            public bool MustChangePass { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }
            public ICollection<ApplicationUserRole> UserRoles { get; set; }
            [ForeignKey("DonVi")]

            public Guid? DonVi_Id { get; set; }
            public DonVi DonVi  { get; set; }
            [ForeignKey("BoPhan")]
            public Guid? BoPhan_Id { get; set; }
            public BoPhan BoPhan  { get; set; }

        }
        public class ApplicationRole : IdentityRole<Guid>
        {
            public string Description { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }
            public ICollection<ApplicationUserRole> UserRoles { get; set; }
            public ICollection<Menu_Role> Menu_Roles { get; set; }
        }
        public class ApplicationUserRole : IdentityUserRole<Guid>
        {
            public virtual ApplicationUser User { get; set; }
            public virtual ApplicationRole Role { get; set; }
        }
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Loại bỏ quan hệ vòng
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            builder.Entity<ApplicationUserRole>(userRole =>
              {
                  userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                  userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                  userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
              });
            builder.Entity<Menu_Role>(pq =>
            {
                pq.HasKey(ur => new { ur.Menu_Id, ur.Role_Id });

                pq.HasOne(ur => ur.Menu)
              .WithMany(r => r.Menu_Roles)
              .HasForeignKey(ur => ur.Menu_Id)
              .IsRequired();

                pq.HasOne(ur => ur.Role)
              .WithMany(r => r.Menu_Roles)
              .HasForeignKey(ur => ur.Role_Id)
              .IsRequired();
            });
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Loai> Loais { get; set; }
        public DbSet<Nhom> Nhoms { get; set; }
        public DbSet<VatTu> VatTus { get; set; }
        public DbSet<LoaiVatTu> LoaiVatTus { get; set; }
        public DbSet<Hang> Hangs { get; set; }
        public DbSet<PhanHoi> PhanHois { get; set; }
        public DbSet<PhuongThucDangNhap> PhuongThucDangNhaps { get; set; }
        public DbSet<ChiTietHang> ChiTietHangs { get; set; }
        public DbSet<ChiTietNhom> ChiTietNhoms { get; set; }
        public DbSet<ChiTietNhomLoai> ChiTietNhomLoais { get; set; }
        public DbSet<DonViTinh> DonViTinhs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<Menu_Role> Menu_Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<BoPhan> BoPhans { get; set; }

        public DbSet<TapDoan> TapDoans { get; set; }
        public DbSet<DonVi> DonVis { get; set; }
        public DbSet<PhongBan> PhongBans { get; set; }

    }
}