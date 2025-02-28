using Medyx_EMR_BCA.ApiAssets.AttributeCustom;
using Medyx_EMR_BCA.ApiAssets.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Medyx_EMR_BCA.ApiAssets.Dto
{
    public sealed class BenhAnDetailDto
    {
        public decimal? Idba { get; set; }
        [Required(ErrorMessage = "Mã bệnh án là bắt buộc.")]
        public string MaBa { get; set; }
        public string TenDvcq { get; set; }
        public string MaBv { get; set; }
        public string TenBv { get; set; }
        [Required(ErrorMessage = "Số vào viện là bắt buộc.")]
        public string SoVaoVien { get; set; }
        public string NguyenNhanTBBC { get; set; }
        public int? TongSoNgayDtsauPt { get; set; }
        public byte? TongSoLanPt { get; set; }
        public string MaKhoaVv { get; set; }
        public string SoLuuTru { get; set; }
        [Required(ErrorMessage = "Mã y tế là bắt buộc.", AllowEmptyStrings = true)]
        public string MaYt { get; set; }
        public string MaBvChuyenDen { get; set; }
        public string GiaiPhauBenh { get; set; }
        [RangeDateTime(null, "DateTime.Now", ErrorMessage = "Ngày vào viện là bắt buộc và phải nhỏ hơn hoặc bằng ngày giờ hiện tại.")]
        public DateTime NgayVv { get; set; }
        [RangeDateTime("NgayVv", "DateTime.Now", ErrorMessage = "Ngày ra viện phải lớn hơn hoặc bằng ngày vào viện {1} và nhỏ hơn hoặc bằng ngày giờ hiện tại.")]
        public DateTime? NgayRv { get; set; }
        public bool? Huy { get; set; }
        public DateTime? NgayKy { get; set; }
        public DateTime? NgayTuVong { get; set; }
        public string NguyenNhanTuVong { get; set; }
        public byte? KhamNghiemTuThi { get; set; }
        public BenhAnDetailThongTinBnDto BenhNhan { get; set; }
        public DmbaLoaiBaDto LoaiBenhAn { get; set; }
        public DmkhoaDto Khoa { get; set; }
        public DmkhoaDto KhoaRV { get; set; }
        public DmkhoaBuongDto Buong { get; set; }
        public DmkhoaGiuongDto Giuong { get; set; }
        public string TrucTiepVao { get; set; }
        public string NoiGt { get; set; }
        public string ChuyenVien { get; set; }
        public DmbenhVienDto benhVienChuyenDen { get; set; }
        public string HtraVien { get; set; }
        public int? TongSoNgayDt { get; set; }
        public DmbenhTatDto BenhTatNoiChuyenDen { get; set; }
        public string MaBenhChinhVv { get; set; }
        public string TenBenhChinhVv { get; set; }
        public DmbenhTatDto BenhChinhVv { get; set; }
        public DmbenhTatDto BenhKemVV1 { get; set; }
        public DmbenhTatDto BenhKKBYHHD { get; set; }
        public DmbenhTatYhctDto BenhKKBYHCT { get; set; }
        public DmbenhTatDto BenhKemVV2 { get; set; }
        public DmbenhTatDto BenhKemVV3 { get; set; }
        public DmbenhTatDto BenhKemVV4 { get; set; }
        public DmbenhTatDto BenhKemVV5 { get; set; }
        public DmbenhTatDto BenhKemVV6 { get; set; }
        public DmbenhTatDto BenhKemVV7 { get; set; }
        public DmbenhTatDto BenhKemVV8 { get; set; }
        public DmbenhTatDto BenhKemVV9 { get; set; }
        public DmbenhTatDto BenhKemVV10 { get; set; }
        public DmbenhTatDto BenhKemVV11 { get; set; }
        public DmbenhTatDto BenhKemVV12 { get; set; }

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


        public byte? ThuThuatYhhd { get; set; }
        public byte? PhauThuatYhhd { get; set; }
        public string MaBenhChinhRv { get; set; }
        public string TenBenhChinhRv { get; set; }
        public DmbenhTatDto BenhChinhRv { get; set; }
        public DmbenhTatDto BenhKemRV1 { get; set; }
        public DmbenhTatDto BenhKemRV2 { get; set; }
        public DmbenhTatDto BenhKemRV3 { get; set; }
        public DmbenhTatDto BenhKemRV4 { get; set; }
        public DmbenhTatDto BenhKemRV5 { get; set; }
        public DmbenhTatDto BenhKemRV6 { get; set; }
        public DmbenhTatDto BenhKemRV7 { get; set; }
        public DmbenhTatDto BenhKemRV8 { get; set; }
        public DmbenhTatDto BenhKemRV9 { get; set; }
        public DmbenhTatDto BenhKemRV10 { get; set; }
        public DmbenhTatDto BenhKemRV11 { get; set; }
        public DmbenhTatDto BenhKemRV12 { get; set; }

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

        public byte? TaiBienYhhd { get; set; }
        public byte? BienChungYhhd { get; set; }
        public DmbenhTatYhctDto BenhNoiChuyenDenYHCT { get; set; }
        public string MaBenhChinhVvyhct { get; set; }
        public string TenBenhChinhVvyhct { get; set; }
        public DmbenhTatYhctDto BenhChinhVvyhct { get; set; }
        public DmbenhTatYhctDto BenhKemVV1YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV2YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV3YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV4YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV5YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV6YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV7YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV8YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV9YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV10YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV11YHCT { get; set; }
        public DmbenhTatYhctDto BenhKemVV12YHCT { get; set; }

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
        public DmbenhTatYhctDto BenhKemRv1yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv2yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv3yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv4yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv5yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv6yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv7yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv8yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv9yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv10yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv11yhct { get; set; }
        public DmbenhTatYhctDto BenhKemRv12yhct { get; set; }

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
        public string TinhTrangTuVong { get; set; }
        public DmnhanVienDto Giamdoc { get; set; }
        public DmnhanVienDto TruongKhoa { get; set; }
        public DmnhanVienDto BsDieutri { get; set; }
        public DmbenhTatDto BenhGPTuThis { get; set; }
        public List<BenhAnKhoaDieuTriDto> BenhAnKhoaDieuTris { get; set; }
        public int? Vvlan { get; set; }
        public string NguoiXacNhanKetThucHs { get; set; }
        public byte? XacNhanKetThucHs { get; set; }
        public DateTime? NgayXacNhanKetThucHs { get; set; }
        public DateTime? NgayTruongKhoaKy { get; set; }

    }

    public class BenhAnDetailThongTinBnDto
    {
        public string MaBn { get; set; }
        public decimal? Idba { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public byte? Tuoi { get; set; }
        public byte GioiTinh { get; set; }
        public DmngheNghiepDto NgheNghiep { get; set; }
        public DmdanTocDto DanToc { get; set; }
        public DmquocGiaDto QuocGia { get; set; }
        public DmtinhDto Tinh { get; set; }
        public DmquanHuyenDto QuanHuyen { get; set; }
        public DmphuongXaDto PhuongXa { get; set; }
        public DmdoiTuongDto DoiTuong { get; set; }
        public string SoNha { get; set; }
        public string Thon { get; set; }
        public string NoiLamViec { get; set; }
        public DateTime? Gtbhytdn { get; set; }
        public DateTime? Gtbhyttn { get; set; }
        public string SoTheBhyt { get; set; }
        public string LienHe { get; set; }
        public string SoDienThoai { get; set; }
        public string HoTenCha { get; set; }
        public string HoTenMe { get; set; }
        public string NguoiGiamHo { get; set; }
        public string Cmnd { get; set; }
        public string NoiCapCmnd { get; set; }
        public string NgheNghiepNguoiGiamHo { get; set; }
        public string QuanHeNguoiGiamHo { get; set; }
        public string GiayCnkhuyetTat { get; set; }
        public string DangKhuyetTat { get; set; }
        public string MucDoKhuyetTat { get; set; }
        public DateTime? NgayCapCmnd { get; set; }
    }



    public class DetailBenhAnDto{
        public BenhAn BenhAn { get; set; } = new BenhAn();
        public ThongTinBn BenhNhan {  get; set; }  = new ThongTinBn();
        public DmbenhVien benhVienChuyenDen { get; set; } = new DmbenhVien();
        public DmngheNghiep NgheNghiep {  get; set; } = new DmngheNghiep();
        public DmdanToc DanToc { get; set; } = new DmdanToc();
        public DmquocGia QuocGia { get; set; } = new DmquocGia();
        public Dmtinh Tinh { get; set; } = new Dmtinh();
        public DmquanHuyen QuanHuyen { get;set; } = new DmquanHuyen();
        public DmphuongXa PhuongXa {  get; set; } = new DmphuongXa();
        public DmdoiTuong DoiTuong { get; set; } = new DmdoiTuong();
        public DmkhoaBuong Buong { get; set; } = new DmkhoaBuong();
        public DmkhoaGiuong Giuong { get; set; } = new DmkhoaGiuong();
        public DmbaLoaiBa LoaiBenhAn { get; set; } = new DmbaLoaiBa();
        public Dmkhoa Khoa { get; set; } = new Dmkhoa();
        public DmbenhTat BenhKKBYHHD { get; set; } = new DmbenhTat();
        public DmbenhTatYhct BenhKKBYHCT { get; set; } = new DmbenhTatYhct();
        public DmbenhTat BenhTatNoiChuyenDen { get; set; } = new DmbenhTat();
        public DmbenhTatYhct BenhNoiChuyenDenYHCT { get; set; } = new DmbenhTatYhct();
        public DmnhanVien GiamDoc { get; set; } = new DmnhanVien();
        public DmnhanVien TruongKhoa { get; set; } = new DmnhanVien();
        public DmnhanVien BsDieutri { get; set; } = new DmnhanVien();
        public DmbenhTat BenhGPTuThis { get; set; } = new DmbenhTat();
    }
    public class BenhAnhDto
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
    }

    public class ThongTinBenhAnDtoV2
    {
        public string MaBn { get; set; }
        public decimal Idba { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public byte? Tuoi { get; set; }
        public byte GioiTinh { get; set; }
        public string MaNgheNghiep { get; set; }
        public string MaDanToc { get; set; }
        public string MaQuocTich { get; set; }
        public string SoNha { get; set; }
        public string Thon { get; set; }
        public string MaPxa { get; set; }
        public string MaHuyen { get; set; }
        public string MaTinh { get; set; }
        public string NoiLamViec { get; set; }
        public string DoiTuong { get; set; }
        public DateTime? Gtbhyttn { get; set; }
        public DateTime? Gtbhytdn { get; set; }
        public string SoTheBhyt { get; set; }
        public string MaNoiDkbd { get; set; }
        public string LienHe { get; set; }
        public string SoDienThoai { get; set; }
        public string HoTenCha { get; set; }
        public string HoTenMe { get; set; }
        public string NguoiGiamHo { get; set; }
        public string GiayCnkhuyetTat { get; set; }
        public string DangKhuyetTat { get; set; }
        public string MucDoKhuyetTat { get; set; }
        public string NgheNghiepNguoiGiamHo { get; set; }
        public string QuanHeNguoiGiamHo { get; set; }
        public string Cmnd { get; set; }
        public string NoiCapCmnd { get; set; }
        public DateTime? NgayCapCmnd { get; set; }
        public string NhomMau { get; set; }
        public string Rh { get; set; }
        public bool? Huy { get; set; }
        public string MaMay { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NguoiLap { get; set; }
        public DateTime? NgaySd { get; set; }
        public string NguoiSd { get; set; }
        public DateTime? NgayHuy { get; set; }
        public string NguoiHuy { get; set; }
    }

    public class DmbenhVienDtoV2 {
        public string MaBv { get; set; }
        public string TenBv { get; set; }
        public string TenTa { get; set; }
        public string MaBhxh { get; set; }
        public string Matinh { get; set; }
        public string Diachi { get; set; }
        public string Tel { get; set; }
    }

}
