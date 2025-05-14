using Medyx.ApiAssets.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medyx_EMR.ApiAssets.Models
{
    [Table("BenhAn_PhuSan")]
    public partial class BenhAnPhuSan : ITrackableIDBA
    {
        public decimal Idba { get; set; }
        public string MaBa { get; set; }
        public string MaBn { get; set; }
        public string ToanThan { get; set; }
        public string Hach { get; set; }
        public string Vu { get; set; }
        public string TuanHoan { get; set; }
        public string HoHap { get; set; }
        public string TieuHoa { get; set; }
        public string ThanKinh { get; set; }
        public string XuongKhop { get; set; }
        public string ThanTietNieu { get; set; }
        public string Khac { get; set; }
        public string DHSDThuPhat { get; set; }
        public string MoiLon { get; set; }
        public string MoiBe { get; set; }
        public string AmVat { get; set; }
        public string AmHo { get; set; }
        public string MangTrinh { get; set; }
        public string TanSinhMon { get; set; }
        public string AmDao { get; set; }
        public string CoTuCung { get; set; }
        public string PhanPhu { get; set; }
        public string CacTuiCung { get; set; }
        public string CanLamSang { get; set; }
        public string TomTatBenhAn { get; set; }
        public string MaCDPhanBiet { get; set; }
        public string TienLuong { get; set; }
        public string HuongDT { get; set; }
        public DateTime? NgayKyDT { get; set; }
        public bool? Huy { get; set; }
        public string MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public DateTime? NgaySd { get; set; }
        public string NguoiSd { get; set; }
        public DateTime? NgayHuy { get; set; }
        public string NguoiHuy { get; set; }
        public bool? SeoPTCu { get; set; }
        public string HinhDangTC { get; set; }
        public string TuThe { get; set; }
        public decimal? ChieuCaoTC { get; set; }
        public decimal? VongBung { get; set; }
        public string ConCoTC { get; set; }
        public int? TimThai { get; set; }
        public string CSBishop { get; set; }
        public DateTime? OiVoLuc { get; set; }
        public string TinhTrangOi { get; set; }
        public string OiVo { get; set; }
        public string MauSacNuocOi { get; set; }
        public string NuoiOiNhieu { get; set; }
        public string Ngoi { get; set; }
        public string The { get; set; }
        public string KieuThe { get; set; }
        public string DoLot { get; set; }
        public string DuongKinhNhoHaVe { get; set; }
        public string PPDTChinh { get; set; }
        public DateTime? VaoBuongDe { get; set; }
        public string NguoiTheoDoi { get; set; }
        public string MaChucDanh { get; set; }
        public int? Mach { get; set; }
        public decimal? NhietDo { get; set; }
        public string HuyetAp { get; set; }
        public int? NhipTho { get; set; }
        public decimal? CanNang { get; set; }
        public decimal? ChieuCao { get; set; }
        public decimal? BMI { get; set; }
        public int? CanNangCon { get; set; }
        public decimal? ChieuCaoCon { get; set; }
        public decimal? VongDauCon { get; set; }
        public string Apgar1Phut { get; set; }
        public string Apgar5Phut { get; set; }
        public string Apgar10Phut { get; set; }
        public byte? DonThai { get; set; }
        public byte? GTCon { get; set; }
        public bool? TatBamSinh { get; set; }
        public bool? CoHauMon { get; set; }
        public string CuTheTatBamSinh { get; set; }
        public string TTConSauDe { get; set; }
        public string XuLyCon { get; set; }
        public byte? Rau { get; set; }
        public DateTime? NgaySoRau { get; set; }
        public string CachSoRau { get; set; }
        public string MatMang { get; set; }
        public string MatMui { get; set; }
        public string BanhRau { get; set; }
        public decimal? CanNangRau { get; set; }
        public bool? RauCuonCo { get; set; }
        public decimal? CuongRauDai { get; set; }
        public bool? ChayMauCo { get; set; }
        public int? LuongMauMat { get; set; }
        public bool? KiemSoatTC { get; set; }
        public string XuLyRau { get; set; }
        public string DaMeSauDe { get; set; }
        public string PPDe { get; set; }
        public int? MachMeSauDe { get; set; }
        public decimal? NhietDoMeSauDe { get; set; }
        public string HuyetApMeSauDe { get; set; }
        public int? NhipThoMeSauDe { get; set; }
        public string LyDoCanThiep { get; set; }
        public byte? TangSinhMonSauDe { get; set; }
        public string PhuongPhapKhau { get; set; }
        public string SoMuiKhau { get; set; }
        public byte? RachCoTCSauDe { get; set; }
        public string CachKiemSoatTC { get; set; }
        public DateTime? NgayDe { get; set; }
        public bool? Phu { get; set; }
        public string ThanTuCung { get; set; }
        public string LyDoVV { get; set; }
        public int? VaoNgayThuCuaBenh { get; set; }

    }
}
