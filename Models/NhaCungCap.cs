using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
  public class NhaCungCap : Auditable
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [StringLength(50)]
    [Required(ErrorMessage = "Mã bắt buộc")]
    public string MaNhaCungCap { get; set; }
    [StringLength(250)]
    [Required(ErrorMessage = "Tên bắt buộc")]
    public string TenNhaCungCap { get; set; }
    public string SoDienThoai { get; set; }
    public string DiaChi { get; set; }
  }
}