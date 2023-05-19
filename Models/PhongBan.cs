using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETCORE3.Models
{
	public class PhongBan : Auditable
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mã bắt buộc")]
        public string MaPhongBan { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Tên bắt buộc")]
        public string TenPhongBan { get; set; }

        [ForeignKey("DonVi")]
        public Guid MaDonVi { get; set; }
        public DonVi Donvi { get; set; }
    }
}

