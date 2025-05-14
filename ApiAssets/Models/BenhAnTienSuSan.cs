using Medyx.ApiAssets.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medyx_EMR.ApiAssets.Models
{
    [Table("BenhAn_TienSuSan")]
    public class BenhAnTienSuSan : ITrackable, ITrackableIDBA
    {
        public decimal Idba { get; set; }
        public string MaBa { get; set; }
        public string MaBn { get; set; }
        public int STT { get; set; }
        public byte? SoLanCoThai { get; set; }
        public string Nam { get; set; }
        public string DeDuThang { get; set; }
        public string DeThieuThang { get; set; }
        public string Say { get; set; }
        public string Hut { get; set; }
        public string Nao { get; set; }
        public string CoVac { get; set; }
        public string ChuaNgoaiTC { get; set; }
        public string ThaiChetLuu { get; set; }
        public string ConSong { get; set; }
        public string CanNang { get; set; }
        public string PhuongPhapDe { get; set; }
        public string TaiBien { get; set; }
        public string ChuaTrung { get; set; }
        public bool? Huy { get; set; }
        public string MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public DateTime? NgaySd { get; set; }
        public string NguoiSd { get; set; }
        public DateTime? NgayHuy { get; set; }
        public string NguoiHuy { get; set; }
    }
}
