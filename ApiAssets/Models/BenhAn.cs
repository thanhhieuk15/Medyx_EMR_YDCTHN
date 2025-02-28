using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Medyx.ApiAssets.Models.Interface;
using Newtonsoft.Json;

namespace Medyx_EMR_BCA.ApiAssets.Models
{
    public partial class BenhAn : ITrackable, ITrackableIDBA
    {
        public decimal Idba { get; set; }
        public string MaBa { get; set; }
        public string TenDvcq { get; set; }
        public string MaBv { get; set; }
        public string TenBv { get; set; }
        public string MaKhoaVv { get; set; }
        public string Buong { get; set; }
        public string Giuong { get; set; }
        public string SoVaoVien { get; set; }
        public string SoLuuTru { get; set; }
        public string MaBn { get; set; }
        public string MaYt { get; set; }
        public byte LoaiBa { get; set; }
        public DateTime NgayVv { get; set; }
        public DateTime? NgayRv { get; set; }
        public string TrucTiepVao { get; set; }
        public string NoiGt { get; set; }
        public int? Vvlan { get; set; }
        public string ChuyenVien { get; set; }
        public string MaBvChuyenDen { get; set; }
        public string HtraVien { get; set; }
        public int? TongSoNgayDt { get; set; }
        public string MaBenhNoiChuyenDenYhhd { get; set; }
        public string MaBenhKkbyhhd { get; set; }
        public string MaBenhChinhVv { get; set; }
        public string TenBenhChinhVv { get; set; }
        public string MaBenhKemVv1 { get; set; }
        public string MaBenhKemVv2 { get; set; }
        public string MaBenhKemVv3 { get; set; }
        public string MaBenhKemVv4 { get; set; }
        public string MaBenhKemVv5 { get; set; }
        public string MaBenhKemVv6 { get; set; }
        public string MaBenhKemVv7 { get; set; }
        public string MaBenhKemVv8 { get; set; }
        public string MaBenhKemVv9 { get; set; }
        public string MaBenhKemVv10 { get; set; }
        public string MaBenhKemVv11 { get; set; }
        public string MaBenhKemVv12 { get; set; }
        public string MaBenhChinhRv { get; set; }
        public string TenBenhChinhRv { get; set; }
        public string MaBenhKemRv1 { get; set; }
        public string MaBenhKemRv2 { get; set; }
        public string MaBenhKemRv3 { get; set; }
        public string MaBenhKemRv4 { get; set; }
        public string MaBenhKemRv5 { get; set; }
        public string MaBenhKemRv6 { get; set; }
        public string MaBenhKemRv7 { get; set; }
        public string MaBenhKemRv8 { get; set; }
        public string MaBenhKemRv9 { get; set; }
        public string MaBenhKemRv10 { get; set; }
        public string MaBenhKemRv11 { get; set; }
        public string MaBenhKemRv12 { get; set; }

