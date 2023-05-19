using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
    public class ChiTietNhom : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Nhom")]
        public Guid Nhom_Id { get; set; }
        public Nhom Nhom { get; set; }
        [ForeignKey("Hang")]
        public Guid Hang_Id { get; set; }
        public Hang Hang { get; set; }
    }
}