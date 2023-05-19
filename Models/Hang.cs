using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
    public class Hang : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Mã bắt buộc")]
        public string MaHang { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Tên bắt buộc")]
        public string TenHang { get; set; }
        [JsonIgnore]
        public virtual ICollection<ChiTietNhom> ChiTietNhoms {get; set;}
        [NotMapped]
        public List<ChiTietNhom> LstChiTietNhom { get; set; }
    }
}