        //Tên bệnh kèm theo vào viện 
        public string TenBenhKemVv1 { get; set; }
        public string TenBenhKemVv2 { get; set; }
        public string TenBenhKemVv3 { get; set; }
        public string TenBenhKemVv4 { get; set; }
        public string TenBenhKemVv5 { get; set; }
        public string TenBenhKemVv6 { get; set; }
        public string TenBenhKemVv7 { get; set; }
        public string TenBenhKemVv8 { get; set; }
        public string TenBenhKemVv9 { get; set; }
        public string TenBenhKemVv10 { get; set; }
        public string TenBenhKemVv11 { get; set; }
        public string TenBenhKemVv12 { get; set; }
        //Tên bệnh kèm ra viện
        public string TenBenhKemRv1 { get; set; }
        public string TenBenhKemRv2 { get; set; }
        public string TenBenhKemRv3 { get; set; }
        public string TenBenhKemRv4 { get; set; }
        public string TenBenhKemRv5 { get; set; }
        public string TenBenhKemRv6 { get; set; }
        public string TenBenhKemRv7 { get; set; }
        public string TenBenhKemRv8 { get; set; }
        public string TenBenhKemRv9 { get; set; }
        public string TenBenhKemRv10 { get; set; }
        public string TenBenhKemRv11 { get; set; }
        public string TenBenhKemRv12 { get; set; }
        public string NguyenNhanTBBC { get; set; }
        public int? TongSoNgayDtsauPt { get; set; }
        public byte? TongSoLanPt { get; set; }
        public byte? ThuThuatYhhd { get; set; }
        public byte? PhauThuatYhhd { get; set; }
        public byte? TaiBienYhhd { get; set; }
        public byte? BienChungYhhd { get; set; }
        public string MaBenhNoiChuyenDenYhct { get; set; }
        public string MaBenhKkbyhct { get; set; }
        public string MaBenhChinhVvyhct { get; set; }
        public string TenBenhChinhVvyhct { get; set; }
        public string MaBenhKemVv1yhct { get; set; }
        public string MaBenhKemVv2yhct { get; set; }
        public string MaBenhKemVv3yhct { get; set; }
        public string MaBenhKemVv4yhct { get; set; }
        public string MaBenhKemVv5yhct { get; set; }
        public string MaBenhKemVv6yhct { get; set; }
        public string MaBenhKemVv7yhct { get; set; }
        public string MaBenhKemVv8yhct { get; set; }
        public string MaBenhKemVv9yhct { get; set; }
        public string MaBenhKemVv10yhct { get; set; }
        public string MaBenhKemVv11yhct { get; set; }
        public string MaBenhKemVv12yhct { get; set; }
        //Tên bệnh kèm vào viện yhct
        public string TenBenhKemVv1yhct { get; set; }
        public string TenBenhKemVv2yhct { get; set; }
        public string TenBenhKemVv3yhct { get; set; }
        public string TenBenhKemVv4yhct { get; set; }
        public string TenBenhKemVv5yhct { get; set; }
        public string TenBenhKemVv6yhct { get; set; }
        public string TenBenhKemVv7yhct { get; set; }
        public string TenBenhKemVv8yhct { get; set; }
        public string TenBenhKemVv9yhct { get; set; }
        public string TenBenhKemVv10yhct { get; set; }
        public string TenBenhKemVv11yhct { get; set; }
        public string TenBenhKemVv12yhct { get; set; }

