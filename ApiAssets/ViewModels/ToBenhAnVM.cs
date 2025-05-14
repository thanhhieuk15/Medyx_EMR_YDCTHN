﻿using System;
using System.ComponentModel.DataAnnotations;
using Medyx_EMR.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.AttributeCustom;
using Medyx_EMR_BCA.ApiAssets.Models;

namespace Medyx_EMR_BCA.ApiAssets.ViewModels
{
    public class BenhAnTongKetBenhAnVM : BenhAnTongKetBenhAn
    {
        public new decimal? Idba { get; set; }
    }
    public class BenhAnKhamYhhdVM : BenhAnKhamYhhd
    {
        public new decimal? Idba { get; set; }
        [Range(0, 226, ErrorMessage = "Mạch phải nằm trong khoảng từ {1} đến {2}.")]
        public new int? Mach { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "BMI phải lớn hơn hoặc bằng 0.")]
        public new decimal? Bmi { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Nhịp thở phải lớn hơn hoặc bằng 0.")]
        public new int? NhipTho { get; set; }
        [Range(34, 42, ErrorMessage = "Nhiệt độ phải nằm trong khoảng từ {1} đến {2}.")]
        public new decimal? NhietDo { get; set; }
    }
    public class BenhAnKhamYhctVM : BenhAnKhamYhct
    {
        public new decimal? Idba { get; set; }
    }

    public class ToBenhAnBenhAnKhoaDieuTriVM : BenhAnKhoaDieuTri
    {
        public new DateTime? NgayVaoKhoa { get; set; }
        public new decimal? Idba { get; set; }
        public new int? Stt { get; set; }
    }

    public class ToBenhAnBenhAnVM : BenhAn
    {
        public new decimal? Idba { get; set; }
        //[RangeDateTime(null, "DateTime.Now", ErrorMessage = "Ngày vào viện là bắt buộc và phải nhỏ hơn hoặc bằng ngày giờ hiện tại.")]
        public new DateTime NgayVv { get; set; }
        [RangeDateTime("NgayVv", "DateTime.Now", ErrorMessage = "Ngày ra viện phải lớn hơn hoặc bằng ngày vào viện {1} và nhỏ hơn hoặc bằng ngày giờ hiện tại.")]
        public new DateTime? NgayRv { get; set; }
    }
    public class BenhAnTienSuBenhVM : BenhAnTienSuBenh
    {
        public new decimal? Idba { get; set; }
        public new byte? canNangNhi { get; set; }
        public new byte? canNangSinh { get; set; }
        public DateTime? kinhTuNgay { get; set; }
        public DateTime? kinhDenNgay { get; set; }
    }
    public class BenhAnPhuSanVM : BenhAnPhuSan
    {
        public new decimal? Idba { get; set; }
        public decimal? CanNangps { get; set; }
        public string MaBenhChinhVv { get; set; }
    }
    public class ThongTinBnVM : ThongTinBn
    {
        public new decimal? Idba { get; set; }
        public new string maDt { get; set; }
        public new byte gioiTinh {  get; set; }
    }
    public class BenhAnPtPhieuPtttVM : BenhanPhauThuatPhieuPttt
    {
        public new decimal? Idba { get; set; }
        public string ChanDoanTruocPt { get; set; }
        public string ChanDoanSauPt { get; set; }
    }

    public class ToBenhAnVM
    {
        [Required]
        public ToBenhAnBenhAnVM benhAn { get; set; }
        [Required]
        public BenhAnTongKetBenhAnVM BenhAnTongKetBenhAn { get; set; }
        public BenhAnKhamYhhdVM BenhAnKhamYhhd { get; set; }
        public BenhAnKhamYhctVM BenhAnKhamYhct { get; set; }
        [Required]
        public ToBenhAnBenhAnKhoaDieuTriVM BenhAnKhoaDieuTri { get; set; }
        [Required]
        public BenhAnTienSuBenhVM BenhAnTienSuBenh { get; set; }
        public BenhAnPhuSanVM BenhAnPhuSan { get; set; }
        public ThongTinBnVM thongTinBenhNhan { get; set; }
        public BenhAnPtPhieuPtttVM benhAnPhauthuatPhieuPttt { get; set; }
    }
}
