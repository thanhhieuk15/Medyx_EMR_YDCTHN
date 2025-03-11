using System;
using System.Collections.Generic;
using Medyx.ApiAssets.Models.Interface;

namespace Medyx_EMR_BCA.ApiAssets.Models
{
    public partial class BenhAnTienSuBenh : ITrackableIDBA
    {
        public decimal Idba { get; set; }
        public string MaBa { get; set; }
        public string MaBn { get; set; }
        public string TienSuBanThan { get; set; }
        public string MaTienSu { get; set; }
        public string MoTaTienSu { get; set; }
        public string DacDiemLienQuanBenh { get; set; }
        public string TienSuGiaDinh { get; set; }
        public byte DiUng { get; set; }
        public byte? BenhTangHa { get; set; }
        public string ThoiDiemPhatHienTangHa { get; set; }
        public string NoiDieuTriTangHa { get; set; }
        public string DieuTriTangHathuongXuyen { get; set; }
        public string DonDaTriLieuHa { get; set; }
        public string ChiSoHamax { get; set; }
        public byte? BenhDtd { get; set; }
        public string ThoiDiemPhatHienDtd { get; set; }
        public string NoiDieuTriDtd { get; set; }
        public string DieuTriDtdthuongXuyen { get; set; }
        public string DonDaTriLieuDtd { get; set; }
        public string Dtdmax { get; set; }
        public byte? ThucHienCdanDtd { get; set; }
        public byte? UongThuocDtd { get; set; }
        public string LoaiThuocDtd { get; set; }
        public byte? NamMacTmHaDtd { get; set; }
        public byte? NuMacTmHaDtd { get; set; }
        public byte? HutThuoc { get; set; }
        public int? SoDieu { get; set; }
        public decimal? SoBao { get; set; }
        public byte? UongRuou { get; set; }
        public decimal? LuongRuou { get; set; }
        public string YeuToNguyCoKhac { get; set; }
        public byte? BenhPhoiHopTangHa { get; set; }
        public byte? BenhPhoiHopDtd { get; set; }
        public byte? BenhPhoiHopGout { get; set; }
        public byte? BenhPhoiHopKhopManTinh { get; set; }
        public byte? BenhPhoiHopRlchLipid { get; set; }
        public byte? BenhPhoiHopThan { get; set; }
        public byte? BenhPhoiHopMachVanh { get; set; }
        public byte? BenhPhoiHopNoiTietKhac { get; set; }
        public string BenhPhoiHopBenhLyKhac { get; set; }
        public decimal? UongRuouThoigian { get; set; }
        public decimal? HutThuocThoigian { get; set; }
        public decimal? HutThuocLao { get; set; }
        public decimal? HutThuocLaoThoigian { get; set; }
        public byte? MaTuy { get; set; }
        public decimal? MaTuyThoigian { get; set; }
        public decimal? DacDiemKhac { get; set; }
        public string DiUngDiNguyen { get; set; }
        public bool? Huy { get; set; }
        public string MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public DateTime? NgaySd { get; set; }
        public string NguoiSd { get; set; }
        public DateTime? NgayHuy { get; set; }
        public string NguoiHuy { get; set; }

        //Bệnh án nội trú Nhi
        public byte? ConThu { get; set; }
        public byte? DeDuThang { get; set; }
        public byte? CanNang { get; set; }
        public byte? DeKho { get; set; }
        public byte? DeNgatTho { get; set; }
        public byte? RungRon { get; set; }
        public string AnDuoi1Tuoi { get; set; }
        public string AnTren1Tuoi { get; set; }
        public string ThangCaiSua { get; set; }
        public byte? ThangLay { get; set; }
        public byte? ThangBo { get; set; }
        public byte? ThangDi { get; set; }
        public string ThangNoi { get; set; }
        public byte? ThangMocRang { get; set; }
        public int? TuoiCoKinh { get; set; }
        public byte? DaTiemChung { get; set; }
        public byte? BenhDaMac { get; set; }
        public string DacDienSH { get; set; }
        public string Para { get; set; }
        public string TinhTrangSinh { get; set; }
        public bool? DiTatBamSinh { get; set; }
        public string CuTheDiTat { get; set; }
        public string PhatTrienTinhThan { get; set; }
        public string PhatTrienVanDong { get; set; }
        public string BenhLyKhac { get; set; }
        public string NuoiDuong { get; set; }
        public string ChamSoc { get; set; }
        public string TiemChungBenhKhac { get; set; }
    }
}
