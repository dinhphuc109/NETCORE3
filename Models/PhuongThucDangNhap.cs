using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
  public class PhuongThucDangNhap : Auditable
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [StringLength(250)]
    [Required(ErrorMessage = "Tên bắt buộc")]
    public string TenPhuongThucDangNhap { get; set; }
    [StringLength(250)]
    [Required(ErrorMessage = "Phương thức xác thực bắt buộc")]
    public string Domain { get; set;}
    public string LinkCheckLogin {get; set;}
  }
}