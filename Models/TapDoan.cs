using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETCORE3.Models
{
	public class TapDoan : Auditable
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mã bắt buộc")]
        public string MaTapDoan { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Tên bắt buộc")]
        public string TenTapDoan { get; set; }
        
    }
}

