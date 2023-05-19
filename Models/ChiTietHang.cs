using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
    public class ChiTietHang : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Hang")]
        public Guid Hang_Id { get; set; }
        public Hang Hang { get; set; }
        [ForeignKey("Loai")]
        public Guid Loai_Id { get; set; }
        public Loai Loai { get; set; }
    }
}