        public string MaBenhChinhRvyhct { get; set; }
        public string TenBenhChinhRvyhct { get; set; }
        public string MaBenhKemRv1yhct { get; set; }
        public string MaBenhKemRv2yhct { get; set; }
        public string MaBenhKemRv3yhct { get; set; }
        public string MaBenhKemRv4yhct { get; set; }
        public string MaBenhKemRv5yhct { get; set; }
        public string MaBenhKemRv6yhct { get; set; }
        public string MaBenhKemRv7yhct { get; set; }
        public string MaBenhKemRv8yhct { get; set; }
        public string MaBenhKemRv9yhct { get; set; }
        public string MaBenhKemRv10yhct { get; set; }
        public string MaBenhKemRv11yhct { get; set; }
        public string MaBenhKemRv12yhct { get; set; }
        //Tên bệnh kèm theo ra viện yhct
        public string TenBenhKemRv1yhct { get; set; }
        public string TenBenhKemRv2yhct { get; set; }
        public string TenBenhKemRv3yhct { get; set; }
        public string TenBenhKemRv4yhct { get; set; }
        public string TenBenhKemRv5yhct { get; set; }
        public string TenBenhKemRv6yhct { get; set; }
        public string TenBenhKemRv7yhct { get; set; }
        public string TenBenhKemRv8yhct { get; set; }
        public string TenBenhKemRv9yhct { get; set; }
        public string TenBenhKemRv10yhct { get; set; }
        public string TenBenhKemRv11yhct { get; set; }
        public string TenBenhKemRv12yhct { get; set; }
        public byte? ThuThuatYhct { get; set; }
        public byte? PhauThuatYhct { get; set; }
        public byte? TaiBienYhct { get; set; }
        public byte? BienChungYhct { get; set; }
        public string Kqdt { get; set; }
        public string GiaiPhauBenh { get; set; }
        public DateTime? NgayTuVong { get; set; }
        public string TinhTrangTuVong { get; set; }
        public string NguyenNhanTuVong { get; set; }
        public byte? KhamNghiemTuThi { get; set; }
        public string MaBenhGptuThi { get; set; }
        public string BsdieuTri { get; set; }
        public DateTime? NgayKy { get; set; }
        public string GiamDoc { get; set; }
        public string TruongKhoa { get; set; }
        public string LoiDan { get; set; }
        public DateTime? NgayTruongKhoaKy { get; set; }
        public DateTime? NgayBatDauNghiViecHuongBhxh { get; set; }
        public DateTime? NgayKetThucNghiViecHuongBhxh { get; set; }
        public DateTime? NgayCapGiayCnnvhuongBhxh { get; set; }
        public string NguoiXacnhanKetThucHs { get; set; }
        public byte? XacNhanKetThucHs { get; set; }
        public DateTime? NgayXacNhanKetThucHs { get; set; }
        public string NguoiLuuTru { get; set; }
        public byte? XacNhanLuuTru { get; set; }
        public DateTime? NgayLuuTru { get; set; }
        public bool? Huy { get; set; }
        public string MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public DateTime? NgaySd { get; set; }
        public string NguoiSd { get; set; }
        public DateTime? NgayHuy { get; set; }
        public string NguoiHuy { get; set; }
        [JsonIgnore]
        public virtual ThongTinBn ThongTinBn { get; set; }
        [JsonIgnore]
        public virtual DmbaLoaiBa DmbaLoaiBa { get; set; }
        [JsonIgnore]
        public virtual Dmkhoa Dmkhoa { get; set; }
        [JsonIgnore]
        public virtual DmkhoaBuong DmkhoaBuong { get; set; }
        [JsonIgnore]
        public virtual DmkhoaGiuong DmkhoaGiuong { get; set; }
        [JsonIgnore]
        public virtual DmbaNoiKham DmbaNoiKham { get; set; }
        [JsonIgnore]
        public virtual DmbaNoiGt DmbaNoiGt { get; set; }
        [JsonIgnore]
        public virtual DmbaChuyenVien DmbaChuyenVien { get; set; }
        [JsonIgnore]
        public virtual DmbenhVien DmbenhVien { get; set; }
        [JsonIgnore]
        public virtual DmbaHtraVien DmbaHtraVien { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhTatNoiChuyenDen { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKKBYHHD { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhChinhVV { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV1 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV2 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV3 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV4 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV5 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV6 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV7 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV8 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV9 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV10 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV11 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemVV12 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhChinhRV { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV1 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV2 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV3 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV4 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV5 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV6 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV7 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV8 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV9 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV10 { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV11{ get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhKemRV12 { get; set; }
        // public virtual DmbenhTat DmBenhGptuThi { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhNoiChuyenDenYHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKKBYHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhChinhVVYHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV1YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV2YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV3YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV4YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV5YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV6YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV7YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV8YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV9YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV10YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV11YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemVV12YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhChinhRVYHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV1YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV2YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV3YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV4YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV5YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV6YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV7YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV8YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV9YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV10YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV11YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbenhTatYhct DmBenhKemRV12YHCT { get; set; }
        [JsonIgnore]
        public virtual DmbaLdtvong DmbaLdtvong { get; set; }
        [JsonIgnore]
        public virtual DmbenhTat DmBenhGPTuThi { get; set; }
        [JsonIgnore]
        public virtual DmnhanVien DmBsDieutri { get; set; }
        [JsonIgnore]
        public virtual DmnhanVien DmGiamdoc { get; set; }
        [JsonIgnore]
        public virtual DmnhanVien DmTruongKhoa { get; set; }
        [JsonIgnore]
        public virtual DmnhanVien DmNguoiLap { get; set; }
        [JsonIgnore]
        public virtual DmnhanVien DmNguoiHuy { get; set; }
        [JsonIgnore]
        public virtual BenhAnTvBbkiemDiem BenhAnTvBbkiemDiem { get; set; }
        [JsonIgnore]
        public virtual BenhAnTvPhieuCdnguyenNhan BenhAnTvPhieuCdnguyenNhan { get; set; }
        [JsonIgnore]
        public virtual ICollection<BenhAnKhoaDieuTri> BenhAnKhoaDieuTris { get; set; }
        [JsonIgnore]
        public virtual ICollection<BenhAnKhamVaoVien> BenhAnKhamVaoViens { get; set; }
        [JsonIgnore]
        public virtual ICollection<BenhAnPhieuChamSoc> BenhAnPhieuChamSocs { get; set; }
        [JsonIgnore]
        public virtual ICollection<BenhAnTvBbkiemDiemTv> BenhAnTvBbkiemDiemTvs { get; set; }
    }
}
