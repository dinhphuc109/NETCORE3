using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
    public class Loai : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Mã bắt buộc")]
        public string MaLoai { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Tên bắt buộc")]
        public string TenLoai { get; set; }
        [NotMapped]
        public List<ChiTietHang> LstChiTietHang { get; set; }
        [JsonIgnore]
        public virtual ICollection<ChiTietHang> ChiTietHangs {get; set;}
        [NotMapped]
        public List<ChiTietNhomLoai> LstChiTietNhomLoai { get; set; }
        [JsonIgnore]
        public virtual ICollection<ChiTietNhomLoai> ChiTietNhomLoais {get; set;}
    }
}