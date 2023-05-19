using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static NETCORE3.Data.MyDbContext;

namespace NETCORE3.Models
{
  public class VatTu : Auditable
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [StringLength(50)]
    [Required(ErrorMessage = "Mã bắt buộc")]
    public string MaVatTu { get; set; }
    [StringLength(250)]
    [Required(ErrorMessage = "Tên bắt buộc")]
    public string TenVatTu { get; set; }
    [ForeignKey("Hang")]
    public Guid? Hang_Id { get; set; }
    public Hang Hang { get; set; }
    [ForeignKey("Nhom")]
    public Guid? Nhom_Id { get; set; }
    public Nhom Nhom { get; set; }
    [ForeignKey("DonViTinh")]
    public Guid? DVT_Id { get; set; }
    public DonViTinh DonViTinh { get; set; }
    [ForeignKey("User")]
    public Guid? User_Id { get; set; }
    public ApplicationUser User { get; set; }
    public string ViTri { get; set; }
    public string PathImage { get; set; }
    public string ThongSoKyThuat { get; set; }
    public string TenVatTuKhongDau { get; set; }
    [JsonIgnore]
    public virtual ICollection<LoaiVatTu> LoaiVatTus { get; set; }
    [NotMapped]
    public List<LoaiVatTu> LstLoai { get; set; }
  }
}