using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NETCORE3.Models
{
    public class PhanHoi : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid NguoiDung_Id { get; set; }
        public DateTime Date { get; set; }
        public String NoiDungPhanHoi {get; set; }
        public String Image { get; set; }
        public String GopY {get; set; }
        public int TrangThaiXuLy { get; set; }
    }
}