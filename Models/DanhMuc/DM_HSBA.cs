using System;

namespace Medyx_EMR.Models.DanhMuc
{
    public class DM_HSBA
    {
        public long Id { get; set; }
        public String MaHS { get; set; }
        public String TenLoaiHSBA { get; set; }
        public DateTime? NgayLap { get; set; }
        public String NguoiLap { get; set; }
        public DateTime? NgayHuy { get; set; }
        public String NguoiHuy { get; set; }
    }
}
