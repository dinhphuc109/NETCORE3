using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace NETCORE3.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }
    public class LoginMobileModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public Guid DonVi_Id { get; set; }
        public Guid? CongDoan_Id { get; set; }
    }
    public class InfoLogin
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public bool MustChangePass { get; set; }
    }
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string MaNhanVien { get; set; }

        public string FullName { get; set; }
        public List<string> RoleNames { get; set; }
        public bool IsActive { get; set; }
        public Guid? DonVi_Id { get; set; }
        public Guid? BoPhan_Id { get; set; }

    }
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Mật khẩu {0} ngắn nhất phải {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string Password { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Mật khẩu {0} ngắn nhất phải {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu xác nhận")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu mới không đúng.")]
        public string ConfirmNewPassword { get; set; }
    }

    public class ListUserModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }
        public string? TenDonVi { get; set; }
        public string? TenBoPhan { get; set; }


    }
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string MaNhanVien { get; set; }

        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public List<string> RoleNames { get; set; }
        public Guid? DonVi_Id { get; set; }
        public Guid? BoPhan_Id { get; set; }
        public string? TenDonVi { get; set; }
        public string? TenBoPhan { get; set; }

    }
    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool MustChangePass { get; set; }
    }
    public class FileModel
    {
        public Guid? id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public bool IsRemoved { get; set; }
    }
    public class TuKhoaModel
    {
        public Guid? id { get; set; }
        public string TenTuKhoa { get; set; }
        public bool IsRemoved { get; set; }
    }
    public class PhanQuyenDonVi
    {
        public List<DonViViewModel> lst_DonVis { get; set; }
        public Guid User_Id { get; set; }
    }
    public class User_PhanQuyen
    {
        public Guid DonVi_Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsFull { get; set; }
    }
    public class MenuInfo
    {
        public Guid Id { get; set; }
        public string STT { get; set; }
        public string TenMenu { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int ThuTu { get; set; }
        public Guid? Parent_Id { get; set; }
        public List<MenuInfo> children { set; get; }
        public bool IsUsed { get; set; }
        public bool IsRemove { get; set; }
    }
    public class Permission
    {
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Del { get; set; }
        public bool Print { get; set; }
        public bool Cof { get; set; }
    }
    public class MenuView
    {
        public Guid Id { get; set; }
        public string STT { get; set; }
        public string TenMenu { get; set; }
        public string Url { get; set; }
        public Guid? Parent_Id { get; set; }
        public int ThuTu { get; set; }
        public string Icon { get; set; }
        public List<MenuView> children { set; get; }
        public Permission permission { set; get; }
    }
    public class ApplicationUserListViewModel
    {
        public Guid Id { get; set; }

        public List<IdentityUserRole<string>> Roles { get; set; }
    }
    public class DonViViewModel
    {
        public Guid Id { get; set; }
        public string MaDonVi { get; set; }
        public string TenDonVi { get; set; }
        public string STT { get; set; }
        public bool IsUsed { set; get; }
        public bool IsRemove { set; get; }
        public Guid? Parent_Id { set; get; }
        public int ThuTu { set; get; }
        public bool Checked { set; get; }
        public bool HasChild { set; get; }
        public bool IsFull { get; set; }
        public bool IsLeaf { get; set; }
        public int Level { get; set; }
        public List<DonViViewModel> children { set; get; }
        public List<User_PhanQuyen> lst_user_phanquyen { set; get; }
    }
    public class ImportDonVi
    {
        public IFormFile file { get; set; }
        public Guid? Parent_Id { get; set; }
    }
    public class ClassTonDau
    {
        public int Ton { get; set; }
        public DateTime Ngay { get; set; }
        public Guid Id { get; set; }
        public string MaSanPham { get; set; }
    }

    public class ClassListVatTu{
        public Guid Id {get; set;}
        public string MaVatTu {get; set;}
        public string TenVatTu {get; set;}
        public string TenDonVi {get; set;}
        public string TenBoPhan {get; set;}
        public string ViTri {get; set;}
        public string FullName {get; set;}
    }
    public class ClassVatTuDetail{
        public Guid Id {get; set;}
        public string MaVatTu {get; set;}
        public string TenVatTu {get; set;}
        public string TenDonVi {get; set;}
        public string TenBoPhan {get; set;}
        public string ViTri {get; set;}
        public string FullName {get; set;}
        public string ThongSoKyThuat {get; set;}
        public string TenHang {get; set;}
        public string TenNhom {get; set;}
        public string TenLoai {get; set;}


    }

    public class ClassVatTuImport
    {
        public Guid Id { get; set; }
        public string MaVatTu { get; set; }
        public string TenVatTu { get; set; }
        public string GhiChu { get; set; }
        public string MaNhom { get; set; }
        public string MaDVT { get; set; }
        public string MaHang { get; set; }
        public string MaLoai { get; set; }
        public string ClassName { get; set; }
        public string GhiChuImport { get; set; }
        public Guid? Nhom_Id { get; set; }
        public Guid? DVT_Id { get; set; }
        public Guid? Hang_Id { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<LoaiVatTu> LstLoai { get; set; }
        public string ThongSoKyThuat { get; set; }
    }

    public class ExportExcel
    {
        public string MaVatTu { get; set; }
        public string TenVatTu { get; set; }
        public string TenDonViTinh { get; set; }
        public string SoLuong { get; set; }
        public string ThongSoKyThuat { get; set; }
        public string PathImage { get; set; }
    }

}