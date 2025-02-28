using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medyx_EMR_BCA.Models.DanhMuc
{
    public class DMTrangThaiKyVM
    {
        public Int32 STT { get; set; }
        public String IDBA { get; set; }
        public String MaBN { get; set; }
        public String MaBS { get; set; }
        public String TenBS { get; set; }
        public String TrangThaiKy { get; set; }
        public Int32 LanKy { get; set; }
        public String DuongDanFile { get; set; }
        public String MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public DateTime? NgaySD { get; set; }
        public String NguoiSD { get; set; }
        public DateTime? NgayHuy { get; set; }
        public String NguoiHuy { get; set; }
        public Boolean Huy { get; set; }
        public Int32 TotalRows { get; set; }
    }
    public class DMTrangThaiKy
    {
        public Int32 STT { get; set; }
        public String IDBA { get; set; }
        public String MaBN { get; set; }
        public String MaBS { get; set; }
        public String TenBS { get; set; }
        public String TrangThaiKy { get; set; }
        public Int32 LanKy { get; set; }
        public String DuongDanFile { get; set; }
        public String LoaiGiayTo { get; set; }
        public String MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public DateTime? NgaySD { get; set; }
        public String NguoiSD { get; set; }
        public DateTime? NgayHuy { get; set; }
        public String NguoiHuy { get; set; }
        public Boolean Huy { get; set; }
    }
}
