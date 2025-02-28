using Amazon.Runtime;
using Amazon.Runtime.Internal;
using AutoMapper;
using Medyx.ApiAssets.Models.Configure;
using Medyx_EMR_BCA.ApiAssets.Dto;
using Medyx_EMR_BCA.ApiAssets.Enums;
using Medyx_EMR_BCA.ApiAssets.Helpers;
using Medyx_EMR_BCA.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.Models.Session;
using Medyx_EMR_BCA.ApiAssets.QueryParameters;
using Medyx_EMR_BCA.ApiAssets.Repository;
using Medyx_EMR_BCA.ApiAssets.ResponseHandler;
using Medyx_EMR_BCA.ApiAssets.ViewModels;
using Medyx_EMR_BCA.Controllers.API.BenhAns;
using Medyx_EMR_BCA.Models;
using Medyx_EMR_BCA.Models.DanhMuc;
using Medyx_EMR_BCA.Models.DBConText;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Spire.Doc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.UI;
//using static Medyx_EMR_BCA.Controllers.API.BenhAns.MaBaStorage;

namespace Medyx_EMR_BCA.ApiAssets.Services
{
    public class BenhAnService
    {
        private IRepository<BenhAn> _benhAnRepository = null;
        private IRepository<ThongTinBn> _thongTinBenhNhanRepository = null;
        private IRepository<DmbenhVien> _dMBenhVien = null;
        private IRepository<DmdanToc> _dMDanToc = null;
        private IRepository<DmquocGia> _dMQuocGia = null;
        private IRepository<DmngheNghiep> _dMNgheNghiep = null;
        private IRepository<Dmtinh> _dMTinh = null;
        private IRepository<DmquanHuyen> _dMQuanHuyen = null;
        private IRepository<DmphuongXa> _dMPhuongXa = null;
        private IRepository<DmdoiTuong> _dMDoiTuong = null;
        private IRepository<DmkhoaBuong> _dMKhoaBuong = null;
        private IRepository<DmkhoaGiuong> _dMKhoaGiuong = null;
        private IRepository<DmbaLoaiBa> _dMBaLoaiBA = null;
        private IRepository<Dmkhoa> _dMKhoa = null;
        private IRepository<DmbenhTatYhct> _dMBenhTatYhct = null;
        private IRepository<DmbenhTat> _dMBenhTat = null;
        private IRepository<DmnhanVien> _dMNhanVien = null;
        private IRepository<BenhAnKhoaDieuTri> _KhoaDieuTriBA = null;
        private IRepository<BenhAnTienSuBenh> _benhAnTienSuBenhRepository = null;
        private PrintSetting PrintSetting { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;
        private IHttpContextAccessor _context { get; set; }
        public BenhAnService(IHostingEnvironment hostingEnvironment = null, IHttpContextAccessor context = null, IOptions<PrintSetting> options = null)
        {
            _benhAnRepository = new GenericRepository<BenhAn>(context);
            _thongTinBenhNhanRepository = new GenericRepository<ThongTinBn>(context);
            _benhAnTienSuBenhRepository = new GenericRepository<BenhAnTienSuBenh>(context);
            _dMBenhVien = new GenericRepository<DmbenhVien>(context);
            _dMDanToc = new GenericRepository<DmdanToc>(context);
            _dMQuocGia = new GenericRepository<DmquocGia>(context);
            _dMNgheNghiep = new GenericRepository<DmngheNghiep>(context);
            _dMTinh = new GenericRepository<Dmtinh>(context);
            _dMQuanHuyen = new GenericRepository<DmquanHuyen>(context);
            _dMPhuongXa = new GenericRepository<DmphuongXa>(context);
            _dMDoiTuong = new GenericRepository<DmdoiTuong>(context);
            _dMKhoaBuong = new GenericRepository<DmkhoaBuong>(context);
            _dMKhoaGiuong = new GenericRepository<DmkhoaGiuong>(context);
            _dMBaLoaiBA = new GenericRepository<DmbaLoaiBa>(context);
            _dMKhoa = new GenericRepository<Dmkhoa>(context);
            _dMBenhTat = new GenericRepository<DmbenhTat>(context);
            _dMNhanVien = new GenericRepository<DmnhanVien>(context);
            _KhoaDieuTriBA = new GenericRepository<BenhAnKhoaDieuTri>(context);
            _dMBenhTatYhct = new GenericRepository<DmbenhTatYhct>(context);



            _dMNgheNghiep = new GenericRepository<DmngheNghiep>(context);
            PrintSetting = options != null ? options.Value : new PrintSetting();
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IQueryable<BenhAnDto> Get(BenhAnParameters parameters, UserSession user = null)
        {
            HL7CoreDataDataContext db = new HL7CoreDataDataContext();
            List<DmkhoaDto> dskhoa = user.DMKhoaAcc.Select(ba => new DmkhoaDto()
            {
                MaKhoa = ba.MaKhoa,
                TenKhoa = ba.TenKhoa
            }).ToList();

            // Lấy dữ liệu khoa điều trị, tách biệt nhóm query để tránh lỗi 'expression already associated'

            // Lấy dữ liệu bệnh án
            var query = _benhAnRepository.Table
                .Include(x => x.ThongTinBn)
                .Include(x => x.DmbaLoaiBa)
                .Include(x => x.DmkhoaBuong)
                .Include(x => x.DmkhoaGiuong)
                .Include(x => x.ThongTinBn).ThenInclude(x => x.Dmtinh)
                .Include(x => x.ThongTinBn).ThenInclude(x => x.DmquanHuyen)
                .Include(x => x.ThongTinBn).ThenInclude(x => x.DmphuongXa)
                .Where(x => user.DMKhoaAcc.Contains(x.Dmkhoa))

                .AsQueryable();
            // Nếu người dùng không phải là admin, chỉ lấy những bản ghi không bị hủy
            if (user == null || !Convert.ToBoolean(user.Pub_bQadmin))
            {
                query = query.Where(x => !Convert.ToBoolean(x.Huy));
            }

            // Lọc và sắp xếp theo các tham số
            query = QueryFilter(query, parameters);
            query = SortHelper.ApplySort(query, parameters.SortBy);

            var benhAnWithMaxSTT = from ba in query
                                   join bakd in _KhoaDieuTriBA.Table on ba.Idba equals bakd.Idba into bakdGroup
                                   from bakd in bakdGroup.DefaultIfEmpty()
                                   where bakd.MaKhoa == ba.Dmkhoa.MaKhoa || ba.MaKhoaVv == ba.Dmkhoa.MaKhoa  // Thêm điều kiện JOIN với MaKhoa
                                   group bakd by ba.Idba into g
                                   select new
                                   {
                                       Idba = g.Key,
                                       MaxSTT = g.Max(bakd => bakd.Stt) // lấy STT max
                                   };
            var result = benhAnWithMaxSTT.ToList();
            var benhAnQuery = query
       .AsNoTracking()
       .Join(benhAnWithMaxSTT, ba => ba.Idba, bamax => bamax.Idba, (ba, bamax) => new BenhAnDto()
       {
           Idba = ba.Idba,
           SoVaoVien = ba.SoVaoVien,
           SoLuuTru = ba.SoLuuTru,
           MaBa = ba.MaBa,
           MaYt = ba.MaYt,
           TenDvcq = ba.TenDvcq,
           MaBv = ba.MaBv,
           TenBv = ba.TenBv,
           XacNhanLuuTru = ba.XacNhanLuuTru,
           NgayLuuTru = ba.NgayLuuTru,
           XacNhanKetThucHs = ba.XacNhanKetThucHs,
           NgayXacNhanKetThucHs = ba.NgayXacNhanKetThucHs,
           NgayVv = ba.NgayVv,
           NgayRv = ba.NgayRv,
           NgayLap = ba.NgayLap,
           NgayHuy = ba.NgayHuy,
           Huy = ba.Huy,
           MaBvChuyenDen = ba.MaBvChuyenDen,
           BenhNhan = new ThongTinBnDto()
           {
               MaBn = ba.ThongTinBn.MaBn,
               Idba = ba.ThongTinBn.Idba,
               HoTen = ba.ThongTinBn.HoTen,
               NgaySinh = ba.ThongTinBn.NgaySinh,
               Tuoi = ba.ThongTinBn.Tuoi,
               GioiTinh = ba.ThongTinBn.GioiTinh,
               SoNha = ba.ThongTinBn.SoNha,
               Thon = ba.ThongTinBn.Thon,
               Tinh = new DmtinhDto()
               {
                   MaTinh = ba.ThongTinBn.Dmtinh.MaTinh,
                   TenTinh = ba.ThongTinBn.Dmtinh.TenTinh
               },
               QuanHuyen = new DmquanHuyenDto()
               {
                   MaQh = ba.ThongTinBn.DmquanHuyen.MaQh,
                   TenQh = ba.ThongTinBn.DmquanHuyen.TenQh
               },
               PhuongXa = new DmphuongXaDto()
               {
                   MaPxa = ba.ThongTinBn.DmphuongXa.MaPxa,
               },
               QuocGia = new DmquocGiaDto()
               {
                   MaQg = ba.ThongTinBn.DmquocGia.MaQg,
                   TenQg = ba.ThongTinBn.DmquocGia.TenQg,
                   MaQL = ba.ThongTinBn.DmquocGia.MaQL,
               },
           },
           LoaiBenhAn = new DmbaLoaiBaDto()
           {
               MaLoaiBa = ba.DmbaLoaiBa.MaLoaiBa,
               TenLoaiBa = ba.DmbaLoaiBa.TenLoaiBa
           },
           Khoa = new DmkhoaDto()
           {
               MaKhoa = ba.Dmkhoa.MaKhoa,
               TenKhoa = ba.Dmkhoa.TenKhoa
           },
           Buong = new DmkhoaBuongDto()
           {
               MaBuong = ba.DmkhoaBuong.MaBuong,
               TenBuong = ba.DmkhoaBuong.TenBuong
           },
           Giuong = new DmkhoaGiuongDto()
           {
               MaGiuong = ba.DmkhoaGiuong.MaGiuong,
               TenGiuong = ba.DmkhoaGiuong.TenGiuong
           },
           NguoiHuy = new DmnhanVienDto()
           {
               MaNv = ba.DmNguoiHuy.MaNv,
               HoTen = ba.DmNguoiHuy.HoTen
           },
           NguoiLap = new DmnhanVienDto()
           {
               MaNv = ba.DmNguoiLap.MaNv,
               HoTen = ba.DmNguoiLap.HoTen
           },

       });

            return benhAnQuery;
        }



        public void SaveToDatabaseEMR(string idbahis, string Idbaemr, string LoaiGiayTo, string IDchuoi, string stt, string sttkhoa, string MaBA)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SpInsertURLDMTrangThaiKyEMR", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IDBA", idbahis);
                        command.Parameters.AddWithValue("@Idbaemr", Idbaemr);
                        command.Parameters.AddWithValue("@LoaiGiayTo", LoaiGiayTo);
                        command.Parameters.AddWithValue("@IDchuoi", IDchuoi);
                        command.Parameters.AddWithValue("@stt", stt);
                        command.Parameters.AddWithValue("@sttkhoa", sttkhoa);
                        command.Parameters.AddWithValue("@MaBA", MaBA);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle database error, log or throw as needed

            }
        }

        private IQueryable<BenhAn> QueryFilter(IQueryable<BenhAn> query, BenhAnParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(x => x.ThongTinBn.HoTen.Contains(parameters.Search) || x.MaBa.Contains(parameters.Search));
            }
            if (!string.IsNullOrEmpty(parameters.MaBa))
            {
                query = query.Where(x => x.MaBa.Contains(parameters.MaBa));
            }
            if (!string.IsNullOrEmpty(parameters.TenBenhNhan))
            {
                query = query.Where(x => x.ThongTinBn.HoTen.ToLower().Trim().Contains(parameters.TenBenhNhan.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(parameters.SoVaoVien))
            {
                query = query.Where(x => x.SoVaoVien.Contains(parameters.SoVaoVien));
            }
            if (parameters.Tuoi.HasValue)
            {
                query = query.Where(x => x.ThongTinBn.Tuoi == parameters.Tuoi);
            }
            if (parameters.GioiTinh.HasValue)
            {
                query = query.Where(x => x.ThongTinBn.GioiTinh == parameters.GioiTinh);
            }
            if (!string.IsNullOrEmpty(parameters.MaBv))
            {
                query = query.Where(x => x.MaBv.Contains(parameters.MaBv));
            }
            if (!string.IsNullOrEmpty(parameters.TenBv))
            {
                query = query.Where(x => x.TenBv.Contains(parameters.TenBv));
            }
            // if (parameters.ngayVaoVien.Any())
            // {
            // 	query = query.Where(x => DateTime.Compare((DateTime)parameters.ngayVaoVien[0], x.NgayVv) <= 0)
            // 		.Where(x => DateTime.Compare((DateTime)parameters.ngayVaoVien[1], x.NgayVv) >= 0);
            // }
            // if (parameters.ngayRaVien.Any())
            // {
            // 	query = query.Where(x => DateTime.Compare((DateTime)parameters.ngayRaVien[0], x.NgayVv) <= 0)
            // 		.Where(x => DateTime.Compare((DateTime)parameters.ngayRaVien[1], x.NgayVv) >= 0);
            // }
            if (parameters.TuNgayVaoVien.HasValue)
            {
                query = query.Where(x => DateTime.Compare((DateTime)parameters.TuNgayVaoVien, x.NgayVv) <= 0);
            }
            if (parameters.DenNgayVaoVien.HasValue)
            {
                query = query.Where(x => DateTime.Compare((DateTime)parameters.DenNgayVaoVien, x.NgayVv) >= 0);
            }
            if (parameters.TuNgayRaVien.HasValue)
            {
                query = query.Where(x => DateTime.Compare((DateTime)parameters.TuNgayRaVien, x.NgayVv) <= 0);
            }
            if (parameters.DenNgayRaVien.HasValue)
            {
                query = query.Where(x => DateTime.Compare((DateTime)parameters.DenNgayRaVien, x.NgayVv) >= 0);
            }

            if (parameters.MaKhoa.Any())
            {
                query = query.Where(x => parameters.MaKhoa.Any(mk => mk.Trim() == x.Dmkhoa.MaKhoa));
            }
            if (parameters.LoaiBenhAn.Any())
            {
                query = query.Where(x => parameters.LoaiBenhAn.Any(lba => lba.Trim() == x.DmbaLoaiBa.MaLoaiBa.ToString()));
            }
            if (parameters.XacNhanLuuTru.HasValue)
            {
                query = query.Where(x => x.XacNhanLuuTru == parameters.XacNhanLuuTru);
            }
            if (parameters.XacNhanKetThucHs.HasValue)
            {
                query = query.Where(x => x.XacNhanKetThucHs == parameters.XacNhanKetThucHs);
            }
            return query;
        }

        //public DetailBenhAnDto DetaiBenhAn(decimal idba)
        //{
        //    try
        //    {
        //        var query = (from benhAn in _benhAnRepository.Table.Where(x => x.Idba == idba)
        //                     join thongTinBenhNhan in _thongTinBenhNhanRepository.Table on benhAn.MaBn equals thongTinBenhNhan.MaBn

        //                     join benhVienChuyenDen in _dMBenhVien.Table on benhAn.MaBvChuyenDen equals benhVienChuyenDen.MaBv
        //                     into bvChuyenDenGroup
        //                     from benhVienChuyenDenInfo in bvChuyenDenGroup.DefaultIfEmpty()

        //                     join ngheNghiep in _dMNgheNghiep.Table on thongTinBenhNhan.MaNgheNghiep equals ngheNghiep.MaNn
        //                     into NgheNghiepGroup
        //                     from ngheNghiepInfo in NgheNghiepGroup.DefaultIfEmpty()
        //                     join danToc in _dMDanToc.Table on thongTinBenhNhan.MaDanToc equals danToc.MaDanToc
        //                     into DanTocGroup
        //                     from DanTocInfo in DanTocGroup.DefaultIfEmpty()

        //                     join quocGia in _dMQuocGia.Table on thongTinBenhNhan.MaQuocTich equals quocGia.MaQg
        //                     into QuocGiaGroup
        //                     from QuocGiaInfo in QuocGiaGroup.DefaultIfEmpty()

        //                     join tinh in _dMTinh.Table on thongTinBenhNhan.MaTinh equals tinh.MaTinh
        //                     into TinhGroup
        //                     from TinhInfo in TinhGroup.DefaultIfEmpty()

        //                     join huyen in _dMQuanHuyen.Table on thongTinBenhNhan.MaHuyen equals huyen.MaQh
        //                     into huyenGroup
        //                     from HuyenInfo in huyenGroup.DefaultIfEmpty()

        //                     join xa in _dMPhuongXa.Table on thongTinBenhNhan.MaPxa equals xa.MaPxa
        //                     into xaGroup
        //                     from XaInfo in xaGroup.DefaultIfEmpty()

        //                     join doiTuong in _dMDoiTuong.Table on thongTinBenhNhan.DoiTuong equals doiTuong.MaDt
        //                     into DoiTuongGroup
        //                     from DoiTuongInfo in DoiTuongGroup.DefaultIfEmpty()

        //                     join khoaBuong in _dMKhoaBuong.Table on benhAn.Buong equals khoaBuong.MaBuong
        //                     into KhoaBuongGroup
        //                     from KhoaBuongInfo in KhoaBuongGroup.DefaultIfEmpty()

        //                     join giuong in _dMKhoaGiuong.Table on benhAn.Giuong equals giuong.MaGiuong
        //                     into GiuongGroup
        //                     from GiuongInfo in GiuongGroup.DefaultIfEmpty()

        //                     join loaiBA in _dMBaLoaiBA.Table on benhAn.LoaiBa equals loaiBA.MaLoaiBa
        //                     into LoaiBAGroup
        //                     from LoaiBAInfo in LoaiBAGroup.DefaultIfEmpty()

        //                     join khoa in _dMKhoa.Table on benhAn.MaKhoaVv equals khoa.MaKhoa
        //                     into KhoaGroup
        //                     from KhoaInfo in KhoaGroup.DefaultIfEmpty()

        //                     join benhKKBhyhhd in _dMBenhTat.Table on benhAn.MaBenhKkbyhhd equals benhKKBhyhhd.MaBenh
        //                     into benhKKbhhyGroup
        //                     from benhKKbhhyInfo in benhKKbhhyGroup.DefaultIfEmpty()

        //                     join benhTatNoiCD in _dMBenhTat.Table on benhAn.MaBenhKkbyhhd equals benhTatNoiCD.MaBenh
        //                     into benhTatNCDGroup
        //                     from BenhTatNCDInfo in benhTatNCDGroup.DefaultIfEmpty()

        //                     join benhTatNCDYhct in _dMBenhTatYhct.Table on benhAn.MaBenhKkbyhct equals benhTatNCDYhct.MaBenh
        //                     into benhTatNCDYhtcGroup
        //                     from BenhTatNCDYHCTInfo in benhTatNCDYhtcGroup.DefaultIfEmpty()

        //                     join benhkKBhyhct in _dMBenhTatYhct.Table on benhAn.MaBenhKkbyhct equals benhkKBhyhct.MaBenh
        //                     into benhKKBhytcGroup
        //                     from benhKKhytcInfo in benhKKBhytcGroup.DefaultIfEmpty()

        //                     join giamDoc in _dMNhanVien.Table on benhAn.GiamDoc equals giamDoc.MaNv
        //                     into GiamDocGroup
        //                     from GiamDocInfo in GiamDocGroup.DefaultIfEmpty()

        //                     join truongKhoa in _dMNhanVien.Table on benhAn.TruongKhoa equals truongKhoa.MaNv
        //                     into TruongKhoaGroup
        //                     from TruongKhoaInfo in TruongKhoaGroup.DefaultIfEmpty()

        //                     join bsDieuTri in _dMNhanVien.Table on benhAn.BsdieuTri equals bsDieuTri.MaNv
        //                     into bsDieuTriGroup
        //                     from bsDieuTriInfo in bsDieuTriGroup.DefaultIfEmpty()

        //                     join benhGPTuthi in _dMBenhTat.Table on benhAn.MaBenhGptuThi equals benhGPTuthi.MaBenh
        //                     into benhGPTuthiGroup
        //                     from benhGPTuThiInfo in benhGPTuthiGroup.DefaultIfEmpty()
        //                     select new DetailBenhAnDto
        //                     {
        //                         BenhAn = benhAn,
        //                         BenhNhan = thongTinBenhNhan,
        //                         benhVienChuyenDen = benhVienChuyenDenInfo,
        //                         NgheNghiep = ngheNghiepInfo,
        //                         DanToc = DanTocInfo,
        //                         QuocGia = QuocGiaInfo,
        //                         Tinh = TinhInfo,
        //                         QuanHuyen = HuyenInfo,
        //                         PhuongXa = XaInfo,
        //                         DoiTuong = DoiTuongInfo,
        //                         Buong = KhoaBuongInfo,
        //                         Giuong = GiuongInfo,
        //                         LoaiBenhAn = LoaiBAInfo,
        //                         Khoa = KhoaInfo,
        //                         BenhKKBYHHD = benhKKbhhyInfo,
        //                         BenhKKBYHCT = benhKKhytcInfo,
        //                         BenhTatNoiChuyenDen = BenhTatNCDInfo,
        //                         BenhNoiChuyenDenYHCT = BenhTatNCDYHCTInfo,
        //                         GiamDoc = GiamDocInfo,
        //                         TruongKhoa = TruongKhoaInfo,
        //                         BsDieutri = bsDieuTriInfo,
        //                         BenhGPTuThis = benhGPTuThiInfo

        //                     }).FirstOrDefault(x => x.BenhAn.Idba == idba);

        //        if (query == null)
        //        {
        //            throw new Exception("Không có dữ liệu");
        //        }
        //        return query;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public DetailBenhAnDto DetaiBenhAn(decimal idba)
        {
            try
            {
                var oBenhan = new DetailBenhAnDto();
                DataTable infoBenhAn = GetInforBenhAn(idba);
                DateTime date;
                if (infoBenhAn.Rows.Count > 0)
                {
                    oBenhan.BenhAn.Idba = decimal.Parse(infoBenhAn.Rows[0]["IDBA"].ToString());
                    oBenhan.BenhAn.MaBa = infoBenhAn.Rows[0]["MaBA"].ToString();
                    oBenhan.BenhAn.TenDvcq = infoBenhAn.Rows[0]["TenDVCQ"].ToString();
                    oBenhan.BenhAn.MaBv = infoBenhAn.Rows[0]["MaBV"].ToString();
                    oBenhan.benhVienChuyenDen.TenBv = infoBenhAn.Rows[0]["TenBV"].ToString();
                    oBenhan.Buong.TenBuong = infoBenhAn.Rows[0]["TenBuong"].ToString();
                    oBenhan.Giuong.TenGiuong = infoBenhAn.Rows[0]["TenGiuong"].ToString();
                    oBenhan.BenhAn.SoVaoVien = infoBenhAn.Rows[0]["SoVaoVien"].ToString();
                    oBenhan.BenhAn.SoLuuTru = infoBenhAn.Rows[0]["SoLuuTru"].ToString();
                    oBenhan.BenhNhan.MaBn = infoBenhAn.Rows[0]["MaBN"].ToString();
                    oBenhan.BenhAn.MaYt = infoBenhAn.Rows[0]["MaYT"].ToString();
                    oBenhan.BenhNhan.HoTen = infoBenhAn.Rows[0]["HoTen"].ToString();
                    oBenhan.DanToc.TenDanToc = infoBenhAn.Rows[0]["TenDanToc"].ToString();
                    oBenhan.DanToc.MaDanToc = infoBenhAn.Rows[0]["MaDanToc"].ToString();
                    oBenhan.NgheNghiep.MaNn = infoBenhAn.Rows[0]["MaNN"].ToString();
                    oBenhan.NgheNghiep.TenNn = infoBenhAn.Rows[0]["TenNN"].ToString();
                    oBenhan.QuocGia.TenQg = infoBenhAn.Rows[0]["TenQG"].ToString();
                    oBenhan.QuocGia.MaQL = infoBenhAn.Rows[0]["MaQL"].ToString();
                    oBenhan.BenhNhan.SoNha = infoBenhAn.Rows[0]["SoNha"].ToString();
                    oBenhan.BenhNhan.Thon = infoBenhAn.Rows[0]["Thon"].ToString();
                    oBenhan.PhuongXa.TenPxa = infoBenhAn.Rows[0]["TenPXa"].ToString();
                    oBenhan.PhuongXa.MaBhxh = infoBenhAn.Rows[0]["MaXa"].ToString();
                    oBenhan.QuanHuyen.MaQh = infoBenhAn.Rows[0]["MaQH"].ToString();
                    oBenhan.QuanHuyen.MaBhxh = infoBenhAn.Rows[0]["MaHuyen"].ToString();
                    oBenhan.QuanHuyen.TenQh = infoBenhAn.Rows[0]["TenQH"].ToString();
                    oBenhan.Tinh.TenTinh = infoBenhAn.Rows[0]["TenTinh"].ToString();
                    oBenhan.Tinh.MaBhyt = infoBenhAn.Rows[0]["MaTinh"].ToString();
                    oBenhan.BenhNhan.NoiLamViec = infoBenhAn.Rows[0]["NoiLamViec"].ToString();
                    if (DateTime.TryParse(infoBenhAn.Rows[0]["GTBHYTDN"].ToString(), out date))
                    {
                        oBenhan.BenhNhan.Gtbhytdn = date;
                    }
                    if (DateTime.TryParse(infoBenhAn.Rows[0]["GTBHYTTN"].ToString(), out date))
                    {
                        oBenhan.BenhNhan.Gtbhyttn = date;
                    }

                    oBenhan.BenhNhan.LienHe = infoBenhAn.Rows[0]["LienHe"].ToString();
                    oBenhan.BenhNhan.SoDienThoai = infoBenhAn.Rows[0]["SoDienThoai"].ToString();
                    oBenhan.BenhNhan.GioiTinh = byte.Parse(infoBenhAn.Rows[0]["GioiTinh"].ToString());
                    if (DateTime.TryParse(infoBenhAn.Rows[0]["NgayVV"].ToString(), out date))
                    {
                        oBenhan.BenhAn.NgayVv = date;
                    }
                    if (DateTime.TryParse(infoBenhAn.Rows[0]["NgayRV"].ToString(), out date))
                    {
                        oBenhan.BenhAn.NgayRv = date;
                    }

                    oBenhan.BenhTatNoiChuyenDen.TenBenh = infoBenhAn.Rows[0]["TenBenh"].ToString();
                    oBenhan.BenhAn.TenBenhChinhVv = infoBenhAn.Rows[0]["TenBenhChinhVV"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv1 = infoBenhAn.Rows[0]["TenBenhKemVv1"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv2 = infoBenhAn.Rows[0]["TenBenhKemVv2"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv3 = infoBenhAn.Rows[0]["TenBenhKemVv3"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv4 = infoBenhAn.Rows[0]["TenBenhKemVv4"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv5 = infoBenhAn.Rows[0]["TenBenhKemVv5"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv6 = infoBenhAn.Rows[0]["TenBenhKemVv6"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv7 = infoBenhAn.Rows[0]["TenBenhKemVv7"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv8 = infoBenhAn.Rows[0]["TenBenhKemVv8"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv9 = infoBenhAn.Rows[0]["TenBenhKemVv9"].ToString();
                    oBenhan.BenhAn.MaBenhChinhVv = infoBenhAn.Rows[0]["MaBenhChinhVv"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv1 = infoBenhAn.Rows[0]["MaBenhKemVV1"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv2 = infoBenhAn.Rows[0]["MaBenhKemVV2"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv3 = infoBenhAn.Rows[0]["MaBenhKemVV3"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv4 = infoBenhAn.Rows[0]["MaBenhKemVV4"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv5 = infoBenhAn.Rows[0]["MaBenhKemVV5"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv6 = infoBenhAn.Rows[0]["MaBenhKemVV6"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv7 = infoBenhAn.Rows[0]["MaBenhKemVV7"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv8 = infoBenhAn.Rows[0]["MaBenhKemVV8"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv9 = infoBenhAn.Rows[0]["MaBenhKemVV9"].ToString();
                    oBenhan.BenhAn.TenBenhChinhRv = infoBenhAn.Rows[0]["TenBenhChinhRV"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv1 = infoBenhAn.Rows[0]["TenBenhKemRv1"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv2 = infoBenhAn.Rows[0]["TenBenhKemRv2"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv3 = infoBenhAn.Rows[0]["TenBenhKemRv3"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv4 = infoBenhAn.Rows[0]["TenBenhKemRv4"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv5 = infoBenhAn.Rows[0]["TenBenhKemRv5"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv6 = infoBenhAn.Rows[0]["TenBenhKemRv6"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv7 = infoBenhAn.Rows[0]["TenBenhKemRv7"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv8 = infoBenhAn.Rows[0]["TenBenhKemRv8"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv9 = infoBenhAn.Rows[0]["TenBenhKemRv9"].ToString();
                    oBenhan.BenhAn.MaBenhChinhRv = infoBenhAn.Rows[0]["MaBenhChinhRV"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv1 = infoBenhAn.Rows[0]["MaBenhKemRV1"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv2 = infoBenhAn.Rows[0]["MaBenhKemRV2"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv3 = infoBenhAn.Rows[0]["MaBenhKemRV3"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv4 = infoBenhAn.Rows[0]["MaBenhKemRV4"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv5 = infoBenhAn.Rows[0]["MaBenhKemRV5"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv6 = infoBenhAn.Rows[0]["MaBenhKemRV6"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv7 = infoBenhAn.Rows[0]["MaBenhKemRV7"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv8 = infoBenhAn.Rows[0]["MaBenhKemRV8"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv9 = infoBenhAn.Rows[0]["MaBenhKemRV9"].ToString();
                    oBenhan.BenhAn.TenBenhChinhVvyhct = infoBenhAn.Rows[0]["TenBenhChinhVVYHCT"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv1yhct = infoBenhAn.Rows[0]["TenBenhKemVv1yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv2yhct = infoBenhAn.Rows[0]["TenBenhKemVv2yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv3yhct = infoBenhAn.Rows[0]["TenBenhKemVv3yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv4yhct = infoBenhAn.Rows[0]["TenBenhKemVv4yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv5yhct = infoBenhAn.Rows[0]["TenBenhKemVv5yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv6yhct = infoBenhAn.Rows[0]["TenBenhKemVv6yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv7yhct = infoBenhAn.Rows[0]["TenBenhKemVv7yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv8yhct = infoBenhAn.Rows[0]["TenBenhKemVv8yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemVv9yhct = infoBenhAn.Rows[0]["TenBenhKemVv9yhct"].ToString();
                    oBenhan.BenhAn.MaBenhChinhVvyhct = infoBenhAn.Rows[0]["MaBenhChinhVVYHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv1yhct = infoBenhAn.Rows[0]["MaBenhKemVV1YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv2yhct = infoBenhAn.Rows[0]["MaBenhKemVV2YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv3yhct = infoBenhAn.Rows[0]["MaBenhKemVV3YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv4yhct = infoBenhAn.Rows[0]["MaBenhKemVV4YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv5yhct = infoBenhAn.Rows[0]["MaBenhKemVV5YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv6yhct = infoBenhAn.Rows[0]["MaBenhKemVV6YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv7yhct = infoBenhAn.Rows[0]["MaBenhKemVV7YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv8yhct = infoBenhAn.Rows[0]["MaBenhKemVV8YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemVv9yhct = infoBenhAn.Rows[0]["MaBenhKemVV9YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhChinhRvyhct = infoBenhAn.Rows[0]["MaBenhChinhRVYHCT"].ToString();
                    oBenhan.BenhAn.TenBenhChinhRvyhct = infoBenhAn.Rows[0]["TenBenhChinhRVYHCT"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv1yhct = infoBenhAn.Rows[0]["TenBenhKemRv1yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv2yhct = infoBenhAn.Rows[0]["TenBenhKemRv2yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv3yhct = infoBenhAn.Rows[0]["TenBenhKemRv3yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv4yhct = infoBenhAn.Rows[0]["TenBenhKemRv4yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv5yhct = infoBenhAn.Rows[0]["TenBenhKemRv5yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv6yhct = infoBenhAn.Rows[0]["TenBenhKemRv6yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv7yhct = infoBenhAn.Rows[0]["TenBenhKemRv7yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv8yhct = infoBenhAn.Rows[0]["TenBenhKemRv8yhct"].ToString();
                    oBenhan.BenhAn.TenBenhKemRv9yhct = infoBenhAn.Rows[0]["TenBenhKemRv9yhct"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv1yhct = infoBenhAn.Rows[0]["MaBenhKemRV1YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv2yhct = infoBenhAn.Rows[0]["MaBenhKemRV2YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv3yhct = infoBenhAn.Rows[0]["MaBenhKemRV3YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv4yhct = infoBenhAn.Rows[0]["MaBenhKemRV4YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv5yhct = infoBenhAn.Rows[0]["MaBenhKemRV5YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv6yhct = infoBenhAn.Rows[0]["MaBenhKemRV6YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv7yhct = infoBenhAn.Rows[0]["MaBenhKemRV7YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv8yhct = infoBenhAn.Rows[0]["MaBenhKemRV8YHCT"].ToString();
                    oBenhan.BenhAn.MaBenhKemRv9yhct = infoBenhAn.Rows[0]["MaBenhKemRV9YHCT"].ToString();
                    oBenhan.BenhAn.ThuThuatYhct = byte.Parse(infoBenhAn.Rows[0]["ThuThuatYHCT"].ToString());
                    oBenhan.BenhAn.PhauThuatYhct = byte.Parse(infoBenhAn.Rows[0]["PhauThuatYHCT"].ToString());
                    oBenhan.BenhAn.ThuThuatYhhd = byte.Parse(infoBenhAn.Rows[0]["ThuThuatYHHD"].ToString());
                    oBenhan.BenhAn.PhauThuatYhhd = byte.Parse(infoBenhAn.Rows[0]["PhauThuatYHHD"].ToString());
                    oBenhan.BenhNoiChuyenDenYHCT.TenBenhIcd = infoBenhAn.Rows[0]["TenBenhICD"].ToString();
                    oBenhan.GiamDoc.HoTen = infoBenhAn.Rows[0]["GiamDoc"].ToString();
                    oBenhan.TruongKhoa.HoTen = infoBenhAn.Rows[0]["TruongKhoa"].ToString();
                    oBenhan.BsDieutri.HoTen = infoBenhAn.Rows[0]["BsiDieuTri"].ToString();
                    oBenhan.BenhAn.Vvlan = int.Parse(infoBenhAn.Rows[0]["VVLan"].ToString());
                    if (DateTime.TryParse(infoBenhAn.Rows[0]["NgayKy"].ToString(), out date))
                    {
                        oBenhan.BenhAn.NgayKy = date;
                    }
                    oBenhan.BenhKKBYHCT.TenBenhBhyt = infoBenhAn.Rows[0]["TenBenhBHYT"].ToString();
                    oBenhan.BenhKKBYHHD.TenBenh = infoBenhAn.Rows[0]["TenBenhKKBHYHHD"].ToString();
                    oBenhan.BenhNhan.Cmnd = infoBenhAn.Rows[0]["CMND"].ToString();
                    oBenhan.BenhNhan.SoTheBhyt = infoBenhAn.Rows[0]["SoTheBHYT"].ToString();
                    oBenhan.NgheNghiep.MaNn = infoBenhAn.Rows[0]["MaNN"].ToString();
                    if (DateTime.TryParse(infoBenhAn.Rows[0]["NgaySinh"].ToString(), out date))
                    {
                        oBenhan.BenhNhan.NgaySinh = date;
                    }
                    oBenhan.BenhNhan.Tuoi = byte.Parse(infoBenhAn.Rows[0]["Tuoi"].ToString());
                    oBenhan.BenhAn.TongSoNgayDt = int.Parse(infoBenhAn.Rows[0]["TongSoNgayDT"].ToString());
                    oBenhan.DoiTuong.MaDt = infoBenhAn.Rows[0]["MaDT"].ToString();
                    oBenhan.BenhAn.TrucTiepVao = infoBenhAn.Rows[0]["TrucTiepVao"].ToString();
                    oBenhan.BenhAn.NoiGt = infoBenhAn.Rows[0]["NoiGT"].ToString();
                    oBenhan.BenhAn.ChuyenVien = infoBenhAn.Rows[0]["ChuyenVien"].ToString();
                    oBenhan.BenhAn.HtraVien = infoBenhAn.Rows[0]["HTRaVien"].ToString();
                    oBenhan.BenhTatNoiChuyenDen.MaBenh = infoBenhAn.Rows[0]["MaBenhTatNoiCD"].ToString();
                    oBenhan.BenhKKBYHHD.MaBenh = infoBenhAn.Rows[0]["MaBenhKKBHYHHD"].ToString();
                    oBenhan.BenhNoiChuyenDenYHCT.MaBenh = infoBenhAn.Rows[0]["MaBenhNCDYHCT"].ToString();
                    oBenhan.BenhKKBYHCT.MaBenh = infoBenhAn.Rows[0]["MaBenhKKBHYHCT"].ToString();
                    oBenhan.BenhAn.TaiBienYhct = byte.Parse(infoBenhAn.Rows[0]["TaiBienYHCT"].ToString());
                    oBenhan.BenhAn.TaiBienYhhd = byte.Parse(infoBenhAn.Rows[0]["TaiBienYHHD"].ToString());
                    oBenhan.BenhAn.BienChungYhct = byte.Parse(infoBenhAn.Rows[0]["BienChungYHCT"].ToString());
                    oBenhan.BenhAn.BienChungYhhd = byte.Parse(infoBenhAn.Rows[0]["BienChungYHHD"].ToString());
                    oBenhan.BenhAn.Kqdt = infoBenhAn.Rows[0]["KQDT"].ToString();
                    oBenhan.BenhAn.GiaiPhauBenh = infoBenhAn.Rows[0]["GiaiPhauBenh"].ToString();
                    oBenhan.BenhAn.TinhTrangTuVong = infoBenhAn.Rows[0]["TinhTrangTuVong"].ToString();
                    oBenhan.BenhGPTuThis.MaBenh = infoBenhAn.Rows[0]["MaBenhChuanDoanGiaiPhau"].ToString();
                    oBenhan.LoaiBenhAn.MaLoaiBa = byte.Parse(infoBenhAn.Rows[0]["MaLoaiBA"].ToString());


                }
                else
                {
                    throw new Exception("Không có dữ liệu");
                }
                return oBenhan;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public DataTable GetInforBenhAn(decimal Idba)
        {
            DataTable dr = new DataTable();
            string tenStore = "sp_GetInforBenhAn";
            string StrConection = ConfigurationManager.ConnectionStrings["Medyx_EMR_BCA_SQLConnectionString"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            using (SqlConnection Conection = new SqlConnection(StrConection))
            {
                Conection.Open();
                using (SqlCommand Command = new SqlCommand(tenStore, Conection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.Add(new SqlParameter("@IDBA", Idba));
                    SqlDataAdapter dp = new SqlDataAdapter(Command);
                    dp.Fill(dr);
                }
                return dr;
            }
        }

        public BenhAnDetailDto Detail(decimal id)
        {
            try
            {
                string mss = "";
                var query = _benhAnRepository.Table
                .Where(x => x.Idba == id)
                .AsQueryable();
                var queryDieuTri = _KhoaDieuTriBA.Table.Where(x => x.Idba == id).OrderByDescending(x => x.Stt);
                var dieutri = queryDieuTri.FirstOrDefault();
                var queryBenhAnDetail = query.Select(benhAn => new BenhAnDetailDto()
                {
                    //benhAnVM = benhAn,
                    Idba = benhAn.Idba,
                    MaBa = benhAn.MaBa,
                    TenDvcq = benhAn.TenDvcq,
                    MaBv = benhAn.MaBv,
                    TenBv = benhAn.TenBv,
                    SoVaoVien = benhAn.SoVaoVien,
                    SoLuuTru = benhAn.SoLuuTru,
                    MaYt = benhAn.MaYt,
                    Huy = benhAn.Huy,
                    NgayVv = benhAn.NgayVv,
                    NgayRv = benhAn.NgayRv,
                    TrucTiepVao = benhAn.TrucTiepVao,
                    NoiGt = benhAn.NoiGt,
                    ChuyenVien = benhAn.ChuyenVien,
                    MaBvChuyenDen = benhAn.MaBvChuyenDen,
                    HtraVien = benhAn.HtraVien,
                    NguyenNhanTBBC = benhAn.NguyenNhanTBBC,
                    TongSoNgayDt = benhAn.TongSoNgayDt,
                    MaBenhChinhVv = benhAn.MaBenhChinhVv,
                    TenBenhChinhVv = benhAn.TenBenhChinhVv,
                    ThuThuatYhhd = benhAn.ThuThuatYhhd,
                    PhauThuatYhhd = benhAn.PhauThuatYhhd,
                    MaBenhChinhRv = benhAn.MaBenhChinhRv,
                    TenBenhChinhRv = benhAn.TenBenhChinhRv,
                    TaiBienYhhd = benhAn.TaiBienYhhd,
                    BienChungYhhd = benhAn.BienChungYhhd,

                    MaBenhChinhVvyhct = benhAn.MaBenhChinhVvyhct,
                    TenBenhChinhVvyhct = benhAn.TenBenhChinhVvyhct,
                    MaBenhChinhRvyhct = benhAn.MaBenhChinhRvyhct,
                    TenBenhChinhRvyhct = benhAn.TenBenhChinhRvyhct,
                    ThuThuatYhct = benhAn.ThuThuatYhct,
                    PhauThuatYhct = benhAn.PhauThuatYhct,
                    TaiBienYhct = benhAn.TaiBienYhct,
                    BienChungYhct = benhAn.BienChungYhct,
                    Kqdt = benhAn.Kqdt,
                    GiaiPhauBenh = benhAn.GiaiPhauBenh,
                    NgayTuVong = benhAn.NgayTuVong,
                    NguyenNhanTuVong = benhAn.NguyenNhanTuVong,
                    KhamNghiemTuThi = benhAn.KhamNghiemTuThi,
                    NgayKy = benhAn.NgayKy,
                    NgayTruongKhoaKy = benhAn.NgayTruongKhoaKy,
                    TinhTrangTuVong = benhAn.TinhTrangTuVong,
                    TongSoLanPt = benhAn.TongSoLanPt,
                    Vvlan = benhAn.Vvlan,
                    TongSoNgayDtsauPt = benhAn.TongSoNgayDtsauPt,
                    benhVienChuyenDen = new DmbenhVienDto()
                    {
                        MaBv = benhAn.DmbenhVien.MaBv == null ? "" : benhAn.DmbenhVien.MaBv,
                        TenBv = benhAn.DmbenhVien.TenBv == null ? "" : benhAn.DmbenhVien.TenBv
                    },
                    BenhNhan = new BenhAnDetailThongTinBnDto()
                    {

                        MaBn = benhAn.ThongTinBn.MaBn,
                        Idba = benhAn.ThongTinBn.Idba,
                        HoTen = benhAn.ThongTinBn.HoTen,
                        NgaySinh = benhAn.ThongTinBn.NgaySinh,
                        Tuoi = benhAn.ThongTinBn.Tuoi,
                        GioiTinh = benhAn.ThongTinBn.GioiTinh,
                        SoNha = benhAn.ThongTinBn.SoNha,
                        Thon = benhAn.ThongTinBn.Thon,
                        NoiLamViec = benhAn.ThongTinBn.NoiLamViec,
                        Gtbhytdn = benhAn.ThongTinBn.Gtbhytdn,
                        Gtbhyttn = benhAn.ThongTinBn.Gtbhyttn,
                        SoTheBhyt = benhAn.ThongTinBn.SoTheBhyt,
                        LienHe = benhAn.ThongTinBn.LienHe,
                        SoDienThoai = benhAn.ThongTinBn.SoDienThoai,
                        HoTenCha = benhAn.ThongTinBn.HoTenCha,
                        HoTenMe = benhAn.ThongTinBn.HoTenMe,
                        NguoiGiamHo = benhAn.ThongTinBn.NguoiGiamHo,
                        GiayCnkhuyetTat = benhAn.ThongTinBn.GiayCnkhuyetTat,
                        DangKhuyetTat = benhAn.ThongTinBn.DangKhuyetTat,
                        MucDoKhuyetTat = benhAn.ThongTinBn.MucDoKhuyetTat,
                        NgheNghiepNguoiGiamHo = benhAn.ThongTinBn.NgheNghiepNguoiGiamHo,
                        QuanHeNguoiGiamHo = benhAn.ThongTinBn.QuanHeNguoiGiamHo,
                        Cmnd = benhAn.ThongTinBn.Cmnd,
                        NoiCapCmnd = benhAn.ThongTinBn.NoiCapCmnd,
                        NgayCapCmnd = benhAn.ThongTinBn.NgayCapCmnd,
                        NgheNghiep = new DmngheNghiepDto()
                        {
                            MaNn = benhAn.ThongTinBn.DmngheNghiep.MaNn == null ? "" : benhAn.ThongTinBn.DmngheNghiep.MaNn,
                            TenNn = benhAn.ThongTinBn.DmngheNghiep.TenNn == null ? "" : benhAn.ThongTinBn.DmngheNghiep.TenNn
                        },
                        DanToc = new DmdanTocDto()
                        {
                            MaDanToc = benhAn.ThongTinBn.DmdanToc.MaDanToc == null ? "" : benhAn.ThongTinBn.DmdanToc.MaDanToc,
                            TenDanToc = benhAn.ThongTinBn.DmdanToc.TenDanToc == null ? "" : benhAn.ThongTinBn.DmdanToc.TenDanToc,
                            MaQl = benhAn.ThongTinBn.DmdanToc.MaQl
                        },
                        QuocGia = new DmquocGiaDto()
                        {
                            MaQg = benhAn.ThongTinBn.DmquocGia.MaQg == null ? "" : benhAn.ThongTinBn.DmquocGia.MaQg,
                            TenQg = benhAn.ThongTinBn.DmquocGia.TenQg == null ? "" : benhAn.ThongTinBn.DmquocGia.TenQg,
                            MaQL = benhAn.ThongTinBn.DmquocGia.MaQL == null ? "" : benhAn.ThongTinBn.DmquocGia.MaQL
                        },
                        Tinh = new DmtinhDto()
                        {
                            MaTinh = benhAn.ThongTinBn.Dmtinh.MaTinh == null ? "" : benhAn.ThongTinBn.Dmtinh.MaTinh,
                            TenTinh = benhAn.ThongTinBn.Dmtinh.TenTinh == null ? "" : benhAn.ThongTinBn.Dmtinh.TenTinh,
                            MaBHYT = benhAn.ThongTinBn.Dmtinh.MaBhyt == null ? "" : benhAn.ThongTinBn.Dmtinh.MaBhyt
                        },
                        QuanHuyen = new DmquanHuyenDto()
                        {
                            MaQh = benhAn.ThongTinBn.DmquanHuyen.MaQh == null ? "" : benhAn.ThongTinBn.DmquanHuyen.MaQh,
                            TenQh = benhAn.ThongTinBn.DmquanHuyen.TenQh == null ? "" : benhAn.ThongTinBn.DmquanHuyen.TenQh,
                            MaBhxh = benhAn.ThongTinBn.DmquanHuyen.MaBhxh == null ? "" : benhAn.ThongTinBn.DmquanHuyen.MaBhxh
                        },
                        PhuongXa = new DmphuongXaDto()
                        {
                            MaPxa = benhAn.ThongTinBn.DmphuongXa.MaPxa == null ? "" : benhAn.ThongTinBn.DmphuongXa.MaPxa,
                            TenPxa = benhAn.ThongTinBn.DmphuongXa.TenPxa == null ? "" : benhAn.ThongTinBn.DmphuongXa.TenPxa,
                            MaBhxh = benhAn.ThongTinBn.DmphuongXa.MaBhxh == null ? "" : benhAn.ThongTinBn.DmphuongXa.MaBhxh
                        },
                        DoiTuong = new DmdoiTuongDto()
                        {
                            MaDt = benhAn.ThongTinBn.DmdoiTuong.MaDt == null ? "" : benhAn.ThongTinBn.DmdoiTuong.MaDt,
                            TenDt = benhAn.ThongTinBn.DmdoiTuong.TenDt == null ? "" : benhAn.ThongTinBn.DmdoiTuong.TenDt
                        }
                    },
                    Buong = new DmkhoaBuongDto()
                    {
                        MaBuong = benhAn.DmkhoaBuong.MaBuong == null ? "" : benhAn.DmkhoaBuong.MaBuong,
                        TenBuong = benhAn.DmkhoaBuong.TenBuong == null ? "" : benhAn.DmkhoaBuong.TenBuong
                    },
                    Giuong = new DmkhoaGiuongDto()
                    {
                        MaGiuong = benhAn.DmkhoaGiuong.MaGiuong == null ? "" : benhAn.DmkhoaGiuong.MaGiuong,
                        TenGiuong = benhAn.DmkhoaGiuong.TenGiuong == null ? "" : benhAn.DmkhoaGiuong.TenGiuong
                    },
                    LoaiBenhAn = new DmbaLoaiBaDto()
                    {
                        MaLoaiBa = benhAn.DmbaLoaiBa.MaLoaiBa == null ? (byte)0 : benhAn.DmbaLoaiBa.MaLoaiBa,
                        TenLoaiBa = benhAn.DmbaLoaiBa.TenLoaiBa == null ? "" : benhAn.DmbaLoaiBa.TenLoaiBa
                    },
                    Khoa = new DmkhoaDto()
                    {
                        MaKhoa = benhAn.Dmkhoa.MaKhoa == null ? "" : benhAn.Dmkhoa.MaKhoa,
                        TenKhoa = benhAn.Dmkhoa.TenKhoa == null ? "" : benhAn.Dmkhoa.TenKhoa
                    },

                    BenhTatNoiChuyenDen = new DmbenhTatDto()
                    {
                        MaBenh = benhAn.DmBenhTatNoiChuyenDen.MaBenh == null ? "" : benhAn.DmBenhTatNoiChuyenDen.MaBenh,
                        TenBenh = benhAn.DmBenhTatNoiChuyenDen.TenBenh == null ? "" : benhAn.DmBenhTatNoiChuyenDen.TenBenh
                    },
                    BenhKKBYHHD = new DmbenhTatDto()
                    {
                        MaBenh = benhAn.DmBenhKKBYHHD.MaBenh == null ? "" : benhAn.DmBenhKKBYHHD.MaBenh,
                        TenBenh = benhAn.DmBenhKKBYHHD.TenBenh == null ? "" : benhAn.DmBenhKKBYHHD.TenBenh
                    },
                    BenhChinhVv = new DmbenhTatDto()
                    {
                        MaBenh = benhAn.DmBenhChinhVV.MaBenh == null ? "" : benhAn.DmBenhChinhVV.MaBenh,
                        TenBenh = benhAn.DmBenhChinhVV.TenBenh == null ? "" : benhAn.DmBenhChinhVV.TenBenh
                    },


                    BenhNoiChuyenDenYHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhNoiChuyenDenYHCT.MaBenhIcd == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.MaBenhIcd,
                        TenBenh = benhAn.DmBenhNoiChuyenDenYHCT.TenBenhIcd == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.TenBenhIcd
                    },

                    Giamdoc = new DmnhanVienDto()
                    {
                        HoTen = benhAn.DmGiamdoc.HoTen == null ? "" : benhAn.DmGiamdoc.HoTen,
                        MaNv = benhAn.DmGiamdoc.MaNv == null ? "" : benhAn.DmGiamdoc.MaNv
                    },
                    TruongKhoa = new DmnhanVienDto()
                    {
                        HoTen = benhAn.DmTruongKhoa.HoTen == null ? "" : benhAn.DmTruongKhoa.HoTen,
                        MaNv = benhAn.DmTruongKhoa.MaNv == null ? "" : benhAn.DmTruongKhoa.MaNv
                    },
                    BsDieutri = new DmnhanVienDto()
                    {
                        HoTen = benhAn.DmBsDieutri.HoTen == null ? "" : benhAn.DmBsDieutri.HoTen,
                        MaNv = benhAn.DmBsDieutri.MaNv == null ? "" : benhAn.DmBsDieutri.MaNv
                    },
                    BenhGPTuThis = new DmbenhTatDto()
                    {
                        MaBenh = benhAn.DmBenhGPTuThi.MaBenh == null ? "" : benhAn.DmBenhGPTuThi.MaBenh,
                        TenBenh = benhAn.DmBenhGPTuThi.TenBenh == null ? "" : benhAn.DmBenhGPTuThi.TenBenh
                    },

                    XacNhanKetThucHs = benhAn.XacNhanKetThucHs,
                    NgayXacNhanKetThucHs = benhAn.NgayXacNhanKetThucHs,
                    NguoiXacNhanKetThucHs = benhAn.NguoiXacnhanKetThucHs,
                });
                //var rslt = queryBenhAnDetail.ToList();
                var result = queryBenhAnDetail.FirstOrDefault();
                var benhanYhctShort = query.Select(benhAn => new
                {
                    TenBenhKemVv1 = benhAn.TenBenhKemVv1,
                    TenBenhKemVv2 = benhAn.TenBenhKemVv2,
                    TenBenhKemVv3 = benhAn.TenBenhKemVv3,
                    TenBenhKemVv4 = benhAn.TenBenhKemVv4,
                    TenBenhKemVv5 = benhAn.TenBenhKemVv5,
                    TenBenhKemVv6 = benhAn.TenBenhKemVv6,
                    TenBenhKemVv7 = benhAn.TenBenhKemVv7,
                    TenBenhKemVv8 = benhAn.TenBenhKemVv8,
                    TenBenhKemVv9 = benhAn.TenBenhKemVv9,
                    TenBenhKemVv10 = benhAn.TenBenhKemVv10,
                    TenBenhKemVv11 = benhAn.TenBenhKemVv11,
                    TenBenhKemVv12 = benhAn.TenBenhKemVv12,

                    TenBenhKemRv1 = benhAn.TenBenhKemRv1,
                    TenBenhKemRv2 = benhAn.TenBenhKemRv2,
                    TenBenhKemRv3 = benhAn.TenBenhKemRv3,
                    TenBenhKemRv4 = benhAn.TenBenhKemRv4,
                    TenBenhKemRv5 = benhAn.TenBenhKemRv5,
                    TenBenhKemRv6 = benhAn.TenBenhKemRv6,
                    TenBenhKemRv7 = benhAn.TenBenhKemRv7,
                    TenBenhKemRv8 = benhAn.TenBenhKemRv8,
                    TenBenhKemRv9 = benhAn.TenBenhKemRv9,
                    TenBenhKemRv10 = benhAn.TenBenhKemRv10,
                    TenBenhKemRv11 = benhAn.TenBenhKemRv11,
                    TenBenhKemRv12 = benhAn.TenBenhKemRv12,
                    TenBenhKemVv1yhct = benhAn.TenBenhKemVv1yhct,
                    TenBenhKemVv2yhct = benhAn.TenBenhKemVv2yhct,
                    TenBenhKemVv3yhct = benhAn.TenBenhKemVv3yhct,
                    TenBenhKemVv4yhct = benhAn.TenBenhKemVv4yhct,
                    TenBenhKemVv5yhct = benhAn.TenBenhKemVv5yhct,
                    TenBenhKemVv6yhct = benhAn.TenBenhKemVv6yhct,
                    TenBenhKemVv7yhct = benhAn.TenBenhKemVv7yhct,
                    TenBenhKemVv8yhct = benhAn.TenBenhKemVv8yhct,
                    TenBenhKemVv9yhct = benhAn.TenBenhKemVv9yhct,
                    TenBenhKemVv10yhct = benhAn.TenBenhKemVv10yhct,
                    TenBenhKemVv11yhct = benhAn.TenBenhKemVv11yhct,
                    TenBenhKemVv12yhct = benhAn.TenBenhKemVv12yhct,

                    TenBenhKemRv1yhct = benhAn.TenBenhKemRv1yhct,
                    TenBenhKemRv2yhct = benhAn.TenBenhKemRv2yhct,
                    TenBenhKemRv3yhct = benhAn.TenBenhKemRv3yhct,
                    TenBenhKemRv4yhct = benhAn.TenBenhKemRv4yhct,
                    TenBenhKemRv5yhct = benhAn.TenBenhKemRv5yhct,
                    TenBenhKemRv6yhct = benhAn.TenBenhKemRv6yhct,
                    TenBenhKemRv7yhct = benhAn.TenBenhKemRv7yhct,
                    TenBenhKemRv8yhct = benhAn.TenBenhKemRv8yhct,
                    TenBenhKemRv9yhct = benhAn.TenBenhKemRv9yhct,
                    TenBenhKemRv10yhct = benhAn.TenBenhKemRv10yhct,
                    TenBenhKemRv11yhct = benhAn.TenBenhKemRv11yhct,
                    TenBenhKemRv12yhct = benhAn.TenBenhKemRv12yhct,
                    BenhNoiChuyenDenYHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhNoiChuyenDenYHCT.MaBenhIcd == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhNoiChuyenDenYHCT.TenBenhIcd == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhNoiChuyenDenYHCT.TenBenh == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.TenBenh,
                        MaBenh = benhAn.DmBenhNoiChuyenDenYHCT.MaBenh == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhNoiChuyenDenYHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhNoiChuyenDenYHCT.TenBenhBhyt
                    },
                    BenhKemVV1YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV1YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV1YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV1YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV1YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV1YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV1YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV1YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV1YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV1YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV1YHCT.TenBenhBhyt
                    },
                    BenhKemVV2YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV2YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV2YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV2YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV2YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV2YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV2YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV2YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV2YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV2YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV2YHCT.TenBenhBhyt
                    },
                    BenhKemVV3YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV3YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV3YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV3YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV3YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV3YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV3YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV3YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV3YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV3YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV3YHCT.TenBenhBhyt
                    },
                    BenhKemVV4YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV4YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV4YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV4YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV4YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV4YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV4YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV4YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV4YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV4YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV4YHCT.TenBenhBhyt
                    },
                    BenhKemVV5YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV5YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV5YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV5YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV5YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV5YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV5YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV5YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV5YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV5YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV5YHCT.TenBenhBhyt
                    },

                    BenhKemVV6YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV6YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV6YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV6YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV6YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV6YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV6YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV6YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV6YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV6YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV6YHCT.TenBenhBhyt
                    },
                    BenhKemVV7YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV7YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV7YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV7YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV7YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV7YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV7YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV7YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV7YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV7YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV7YHCT.TenBenhBhyt
                    },
                    BenhKemVV8YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV8YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV8YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV8YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV8YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV8YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV8YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV8YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV8YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV8YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV8YHCT.TenBenhBhyt
                    },

                    BenhKemVV9YHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemVV9YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemVV9YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemVV9YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemVV9YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemVV9YHCT.TenBenh == null ? "" : benhAn.DmBenhKemVV9YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemVV9YHCT.MaBenh == null ? "" : benhAn.DmBenhKemVV9YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemVV9YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemVV9YHCT.TenBenhBhyt
                    },
                    BenhKemRv1yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV1YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV1YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV1YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV1YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV1YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV1YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemRV1YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV1YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV1YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV1YHCT.TenBenhBhyt
                    },
                    BenhKemRv2yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV2YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV2YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV2YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV2YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV2YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV2YHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKemRV2YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV2YHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV2YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV2YHCT.TenBenhBhyt
                    },

                    BenhKemRv3yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV3YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV3YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV3YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV3YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV3YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV3YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV3YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV3YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV3YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV3YHCT.MaBenh
                    },
                    BenhKemRv4yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV4YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV4YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV4YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV4YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV4YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV4YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV4YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV4YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV4YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV4YHCT.MaBenh
                    },
                    BenhKemRv5yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV5YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV5YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV5YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV5YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV5YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV5YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV5YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV5YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV5YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV5YHCT.MaBenh
                    },

                    BenhKemRv6yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV6YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV6YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV6YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV6YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV6YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV6YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV6YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV6YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV6YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV6YHCT.MaBenh
                    },
                    BenhKemRv7yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV7YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV7YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV7YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV7YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV7YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV7YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV7YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV7YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV7YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV7YHCT.MaBenh
                    },
                    BenhKemRv8yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV8YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV8YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV8YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV8YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV8YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV8YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV8YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV8YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV8YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV8YHCT.MaBenh
                    },
                    BenhKemRv9yhct = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKemRV9YHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKemRV9YHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKemRV9YHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKemRV9YHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKemRV9YHCT.TenBenh == null ? "" : benhAn.DmBenhKemRV9YHCT.TenBenh,
                        TenBenhBHYT = benhAn.DmBenhKemRV9YHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKemRV9YHCT.TenBenhBhyt,
                        MaBenh = benhAn.DmBenhKemRV9YHCT.MaBenh == null ? "" : benhAn.DmBenhKemRV9YHCT.MaBenh
                    },
                    BenhKKBYHCT = new DmbenhTatYhctDto()
                    {
                        MaBenhIcd = benhAn.DmBenhKKBYHCT.MaBenhIcd == null ? "" : benhAn.DmBenhKKBYHCT.MaBenhIcd,
                        TenBenhIcd = benhAn.DmBenhKKBYHCT.TenBenhIcd == null ? "" : benhAn.DmBenhKKBYHCT.TenBenhIcd,
                        TenBenh = benhAn.DmBenhKKBYHCT.TenBenh == null ? "" : benhAn.DmBenhKKBYHCT.TenBenh,
                        MaBenh = benhAn.DmBenhKKBYHCT.MaBenh == null ? "" : benhAn.DmBenhKKBYHCT.MaBenh,
                        TenBenhBHYT = benhAn.DmBenhKKBYHCT.TenBenhBhyt == null ? "" : benhAn.DmBenhKKBYHCT.TenBenhBhyt
                    }


                }).FirstOrDefault();
                //result.BenhNoiChuyenDenYHCT = benhanYhctShort != null ? benhanYhctShort.BenhNoiChuyenDenYHCT : new DmbenhTatYhctDto();
                var KhoaRV = new DmkhoaDto();
                var dmKhoa = _dMKhoa.Table.Where(x => x.MaKhoa == dieutri.MaKhoa).FirstOrDefault();
                if (dieutri != null)
                {
                    KhoaRV.MaKhoa = dieutri.MaKhoa;
                    KhoaRV.TenKhoa = dmKhoa != null ? dmKhoa.TenKhoa : null;

                }
                var queryBa = query.Select(ba => new
                {
                    BenhKemVV1 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV1.MaBenh == null ? "" : ba.DmBenhKemVV1.MaBenh,
                        TenBenh = ba.DmBenhKemVV1.TenBenh == null ? "" : ba.DmBenhKemVV1.TenBenh
                    },
                    BenhKemVV2 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV2.MaBenh == null ? "" : ba.DmBenhKemVV2.MaBenh,
                        TenBenh = ba.DmBenhKemVV2.TenBenh == null ? "" : ba.DmBenhKemVV2.TenBenh
                    },
                    BenhKemVV3 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV3.MaBenh == null ? "" : ba.DmBenhKemVV3.MaBenh,
                        TenBenh = ba.DmBenhKemVV3.TenBenh == null ? "" : ba.DmBenhKemVV3.TenBenh
                    },
                    BenhKemVV4 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV4.MaBenh == null ? "" : ba.DmBenhKemVV4.MaBenh,
                        TenBenh = ba.DmBenhKemVV4.TenBenh == null ? "" : ba.DmBenhKemVV4.TenBenh
                    },
                    BenhKemVV5 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV5.MaBenh == null ? "" : ba.DmBenhKemVV5.MaBenh,
                        TenBenh = ba.DmBenhKemVV5.TenBenh == null ? "" : ba.DmBenhKemVV5.TenBenh
                    },
                    BenhKemVV6 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV6.MaBenh == null ? "" : ba.DmBenhKemVV6.MaBenh,
                        TenBenh = ba.DmBenhKemVV6.TenBenh == null ? "" : ba.DmBenhKemVV6.TenBenh
                    },
                    BenhKemVV7 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV7.MaBenh == null ? "" : ba.DmBenhKemVV7.MaBenh,
                        TenBenh = ba.DmBenhKemVV7.TenBenh == null ? "" : ba.DmBenhKemVV7.TenBenh
                    },
                    BenhKemVV8 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV8.MaBenh == null ? "" : ba.DmBenhKemVV8.MaBenh,
                        TenBenh = ba.DmBenhKemVV8.TenBenh == null ? "" : ba.DmBenhKemVV8.TenBenh
                    },
                    BenhKemVV9 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemVV9.MaBenh == null ? "" : ba.DmBenhKemVV9.MaBenh,
                        TenBenh = ba.DmBenhKemVV9.TenBenh == null ? "" : ba.DmBenhKemVV9.TenBenh
                    },

                    BenhChinhRv = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhChinhRV.MaBenh == null ? "" : ba.DmBenhChinhRV.MaBenh,
                        TenBenh = ba.DmBenhChinhRV.TenBenh == null ? "" : ba.DmBenhChinhRV.TenBenh
                    },
                    BenhKemRV1 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV1.MaBenh == null ? "" : ba.DmBenhKemRV1.MaBenh,
                        TenBenh = ba.DmBenhKemRV1.TenBenh == null ? "" : ba.DmBenhKemRV1.TenBenh
                    },
                    BenhKemRV2 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV2.MaBenh == null ? "" : ba.DmBenhKemRV2.MaBenh,
                        TenBenh = ba.DmBenhKemRV2.TenBenh == null ? "" : ba.DmBenhKemRV2.TenBenh
                    },
                    BenhKemRV3 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV3.MaBenh == null ? "" : ba.DmBenhKemRV3.MaBenh,
                        TenBenh = ba.DmBenhKemRV3.TenBenh == null ? "" : ba.DmBenhKemRV3.TenBenh
                    },
                    BenhKemRV4 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV4.MaBenh == null ? "" : ba.DmBenhKemRV4.MaBenh,
                        TenBenh = ba.DmBenhKemRV4.TenBenh == null ? "" : ba.DmBenhKemRV4.TenBenh
                    },
                    BenhKemRV5 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV5.MaBenh == null ? "" : ba.DmBenhKemRV5.MaBenh,
                        TenBenh = ba.DmBenhKemRV5.TenBenh == null ? "" : ba.DmBenhKemRV5.TenBenh
                    },
                    BenhKemRV6 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV6.MaBenh == null ? "" : ba.DmBenhKemRV6.MaBenh,
                        TenBenh = ba.DmBenhKemRV6.TenBenh == null ? "" : ba.DmBenhKemRV6.TenBenh
                    },
                    BenhKemRV7 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV7.MaBenh == null ? "" : ba.DmBenhKemRV7.MaBenh,
                        TenBenh = ba.DmBenhKemRV7.TenBenh == null ? "" : ba.DmBenhKemRV7.TenBenh
                    },
                    BenhKemRV8 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV8.MaBenh == null ? "" : ba.DmBenhKemRV8.MaBenh,
                        TenBenh = ba.DmBenhKemRV8.TenBenh == null ? "" : ba.DmBenhKemRV8.TenBenh
                    },
                    BenhKemRV9 = new DmbenhTatDto()
                    {
                        MaBenh = ba.DmBenhKemRV9.MaBenh == null ? "" : ba.DmBenhKemRV9.MaBenh,
                        TenBenh = ba.DmBenhKemRV9.TenBenh == null ? "" : ba.DmBenhKemRV9.TenBenh
                    },
                }).FirstOrDefault();

                result.BenhKemVV1 = queryBa != null ? queryBa.BenhKemVV1 : new DmbenhTatDto();
                result.BenhKemVV2 = queryBa != null ? queryBa.BenhKemVV2 : new DmbenhTatDto();
                result.BenhKemVV3 = queryBa != null ? queryBa.BenhKemVV3 : new DmbenhTatDto();
                result.BenhKemVV4 = queryBa != null ? queryBa.BenhKemVV4 : new DmbenhTatDto();
                result.BenhKemVV5 = queryBa != null ? queryBa.BenhKemVV5 : new DmbenhTatDto();
                result.BenhKemVV6 = queryBa != null ? queryBa.BenhKemVV6 : new DmbenhTatDto();
                result.BenhKemVV7 = queryBa != null ? queryBa.BenhKemVV7 : new DmbenhTatDto();
                result.BenhKemVV8 = queryBa != null ? queryBa.BenhKemVV8 : new DmbenhTatDto();
                result.BenhKemVV9 = queryBa != null ? queryBa.BenhKemVV9 : new DmbenhTatDto();

                result.BenhKemRV1 = queryBa != null ? queryBa.BenhKemRV1 : new DmbenhTatDto();
                result.BenhKemRV2 = queryBa != null ? queryBa.BenhKemRV2 : new DmbenhTatDto();
                result.BenhKemRV3 = queryBa != null ? queryBa.BenhKemRV3 : new DmbenhTatDto();
                result.BenhKemRV4 = queryBa != null ? queryBa.BenhKemRV4 : new DmbenhTatDto();
                result.BenhKemRV5 = queryBa != null ? queryBa.BenhKemRV5 : new DmbenhTatDto();
                result.BenhKemRV6 = queryBa != null ? queryBa.BenhKemRV6 : new DmbenhTatDto();
                result.BenhKemRV7 = queryBa != null ? queryBa.BenhKemRV7 : new DmbenhTatDto();
                result.BenhKemRV8 = queryBa != null ? queryBa.BenhKemRV8 : new DmbenhTatDto();
                result.BenhKemRV9 = queryBa != null ? queryBa.BenhKemRV9 : new DmbenhTatDto();


                result.BenhKemVV1YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV1YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV2YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV2YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV3YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV3YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV4YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV4YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV5YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV5YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV6YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV6YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV7YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV7YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV8YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV8YHCT : new DmbenhTatYhctDto();
                result.BenhKemVV9YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV9YHCT : new DmbenhTatYhctDto();
                result.TenBenhKemVv1yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv1yhct : string.Empty;
                result.TenBenhKemVv2yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv2yhct : string.Empty;
                result.TenBenhKemVv3yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv3yhct : string.Empty;
                result.TenBenhKemVv4yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv4yhct : string.Empty;
                result.TenBenhKemVv5yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv5yhct : string.Empty;
                result.TenBenhKemVv6yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv6yhct : string.Empty;
                result.TenBenhKemVv7yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv7yhct : string.Empty;
                result.TenBenhKemVv8yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv8yhct : string.Empty;
                result.TenBenhKemVv9yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv9yhct : string.Empty;
                //result.TenBenhKemVv10yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv10yhct : string.Empty;
                //result.TenBenhKemVv11yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv11yhct : string.Empty;
                //result.TenBenhKemVv12yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv12yhct : string.Empty;
                result.TenBenhKemRv1yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv1yhct : string.Empty;
                result.TenBenhKemRv2yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv2yhct : string.Empty;
                result.TenBenhKemRv3yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv3yhct : string.Empty;
                result.TenBenhKemRv4yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv4yhct : string.Empty;
                result.TenBenhKemRv5yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv5yhct : string.Empty;
                result.TenBenhKemRv6yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv6yhct : string.Empty;
                result.TenBenhKemRv7yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv7yhct : string.Empty;
                result.TenBenhKemRv8yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv8yhct : string.Empty;
                result.TenBenhKemRv9yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv9yhct : string.Empty;
                //result.TenBenhKemRv10yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv10yhct : string.Empty;
                //result.TenBenhKemRv11yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv11yhct : string.Empty;
                //result.TenBenhKemRv12yhct = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv12yhct : string.Empty;
                result.TenBenhKemVv1 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv1 : string.Empty;
                result.TenBenhKemVv2 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv2 : string.Empty;
                result.TenBenhKemVv3 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv3 : string.Empty;
                result.TenBenhKemVv4 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv4 : string.Empty;
                result.TenBenhKemVv5 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv5 : string.Empty;
                result.TenBenhKemVv6 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv6 : string.Empty;
                result.TenBenhKemVv7 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv7 : string.Empty;
                result.TenBenhKemVv8 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv8 : string.Empty;
                result.TenBenhKemVv9 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv9 : string.Empty;
                //result.TenBenhKemVv10 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv10 : string.Empty;
                //result.TenBenhKemVv11 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv11 : string.Empty;
                //result.TenBenhKemVv12 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemVv12 : string.Empty;
                result.TenBenhKemRv1 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv1 : string.Empty;
                result.TenBenhKemRv2 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv2 : string.Empty;
                result.TenBenhKemRv3 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv3 : string.Empty;
                result.TenBenhKemRv4 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv4 : string.Empty;
                result.TenBenhKemRv5 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv5 : string.Empty;
                result.TenBenhKemRv6 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv6 : string.Empty;
                result.TenBenhKemRv7 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv7 : string.Empty;
                result.TenBenhKemRv8 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv8 : string.Empty;
                result.TenBenhKemRv9 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv9 : string.Empty;
                //result.TenBenhKemRv10 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv10 : string.Empty;
                //result.TenBenhKemRv11 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv11 : string.Empty;
                //result.TenBenhKemRv12 = benhanYhctShort != null ? benhanYhctShort.TenBenhKemRv12 : string.Empty;
                //result.BenhKemVV10YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV10YHCT : new DmbenhTatYhctDto();
                //result.BenhKemVV11YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV11YHCT : new DmbenhTatYhctDto();
                //result.BenhKemVV12YHCT = benhanYhctShort != null ? benhanYhctShort.BenhKemVV12YHCT : new DmbenhTatYhctDto();
                //Ra viện
                result.BenhKemRv1yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv1yhct : new DmbenhTatYhctDto();
                result.BenhKemRv2yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv2yhct : new DmbenhTatYhctDto();
                result.BenhKemRv3yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv3yhct : new DmbenhTatYhctDto();
                result.BenhKemRv4yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv4yhct : new DmbenhTatYhctDto();
                result.BenhKemRv5yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv5yhct : new DmbenhTatYhctDto();
                result.BenhKemRv6yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv6yhct : new DmbenhTatYhctDto();
                result.BenhKemRv7yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv7yhct : new DmbenhTatYhctDto();
                result.BenhKemRv8yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv8yhct : new DmbenhTatYhctDto();
                result.BenhKemRv9yhct = benhanYhctShort != null ? benhanYhctShort.BenhKemRv9yhct : new DmbenhTatYhctDto();
                result.BenhKKBYHCT = benhanYhctShort != null ? benhanYhctShort.BenhKKBYHCT : new DmbenhTatYhctDto();

                result.KhoaRV = KhoaRV != null ? KhoaRV : new DmkhoaDto();

                string idbahis = result?.MaBa + "-" + "1" + "-" + "c" + "-" + 1 + "-" + "c";
                string MaBA = result?.MaBa;
                string LoaiGiayTo = "1";
                //string Idbaemr = result.Idba?.ToString();
                string Idbaemr = result?.Idba.ToString();
                string IDchuoi = "_" + 1 + "_" + result?.MaBa + "_" + "c";
                string stt = "1";
                string sttkhoa = "1";
                SaveToDatabaseEMR(idbahis, Idbaemr, LoaiGiayTo, IDchuoi, stt, sttkhoa, MaBA);

                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public object GetLoaiBA(decimal id)
        {
            var data = _benhAnRepository.Table.Include(x => x.DmbaLoaiBa).Where(x => x.Idba == id).FirstOrDefault();
            if (data == null)
            {
                return data;
            }
            return new
            {
                ma = data.LoaiBa,
                tenLoaiBA = data.DmbaLoaiBa.TenLoaiBa
            };
        }
        public BenhAn Show(decimal id)
        {
            var data = _benhAnRepository.Table.First(x => x.Idba == id);
            return data;
        }
        public void Store(BenhAn benhAn)
        {
            benhAn.Idba = (_benhAnRepository.Table.Max(x => (decimal?)x.Idba) ?? 0) + 1;
            _benhAnRepository.Insert(benhAn);
        }
        public void Update(decimal id, BenhAn benhAn)
        {
            _benhAnRepository.Update(benhAn, id);
        }

        public void ThongTinBnCreate(BenhAnDetailDto benhAn)
        {
            benhAn.Idba = (_benhAnRepository.Table.Max(x => (decimal?)x.Idba) ?? 0) + 1;
            benhAn.MaBv = "01062";
            // benhAn.MaBvChuyenDen = "01062";
            benhAn.TenBv = "Bệnh viện YHCT Bộ Công An";
            benhAn.TenDvcq = "Bộ Công An";
            if (benhAn.Khoa != null)
            {
                benhAn.MaKhoaVv = benhAn.Khoa.MaKhoa;
            }
            _benhAnRepository.Insert<BenhAnDetailDto>(benhAn, (model) =>
            {
                model.BsdieuTri = benhAn?.BsDieutri?.MaNv;
                if (benhAn.BenhNhan != null)
                {
                    var infoBn = new ThongTinBn();
                    model.MaBn = benhAn.BenhNhan.MaBn;
                    MapEntityHelper.MapEntity<ThongTinBn, BenhAnDetailThongTinBnDto>(infoBn, benhAn.BenhNhan);
                    infoBn.Idba = model.Idba;
                    infoBn.MaNgheNghiep = benhAn.BenhNhan.NgheNghiep.MaNn;
                    infoBn.MaDanToc = benhAn.BenhNhan.DanToc.MaDanToc;
                    infoBn.MaQuocTich = benhAn.BenhNhan.QuocGia.MaQg;
                    infoBn.MaTinh = benhAn.BenhNhan.Tinh.MaTinh;
                    infoBn.MaHuyen = benhAn.BenhNhan.QuanHuyen.MaQh;
                    infoBn.MaPxa = benhAn.BenhNhan.PhuongXa.MaPxa;
                    infoBn.DoiTuong = benhAn.BenhNhan.DoiTuong.MaDt;
                    _benhAnRepository._context.ThongTinBn.Add(infoBn);
                    _benhAnRepository.Log<ThongTinBn>(ActionLogType.Create, infoBn);
                }
                if (benhAn.LoaiBenhAn != null)
                {
                    model.LoaiBa = benhAn.LoaiBenhAn.MaLoaiBa;
                }
                if (benhAn.Buong != null)
                {
                    model.Buong = benhAn.Buong.MaBuong;
                }
                if (benhAn.Giuong != null)
                {
                    model.Giuong = benhAn.Giuong.MaGiuong;
                }
                var benhAnKhoaDieuTri = new BenhAnKhoaDieuTri();
                benhAnKhoaDieuTri.Stt = 1;
                benhAnKhoaDieuTri.Idba = model.Idba;
                benhAnKhoaDieuTri.MaKhoa = model.MaKhoaVv;
                benhAnKhoaDieuTri.MaBa = model.MaBa;
                benhAnKhoaDieuTri.MaBn = benhAn?.BenhNhan?.MaBn ?? "";
                benhAnKhoaDieuTri.NgayVaoKhoa = model.NgayVv;
                benhAnKhoaDieuTri.Buong = model.Buong;
                benhAnKhoaDieuTri.Giuong = model.Giuong;
                benhAnKhoaDieuTri.BsdieuTri = benhAn?.BsDieutri?.MaNv != null ? benhAn?.BsDieutri?.MaNv : "";
                _benhAnRepository._context.BenhAnKhoaDieuTri.Add(benhAnKhoaDieuTri);
                _benhAnRepository.Log<BenhAnKhoaDieuTri>(ActionLogType.Create, benhAnKhoaDieuTri);
            });
        }

        public void ThongTinBnUpdate(BenhAnDetailDto benhAn, decimal idba, bool withoutPermission = false)
        {
            //Update KhoaDieuTri


            _benhAnRepository.Update<BenhAnDetailDto>(benhAn, (model) =>
            {
                if (!withoutPermission)
                {
                    PermissionThrowHelper.DongBenhAnCheck(model.XacNhanKetThucHs);
                }
                model.GiamDoc = benhAn?.Giamdoc?.MaNv;
                model.TruongKhoa = benhAn?.TruongKhoa?.MaNv;
                model.BsdieuTri = benhAn?.BsDieutri?.MaNv;
                var khoaDieuTri = _benhAnRepository._context.BenhAnKhoaDieuTri.FirstOrDefault(x => x.Idba == model.Idba && x.Stt == 1);
                //_benhAnRepository._context.Entry(khoaDieuTri).State = EntityState.Detached;

                if (benhAn.Khoa != null)
                {
                    model.MaKhoaVv = benhAn.Khoa.MaKhoa;
                }
                if (benhAn.LoaiBenhAn != null)
                {
                    model.LoaiBa = benhAn.LoaiBenhAn.MaLoaiBa;
                }
                if (benhAn.Buong != null)
                {
                    model.Buong = benhAn.Buong.MaBuong;
                }
                if (benhAn.Giuong != null)
                {
                    model.Giuong = benhAn.Giuong.MaGiuong;
                }
                if (benhAn.BenhNhan != null)
                {
                    model.MaBn = benhAn.BenhNhan.MaBn;
                    var infoBn = _benhAnRepository._context.ThongTinBn.FirstOrDefault(x => x.Idba == model.Idba);
                    infoBn = MapEntityHelper.MapEntity<ThongTinBn, BenhAnDetailThongTinBnDto>(infoBn, benhAn.BenhNhan);
                    infoBn.MaNgheNghiep = benhAn.BenhNhan.NgheNghiep.MaNn;
                    infoBn.MaDanToc = benhAn.BenhNhan.DanToc.MaDanToc;
                    infoBn.MaQuocTich = benhAn.BenhNhan.QuocGia.MaQL;
                    infoBn.MaTinh = benhAn.BenhNhan.Tinh.MaTinh;
                    infoBn.MaHuyen = benhAn.BenhNhan.QuanHuyen.MaQh;
                    infoBn.MaPxa = benhAn.BenhNhan.PhuongXa.MaPxa;
                    infoBn.DoiTuong = benhAn.BenhNhan.DoiTuong.MaDt;
                    infoBn.GioiTinh = benhAn.BenhNhan.GioiTinh;

                    _benhAnRepository.Log<ThongTinBn>(ActionLogType.Modify, infoBn);
                }
                if (khoaDieuTri != null)
                {
                    //khoaDieuTri.MaKhoa = model.MaKhoaVv;
                    //khoaDieuTri.MaBa = model.MaBa;
                    //khoaDieuTri.MaBn = model.MaBn;
                    //khoaDieuTri.NgayVaoKhoa = model.NgayVv;
                    //khoaDieuTri.Buong = model.Buong;
                    //khoaDieuTri.Giuong = model.Giuong;
                    //khoaDieuTri.BsdieuTri = model.BsdieuTri != null ? model.BsdieuTri : "";
                    //_benhAnRepository.Log<BenhAnKhoaDieuTri>(ActionLogType.Modify, khoaDieuTri);
                    //HanlderCreateUpdate<BenhAnKhoaDieuTri, BenhAnKhoaDieuTri>(khoaDieuTri, null, true, khoaDieuTri.Idba, khoaDieuTri.Stt);
                    //_benhAnRepository._context.Entry(khoaDieuTri).State = EntityState.Modified;
                }
            }, idba);
        }

        public void ThongTinToBenhAnCreateUpdate(ToBenhAnVM info, decimal idba)
        {
            _benhAnRepository.setLogActionName("Tờ bệnh án");
            _benhAnRepository.Update(info.benhAn, (model) =>
            {
                PermissionThrowHelper.DongBenhAnCheck(model.XacNhanKetThucHs);
                //info.benhAn.DmBenhKemVV1.MaBenh = info.benhAn.MaBenhKemVv1;
                //info.benhAn.DmBenhKemVV1.TenBenh = info.benhAn.TenBenhKemVv1;
                // BenhAnKhamYhct
                info.BenhAnKhamYhct.Idba = model.Idba;
                info.BenhAnKhamYhct.MaBa = model.MaBa;
                info.BenhAnKhamYhct.MaBn = model.MaBn;
                // BenhAnKhamYhhd
                info.BenhAnKhamYhhd.Idba = model.Idba;
                info.BenhAnKhamYhhd.MaBa = model.MaBa;
                info.BenhAnKhamYhhd.MaBn = model.MaBn;
                // BenhAnTongKetBenhAn
                info.BenhAnTongKetBenhAn.Idba = model.Idba;
                info.BenhAnTongKetBenhAn.MaBa = model.MaBa;
                info.BenhAnTongKetBenhAn.MaBn = model.MaBn;
                // BenhAnKhoaDieuTri
                info.BenhAnKhoaDieuTri.Idba = model.Idba;
                info.BenhAnKhoaDieuTri.MaBn = model.MaBn;
                info.BenhAnKhoaDieuTri.MaBa = model.MaBa;
                //info.BenhAnKhoaDieuTri.MaKhoa = model.MaKhoaVv;
                //info.BenhAnKhoaDieuTri.NgayVaoKhoa = model.NgayVv;
                //BenhAnTienSuBenh
                info.BenhAnTienSuBenh.Idba = model.Idba;
                info.BenhAnTienSuBenh.MaBn = model.MaBn;
                info.BenhAnTienSuBenh.MaBa = model.MaBa;
                //info.ThongTinBn.DoiTuong = model.ThongTinBn.DoiTuong;
                //MaBaStorage.MaBaValue = info.benhAn.MaBa;
                if (info.BenhAnKhoaDieuTri.Stt == null)
                {
                    info.BenhAnKhoaDieuTri.Stt = 1;
                }

                var kdt = _benhAnRepository._context.BenhAnKhoaDieuTri.FirstOrDefault(x => x.Idba == idba && x.Stt == info.BenhAnKhoaDieuTri.Stt);
                if (kdt != null)
                {
                    info.BenhAnKhoaDieuTri.MaKhoa = kdt.MaKhoa;
                    info.BenhAnKhoaDieuTri.NgayVaoKhoa = kdt.NgayVaoKhoa;
                }

                HanlderCreateUpdate<BenhAnKhamYhct, BenhAnKhamYhctVM>(info.BenhAnKhamYhct, null, false, info.BenhAnKhamYhct.Idba);
                HanlderCreateUpdate<BenhAnKhamYhhd, BenhAnKhamYhhdVM>(info.BenhAnKhamYhhd, null, false, info.BenhAnKhamYhhd.Idba);
                HanlderCreateUpdate<BenhAnTienSuBenh, BenhAnTienSuBenhVM>(info.BenhAnTienSuBenh, null, false, info.BenhAnTienSuBenh.Idba);
                HanlderCreateUpdate<BenhAnTongKetBenhAn, BenhAnTongKetBenhAnVM>(info.BenhAnTongKetBenhAn, null, false, info.BenhAnTongKetBenhAn.Idba);
                HanlderCreateUpdate<BenhAnKhoaDieuTri, ToBenhAnBenhAnKhoaDieuTriVM>(info.BenhAnKhoaDieuTri, null, true, info.BenhAnKhoaDieuTri.Idba, info.BenhAnKhoaDieuTri.Stt);
                model.BsdieuTri = info.BenhAnTongKetBenhAn?.BsdieuTri;
                //model.ThongTinBn = info.ThongTinBn;
            }, idba);
            var thongTinBn = _thongTinBenhNhanRepository._context.ThongTinBn.FirstOrDefault(x => x.Idba == idba);
            if (info.thongTinBenhNhan != null)
            {
                thongTinBn.DoiTuong = info.thongTinBenhNhan.maDt;
                thongTinBn.GioiTinh = info.thongTinBenhNhan.gioiTinh;
                _thongTinBenhNhanRepository.Update(thongTinBn);
                _thongTinBenhNhanRepository.Save();
            }
            var benhAnTienSu = _benhAnTienSuBenhRepository._context.BenhAnTienSuBenh.FirstOrDefault(x => x.Idba == idba);
            if (info.BenhAnTienSuBenh != null)
            {
                benhAnTienSu.CanNang = info.BenhAnTienSuBenh.canNangNhi;
                _benhAnTienSuBenhRepository.Update(benhAnTienSu);
                _benhAnTienSuBenhRepository.Save();
            }

        }



        //public void ThongTinToBenhAnCreateUpdate(ToBenhAnVM info, decimal idba)
        //{
        //    // Các bước xử lý khác của phương thức ThongTinToBenhAnCreateUpdate ở đây

        //    // Gán giá trị MaBa vào biến đã được khai báo trong file khác
        //    MaBaStorage.MaBaValue = info.benhAn.MaBa;
        //}

        public void Destroy(decimal idba)
        {
            _benhAnRepository.setLogActionName("Bệnh án");
            _benhAnRepository.Delete((model) =>
            {
                PermissionThrowHelper.DongBenhAnCheck(model.XacNhanKetThucHs);
            }, idba);
        }

        private void HanlderCreateUpdate<T, Tb>(Tb info, Action<T> callback = null, bool withoutInsert = false, params object[] id) where T : class
        {
            // need to fix
            // var response = new GenericRepository<T>(_context, _benhAnRepository._context);
            var response = new GenericRepository<T>(_context);
            try
            {
                response.UpdateWithoutTransaction<Tb>(info, callback, id);
            }
            catch (Exception ex)
            {
                if (withoutInsert == false)
                {
                    if (ex is HttpStatusException httpException)
                    {
                        response.InsertWithoutTransaction<Tb>(info, callback);
                    }
                }
            }
        }

        // in tờ  thứ ba


        public string Print3(decimal idba)
        {
            try
            {


                if (idba < 0 || idba == 0)
                {
                    return "";
                }
                //var benhAn3 = Detail(idba);
                var benhAn3 = DetaiBenhAn(idba);
                var template3 = "";

                switch (benhAn3.LoaiBenhAn.MaLoaiBa)
                {
                    case 1:
                        template3 = "Benhan_YHCT_noi_tru-cp_To3.docx";
                        break;
                    case 2:
                        // code block
                        template3 = "Benhan_YHCT_ngoai_tru_v1_To3.docx";
                        break;
                    case 3:
                        template3 = "Benh_an_YHCT_man_tinh_To3.docx";
                        break;
                    case 4:
                        template3 = "Benhan_YHCT_noi_tru_theo_ngay_To3.docx";
                        break;
                    case 5:
                        template3 = "Benhan_Noikhoa_To3.docx";
                        break;
                    case 6:
                        template3 = "Benhan_Ngoaikhoa_To3.docx";
                        break;
                    case 7:
                        template3 = "Benhan_YHCT_noi_tru-nhi_To3.docx";
                        break;
                    //break;
                    default:
                        // code block
                        throw new HttpStatusException(HttpStatusCode.UnsupportedMediaType, "Chưa hỗ trợ");
                }


                var benhAnKhoaDieuTriService = new BenhAnKhoaDieuTriService();
                //benhAnKhoaDieuTri
                var benhAnKhoaDieuTris = benhAnKhoaDieuTriService.Get(new BenhAnKhoaDieuTriParameters()
                {
                    Idba = idba
                }).data.OrderBy(x => x.Stt).ToList();
                var benhAnKhoaDieuTri = benhAnKhoaDieuTris.Count > 0 ? benhAnKhoaDieuTris[0] : null;
                //benhanKhamVV
                var phieukhamvv = _benhAnRepository._context.BenhAnKhamVaoVien.Where(
                    x => x.Idba == idba
                ).FirstOrDefault();
                phieukhamvv = phieukhamvv ?? new BenhAnKhamVaoVien();
                //benhanTienSuBenh
                var tienSuBenh = _benhAnRepository._context.BenhAnTienSuBenh.FirstOrDefault(x => x.Idba == idba);
                tienSuBenh = tienSuBenh ?? new BenhAnTienSuBenh();
                //benhanYHCT
                var benhanYHHD = _benhAnRepository._context.BenhAnKhamYhhd.Include(x => x.DmBskham).Include(x => x.DmBenhPhanBiet).FirstOrDefault(x => x.Idba == idba);
                benhanYHHD = benhanYHHD ?? new BenhAnKhamYhhd();
                //benhanYHCT
                Mapper.CreateMap<BenhAnKhamYhctDto, BenhAnKhamYhct>().ReverseMap();
                Mapper.CreateMap<DmbenhTatYhctDto, DmbenhTatYhct>().ReverseMap();
                var benhanYHCT = _benhAnRepository._context.BenhAnKhamYhct.Include(x => x.DmBenhDanhTheoYHCT).Select(Mapper.Map<BenhAnKhamYhctDto>).FirstOrDefault(x => x.Idba == idba);
                benhanYHCT = benhanYHCT ?? new BenhAnKhamYhctDto();
                //TongKetBenhAn
                Mapper.CreateMap<BenhAnTongKetBenhAnDto, BenhAnTongKetBenhAn>().ReverseMap();
                Mapper.CreateMap<DmnhanVienDto, DmnhanVien>().ReverseMap();
                var tongKetBenhAn = _benhAnRepository._context.BenhAnTongKetBenhAn
                    .Include(x => x.DmBsDieutri)
                    .Include(x => x.DmNguoiGiao)
                    .Include(x => x.DmNguoiNhan)
                    .Select(Mapper.Map<BenhAnTongKetBenhAnDto>)
                    .FirstOrDefault(x => x.Idba == idba);
                tongKetBenhAn = tongKetBenhAn ?? new BenhAnTongKetBenhAnDto();

                List<string> fields = new List<string>(){
                /*"BenhVien",
                "SoYTe",
                "Khoa",
                "Buong",
                "SoVVien",
                "SoLuuTru",
                "MaNgBenh",
                "MaNB",
                "MaYTe",
                "HoVaTen",
                "DanToc",
                "NgheNghiep",
                "QuocTich",
                "SoNha",
                "Thon",
                "Xa",
                "Huyen",
                "Tinh",
                "NoiLamViec",
                "GiaTriBaoHiemYTe",
                "LienHe",
                "SoDienThoai",
                "Nam",
                "Nu",
                "VaoVienGioPhut",
                "VaoVienNgayThang",
                "ChuyenDen",
                "RaVienGioPhut",
                "RaVienNgayThang",
                "NoiChuyenDenYHHD",*/
                "BenhChinhYHHD",
                "BenhKemTheoYHHD_1",
                "BenhKemTheoYHHD_2",
                "BenhKemTheoYHHD_3",
                /*"BenhKemTheoYHHD_4",
                "BenhKemTheoYHHD_5",
                "BenhKemTheoYHHD_6",
                "BenhKemTheoYHHD_7",
                "BenhKemTheoYHHD_8",
                "BenhKemTheoYHHD_9",
                "ThuThuatYHHD",
                "PhauThuatYHHD",*/
                "RaVienBenhChinhYHHD",
                "RaVienBenhKemTheoYHHD",
                /*"RaVienTaiBien",
                "RaVienBienChung",
                "NoiChuyenDenYHCT",*/
                "BenhChinhYHCT",
                "BenhKemTheoYHCT_1",
                "BenhKemTheoYHCT_2",
                "BenhKemTheoYHCT_3",
                /*"BenhKemTheoYHCT_4",
                "BenhKemTheoYHCT_5",
                "BenhKemTheoYHCT_6",
                "BenhKemTheoYHCT_7",
                "BenhKemTheoYHCT_8",
                "BenhKemTheoYHCT_9",
                "ThuThuatYHCT",
                "PhauThuatYHCT",
                "RaVienTaiBien",
                "RaVienBienChung",*/
                "RaVienBenhChinhYHCT",
                "RaVienBenhKemTheoYHCT",
                /*"GiamDoc",
                "TruongKhoa",
                "BsKhamBenh",
                "LyDoVaoVien",
                "BenhSu",
                "BanThan",
                "DacDiem",
                "GiaDinh",
                "ToanThan",
                // bang mach
                "mach",
                "nhietdo",
                "huyetap",
                "nhiptho",
                "cannang",
                "chieucao",
                "bmi",
                "TuanHoan",
                "HoHap",
                "TieuHoa",
                "TietNieu",
                "ThanKinh",
                "CoXuongKhop",
                "TaiMuiHong",
                "RangHamMat",
                "Mat",
                "NoiTiet",
                "TomTatCls",
                "TomTatBenhAn",
                "BenhKemTheoYHHD",
                "PhanBiet",
                "VongChan",
                "VanChan",
                "VaanChan",
                "XuaChan",
                "MachTayTrai",
                "MachTayPhai",
                "TomTatTuChuan",
                "BienChuong",
                "BenhDanh",
                "BatCuong",
                "NguyenNhan",
                "TangPhu",
                "KinhMach",
                "DinhViBenh",
                "PhapDieuTri",
                "PhuongDuoc",
                "KhongDungThuoc",
                "HuongDieuTri",
                "DuHau",*/
                "TongKetLyDoVaoVien",
                "TongKetQuaTrinhBenh",
                "KetQuaXetNghiemCanLamSang",
                "TinhTrangRaVien",
                "HuongDieuTriCheDoTiep",
                "TongKetBenhAnPPDTYHHD",
                "TongKetBenhAnPPDTYHCT",
                /*"VongChanMTKhac",
                "VanChanMoTaKhac",
                "VaanChanMTKhac",
                "ThietChanMTKhac",
                "PhuongPhapDieuTriKhongDungThuoc",
                "PhuongPhapKhac",
                "YHocHienDai",
                "NgayTuVong",
                "NguyenNhanTuVong",
                "KhamNghiemTuThi",
                "ChanDoanGiaiPhauTuThi",*/
				/*tổng kết bệnh án*/
				"soTongKetXQuang",
                "soTongKetCT",
                "soTongKetMRI",
                "soToSieuAm",
                "soToXetNghiem",
                //"tongKetKhac",
                "soTongKetKhac",
                "soTongKetToanHoSo",
                "HoTenNguoiGiao",
                "HoTenNhan",
                "HoTenThay",
                "NgayThangTongKet",
                //"soToSieuAm",
                //"soToXetNghiem",
				/*thông tin chung bệnh án*/
               /* "NgayKy",
                "NgayGioLamBenh",
                "ThayThuocLamBenh",
                "BenhChinh",
                "VaoVienLanThu",
                "solanPT",
                "NgayThang",
                "TongSoNgayDtsauPT",
                "KhoaKhamBenhYHCT",
                "KhoaKhamBenhYHHD",
                "MoTa",
                //yhct
                "MoTaKhac",
                "MoTaKhac2",
                "MoTaKhac3",
                "XucChan",
                "TangPhu",
                "KinhMach",*/
                 "VaoVienBenhKemTheoYHCT",
                "VaoVienBenhKemTheoYHHD"
            };
                List<string> values = new List<string>(){
                /*PrintSetting.BenhVien,
                PrintSetting.SoYTe,
                benhAn3?.Khoa?.TenKhoa,
                benhAn3?.Buong?.TenBuong,
                benhAn3?.BenhAn?.SoVaoVien,
                benhAn3?.BenhAn?.SoLuuTru,
                benhAn3?.BenhNhan?.MaBn,
                benhAn3?.BenhNhan?.MaBn,
                benhAn3?.BenhAn?.MaYt,
                benhAn3?.BenhNhan?.HoTen?.ToUpper(),
                benhAn3?.DanToc?.TenDanToc,
                benhAn3?.NgheNghiep?.TenNn,
                benhAn3?.QuocGia?.TenQg,
                benhAn3?.BenhNhan?.SoNha,
                benhAn3?.BenhNhan?.Thon,
                benhAn3?.PhuongXa?.TenPxa,
                benhAn3?.QuanHuyen?.TenQh,
                benhAn3?.Tinh?.TenTinh,
                benhAn3?.BenhNhan?.NoiLamViec,
                benhAn3?.BenhNhan?.Gtbhytdn?.ToString("dd/MM/yyyy"),
                benhAn3?.BenhNhan?.LienHe,
                benhAn3?.BenhNhan?.SoDienThoai,
                benhAn3?.BenhNhan?.GioiTinh == 1 ? "x" : null,
                benhAn3?.BenhNhan?.GioiTinh == 2 ? "x": null,
                PrintHelper.TimeText(benhAn3?.BenhAn?.NgayVv),
                PrintHelper.DateText(benhAn3?.BenhAn?.NgayVv),
                benhAn3?.benhVienChuyenDen?.TenBv,
                PrintHelper.TimeText(benhAn3?.BenhAn?.NgayRv),
                PrintHelper.DateText(benhAn3?.BenhAn?.NgayRv),
                benhAn3?.BenhTatNoiChuyenDen?.TenBenh,*/
                !String.IsNullOrEmpty(benhAn3?.BenhAn?.TenBenhChinhVv) ? PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhChinhVv) : PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhChinhVv),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv1 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv2 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv3 ),
               /* PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv4 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv5 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv6 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv7 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv8 ),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv9 ),
                benhAn3?.BenhAn?.ThuThuatYhhd == 1 ? "x" : "",
                benhAn3?.BenhAn?.PhauThuatYhhd == 1 ? "x" : "",*/
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhChinhRv),
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv1), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv2), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv3),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv4),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv5),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv6),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv7),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv8),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv9)),
                 //benhAn3?.BenhAn?.TaiBienYhhd == 1 ? "x" : "",
                // benhAn3?.BenhAn?.BienChungYhhd == 1 ? "x" : "",
                // PrintHelper.Trimmer(benhAn3?.BenhNoiChuyenDenYHCT?.TenBenhIcd),
                  !String.IsNullOrEmpty(benhAn3?.BenhAn?.TenBenhChinhVvyhct) ? PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhChinhVvyhct) : PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhChinhVvyhct),
                 PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv1yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv2yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv3yhct),
                /*PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv4yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv5yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv6yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv7yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv8yhct),
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv9yhct),

                benhAn3?.BenhAn?.ThuThuatYhct == 1 ? "x" : "",
                benhAn3?.BenhAn?.PhauThuatYhct == 1 ? "x" : "",
                benhAn3?.BenhAn?.TaiBienYhct == 1 ? "x" : "",
                benhAn3?.BenhAn?.BienChungYhct == 1 ? "x" : "",*/
                benhAn3?.BenhAn?.TenBenhChinhRvyhct,
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv1yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv2yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv3yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv4yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv5yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv6yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv7yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv8yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemRv9yhct)),
                /*benhAn3?.GiamDoc?.HoTen,
                benhAn3?.TruongKhoa?.HoTen,
                benhAn3?.BsDieutri?.HoTen,
                benhanYHHD?.LyDoVv,
                benhanYHHD?.BenhSu, //BenhSu
				tienSuBenh?.TienSuBanThan,
                tienSuBenh?.DacDiemLienQuanBenh, //DacDiem
				tienSuBenh?.TienSuGiaDinh,
                benhanYHHD?.ToanThan,
                benhanYHHD?.Mach?.ToString(),
                benhanYHHD?.NhietDo?.ToString(),
                benhanYHHD?.HuyetAp?.ToString(),
                benhanYHHD?.NhipTho?.ToString(),
                ((int?)benhanYHHD?.CanNang)?.ToString(),
                ((int?)benhanYHHD?.ChieuCao)?.ToString(),
                benhanYHHD?.Bmi?.ToString("0.00"),
                benhanYHHD?.TuanHoan,
                benhanYHHD?.HoHap,
                benhanYHHD?.TieuHoa,
                benhanYHHD?.ThanTnieuSduc, //TietNieu
				benhanYHHD?.ThanKinh,
                benhanYHHD?.XuongKhop,
                benhanYHHD?.TaiMuiHong,
                benhanYHHD?.RangHamMat,
                benhanYHHD?.Mat,
                benhanYHHD?.NoiTietDd,
                benhanYHHD?.CanLamSang,
                benhanYHHD?.TomTatBenhAn,
                "",//BenhKemTheoYHHD
				benhanYHHD?.DmBenhPhanBiet != null ? $"{benhanYHHD?.DmBenhPhanBiet?.MaBenh} - {benhanYHHD?.DmBenhPhanBiet?.TenBenh}" : "", //PhanBiet
				benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MtvaanChan,
                benhanYHCT?.MtthietChan,
                benhanYHCT?.MachTrai,
                benhanYHCT?.MachPhai,
                benhanYHCT?.TomTatTuChan,
                benhanYHCT?.BienChungLuanTri,
                $"{benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh} - {benhanYHCT?.DmBenhDanhTheoYHCT?.TenBenh}",
                benhanYHCT?.BatCuong,
                benhanYHCT?.MoTaNguyenNhan,
                benhanYHCT?.MoTaTangPhu,
                benhanYHCT?.MoTaKinhMach,
                benhanYHCT?.MoTaDinhViBenhTheo,
                benhanYHCT?.Ppdtyhct,
                benhanYHCT?.PhuongDuoc,
                benhanYHCT?.PpdtkhongDungThuoc,
                benhanYHHD?.HuongDtyhhd, //Hướng điều trị
				benhanYHHD?.TienLuong, //Dư hậu,*/
				tongKetBenhAn?.LyDoVv,
                tongKetBenhAn?.QuaTrinhBenhLy,
                tongKetBenhAn?.TomTatKetQuaCls,
                tongKetBenhAn?.TinhTrangBnrv,
                tongKetBenhAn?.HuongDt,
                tongKetBenhAn?.PpdttheoYhhd,
                tongKetBenhAn?.PpdttheoYhct,
                /*benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MtvaanChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MoTaXucChan,
                benhanYHCT?.PpdtkhongDungThuoc,
                benhanYHCT?.Ppkhac,
                benhanYHHD?.Ppdtyhhd,
                PrintHelper.DateTimeText(benhAn3?.BenhAn?.NgayTuVong),
                benhAn3?.BenhAn?.NguyenNhanTuVong,
                benhAn3?.BenhAn?.KhamNghiemTuThi == 1 ? "x" : "",
                benhAn3?.BenhGPTuThis?.TenBenh,*/
                tongKetBenhAn?.SoToXquang > 0 ? tongKetBenhAn?.SoToXquang.ToString() : "",
                tongKetBenhAn?.SoToCt > 0 ? tongKetBenhAn?.SoToCt.ToString() : "" ,
                tongKetBenhAn?.SoToMri > 0 ? tongKetBenhAn?.SoToMri.ToString() : "",
                tongKetBenhAn?.SoToSa > 0 ? tongKetBenhAn?.SoToSa.ToString() : "",
                tongKetBenhAn?.SoToXn > 0 ? tongKetBenhAn?.SoToXn.ToString() : "",
                //String.IsNullOrEmpty(tongKetBenhAn?.Khac)? "Khác ...." : tongKetBenhAn?.Khac,
                tongKetBenhAn?.SoToKhac > 0 ? tongKetBenhAn?.SoToKhac.ToString() : "",
                tongKetBenhAn?.SoToToanBoHs > 0 ? tongKetBenhAn?.SoToToanBoHs.ToString() : "",
                tongKetBenhAn?.DmNguoiGiao?.HoTen,
                tongKetBenhAn?.DmNguoiNhan?.HoTen,
                tongKetBenhAn?.DmBsDieutri?.HoTen,
                PrintHelper.DateText(tongKetBenhAn?.NgayKy),
                /*tongKetBenhAn?.SoToSa > 0 ? tongKetBenhAn?.SoToSa.ToString() : "",
                tongKetBenhAn?.SoToXn > 0 ? tongKetBenhAn?.SoToXn.ToString() : "",
                PrintHelper.DateText(benhAn3?.BenhAn?.NgayKy),
                //yhhd
                PrintHelper.DateTimeText(benhanYHHD?.NgayKham), //NgayGioLamBenh
				benhanYHHD?.DmBskham?.HoTen, //ThayThuocLamBenh
                PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhChinhVv),
                 benhAn3?.BenhAn?.Vvlan?.ToString(),
                 benhAn3?.BenhAn?.TongSoLanPt.ToString(),
                 benhAn3?.BenhAn?.TongSoNgayDtsauPt?.ToString(),
                 PrintHelper.DateText(benhAn3?.BenhAn?.NgayKy),
                benhAn3?.BenhKKBYHCT?.TenBenhIcd,
                benhAn3?.BenhKKBYHHD?.TenBenh,
                tienSuBenh?.MoTaTienSu,
                //yhct
                //MoTaKhac
                benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MtthietChan,
                benhanYHCT?.MoTaXucChan,
                benhanYHCT?.MoTaTangPhu,
                benhanYHCT?.MoTaKinhMach,*/
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv1yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv2yhct), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv3yhct),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv4yhct),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv5yhct),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv6yhct),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv7yhct),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv8yhct),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv9yhct)),
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv1 ), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv2), PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv3),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv4),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv5),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv6),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv7),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv8),PrintHelper.Trimmer(benhAn3?.BenhAn?.TenBenhKemVv9)),

                };
                var combo_ds = _benhAnRepository._context.DmbaCombods.Where(x => maDsCombo.Contains(x.MaParent)).OrderBy(x => x.MaParent).ThenBy(x => x.Ma).ToList();

                /* PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.NgheNghiep?.MaNn, "Nghenghiep", 2, '0', true);
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.DanToc?.MaDanToc, "dantoc", 2, '0', true);
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.QuanHuyen?.MaBhxh, "huyen", 3, '0', true);
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.PhuongXa?.MaBhxh, "xaphuong", 5, '0', true);
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.Tinh?.MaTinh, "thanhpho", 2, '0', true);

                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, PrintHelper.BirthText(benhAn3?.BenhNhan?.NgaySinh), "NgaySinh", 8, ' ');
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhNhan?.Tuoi?.ToString(), "Tuoi", 2, '0');
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhAn?.TongSoNgayDt.ToString(), "TongSoNgayDT", 2, '0');
                 PrintHelper.TextboxFieldDoiTuongHanlder(ref fields, ref values, benhAn3?.DoiTuong?.MaDt);
                 PrintHelper.TextboxFieldBHYTHanlder(ref fields, ref values, benhAn3?.BenhNhan?.SoTheBhyt, "Bhyt", false);
                 PrintHelper.OptionFieldHanlder(ref fields, ref values, "tructiepvao", benhAn3?.BenhAn?.TrucTiepVao, new string[] { "1", "2", "3" });
                 PrintHelper.OptionFieldHanlder(ref fields, ref values, "NoiGioiThieu", benhAn3?.BenhAn?.NoiGt, new string[] { "1", "2", "3" });
                 PrintHelper.OptionFieldHanlder(ref fields, ref values, "ChuyenVien", benhAn3?.BenhAn?.ChuyenVien, new string[] { "1", "2", "3" });
                 PrintHelper.OptionFieldHanlder(ref fields, ref values, "RaVien", benhAn3?.BenhAn?.HtraVien, new string[] { "1", "2", "3", "4" });
                 PrintHelper.OptionFieldHanlder(ref fields, ref values, "nguyennhan", benhAn3?.BenhAn?.NguyenNhanTBBC, new string[] { "1", "2", "3", "4" });
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhTatNoiChuyenDen?.MaBenh, "Noichuyenden", 5, ' ');
                 PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhKKBYHHD?.MaBenh, "KhoaKhamBenh", 5, ' ');*/
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhAn?.MaBenhChinhVv, "BenhChinh", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv1, "BenhKemTheo_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv2, "BenhKemTheo_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv3, "BenhKemTheo_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv4, "BenhKemTheo_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv5, "BenhKemTheo_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv6, "BenhKemTheo_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv7, "BenhKemTheo_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv8, "BenhKemTheo_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv9, "BenhKemTheo_9", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhAn?.MaBenhChinhRv, "MaBenhChinhRv", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv1, "RaVienBenhKemTheoYHHD_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv2, "RaVienBenhKemTheoYHHD_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv3, "RaVienBenhKemTheoYHHD_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv4, "RaVienBenhKemTheoYHHD_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv5, "RaVienBenhKemTheoYHHD_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv6, "RaVienBenhKemTheoYHHD_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv7, "RaVienBenhKemTheoYHHD_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv8, "RaVienBenhKemTheoYHHD_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv9, "RaVienBenhKemTheoYHHD_9", 1, ' ');
                // PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhNoiChuyenDenYHCT?.MaBenh, "NoichuyendenYHCT", 1, ' ');
                // PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhKKBYHCT?.MaBenh, "KhoaKhamBenhYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhAn?.MaBenhChinhVvyhct, "BenhChinhYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv1yhct, "BenhKemTheoYHCT_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv2yhct, "BenhKemTheoyhct_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv3yhct, "BenhKemTheoyhct_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv4yhct, "BenhKemTheoyhct_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv5yhct, "BenhKemTheoyhct_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv6yhct, "BenhKemTheoyhct_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv7yhct, "BenhKemTheoyhct_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv8yhct, "BenhKemTheoyhct_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemVv9yhct, "BenhKemTheoyhct_9", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhAn?.MaBenhChinhRvyhct, "RaVienBenhChinh", 1, ' ');
                // PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh, "ChanDoanBenhDanh", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv1yhct, "RaVienBenhKeoTheo_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv2yhct, "RaVienBenhKeoTheo_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv3yhct, "RaVienBenhKeoTheo_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv4yhct, "RaVienBenhKeoTheo_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv5yhct, "RaVienBenhKeoTheo_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv6yhct, "RaVienBenhKeoTheo_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv7yhct, "RaVienBenhKeoTheo_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv8yhct, "RaVienBenhKeoTheo_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn3?.BenhAn?.MaBenhKemRv9yhct, "RaVienBenhKeoTheo_9", 1, ' '); ;
                PrintHelper.OptionFieldValueHanlder(ref fields, ref values, "KQDT", benhAn3?.BenhAn?.Kqdt, new string[] { "1", "2", "3", "4", "5" });
                // PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "KQDT_TK", tongKetBenhAn.Kqdt, 1, getDsCombo(combo_ds, "130"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanHinhThai", benhanYHCT.HinhThaiVongChan, 3, getDsCombo(combo_ds, "045"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanThan", benhanYHCT.ThanVongChan, 2, getDsCombo(combo_ds, "046"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanSac", benhanYHCT.SacVongChan, 3, getDsCombo(combo_ds, "047"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanTrach", benhanYHCT.TrachVongChan, 2, getDsCombo(combo_ds, "048"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanLuoi", benhanYHCT.HinhThaiLuoi, 2, getDsCombo(combo_ds, "049"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanChatLuoi", benhanYHCT.ChatLuoi, 3, getDsCombo(combo_ds, "050")); //old 6
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanReuLuoi", benhanYHCT.ReuLuoi, 3, getDsCombo(combo_ds, "051")); //old 6
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAmThanhTiengNoi", benhanYHCT.GiongNoi, 5, getDsCombo(combo_ds, "056"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAmThanhHoiTho", benhanYHCT.HoiTho, 5, getDsCombo(combo_ds, "057"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoAmThanhHo", PrintHelper.HanlderBooleanType(benhanYHCT.CoHo), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAmThanhHo", benhanYHCT.Ho, 3, getDsCombo(combo_ds, "059"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAmThanhO", PrintHelper.HanlderBooleanType(benhanYHCT.OamThanh), 1);
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAmThanhNac", PrintHelper.HanlderBooleanType(benhanYHCT.NacAmThanh), 1);
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoMui", PrintHelper.HanlderBooleanType(benhanYHCT.CoMui), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanMui", benhanYHCT.KieuMui, 3, getDsCombo(combo_ds, "063"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoChatThai", PrintHelper.HanlderBooleanType(benhanYHCT.CoChatThaiBenhLy), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanChatThai", benhanYHCT.KieuChatThai, 3, getDsCombo(combo_ds, "065"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoHanNhiet", PrintHelper.HanlderBooleanType(benhanYHCT.BhhanNhiet), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanHanNhiet", benhanYHCT.Hannhiet, 3, getDsCombo(combo_ds, "067"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanMoHoi", benhanYHCT.MoHoi, 3, getDsCombo(combo_ds, "068"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoDauMatCo", PrintHelper.HanlderBooleanType(benhanYHCT.BhdauMatCo), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoDauDau", benhanYHCT.DauDau, 3, getDsCombo(combo_ds, "070"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanDauMatCoHoaMatChongMat", PrintHelper.HanlderBooleanType(benhanYHCT.HoaMat), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoMat", benhanYHCT.Mat, 3, getDsCombo(combo_ds, "072"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoTai", benhanYHCT.Tai, 3, getDsCombo(combo_ds, "073"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoMui", benhanYHCT.Mui, 3, getDsCombo(combo_ds, "074"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoHong", benhanYHCT.Hong, 3, getDsCombo(combo_ds, "075"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoCoVai", benhanYHCT.CoVai, 3, getDsCombo(combo_ds, "076"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoLung", PrintHelper.HanlderBooleanType(benhanYHCT.Bhlung), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanLung", benhanYHCT.Lung, 4, getDsCombo(combo_ds, "078"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoNguc", PrintHelper.HanlderBooleanType(benhanYHCT.Bhnguc), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanNguc", benhanYHCT.Nguc, 6, getDsCombo(combo_ds, "080"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanBung", PrintHelper.HanlderBooleanType(benhanYHCT.Bhbung), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanBung", benhanYHCT.Bung, 6, getDsCombo(combo_ds, "082"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanChanTay", PrintHelper.HanlderBooleanType(benhanYHCT.BhchanTay), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanChanTay", benhanYHCT.ChanTay, 6, getDsCombo(combo_ds, "084"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAn", PrintHelper.HanlderBooleanType(benhanYHCT.Bhan), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAn", benhanYHCT.An, 3, getDsCombo(combo_ds, "086"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanUong", PrintHelper.HanlderBooleanType(benhanYHCT.Bhuong), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanUong", benhanYHCT.Uong, 3, getDsCombo(combo_ds, "088"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanDaiTieuTien", PrintHelper.HanlderBooleanType(benhanYHCT.BhdaiTieuTien), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanTieuTien", benhanYHCT.TieuTien, 3, getDsCombo(combo_ds, "090"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanDaiTien", benhanYHCT.DaiTien, 3, getDsCombo(combo_ds, "091"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanNgu", PrintHelper.HanlderBooleanType(benhanYHCT.Bhngu), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanNgu", benhanYHCT.Ngu, 3, getDsCombo(combo_ds, "093"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSan", PrintHelper.HanlderBooleanType(benhanYHCT.RlknsinhDuc), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNam", benhanYHCT.Rlnam, 3, getDsCombo(combo_ds, "095"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNu", benhanYHCT.Rlnu, 3, getDsCombo(combo_ds, "096"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuKinhNguyet", PrintHelper.HanlderBooleanType(benhanYHCT.Bhkn), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuKinhNguyet", benhanYHCT.Rlkinhnguyet, 2, getDsCombo(combo_ds, "098"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuThongKinh", benhanYHCT.ThongKinh, 2, getDsCombo(combo_ds, "099"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuDoiHa", PrintHelper.HanlderBooleanType(benhanYHCT.BhdoiHa), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuDoiHa", benhanYHCT.DoiHa, 2, getDsCombo(combo_ds, "101"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanLienQuan", PrintHelper.HanlderBooleanType(benhanYHCT.Dkxhbenh), 1);
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuan", PrintHelper.HanlderBooleanType(benhanYHCT.BhxucChan), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanDa", benhanYHCT.XucChanDa, 3, getDsCombo(combo_ds, "105"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanCoXuong", PrintHelper.HanlderBooleanType(benhanYHCT.BhcoXuongKhop), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanCoXuong", benhanYHCT.XucChanCoXuongKhop, 3, getDsCombo(combo_ds, "107"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanBung", PrintHelper.HanlderBooleanType(benhanYHCT.BhbungXucChan), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanBung", benhanYHCT.XucChanBung, 3, getDsCombo(combo_ds, "109"));
                //PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanMoHoi", PrintHelper.HanlderBooleanType(benhanYHCT.BhmoHoi), 1);
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMoHoi", benhanYHCT.XucChanMoHoi, 3, getDsCombo(combo_ds, "111"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuan", benhanYHCT.MachChan, 3, getDsCombo(combo_ds, "112"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanTongKhamPhai", benhanYHCT.TongKhanPhai, 3, getDsCombo(combo_ds, "113"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanTongKhamTrai", benhanYHCT.TongKhanTrai, 3, getDsCombo(combo_ds, "114"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiThon", benhanYHCT.ViKhanTraiThon, 3, getDsCombo(combo_ds, "115"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiQuan", benhanYHCT.ViKhanTraiQuan, 3, getDsCombo(combo_ds, "116"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiXich", benhanYHCT.ViKhanTraiXich, 3, getDsCombo(combo_ds, "117"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiThon", benhanYHCT.ViKhanPhaiThon, 3, getDsCombo(combo_ds, "118"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiQuan", benhanYHCT.ViKhanPhaiQuan, 3, getDsCombo(combo_ds, "119"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiXich", benhanYHCT.ViKhanPhaiXich, 3, getDsCombo(combo_ds, "120"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanBatCuong", benhanYHCT.BhbatCuong, 5, getDsCombo(combo_ds, "121"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanNguyenNhan", benhanYHCT.NguyenNhan, 1, getDsCombo(combo_ds, "122"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanTangPhu", benhanYHCT.TangPhu, 4, getDsCombo(combo_ds, "123"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanKinhMach", benhanYHCT.KinhMach, 4, getDsCombo(combo_ds, "124"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanDinhViBenh", benhanYHCT.DinhViBenhTheo, 4, getDsCombo(combo_ds, "125"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "LungNhi", benhanYHCT.Lung, 3, getDsCombo(combo_ds, "201"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "NamGioiNhi", benhanYHCT.Rlnam, 3, getDsCombo(combo_ds, "203"));

                //tinh trang ra vien
                // PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVKQDT", benhAn3?.BenhAn?.Kqdt, PrintHelper.CreateArrayIncreate(5));
                // PrintHelper.OptionFieldHanlder(ref fields, ref values, "GiaiPhauBenh", benhAn3?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
                // PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVTTTV", benhAn3?.BenhAn?.TinhTrangTuVong, PrintHelper.CreateArrayIncreate(6));
                // PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVGPB", benhAn3?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
                // PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn3?.BenhGPTuThis?.MaBenh, "ChuanDoanGiaiPhau", 5, ' ');


                // PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "BanThan", tienSuBenh?.MaTienSu, 6, getDsCombo(combo_ds, "017"));


                // fields.Add("Khoa");
                // fields.Add("VaoKhoaNgayGio");
                // if (benhAnKhoaDieuTri != null)
                // {
                //     values.Add(benhAnKhoaDieuTri?.Khoa?.TenKhoa);
                //     values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTri?.NgayVaoKhoa));
                // }
                // else
                // {
                //     values.Add("");
                //     values.Add("");
                //     values.Add("");
                // }
                // to dieu tri
                var toDieuTriService = new BenhAnToDieuTriService();

                var dataset = new DataSet();
                var list = new List<DictionaryEntry>();
                if (benhAn3.LoaiBenhAn.MaLoaiBa == 3)
                {
                    ManTinhPrint(ref fields, ref values, benhAn3, benhAnKhoaDieuTris, phieukhamvv, tienSuBenh, benhanYHHD, benhanYHCT, tongKetBenhAn, combo_ds);

                }
                else
                {
                    for (int i = 1; i < benhAnKhoaDieuTris.Count; i++)
                    {
                        fields.Add($"KhoaChuyen{i}");
                        values.Add(benhAnKhoaDieuTris[i]?.Khoa?.TenKhoa);
                        fields.Add($"NgayGioKhoaChuyen{i}");
                        values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTris[i]?.NgayVaoKhoa));
                        var compareDate = i != (benhAnKhoaDieuTris.Count - 1) ? PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i]?.NgayVaoKhoa, benhAnKhoaDieuTris[i + 1]?.NgayVaoKhoa, 1) : PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i]?.NgayVaoKhoa, benhAn3?.BenhAn?.NgayRv, 1);

                        PrintHelper.TexboxFieldHanlder(ref fields, ref values, compareDate, $"MaKhoaChuyen{i}", 2, '0');
                    }
                }
                string idba_stt = idba + "_3";
                string path = PrintHelper.PrintFileWithTable(idba_stt, _hostingEnvironment, template3, null, null, fields, values);
                return path;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        // in tờ thứ hai 
        public string Print2(decimal idba)
        {
            string mss = "";
            try
            {

                if (idba < 0 || idba == 0)
                {
                    return "";
                }
                //var benhAn2 = Detail(idba);
                var benhAn2 = DetaiBenhAn(idba);
                mss += "\n 2169";
                var template2 = "";

                switch (benhAn2.LoaiBenhAn.MaLoaiBa)
                {
                    case 1:
                        template2 = "Benhan_YHCT_noi_tru-cp_To2.docx";
                        break;
                    case 2:
                        // code block
                        template2 = "Benhan_YHCT_ngoai_tru_v1_To2.docx";
                        break;
                    case 3:
                        template2 = "Benh_an_YHCT_man_tinh_To2.docx";
                        break;
                    case 4:
                        template2 = "Benhan_YHCT_noi_tru_theo_ngay_To2.docx";
                        break;
                    case 5:
                        template2 = "Benhan_Noikhoa_To2.docx";
                        break;
                    case 6:
                        template2 = "Benhan_Ngoaikhoa_To2.docx";
                        break;
                    case 7:
                        template2 = "Benhan_YHCT_noi_tru-nhi_To2.docx";
                        break;
                    //break;
                    default:
                        // code block
                        throw new HttpStatusException(HttpStatusCode.UnsupportedMediaType, "Chưa hỗ trợ");
                }

                mss += "\n 2202";
                var benhAnKhoaDieuTriService = new BenhAnKhoaDieuTriService();
                //benhAnKhoaDieuTri
                var benhAnKhoaDieuTris = benhAnKhoaDieuTriService.Get(new BenhAnKhoaDieuTriParameters()
                {
                    Idba = idba
                }).data.OrderBy(x => x.Stt).ToList();
                mss += "\n 2209";
                var benhAnKhoaDieuTri = benhAnKhoaDieuTris.Count > 0 ? benhAnKhoaDieuTris[0] : null;
                //benhanKhamVV
                var phieukhamvv = _benhAnRepository._context.BenhAnKhamVaoVien.Where(
                    x => x.Idba == idba
                ).FirstOrDefault();
                phieukhamvv = phieukhamvv ?? new BenhAnKhamVaoVien();
                //benhanTienSuBenh
                var tienSuBenh = _benhAnRepository._context.BenhAnTienSuBenh.FirstOrDefault(x => x.Idba == idba);
                tienSuBenh = tienSuBenh ?? new BenhAnTienSuBenh();
                //benhanYHCT
                var benhanYHHD = _benhAnRepository._context.BenhAnKhamYhhd.Include(x => x.DmBskham).Include(x => x.DmBenhPhanBiet).FirstOrDefault(x => x.Idba == idba);
                benhanYHHD = benhanYHHD ?? new BenhAnKhamYhhd();
                //benhanYHCT
                Mapper.CreateMap<BenhAnKhamYhctDto, BenhAnKhamYhct>().ReverseMap();
                Mapper.CreateMap<DmbenhTatYhctDto, DmbenhTatYhct>().ReverseMap();
                var benhanYHCT = _benhAnRepository._context.BenhAnKhamYhct.Include(x => x.DmBenhDanhTheoYHCT).Select(Mapper.Map<BenhAnKhamYhctDto>).FirstOrDefault(x => x.Idba == idba);
                benhanYHCT = benhanYHCT ?? new BenhAnKhamYhctDto();
                //TongKetBenhAn
                Mapper.CreateMap<BenhAnTongKetBenhAnDto, BenhAnTongKetBenhAn>().ReverseMap();
                Mapper.CreateMap<DmnhanVienDto, DmnhanVien>().ReverseMap();
                var tongKetBenhAn = _benhAnRepository._context.BenhAnTongKetBenhAn
                    .Include(x => x.DmBsDieutri)
                    .Include(x => x.DmNguoiGiao)
                    .Include(x => x.DmNguoiNhan)
                    .Select(Mapper.Map<BenhAnTongKetBenhAnDto>)
                    .FirstOrDefault(x => x.Idba == idba);
                tongKetBenhAn = tongKetBenhAn ?? new BenhAnTongKetBenhAnDto();

                List<string> fields = new List<string>(){
                /*"BenhVien",
                "SoYTe",
                "Khoa",
                "Buong",
                "SoVVien",
                "SoLuuTru",
                "MaNgBenh",
                "MaNB",
                "MaYTe",
                "HoVaTen",
                "DanToc",
                "NgheNghiep",
                "QuocTich",
                "SoNha",
                "Thon",
                "Xa",
                "Huyen",
                "Tinh",
                "NoiLamViec",
                "GiaTriBaoHiemYTe",
                "LienHe",
                "SoDienThoai",
                "Nam",
                "Nu",
                "VaoVienGioPhut",
                "VaoVienNgayThang",
                "ChuyenDen",
                "RaVienGioPhut",
                "RaVienNgayThang",
                "NoiChuyenDenYHHD", */
                "BenhChinhYHHD",
                "BenhKemTheoYHHD_1",
                "BenhKemTheoYHHD_2",
                "BenhKemTheoYHHD_3",
                /*"BenhKemTheoYHHD_4",
                "BenhKemTheoYHHD_5",
                "BenhKemTheoYHHD_6",
                "BenhKemTheoYHHD_7",
                "BenhKemTheoYHHD_8",
                "BenhKemTheoYHHD_9",
                "ThuThuatYHHD",
                "PhauThuatYHHD",
                "RaVienBenhChinhYHHD",
                "RaVienBenhKemTheoYHHD",
                "RaVienTaiBien",
                "RaVienBienChung",
                "NoiChuyenDenYHCT",*/
                "BenhChinhYHCT",
                /*"BenhKemTheoYHCT_1",
                "BenhKemTheoYHCT_2",
                "BenhKemTheoYHCT_3",
                "BenhKemTheoYHCT_4",
                "BenhKemTheoYHCT_5",
                "BenhKemTheoYHCT_6",
                "BenhKemTheoYHCT_7",
                "BenhKemTheoYHCT_8",
                "BenhKemTheoYHCT_9",
                "ThuThuatYHCT",
                "PhauThuatYHCT",
                "RaVienTaiBien",
                "RaVienBienChung",
                "RaVienBenhChinhYHCT",
                "RaVienBenhKemTheoYHCT",
                "GiamDoc",
                "TruongKhoa",
                "BsKhamBenh",*/
                "LyDoVaoVien",
                "BenhSu",
                "BanThan",
                "DacDiem",
                "GiaDinh",
                "ToanThan",
                "mach",
                "nhietdo",
                "huyetap",
                "nhiptho",
                "cannang",
                "chieucao",
                "bmi",
                "TuanHoan",
                "HoHap",
                "TieuHoa",
                "TietNieu",
                "ThanKinh",
                "CoXuongKhop",
                "TaiMuiHong",
                "RangHamMat",
                "Mat",
                "NoiTiet",
                "TomTatCls",
                "TomTatBenhAn",
                "BenhKemTheoYHHD",
                "PhanBiet",
                //"MaBenhPhanBiet",
                "VongChan",
                "VanChan",
                "VaanChan",
               //  "XuaChan",
                "MachTayTrai",
                "MachTayPhai",
                "TomTatTuChuan",
                "BienChuong",
                "BenhDanh",
                "BatCuong",
                "NguyenNhan",
               //  "TangPhu",
               //  "KinhMach",
                "DinhViBenh",
                "PhapDieuTri",
                "PhuongDuoc",
                "KhongDungThuoc",
                "HuongDieuTri",
                "DuHau",
               //  "TongKetLyDoVaoVien",
               //  "TongKetQuaTrinhBenh",
               //  "KetQuaXetNghiemCanLamSang",
               //  "TinhTrangRaVien",
               //  "HuongDieuTriCheDoTiep",
               //  "TongKetBenhAnPPDTYHHD",
               //  "TongKetBenhAnPPDTYHCT",
                 "VongChanMTKhac",
                "VanChanMoTaKhac",
               //  "VaanChanMTKhac",
               //  "ThietChanMTKhac",
                "PhuongPhapDieuTriKhongDungThuoc",
                "PhuongPhapKhac",
               //  "YHocHienDai",
               //  "NgayTuVong",
               //  "NguyenNhanTuVong",
               //  "KhamNghiemTuThi",
               //  "ChanDoanGiaiPhauTuThi",
            // "soTongKetXQuang",
               //  "soTongKetCT",
               //  "soTongKetMRI",
               //  "soToSieuAm",
               //  "soToXetNghiem",
               //  "tongKetKhac",
               //  "soTongKetKhac",
               //  "soTongKetToanHoSo",
               //  "HoTenNguoiGiao",
               //  "HoTenNhan",
               //  "HoTenThay",
               //  "NgayThangTongKet",
               //  "soToSieuAm",
               //  "soToXetNghiem",
               //  "NgayKy",
                "NgayGioLamBenh",
                "ThayThuocLamBenh",
               //  "BenhChinh",
               //  "VaoVienLanThu",
               //  "solanPT",
               //  "NgayThang",
               //  "TongSoNgayDtsauPT",
               //  "KhoaKhamBenhYHCT",
               //  "KhoaKhamBenhYHHD",
                "MoTa",
                "MoTaKhac",
                "MoTaKhac2",
                "MoTaKhac3",
                "XucChan",
                "TangPhu",
                "KinhMach",
                "AnDuoi1Tuoi",
                "AnTren1Tuoi",
                "ThangCaiSua",
                "DacDiemSH",
                "ThangNoi",
                 "VaoVienBenhKemTheoYHCT",
                "VaoVienBenhKemTheoYHHD"
            };
                List<string> values = new List<string>(){
                 /*PrintSetting.BenhVien,
                 PrintSetting.SoYTe,
                 benhAn2?.Khoa?.TenKhoa,
                 benhAn2?.Buong?.TenBuong,
                 benhAn2?.BenhAn?.SoVaoVien,
                 benhAn2?.BenhAn?.SoLuuTru,
                 benhAn2?.BenhNhan?.MaBn,
                 benhAn2?.BenhNhan?.MaBn,
                 benhAn2?.BenhAn?.MaYt,
                 benhAn2?.BenhNhan?.HoTen?.ToUpper(),
                 benhAn2?.DanToc?.TenDanToc,
                 benhAn2?.NgheNghiep?.TenNn,
                 benhAn2?.QuocGia?.TenQg,
                 benhAn2?.BenhNhan?.SoNha,
                 benhAn2?.BenhNhan?.Thon,
                 benhAn2?.PhuongXa?.TenPxa,
                 benhAn2?.QuanHuyen?.TenQh,
                 benhAn2?.Tinh?.TenTinh,
                 benhAn2?.BenhNhan?.NoiLamViec,
                 benhAn2?.BenhNhan?.Gtbhytdn?.ToString("dd/MM/yyyy"),
                 benhAn2?.BenhNhan?.LienHe,
                 benhAn2?.BenhNhan?.SoDienThoai,
                 benhAn2?.BenhNhan?.GioiTinh == 1 ? "x" : null,
                 benhAn2?.BenhNhan?.GioiTinh == 2 ? "x": null,
                 PrintHelper.TimeText(benhAn2?.BenhAn?.NgayVv),
                 PrintHelper.DateText(benhAn2?.BenhAn?.NgayVv),
                 benhAn2?.benhVienChuyenDen?.TenBv,
                 PrintHelper.TimeText(benhAn2?.BenhAn?.NgayRv),
                 PrintHelper.DateText(benhAn2?.BenhAn?.NgayRv),
                 benhAn2?.BenhTatNoiChuyenDen?.TenBenh,*/
                 !String.IsNullOrEmpty(benhAn2?.BenhAn?.TenBenhChinhVv) ? PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhChinhVv) : PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhChinhVv),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv1 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv2 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv3 ),
                 /* PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv4 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv5 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv6 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv7 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv8 ),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv9 ),
                 benhAn2?.BenhAn?.ThuThuatYhhd == 1 ? "x" : "",
                 benhAn2?.BenhAn?.PhauThuatYhhd == 1 ? "x" : "",
                 PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhChinhRv),
                 PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv1), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv2), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv3),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv4),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv5),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv6),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv7),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv8),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv9)),
                 benhAn2?.BenhAn?.TaiBienYhhd == 1 ? "x" : "",
                 benhAn2?.BenhAn?.BienChungYhhd == 1 ? "x" : "",
                 PrintHelper.Trimmer(benhAn2?.BenhNoiChuyenDenYHCT?.TenBenhIcd),*/
                !String.IsNullOrEmpty(benhAn2?.BenhAn?.TenBenhChinhVvyhct) ? PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhChinhVvyhct) : PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhChinhVvyhct),
                /*PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv1yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv2yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv3yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv4yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv5yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv6yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv7yhct),
                  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv8yhct),
                 PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv9yhct),
                benhAn2?.BenhAn?.ThuThuatYhct == 1 ? "x" : "",
                benhAn2?.BenhAn?.PhauThuatYhct == 1 ? "x" : "",
                benhAn2?.BenhAn?.TaiBienYhct == 1 ? "x" : "",
                benhAn2?.BenhAn?.BienChungYhct == 1 ? "x" : "",
                benhAn2?.BenhAn?.TenBenhChinhRvyhct,
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv1yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv2yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv3yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv4yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv5yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv6yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv7yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv8yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemRv9yhct)),
                benhAn2?.GiamDoc?.HoTen,
                 benhAn2?.TruongKhoa?.HoTen,
                 benhAn2?.BsDieutri?.HoTen,*/
                 benhanYHHD?.LyDoVv,
                 benhanYHHD?.BenhSu, //BenhSu
                tienSuBenh?.TienSuBanThan,
                 tienSuBenh?.DacDiemLienQuanBenh, //DacDiem
                tienSuBenh?.TienSuGiaDinh,
                 benhanYHHD?.ToanThan,
                 benhanYHHD?.Mach?.ToString(),
                 benhanYHHD?.NhietDo?.ToString(),
                 benhanYHHD?.HuyetAp?.ToString(),
                 benhanYHHD?.NhipTho?.ToString(),
                 ((int?)benhanYHHD?.CanNang)?.ToString(),
                 ((int?)benhanYHHD?.ChieuCao)?.ToString(),
                 benhanYHHD?.Bmi?.ToString("0.00"),
                 benhanYHHD?.TuanHoan,
                 benhanYHHD?.HoHap,
                 benhanYHHD?.TieuHoa,
                 benhanYHHD?.ThanTnieuSduc, //TietNieu
                 benhanYHHD?.ThanKinh,
                 benhanYHHD?.XuongKhop,
                 benhanYHHD?.TaiMuiHong,
                 benhanYHHD?.RangHamMat,
                 benhanYHHD?.Mat,
                 benhanYHHD?.NoiTietDd,
                 benhanYHHD?.CanLamSang,
                 benhanYHHD?.TomTatBenhAn,
                "",//BenhKemTheoYHHD
                benhanYHHD?.DmBenhPhanBiet != null ? $"{benhanYHHD?.DmBenhPhanBiet?.TenBenh}" : "", //PhanBiet
                //benhanYHHD?.DmBenhPhanBiet != null ? $"{benhanYHHD?.DmBenhPhanBiet?.MaBenh}" : "", //MaBenhPhanBiet
                benhanYHCT?.MoTaVongChan,
                 benhanYHCT?.MoTaVanChan,
                 benhanYHCT?.MtvaanChan,
                 //benhanYHCT?.MtthietChan,
                 benhanYHCT?.MachTrai,
                 benhanYHCT?.MachPhai,
                 benhanYHCT?.TomTatTuChan,
                 benhanYHCT?.BienChungLuanTri,
                 $"{benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh} - {benhanYHCT?.DmBenhDanhTheoYHCT?.TenBenhBHYT}",
                 benhanYHCT?.BatCuong,
                benhanYHCT?.MoTaNguyenNhan,
                //  benhanYHCT?.MoTaTangPhu,
                //  benhanYHCT?.MoTaKinhMach,
                 benhanYHCT?.MoTaDinhViBenhTheo,
                 benhanYHCT?.Ppdtyhct,
                 benhanYHCT?.PhuongDuoc,
                 benhanYHCT?.PpdtkhongDungThuoc,
                 benhanYHHD?.HuongDtyhhd, //Hướng điều trị
                benhanYHHD?.TienLuong, //Dư hậu,
                // tongKetBenhAn?.LyDoVv,
                //  tongKetBenhAn?.QuaTrinhBenhLy,
                //  tongKetBenhAn?.TomTatKetQuaCls,
                //  tongKetBenhAn?.TinhTrangBnrv,
                //  tongKetBenhAn?.HuongDt,
                //  tongKetBenhAn?.PpdttheoYhhd,
                //  tongKetBenhAn?.PpdttheoYhct,
                 benhanYHCT?.MoTaVongChan,
                  benhanYHCT?.MtvaanChan,
                //  benhanYHCT?.MoTaVanChan,
                //  benhanYHCT?.MoTaXucChan,
                 benhanYHCT?.PpdtkhongDungThuoc,
                 benhanYHCT?.Ppkhac,
                //  benhanYHHD?.Ppdtyhhd,
                //  PrintHelper.DateTimeText(benhAn2?.BenhAn?.NgayTuVong),
                //  benhAn2?.BenhAn?.NguyenNhanTuVong,
                //  benhAn2?.BenhAn?.KhamNghiemTuThi == 1 ? "x" : "",
                //  benhAn2?.BenhGPTuThis?.TenBenh,
                //  tongKetBenhAn?.SoToXquang?.ToString(),
                //  tongKetBenhAn?.SoToCt?.ToString(),
                //  tongKetBenhAn?.SoToMri?.ToString(),
                //  tongKetBenhAn?.SoToSa?.ToString(),
                //  tongKetBenhAn?.SoToXn?.ToString(),
                //  String.IsNullOrEmpty(tongKetBenhAn?.Khac) ? "Khác ...." : tongKetBenhAn?.Khac,
                //  tongKetBenhAn?.SoToKhac?.ToString(),
                //  tongKetBenhAn?.SoToToanBoHs?.ToString(),
                //  tongKetBenhAn?.DmNguoiGiao?.HoTen,
                //  tongKetBenhAn?.DmNguoiNhan?.HoTen,
                //  tongKetBenhAn?.DmBsDieutri?.HoTen,
                //  PrintHelper.DateText(tongKetBenhAn?.NgayKy),
                //  tongKetBenhAn?.SoToSa?.ToString(),
                //  tongKetBenhAn?.SoToXn?.ToString(),
                //  PrintHelper.DateText(benhAn2?.BenhAn?.NgayKy),
                 PrintHelper.DateTimeText(benhanYHHD?.NgayKham), //NgayGioLamBenh
                benhanYHHD?.DmBskham?.HoTen, //ThayThuocLamBenh
                //  PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhChinhVv),
                //  benhAn2?.BenhAn?.Vvlan?.ToString(),
                //  benhAn2?.BenhAn?.TongSoLanPt.ToString(),
                //  benhAn2?.BenhAn?.TongSoNgayDtsauPt?.ToString(),
                //  PrintHelper.DateText(benhAn2?.BenhAn?.NgayKy),
                //  benhAn2?.BenhKKBYHCT?.TenBenhBhyt,
                //  benhAn2?.BenhKKBYHHD?.TenBenh,
                 tienSuBenh?.MoTaTienSu,
                 benhanYHCT?.MoTaVongChan,
                 benhanYHCT?.MoTaVanChan,
                 benhanYHCT?.MtthietChan,
                 benhanYHCT?.MoTaXucChan,
                 benhanYHCT?.MoTaTangPhu,
                 benhanYHCT?.MoTaKinhMach,
                 tienSuBenh?.AnDuoi1Tuoi,
                 tienSuBenh?.AnTren1Tuoi,
                 tienSuBenh?.ThangCaiSua,
                 tienSuBenh?.DacDienSH,
                 tienSuBenh?.ThangNoi,
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv1yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv2yhct), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv3yhct),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv4yhct),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv5yhct),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv6yhct),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv7yhct),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv8yhct),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv9yhct)),
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv1 ), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv2), PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv3),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv4),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv5),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv6),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv7),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv8),PrintHelper.Trimmer(benhAn2?.BenhAn?.TenBenhKemVv9)),
                };
                mss += "\n 2580";
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.NgheNghiep?.MaNn, "Nghenghiep", 2, '0', true);
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.DanToc?.MaDanToc, "dantoc", 2, '0', true);
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.QuanHuyen?.MaBhxh, "huyen", 3, '0', true);
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.PhuongXa?.MaBhxh, "xaphuong", 5, '0', true);
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.Tinh?.MaTinh, "thanhpho", 2, '0', true);
                mss += "\n 2586";
                var combo_ds = _benhAnRepository._context.DmbaCombods.Where(x => maDsCombo.Contains(x.MaParent)).OrderBy(x => x.MaParent).ThenBy(x => x.Ma).ToList();
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, PrintHelper.BirthText(benhAn2?.BenhNhan?.NgaySinh), "NgaySinh", 8, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhNhan?.Tuoi?.ToString(), "Tuoi", 2, '0');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhAn?.TongSoNgayDt.ToString(), "TongSoNgayDT", 2, '0');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.ConThu.ToString(), "ConThu", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.DeDuThang.ToString(), "DeDuThang", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.DeKho.ToString(), "DeKho", 1, ' ');
                if (tienSuBenh.DeNgatTho.HasValue && tienSuBenh.DeNgatTho == 1)
                {
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.DeNgatTho.ToString(), "CoDeNgatTho", 1, ' ');

                }
                else
                {
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.DeNgatTho.ToString(), "KhongDeNgatTho", 1, ' ');
                }
                if (tienSuBenh.DaTiemChung.HasValue && tienSuBenh.DaTiemChung == 1)
                {
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.DaTiemChung.ToString(), "CoDaTiemChung", 1, '0');

                }
                else
                {
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.DeNgatTho.ToString(), "KhongDaTiemChung", 1, ' ');
                }
                if (tienSuBenh.BenhDaMac.HasValue && tienSuBenh.BenhDaMac == 1)
                {
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.BenhDaMac.ToString(), "CoBenhDaMac", 1, '0');

                }
                else
                {
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.BenhDaMac.ToString(), "KhongBenhDaMac", 1, ' ');
                }
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "LungNhi", benhanYHCT.Lung, 3, getDsCombo(combo_ds, "201"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "NamGioiNhi", benhanYHCT.Rlnam, 3, getDsCombo(combo_ds, "203"));

                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.CanNang.ToString(), "CanNang", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.RungRon.ToString(), "RungRon", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.ThangLay.ToString(), "ThangLay", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.ThangBo.ToString(), "ThangBo", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.ThangDi.ToString(), "ThangDi", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.ThangMocRang.ToString(), "ThangMocRang", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, tienSuBenh?.TuoiCoKinh.ToString(), "TuoiCoKinh", 2, '0');
                //  PrintHelper.TextboxFieldDoiTuongHanlder(ref fields, ref values, benhAn2?.DoiTuong?.MaDt);
                //  PrintHelper.TextboxFieldBHYTHanlder(ref fields, ref values, benhAn2?.BenhNhan?.SoTheBhyt, "Bhyt", false);
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "tructiepvao", benhAn2?.BenhAn?.TrucTiepVao, new string[] { "1", "2", "3" });
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "NoiGioiThieu", benhAn2?.BenhAn?.NoiGt, new string[] { "1", "2", "3" });
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "ChuyenVien", benhAn2?.BenhAn?.ChuyenVien, new string[] { "1", "2", "3" });
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "RaVien", benhAn2?.BenhAn?.HtraVien, new string[] { "1", "2", "3", "4" });
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "nguyennhan", benhAn2?.BenhAn?.NguyenNhanTBBC, new string[] { "1", "2", "3", "4" });
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhTatNoiChuyenDen?.MaBenh, "Noichuyenden", 5, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhKKBYHHD?.MaBenh, "KhoaKhamBenh", 5, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhAn?.MaBenhChinhVv, "BenhChinh", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhanYHHD?.DmBenhPhanBiet?.MaBenh, "MaBenhPhanBiet", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv1, "BenhKemTheo_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv2, "BenhKemTheo_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv3, "BenhKemTheo_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv4, "BenhKemTheo_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv5, "BenhKemTheo_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv6, "BenhKemTheo_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv7, "BenhKemTheo_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv8, "BenhKemTheo_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv9, "BenhKemTheo_9", 1, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhAn?.MaBenhChinhRv, "MaBenhChinhRv", 5, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv1, "RaVienBenhKemTheoYHHD_1", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv2, "RaVienBenhKemTheoYHHD_2", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv3, "RaVienBenhKemTheoYHHD_3", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv4, "RaVienBenhKemTheoYHHD_4", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv5, "RaVienBenhKemTheoYHHD_5", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv6, "RaVienBenhKemTheoYHHD_6", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv7, "RaVienBenhKemTheoYHHD_7", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv8, "RaVienBenhKemTheoYHHD_8", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv9, "RaVienBenhKemTheoYHHD_9", 1, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhNoiChuyenDenYHCT?.MaBenh, "NoichuyendenYHCT", 1, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhKKBYHCT?.MaBenh, "KhoaKhamBenhYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhAn?.MaBenhChinhVvyhct, "BenhChinhYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv1yhct, "BenhKemTheoYHCT_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv2yhct, "BenhKemTheoyhct_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv3yhct, "BenhKemTheoyhct_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv4yhct, "BenhKemTheoyhct_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv5yhct, "BenhKemTheoyhct_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv6yhct, "BenhKemTheoyhct_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv7yhct, "BenhKemTheoyhct_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv8yhct, "BenhKemTheoyhct_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemVv9yhct, "BenhKemTheoyhct_9", 1, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhAn?.MaBenhChinhRvyhct, "RaVienBenhChinh", 1, ' ');
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh, "ChanDoanBenhDanh", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv1yhct, "RaVienBenhKeoTheo_1", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv2yhct, "RaVienBenhKeoTheo_2", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv3yhct, "RaVienBenhKeoTheo_3", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv4yhct, "RaVienBenhKeoTheo_4", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv5yhct, "RaVienBenhKeoTheo_5", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv6yhct, "RaVienBenhKeoTheo_6", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv7yhct, "RaVienBenhKeoTheo_7", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv8yhct, "RaVienBenhKeoTheo_8", 1, ' ');
                //  PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn2?.BenhAn?.MaBenhKemRv9yhct, "RaVienBenhKeoTheo_9", 1, ' ');
                //  PrintHelper.OptionFieldValueHanlder(ref fields, ref values, "KQDT", benhAn2?.BenhAn?.Kqdt, new string[] { "1", "2", "3", "4", "5" });
                //  PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "KQDT_TK", tongKetBenhAn?.Kqdt, 1, getDsCombo(combo_ds, "130"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanHinhThai", benhanYHCT?.HinhThaiVongChan, 3, getDsCombo(combo_ds, "045"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanThan", benhanYHCT?.ThanVongChan, 2, getDsCombo(combo_ds, "046"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanSac", benhanYHCT?.SacVongChan, 3, getDsCombo(combo_ds, "047"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanTrach", benhanYHCT?.TrachVongChan, 2, getDsCombo(combo_ds, "048"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanLuoi", benhanYHCT?.HinhThaiLuoi, 2, getDsCombo(combo_ds, "049"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanChatLuoi", benhanYHCT?.ChatLuoi, 3, getDsCombo(combo_ds, "050")); //old 6
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VongChuanReuLuoi", benhanYHCT?.ReuLuoi, 3, getDsCombo(combo_ds, "051")); //old 6
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanAmThanhTiengNoi", benhanYHCT?.GiongNoi, 5, getDsCombo(combo_ds, "056"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanAmThanhHoiTho", benhanYHCT?.HoiTho, 5, getDsCombo(combo_ds, "057"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoAmThanhHo", PrintHelper.HanlderBooleanType(benhanYHCT?.CoHo), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanAmThanhHo", benhanYHCT?.Ho, 3, getDsCombo(combo_ds, "059"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAmThanhO", PrintHelper.HanlderBooleanType(benhanYHCT?.OamThanh), 1);
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAmThanhNac", PrintHelper.HanlderBooleanType(benhanYHCT?.NacAmThanh), 1);
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoMui", PrintHelper.HanlderBooleanType(benhanYHCT?.CoMui), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanMui", benhanYHCT?.KieuMui, 3, getDsCombo(combo_ds, "063"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoChatThai", PrintHelper.HanlderBooleanType(benhanYHCT?.CoChatThaiBenhLy), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanChatThai", benhanYHCT?.KieuChatThai, 3, getDsCombo(combo_ds, "065"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoHanNhiet", PrintHelper.HanlderBooleanType(benhanYHCT?.BhhanNhiet), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanHanNhiet", benhanYHCT?.Hannhiet, 3, getDsCombo(combo_ds, "067"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanMoHoi", benhanYHCT?.MoHoi, 3, getDsCombo(combo_ds, "068"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoDauMatCo", PrintHelper.HanlderBooleanType(benhanYHCT?.BhdauMatCo), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanDauMatCoDauDau", benhanYHCT?.DauDau, 3, getDsCombo(combo_ds, "070"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanDauMatCoHoaMatChongMat", PrintHelper.HanlderBooleanType(benhanYHCT?.HoaMat), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanDauMatCoMat", benhanYHCT?.Mat, 3, getDsCombo(combo_ds, "072"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanDauMatCoTai", benhanYHCT?.Tai, 3, getDsCombo(combo_ds, "073"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanDauMatCoMui", benhanYHCT?.Mui, 3, getDsCombo(combo_ds, "074"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanDauMatCoHong", benhanYHCT?.Hong, 3, getDsCombo(combo_ds, "075"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanDauMatCoCoVai", benhanYHCT?.CoVai, 3, getDsCombo(combo_ds, "076"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoLung", PrintHelper.HanlderBooleanType(benhanYHCT?.Bhlung), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanLung", benhanYHCT?.Lung, 4, getDsCombo(combo_ds, "078"));
                //if (benhAn2.LoaiBenhAn.MaLoaiBa == 7)
                //{
                //    PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanLung", benhanYHCT.Lung, 3, getDsCombo(combo_ds, "078"));

                //}
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoNguc", PrintHelper.HanlderBooleanType(benhanYHCT.Bhnguc), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanNguc", benhanYHCT?.Nguc, 6, getDsCombo(combo_ds, "080"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanBung", PrintHelper.HanlderBooleanType(benhanYHCT?.Bhbung), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanBung", benhanYHCT?.Bung, 6, getDsCombo(combo_ds, "082"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanChanTay", PrintHelper.HanlderBooleanType(benhanYHCT?.BhchanTay), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChanChanTay", benhanYHCT?.ChanTay, 6, getDsCombo(combo_ds, "084"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAn", PrintHelper.HanlderBooleanType(benhanYHCT?.Bhan), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanAn", benhanYHCT?.An, 3, getDsCombo(combo_ds, "086"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanUong", PrintHelper.HanlderBooleanType(benhanYHCT?.Bhuong), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanUong", benhanYHCT?.Uong, 3, getDsCombo(combo_ds, "088"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanDaiTieuTien", PrintHelper.HanlderBooleanType(benhanYHCT?.BhdaiTieuTien), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanTieuTien", benhanYHCT?.TieuTien, 3, getDsCombo(combo_ds, "090"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanDaiTien", benhanYHCT?.DaiTien, 3, getDsCombo(combo_ds, "091"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanNgu", PrintHelper.HanlderBooleanType(benhanYHCT?.Bhngu), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanNgu", benhanYHCT?.Ngu, 3, getDsCombo(combo_ds, "093"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSan", PrintHelper.HanlderBooleanType(benhanYHCT?.RlknsinhDuc), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNam", benhanYHCT?.Rlnam, 3, getDsCombo(combo_ds, "095"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNu", benhanYHCT?.Rlnu, 3, getDsCombo(combo_ds, "096"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuKinhNguyet", PrintHelper.HanlderBooleanType(benhanYHCT?.Bhkn), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuKinhNguyetRoiLoan", benhanYHCT?.Rlkinhnguyet, 2, getDsCombo(combo_ds, "098"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuThongKinh", benhanYHCT?.ThongKinh, 2, getDsCombo(combo_ds, "099"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuDoiHa", PrintHelper.HanlderBooleanType(benhanYHCT?.BhdoiHa), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuDoiHa", benhanYHCT?.DoiHa, 2, getDsCombo(combo_ds, "101"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanLienQuan", PrintHelper.HanlderBooleanType(benhanYHCT?.Dkxhbenh), 1);
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuan", PrintHelper.HanlderBooleanType(benhanYHCT?.BhxucChan), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanDa", benhanYHCT?.XucChanDa, 3, getDsCombo(combo_ds, "105"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanCoXuong", PrintHelper.HanlderBooleanType(benhanYHCT?.BhcoXuongKhop), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanCoXuong", benhanYHCT?.XucChanCoXuongKhop, 3, getDsCombo(combo_ds, "107"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanBung", PrintHelper.HanlderBooleanType(benhanYHCT?.BhbungXucChan), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanBung", benhanYHCT?.XucChanBung, 3, getDsCombo(combo_ds, "109"));
                PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanMoHoi", PrintHelper.HanlderBooleanType(benhanYHCT?.BhmoHoi), 1);
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMoHoi", benhanYHCT?.XucChanMoHoi, 3, getDsCombo(combo_ds, "111"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuan", benhanYHCT?.MachChan, 3, getDsCombo(combo_ds, "112"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "MachChanNhi", benhanYHCT?.MachChan, 3, getDsCombo(combo_ds, "202"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanTongKhamPhai", benhanYHCT?.TongKhanPhai, 3, getDsCombo(combo_ds, "113"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanTongKhamTrai", benhanYHCT?.TongKhanTrai, 3, getDsCombo(combo_ds, "114"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiThon", benhanYHCT?.ViKhanTraiThon, 3, getDsCombo(combo_ds, "115"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiQuan", benhanYHCT?.ViKhanTraiQuan, 3, getDsCombo(combo_ds, "116"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiXich", benhanYHCT?.ViKhanTraiXich, 3, getDsCombo(combo_ds, "117"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiThon", benhanYHCT?.ViKhanPhaiThon, 3, getDsCombo(combo_ds, "118"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiQuan", benhanYHCT?.ViKhanPhaiQuan, 3, getDsCombo(combo_ds, "119"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiXich", benhanYHCT?.ViKhanPhaiXich, 3, getDsCombo(combo_ds, "120"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ChanDoanBatCuong", benhanYHCT?.BhbatCuong, 5, getDsCombo(combo_ds, "121"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ChanDoanNguyenNhan", benhanYHCT?.NguyenNhan, 3, getDsCombo(combo_ds, "122"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ChanDoanTangPhu", benhanYHCT?.TangPhu, 4, getDsCombo(combo_ds, "123"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ChanDoanKinhMach", benhanYHCT?.KinhMach, 4, getDsCombo(combo_ds, "124"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "ChanDoanDinhViBenh", benhanYHCT?.DinhViBenhTheo, 4, getDsCombo(combo_ds, "125"));

                //Bệnh Án Nhi
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "DuongDiChiTay", benhanYHCT?.DuongDiChiTay, 3, getDsCombo(combo_ds, "052"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "TinhChatChiTay", benhanYHCT?.TinhChatChiTay, 1, getDsCombo(combo_ds, "053"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "HinhDangChiTay", benhanYHCT?.HinhDangChiTay, 1, getDsCombo(combo_ds, "054"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "MauSacChiTay", benhanYHCT?.MauSacChiTay, 1, getDsCombo(combo_ds, "055"));
                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "XucChan_Thop", benhanYHCT?.XucChan_Thop, 1, getDsCombo(combo_ds, "104"));

                string modelBHMoHoi1 = benhanYHCT.MoHoi;
                int modelBHMoHois = modelBHMoHoi1 != null ? 1 : 2;
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, modelBHMoHois.ToString(), "CoMoHoiVC", 1, ' ');


                //  //tinh trang ra vien
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVKQDT", benhAn2?.BenhAn?.Kqdt, PrintHelper.CreateArrayIncreate(5));
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "GiaiPhauBenh", benhAn2?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVTTTV", benhAn2?.BenhAn?.TinhTrangTuVong, PrintHelper.CreateArrayIncreate(6));
                //  PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVGPB", benhAn2?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
                //  PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn2?.BenhGPTuThis?.MaBenh, "ChuanDoanGiaiPhau", 5, ' ');


                PrintHelper.OptionFieldTextMultipleHanlder(ref fields, ref values, "BanThan", tienSuBenh?.MaTienSu, 4, getDsCombo(combo_ds, "017"));
                //PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "BanThan", tienSuBenh.MaTienSu, 4, getDsCombo(combo_ds, "017"));


                //  fields.Add("Khoa");
                //  fields.Add("VaoKhoaNgayGio");
                //  if (benhAnKhoaDieuTri != null)
                //  {
                //      values.Add(benhAnKhoaDieuTri?.Khoa?.TenKhoa);
                //      values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTri?.NgayVaoKhoa));
                //  }
                //  else
                //  {
                //      values.Add("");
                //      values.Add("");
                //      values.Add("");
                //  }
                // to dieu tri
                var toDieuTriService = new BenhAnToDieuTriService();
                mss += "\n 2808";
                var dataset = new DataSet();
                var list = new List<DictionaryEntry>();
                if (benhAn2.LoaiBenhAn.MaLoaiBa == 3)
                {
                    ManTinhPrint(ref fields, ref values, benhAn2, benhAnKhoaDieuTris, phieukhamvv, tienSuBenh, benhanYHHD, benhanYHCT, tongKetBenhAn, combo_ds);

                }
                else
                {
                    for (int i = 1; i < benhAnKhoaDieuTris.Count; i++)
                    {
                        fields.Add($"KhoaChuyen{i}");
                        values.Add(benhAnKhoaDieuTris[i]?.Khoa?.TenKhoa);
                        fields.Add($"NgayGioKhoaChuyen{i}");
                        values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTris[i]?.NgayVaoKhoa));
                        var compareDate = i != (benhAnKhoaDieuTris.Count - 1) ? PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i]?.NgayVaoKhoa, benhAnKhoaDieuTris[i + 1]?.NgayVaoKhoa, 1) : PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i]?.NgayVaoKhoa, benhAn2?.BenhAn?.NgayRv, 1);

                        PrintHelper.TexboxFieldHanlder(ref fields, ref values, compareDate, $"MaKhoaChuyen{i}", 2, '0');
                    }
                }
                mss += "\n 2829";
                string idba_stt = idba + "_2";
                mss += "\n 2831";
                string path = PrintHelper.PrintFileWithTable(idba_stt, _hostingEnvironment, template2, null, null, fields, values);
                return path;

            }
            catch (Exception ex)
            {
                WriteLog("\nLỗi print 2: " + ex.Message + "\nmss:" + mss);
                throw;
            }
        }

        protected string GetLocalIPAddress()
        {
            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            //throw new Exception("Local IP Address Not Found!");
            //string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (string.IsNullOrEmpty(ip))
            //{
            //    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //}
            //return ip;
            string ip = "1111111111";
            return ip;
        }
        public void WriteLog(String log)
        {
            #region ghi log dang nhap
            var session = _context.HttpContext.Session;
            var u = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserProfileSessionData>(
                 session, "UserProfileSessionData");
            //string constr = ConfigurationManager.AppSettings["MongoDBconnectionString"];
            //string constr = _config.GetValue<string>("Medyx_BCADatabase:ConnectionString");
            string constr = u.MongoDBConnectionString;
            var client = new MongoClient(constr);
            //IMongoDatabase db = client.GetDatabase("Medyx_BCA");
            //IMongoDatabase dbm = client.GetDatabase(ConfigurationManager.AppSettings["MongoDBDataBase"]);
            //IMongoDatabase dbm = client.GetDatabase(_config.GetValue<string>("Medyx_BCADatabase:DatabaseName"));
            IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
            IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
            string MaMay = this.GetLocalIPAddress();
            TraceLogMongo emp = new TraceLogMongo();
            emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            emp.TenBang = "Log";
            emp.KieuTacDong = "Error";
            emp.NguoiSD = u.Pub_sNguoiSD;
            emp.MaMay = MaMay;
            emp.NoiDungSD = "Loi print In tờ bệnh án: " + log;
            collection.InsertOne(emp);
            #endregion
        }
        // in ra tờ thứ nhất


        public string Print1(decimal idba)
        {
            string mss = "";
            try
            {
                if (idba < 0 || idba == 0)
                {
                    return "";
                }
                var benhAn1 = DetaiBenhAn(idba);

                var template1 = "";

                switch (benhAn1.LoaiBenhAn.MaLoaiBa)
                {
                    case 1:
                        template1 = "Benhan_YHCT_noi_tru-cp_To1.docx";
                        break;
                    case 2:
                        //// code block
                        template1 = "Benhan_YHCT_ngoai_tru_v1_To1.docx";
                        break;
                    case 3:
                        template1 = "Benh_an_YHCT_man_tinh_To1.docx";
                        break;
                    case 4:
                        template1 = "Benhan_YHCT_noi_tru_theo_ngay_To1.docx";
                        break;
                    case 5:
                        template1 = "Benhan_Noikhoa_To1.docx";
                        break;
                    case 6:
                        template1 = "Benhan_Ngoaikhoa_To1.docx";
                        break;
                    case 7:
                        template1 = "Benhan_YHCT_noi_tru-nhi_To1.docx";
                        break;
                    ////break;
                    default:
                        //// code block
                        throw new HttpStatusException(HttpStatusCode.UnsupportedMediaType, "Chưa hỗ trợ");
                }
                mss += "print1";

                var benhAnKhoaDieuTriService = new BenhAnKhoaDieuTriService();
                ////benhAnKhoaDieuTri
                var benhAnKhoaDieuTris = benhAnKhoaDieuTriService.Get(new BenhAnKhoaDieuTriParameters()
                {
                    Idba = idba
                }).data.Where(x => x.SoNgayDt != 0).OrderBy(x => x.Stt).ToList();


                ////var benhAnKhoaDieuTris = _KhoaDieuTriBA.Table.Where(x => x.Idba == idba && x.SoNgayDt != 0).OrderBy(x => x.Stt);
                string benhAnKhoaDieuTrisJson = JsonConvert.SerializeObject(benhAnKhoaDieuTris, Formatting.Indented);
                mss += "benhAnKhoaDieuTris:";
                var benhAnKhoaDieuTri = benhAnKhoaDieuTris.Count() > 0 ? benhAnKhoaDieuTris.ToList()[0] : null;
                mss += "\benhAnKhoaDieuTri:";
                ////benhanKhamVV
                var phieukhamvv = _benhAnRepository._context.BenhAnKhamVaoVien.Where(
                    x => x.Idba == idba
                ).FirstOrDefault();
                mss += "\n phieukhamvv:";
                phieukhamvv = phieukhamvv ?? new BenhAnKhamVaoVien();
                ////benhanTienSuBenh
                var tienSuBenh = _benhAnRepository._context.BenhAnTienSuBenh.FirstOrDefault(x => x.Idba == idba);
                tienSuBenh = tienSuBenh ?? new BenhAnTienSuBenh();
                mss += "\n tienSuBenh:";
                ////benhanYHCT
                var benhanYHHD = _benhAnRepository._context.BenhAnKhamYhhd.Include(x => x.DmBskham).Include(x => x.DmBenhPhanBiet).FirstOrDefault(x => x.Idba == idba);
                benhanYHHD = benhanYHHD ?? new BenhAnKhamYhhd();
                mss += "\n benhanYHHD:";
                ////benhanYHCT
                Mapper.CreateMap<BenhAnKhamYhctDto, BenhAnKhamYhct>().ReverseMap();
                Mapper.CreateMap<DmbenhTatYhctDto, DmbenhTatYhct>().ReverseMap();
                var benhanYHCT = _benhAnRepository._context.BenhAnKhamYhct.Include(x => x.DmBenhDanhTheoYHCT).Select(Mapper.Map<BenhAnKhamYhctDto>).FirstOrDefault(x => x.Idba == idba);
                benhanYHCT = benhanYHCT ?? new BenhAnKhamYhctDto();
                mss += "\n benhanYHCT:";
                ////TongKetBenhAn
                Mapper.CreateMap<BenhAnTongKetBenhAnDto, BenhAnTongKetBenhAn>().ReverseMap();
                Mapper.CreateMap<DmnhanVienDto, DmnhanVien>().ReverseMap();
                var tongKetBenhAn = _benhAnRepository._context.BenhAnTongKetBenhAn
                    .Include(x => x.DmBsDieutri)
                    .Include(x => x.DmNguoiGiao)
                    .Include(x => x.DmNguoiNhan)
                    .Select(Mapper.Map<BenhAnTongKetBenhAnDto>)
                    .FirstOrDefault(x => x.Idba == idba);
                tongKetBenhAn = tongKetBenhAn ?? new BenhAnTongKetBenhAnDto();
                mss += "\n tongKetBenhAn:";
                List<string> fields = new List<string>(){
                "BenhVien",
                "SoYTe",
                ////"Khoa",
                "Buong",
                "Giuong",
                "SoVVien",
                "SoLuuTru",
                ////"MaNgBenh",
                "MaNB",
                "MaYTe",
                "HoVaTen",
                "DanToc",
                "NgheNghiep",
                "QuocTich",
                "SoNha",
                "Thon",
                "Xa",
                "Huyen",
                "Tinh",
                "NoiLamViec",
                "GiaTriBaoHiemYTe",
                ////"GiaTriBaoHiemYTeTuNgay",
                "TuNgay",
                "TuThang",
                "TuNam",
                "LienHe",
                "SoDienThoai",
                "Nam",
                "Nu",
                "VaoVienGioPhut",
                "VaoVienNgayThang",
                "ChuyenDen",
                "RaVienGioPhut",
                "RaVienNgayThang",
                "NoiChuyenDenYHHD",
                "BenhChinhYHHD",
                "BenhKemTheoYHHD_1",
                "BenhKemTheoYHHD_2",
                "BenhKemTheoYHHD_3",
                // // "BenhKemTheoYHHD_4",
                // // "BenhKemTheoYHHD_5",
                // // "BenhKemTheoYHHD_6",
                // // "BenhKemTheoYHHD_7",
                // // "BenhKemTheoYHHD_8",
                // // "BenhKemTheoYHHD_9",
                "ThuThuatYHHD",
                "PhauThuatYHHD",
                "RaVienBenhChinhYHHD",
                "RaVienBenhKemTheoYHHD",
                // // "RaVienTaiBienYHHD",
                // // "RaVienBienChungYHHD",
                "NoiChuyenDenYHCT",
                "BenhChinhYHCT",
                "BenhKemTheoYHCT_1",
                "BenhKemTheoYHCT_2",
                "BenhKemTheoYHCT_3",
                // // "BenhKemTheoYHCT_4",
                // // "BenhKemTheoYHCT_5",
                // // "BenhKemTheoYHCT_6",
                // // "BenhKemTheoYHCT_7",
                // // "BenhKemTheoYHCT_8",
                // // "BenhKemTheoYHCT_9",
                "ThuThuatYHCT",
                "PhauThuatYHCT",
                // // "RaVienTaiBien",
                // // "RaVienBienChung",
                "RaVienBenhChinhYHCT",
                "RaVienBenhKemTheoYHCT",
                "GiamDoc",
                "TruongKhoa",
                "BsKhamBenh",
                // // "LyDoVaoVien",
                // // "BenhSu",
                // // "BanThan",
                // // "DacDiem",
                // // "GiaDinh",
                // // "ToanThan",
                // // "mach",
                // // "nhietdo",
                // // "huyetap",
                // // "nhiptho",
                // // "cannang",
                // // "chieucao",
                // // "bmi",
                // // "TuanHoan",
                // // "HoHap",
                // // "TieuHoa",
                // // "TietNieu",
                // // "ThanKinh",
                // // "CoXuongKhop",
                // // "TaiMuiHong",
                // // "RangHamMat",
                // // "Mat",
                // // "NoiTiet",
                // // "TomTatCls",
                // // "TomTatBenhAn",
                // // "BenhKemTheoYHHD",
                // // "PhanBiet",
                // // "VongChan",
                // // "VanChan",
                // // "VaanChan",
                // // "XuaChan",
                // // "MachTayTrai",
                // // "MachTayPhai",
                // // "TomTatTuChuan",
                // // "BienChuong",
                // // "BenhDanh",
                // // "BatCuong",
                // // "NguyenNhan",
                // // "TangPhu",
                // // "KinhMach",
                // // "DinhViBenh",
                // // "PhapDieuTri",
                // // "PhuongDuoc",
                // // "KhongDungThuoc",
                // // "HuongDieuTri",
                // // "DuHau",
                // // "TongKetLyDoVaoVien",
                // // "TongKetQuaTrinhBenh",
                // // "KetQuaXetNghiemCanLamSang",
                // // "TinhTrangRaVien",
                // // "HuongDieuTriCheDoTiep",
                // // "TongKetBenhAnPPDTYHHD",
                // // "TongKetBenhAnPPDTYHCT",
                // // "VongChanMTKhac",
                // // "VanChanMoTaKhac",
                // // "VaanChanMTKhac",
                // // "ThietChanMTKhac",
                // // "PhuongPhapDieuTriKhongDungThuoc",
                // // "PhuongPhapKhac",
                // // "YHocHienDai",
                "NgayTuVong",
                "NguyenNhanTuVong",
                "KhamNghiemTuThi",
                "ChanDoanGiaiPhauTuThi",
	            /*"soTongKetXQuang",
                "soTongKetCT",
                "soTongKetMRI",
                "soToSieuAm",
                "soToXetNghiem",
                "tongKetKhac",
                "soTongKetKhac",
                "soTongKetToanHoSo",
                "HoTenNguoiGiao",
                "HoTenNhan",
                "HoTenThay",
                "NgayThangTongKet",
                "soToSieuAm",
                "soToXetNghiem",*/
                /*"NgayKy",
                "NgayGioLamBenh",
                "ThayThuocLamBenh",
                "BenhChinh",*/
                "VaoVienLanThu",
                // // "solanPT",
                "NgayThang",
                // // "TongSoNgayDtsauPT",
                "KhoaKhamBenhYHCT",
                "KhoaKhamBenhYHHD",
                // // "MoTa",
                /*"MoTaKhac",
                "MoTaKhac2",
                "MoTaKhac3",
                "XucChan",
                "TangPhu",
                "KinhMach",*/
                "Cmnd",
                "Bhyt",
                 /*"AnDuoi1Tuoi",
                "AnTren1Tuoi",
                "ThangCaiSua",
                "DacDiemSH",
                "ThangNoi",*/
                "VaoVienBenhKemTheoYHCT",
                "VaoVienBenhKemTheoYHHD"
};
                List<string> values = new List<string>(){
                PrintSetting.BenhVien,
                PrintSetting.SoYTe,
                // // benhAnKhoaDieuTri?.Khoa?.TenKhoa,
                benhAn1?.Buong?.TenBuong,
                benhAn1?.Giuong?.TenGiuong,
                benhAn1?.BenhAn?.SoVaoVien,
                benhAn1?.BenhAn?.SoLuuTru,
                // // benhAn1?.BenhNhan?.MaBn,
                benhAn1?.BenhNhan?.MaBn,
                benhAn1?.BenhAn?.MaYt,
                benhAn1?.BenhNhan?.HoTen?.ToUpper(),
                benhAn1?.DanToc?.TenDanToc,
                benhAn1?.NgheNghiep?.TenNn,
                benhAn1?.QuocGia?.TenQg,
                benhAn1?.BenhNhan?.SoNha,
                benhAn1?.BenhNhan?.Thon,
                benhAn1?.PhuongXa?.TenPxa,
                benhAn1?.QuanHuyen?.TenQh,
                benhAn1?.Tinh?.TenTinh,
                benhAn1?.BenhNhan?.NoiLamViec,
                benhAn1?.BenhNhan?.Gtbhytdn?.ToString("dd/MM/yyyy"),
                ////benhAn1?.BenhNhan?.Gtbhyttn?.ToString("dd/MM/yyyy"),
                benhAn1?.BenhNhan?.Gtbhyttn?.ToString("dd"),
                benhAn1?.BenhNhan?.Gtbhyttn?.ToString("MM"),
                benhAn1?.BenhNhan?.Gtbhyttn?.ToString("yyyy"),
                benhAn1?.BenhNhan?.LienHe,
                benhAn1?.BenhNhan?.SoDienThoai,
                benhAn1?.BenhNhan?.GioiTinh == 1 ? "x" : null,
                benhAn1?.BenhNhan?.GioiTinh == 2 ? "x": null,
                PrintHelper.TimeText(benhAn1?.BenhAn?.NgayVv),
                PrintHelper.DateText(benhAn1?.BenhAn?.NgayVv),
                benhAn1?.benhVienChuyenDen?.TenBv,
                PrintHelper.TimeText(benhAn1?.BenhAn ?.NgayRv),
                PrintHelper.DateText(benhAn1?.BenhAn?.NgayRv),
                benhAn1?.BenhTatNoiChuyenDen?.TenBenh,
                 !String.IsNullOrEmpty(benhAn1?.BenhAn?.TenBenhChinhVv) ? PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVv) : PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVv),
               PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv1 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv2 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv3 ),
                 /*PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv4 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv5 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv6 ),
                PrintH List<string> values = new List<string>(){
                PrintSetting.BenhVien,
                PrintSetting.SoYTe,
                // // benhAnKhoaDieuTri?.Khoa?.TenKhoa,
                benhAn1?.Buong?.TenBuong,
                benhAn1?.Giuong?.TenGiuong,
                benhAn1?.BenhAn?.SoVaoVien,
                benhAn1?.BenhAn?.SoLuuTru,
                // // benhAn1?.BenhNhan?.MaBn,
                benhAn1?.BenhNhan?.MaBn,
                benhAn1?.BenhAn?.MaYt,
                benhAn1?.BenhNhan?.HoTen?.ToUpper(),
                benhAn1?.DanToc?.TenDanToc,
                benhAn1?.NgheNghiep?.TenNn,
                benhAn1?.QuocGia?.TenQg,
                benhAn1?.BenhNhan?.SoNha,
                benhAn1?.BenhNhan?.Thon,
                benhAn1?.PhuongXa?.TenPxa,
                benhAn1?.QuanHuyen?.TenQh,
                benhAn1?.Tinh?.TenTinh,
                benhAn1?.BenhNhan?.NoiLamViec,
                benhAn1?.BenhNhan?.Gtbhytdn?.ToString("dd/MM/yyyy"),
                ////benhAn1?.BenhNhan?.Gtbhyttn?.ToString("dd/MM/yyyy"),
                benhAn1?.BenhNhan?.Gtbhyttn?.ToString("dd"),
                benhAn1?.BenhNhan?.Gtbhyttn?.ToString("MM"),
                benhAn1?.BenhNhan?.Gtbhyttn?.ToString("yyyy"),
                benhAn1?.BenhNhan?.LienHe,
                benhAn1?.BenhNhan?.SoDienThoai,
                benhAn1?.BenhNhan?.GioiTinh == 1 ? "x" : null,
                benhAn1?.BenhNhan?.GioiTinh == 2 ? "x": null,
                PrintHelper.TimeText(benhAn1?.BenhAn?.NgayVv),
                PrintHelper.DateText(benhAn1?.BenhAn?.NgayVv),
                benhAn1?.benhVienChuyenDen?.TenBv,
                PrintHelper.TimeText(benhAn1?.BenhAn ?.NgayRv),
                PrintHelper.DateText(benhAn1?.BenhAn?.NgayRv),
                benhAn1?.BenhTatNoiChuyenDen?.TenBenh,
                 !String.IsNullOrEmpty(benhAn1?.BenhAn?.TenBenhChinhVv) ? PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVv) : PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVv),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv1 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv2 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv3 ),
                /*PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv4 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv5 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv6 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv7 ),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv8 ),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv9 ),*/
                benhAn1?.BenhAn?.ThuThuatYhhd == 1 ? "x" : "",
                benhAn1?.BenhAn?.PhauThuatYhhd == 1 ? "x" : "",
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhRv),
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv1), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv2), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv3),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv4),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv5),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv6),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv7),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv8),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv9)),
               /* benhAn1?.BenhAn?.TaiBienYhhd == 1 ? "x" : "",
                benhAn1?.BenhAn?.BienChungYhhd == 1 ? "x" : "", */
                PrintHelper.Trimmer(benhAn1?.BenhNoiChuyenDenYHCT?.TenBenhIcd),
                !String.IsNullOrEmpty(benhAn1?.BenhAn?.TenBenhChinhVvyhct) ? PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVvyhct) : PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVvyhct),
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv1yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv2yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv3yhct),
                 /*PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv4yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv5yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv6yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv7yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv8yhct),
                 PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv9yhct),*/
                benhAn1?.BenhAn?.ThuThuatYhct == 1 ? "x" : "",
                benhAn1?.BenhAn?.PhauThuatYhct == 1 ? "x" : "",
                /*benhAn1?.BenhAn?.TaiBienYhct == 1 ? "x" : "",
                benhAn1?.BenhAn?.BienChungYhct == 1 ? "x" : "",*/
                benhAn1?.BenhAn?.TenBenhChinhRvyhct,
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv1yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv2yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv3yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv4yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv5yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv6yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv7yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv8yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemRv9yhct)),
                benhAn1?.GiamDoc?.HoTen,
                benhAn1?.TruongKhoa?.HoTen,
                benhAn1?.BsDieutri?.HoTen,
                /*benhanYHHD?.LyDoVv,
                benhanYHHD?.BenhSu, ////BenhSu
	            tienSuBenh?.TienSuBanThan,
                tienSuBenh?.DacDiemLienQuanBenh, ////DacDiem
	            tienSuBenh?.TienSuGiaDinh,
                benhanYHHD?.ToanThan,
                benhanYHHD?.Mach?.ToString(),
                benhanYHHD?.NhietDo?.ToString(),
                benhanYHHD?.HuyetAp?.ToString(),
                benhanYHHD?.NhipTho?.ToString(),
                ((int?)benhanYHHD?.CanNang)?.ToString(),
                ((int?)benhanYHHD?.ChieuCao)?.ToString(),
                benhanYHHD?.Bmi?.ToString("0.00"),
                benhanYHHD?.TuanHoan,
                benhanYHHD?.HoHap,
                benhanYHHD?.TieuHoa,
                benhanYHHD?.ThanTnieuSduc, ////TietNieu
	            benhanYHHD?.ThanKinh,
                benhanYHHD?.XuongKhop,
                benhanYHHD?.TaiMuiHong,
                benhanYHHD?.RangHamMat,
                benhanYHHD?.Mat,
                benhanYHHD?.NoiTietDd,
                benhanYHHD?.CanLamSang,
                benhanYHHD?.TomTatBenhAn,*/
                //"",////BenhKemTheoYHHD
	            /*benhanYHHD?.DmBenhPhanBiet != null ? $"{benhanYHHD?.DmBenhPhanBiet?.MaBenh} - {benhanYHHD?.DmBenhPhanBiet?.TenBenh}" : "",PhanBiet
	            benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MtvaanChan,
                benhanYHCT?.MtthietChan,
                benhanYHCT?.MachTrai,
                benhanYHCT?.MachPhai,
                benhanYHCT?.TomTatTuChan,
                benhanYHCT?.BienChungLuanTri,
                $"{benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh} - {benhanYHCT?.DmBenhDanhTheoYHCT?.TenBenh}",
                benhanYHCT?.BatCuong,
                benhanYHCT?.MoTaNguyenNhan,
                benhanYHCT?.MoTaTangPhu,
                benhanYHCT?.MoTaKinhMach,
                benhanYHCT?.MoTaDinhViBenhTheo,
                benhanYHCT?.Ppdtyhct,
                benhanYHCT?.PhuongDuoc,
                benhanYHCT?.PpdtkhongDungThuoc,
                benhanYHHD?.HuongDtyhhd, ////Hướng điều trị
	            benhanYHHD?.TienLuong, ////Dư hậu,
	            tongKetBenhAn?.LyDoVv,
                tongKetBenhAn?.QuaTrinhBenhLy,
                tongKetBenhAn?.TomTatKetQuaCls,
                tongKetBenhAn?.TinhTrangBnrv,
                tongKetBenhAn?.HuongDt,
                tongKetBenhAn?.PpdttheoYhhd,
                tongKetBenhAn?.PpdttheoYhct,
                benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MtvaanChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MoTaXucChan,
                benhanYHCT?.PpdtkhongDungThuoc,
                benhanYHCT?.Ppkhac,
                benhanYHHD?.Ppdtyhhd,*/
                PrintHelper.DateTimeText(benhAn1?.BenhAn?.NgayTuVong),
                benhAn1?.BenhAn?.NguyenNhanTuVong,
                benhAn1?.BenhAn?.KhamNghiemTuThi == 1 ? "x" : "",
                benhAn1?.BenhGPTuThis?.TenBenh,
                /*tongKetBenhAn?.SoToXquang?.ToString(),
                tongKetBenhAn?.SoToCt?.ToString(),
                tongKetBenhAn?.SoToMri?.ToString(),
                tongKetBenhAn?.SoToSa?.ToString(),
                tongKetBenhAn?.SoToXn?.ToString(),
                String.IsNullOrEmpty(tongKetBenhAn?.Khac) ? "Khác ...." : tongKetBenhAn?.Khac,
                tongKetBenhAn?.SoToKhac?.ToString(),
                tongKetBenhAn?.SoToToanBoHs?.ToString(),
                tongKetBenhAn?.DmNguoiGiao?.HoTen,
                tongKetBenhAn?.DmNguoiNhan?.HoTen,
                tongKetBenhAn?.DmBsDieutri?.HoTen,
                PrintHelper.DateText(tongKetBenhAn?.NgayKy),
                tongKetBenhAn?.SoToSa?.ToString(),
                tongKetBenhAn?.SoToXn?.ToString(),
                PrintHelper.DateText(benhAn1?.BenhAn?.NgayKy),
                PrintHelper.DateTimeText(benhanYHHD?.NgayKham), ////NgayGioLamBenh
	            benhanYHHD?.DmBskham?.HoTen, ////ThayThuocLamBenh
                PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhChinhVv),*/
                benhAn1?.BenhAn?.Vvlan?.ToString(),
                // // benhAn1?.BenhAn?.TongSoLanPt.ToString(),
                PrintHelper.DateText(benhAn1?.BenhAn?.NgayKy),
                // // benhAn1?.BenhAn?.TongSoNgayDtsauPt?.ToString(),
                benhAn1?.BenhKKBYHCT?.TenBenhBhyt,
                benhAn1?.BenhKKBYHHD?.TenBenh,
                // // tienSuBenh?.MoTaTienSu,
                /*benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MtthietChan,
                benhanYHCT?.MoTaXucChan,
                benhanYHCT?.MoTaTangPhu,
                benhanYHCT?.MoTaKinhMach,*/
                benhAn1.BenhNhan.Cmnd,
                benhAn1?.BenhNhan?.SoTheBhyt,
                /*tienSuBenh?.AnDuoi1Tuoi,
                tienSuBenh?.AnTren1Tuoi,
                tienSuBenh?.ThangCaiSua,
                tienSuBenh?.DacDienSH,
                tienSuBenh?.ThangNoi,*/
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv1yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv2yhct), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv3yhct),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv4yhct),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv5yhct),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv6yhct),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv7yhct),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv8yhct),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv9yhct)),
                PrintHelper.ConcatStringArrChamPhay(PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv1 ), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv2), PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv3),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv4),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv5),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv6),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv7),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv8),PrintHelper.Trimmer(benhAn1?.BenhAn?.TenBenhKemVv9)),
};
                mss += "\n 3373:";
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.NgheNghiep?.MaNn, "Nghenghiep", 2, '0', true);
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.DanToc?.MaDanToc, "dantoc", 2, '0', true);
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.QuanHuyen?.MaBhxh, "huyen", 1, '0');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.PhuongXa?.MaBhxh, "xaphuong", 1, '0');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.Tinh?.MaBhyt, "thanhpho", 1, '0');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.QuocGia?.MaQL, "QuocTich", 3, ' ');
                //PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhNhan?.Cmnd, "Cmnd", 1, ' ');

                var combo_ds = _benhAnRepository._context.DmbaCombods.Where(x => maDsCombo.Contains(x.MaParent)).OrderBy(x => x.MaParent).ThenBy(x => x.Ma).ToList();
                mss += "\n combo_ds:";
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, PrintHelper.BirthText(benhAn1?.BenhNhan?.NgaySinh), "NgaySinh", 8, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhNhan?.Tuoi?.ToString(), "Tuoi", 2, '0');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhAn?.TongSoNgayDt.ToString(), "TongSoNgayDT", 2, '0');
                PrintHelper.TextboxFieldDoiTuongHanlder(ref fields, ref values, benhAn1?.DoiTuong?.MaDt);
                PrintHelper.TextboxFieldBHYTHanlder(ref fields, ref values, benhAn1?.BenhNhan?.SoTheBhyt, "Bhyt", false);
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "tructiepvao", benhAn1?.BenhAn?.TrucTiepVao, new string[] { "1", "2", "3" });
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "NoiGioiThieu", benhAn1?.BenhAn?.NoiGt, new string[] { "1", "2", "3" });
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "ChuyenVien", benhAn1?.BenhAn?.ChuyenVien, new string[] { "1", "2", "3" });
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "RaVien", benhAn1?.BenhAn?.HtraVien, new string[] { "1", "2", "3", "4" });
                ////PrintHelper.OptionFieldHanlder(ref fields, ref values, "nguyennhan", benhAn1?.BenhAn?.NguyenNhanTBBC, new string[] { "1", "2", "3", "4" });
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhTatNoiChuyenDen?.MaBenh, "Noichuyenden", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhKKBYHHD?.MaBenh, "KhoaKhamBenh", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhAn?.MaBenhChinhVv, "BenhChinh", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv1, "BenhKemTheo_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv2, "BenhKemTheo_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv3, "BenhKemTheo_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv4, "BenhKemTheo_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv5, "BenhKemTheo_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv6, "BenhKemTheo_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv7, "BenhKemTheo_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv8, "BenhKemTheo_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv9, "BenhKemTheo_9", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhAn?.MaBenhChinhRv, "MaBenhChinhRv", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv1, "RaVienBenhKemTheoYHHD_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv2, "RaVienBenhKemTheoYHHD_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv3, "RaVienBenhKemTheoYHHD_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv4, "RaVienBenhKemTheoYHHD_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv5, "RaVienBenhKemTheoYHHD_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv6, "RaVienBenhKemTheoYHHD_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv7, "RaVienBenhKemTheoYHHD_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv8, "RaVienBenhKemTheoYHHD_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv9, "RaVienBenhKemTheoYHHD_9", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhNoiChuyenDenYHCT?.MaBenh, "NoichuyendenYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhKKBYHCT?.MaBenh, "KhoaKhamBenhYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhAn?.MaBenhChinhVvyhct, "BenhChinhYHCT", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv1yhct, "BenhKemTheoYHCT_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv2yhct, "BenhKemTheoyhct_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv3yhct, "BenhKemTheoyhct_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv4yhct, "BenhKemTheoyhct_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv5yhct, "BenhKemTheoyhct_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv6yhct, "BenhKemTheoyhct_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv7yhct, "BenhKemTheoyhct_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv8yhct, "BenhKemTheoyhct_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemVv9yhct, "BenhKemTheoyhct_9", 1, ' ');

                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhChinhRvyhct, "RaVienBenhChinh", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh, "ChanDoanBenhDanh", 6, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv1yhct, "RaVienBenhKeoTheo_1", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv2yhct, "RaVienBenhKeoTheo_2", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv3yhct, "RaVienBenhKeoTheo_3", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv4yhct, "RaVienBenhKeoTheo_4", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv5yhct, "RaVienBenhKeoTheo_5", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv6yhct, "RaVienBenhKeoTheo_6", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv7yhct, "RaVienBenhKeoTheo_7", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv8yhct, "RaVienBenhKeoTheo_8", 1, ' ');
                PrintHelper.TexboxFieldHanlderChamPhay(ref fields, ref values, benhAn1?.BenhAn?.MaBenhKemRv9yhct, "RaVienBenhKeoTheo_9", 1, ' ');

                PrintHelper.OptionFieldHanlder(ref fields, ref values, "RaVienTaiBien", benhAn1?.BenhAn?.TaiBienYhct.ToString(), new string[] { "1", "2", "3", "4" });
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "RaVienBienChung", benhAn1?.BenhAn?.BienChungYhct.ToString(), new string[] { "1", "2", "3", "4" });
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "KQDT", benhAn1?.BenhAn?.Kqdt, new string[] { "1", "2", "3", "4", "5" });
                /*PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "KQDT_TK", tongKetBenhAn?.Kqdt, 1, getDsCombo(combo_ds, "130"));
                PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanBatCuong", benhanYHCT?.BhbatCuong, 5, getDsCombo(combo_ds, "121"));
                PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanNguyenNhan", benhanYHCT?.NguyenNhan, 1, getDsCombo(combo_ds, "122"));
                PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanTangPhu", benhanYHCT?.TangPhu, 4, getDsCombo(combo_ds, "123"));
                PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanKinhMach", benhanYHCT?.KinhMach, 4, getDsCombo(combo_ds, "124"));
                PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanDinhViBenh", benhanYHCT?.DinhViBenhTheo, 4, getDsCombo(combo_ds, "125"));
                */
                ////tinh trang ra vien
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVKQDT", benhAn1?.BenhAn?.Kqdt, PrintHelper.CreateArrayIncreate(5));
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "GiaiPhauBenh", benhAn1?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
                PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVTTTV", benhAn1?.BenhAn?.TinhTrangTuVong, PrintHelper.CreateArrayIncreate(6));
                ////PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVGPB", benhAn1?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhGPTuThis?.MaBenh, "ChuanDoanGiaiPhau", 5, ' ');
                PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn1?.BenhGPTuThis?.MaBenh, "ChuanDoanGiaiPhau1", 1, ' ');


                ////PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "BanThan", tienSuBenh?.MaTienSu, 6, getDsCombo(combo_ds, "017"));

                mss += "\n 3026:";
                fields.Add("Khoa");
                fields.Add("VaoKhoaNgayGio");
                mss += "\n 3029:";
                if (benhAnKhoaDieuTri != null)
                {
                    mss += "\n 3032:";
                    values.Add(benhAnKhoaDieuTri?.Khoa?.TenKhoa);
                    values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTri?.NgayVaoKhoa));
                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAnKhoaDieuTri?.SoNgayDt.ToString(), "SoNgayDT", 2, ' ');

                }
                else
                {
                    mss += "\n 3040";
                    values.Add("");
                    values.Add("");
                    values.Add("");
                }
                // to dieu tri
                var toDieuTriService = new BenhAnToDieuTriService();

                var dataset = new DataSet();
                var list = new List<DictionaryEntry>();
                mss += "\n 3050";
                if (benhAn1.LoaiBenhAn.MaLoaiBa == 3)
                {
                    mss += "\n benhAn1.LoaiBenhAn.MaLoaiBa :" + benhAn1?.LoaiBenhAn?.MaLoaiBa;
                    ManTinhPrint(ref fields, ref values, benhAn1, benhAnKhoaDieuTris, phieukhamvv, tienSuBenh, benhanYHHD, benhanYHCT, tongKetBenhAn, combo_ds);

                }
                else
                {
                    mss += "\n 3059";
                    for (int i = 1; i < benhAnKhoaDieuTris.Count; i++)
                    {
                        fields.Add($"KhoaChuyen{i}");
                        values.Add(benhAnKhoaDieuTris[i].Khoa.TenKhoa);
                        fields.Add($"NgayGioKhoaChuyen{i}");
                        values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTris[i]?.NgayVaoKhoa));
                        var compareDate = i != (benhAnKhoaDieuTris.Count - 1) ? PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i]?.NgayVaoKhoa, benhAnKhoaDieuTris[i + 1]?.NgayVaoKhoa, 1) : PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i]?.NgayVaoKhoa, benhAn1?.BenhAn?.NgayRv, 1);

                        PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAnKhoaDieuTris[i]?.SoNgayDt.ToString(), $"MaKhoaChuyen{i}", 2, '0');
                        PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAnKhoaDieuTri.SoNgayDt.ToString(), "SoNgayDTChuyenKhoa", 2, ' ');

                    }
                }
                mss += "\n 3073";
                string idba_stt = idba + "_1";
                string path = PrintHelper.PrintFileWithTable(idba_stt, _hostingEnvironment, template1, null, null, fields, values);
                mss += "\n path:" + path;

                return path;
            }
            catch (Exception ex)
            {
                WriteLog("\nLỗi print 1: " + ex.Message + "\nmss:" + mss);
                throw;
            }


        }




        // in tổng bệnh án
        public string Print(decimal idba)
        {
            //var benhAn = Detail(idba);
            var benhAn = DetaiBenhAn(idba);
            var template = "";

            switch (benhAn.LoaiBenhAn.MaLoaiBa)
            {
                case 1:
                    template = "Benhan_YHCT_noi_tru-cp.docx";
                    break;
                case 2:
                    // code block
                    template = "Benhan_YHCT_ngoai_tru_v1.docx";
                    break;
                case 3:
                    template = "Benh_an_YHCT_man_tinh.docx";
                    break;
                case 4:
                    template = "Benhan_YHCT_noi_tru_theo_ngay.docx";
                    break;
                case 5:
                    template = "Benhan_Noikhoa.docx";
                    break;
                case 6:
                    template = "Benhan_Ngoaikhoa.docx";
                    break;
                //break;
                default:
                    // code block
                    throw new HttpStatusException(HttpStatusCode.UnsupportedMediaType, "Chưa hỗ trợ");
            }

            var benhAnKhoaDieuTriService = new BenhAnKhoaDieuTriService();
            //benhAnKhoaDieuTri
            var benhAnKhoaDieuTris = benhAnKhoaDieuTriService.Get(new BenhAnKhoaDieuTriParameters()
            {
                Idba = idba
            }).data.OrderBy(x => x.Stt).ToList();
            var benhAnKhoaDieuTri = benhAnKhoaDieuTris.Count > 0 ? benhAnKhoaDieuTris[0] : null;
            //benhanKhamVV
            var phieukhamvv = _benhAnRepository._context.BenhAnKhamVaoVien.Where(
                x => x.Idba == idba
            ).FirstOrDefault();
            phieukhamvv = phieukhamvv ?? new BenhAnKhamVaoVien();
            //benhanTienSuBenh
            var tienSuBenh = _benhAnRepository._context.BenhAnTienSuBenh.FirstOrDefault(x => x.Idba == idba);
            tienSuBenh = tienSuBenh ?? new BenhAnTienSuBenh();
            //benhanYHCT
            var benhanYHHD = _benhAnRepository._context.BenhAnKhamYhhd.Include(x => x.DmBskham).Include(x => x.DmBenhPhanBiet).FirstOrDefault(x => x.Idba == idba);
            benhanYHHD = benhanYHHD ?? new BenhAnKhamYhhd();
            //benhanYHCT
            Mapper.CreateMap<BenhAnKhamYhctDto, BenhAnKhamYhct>().ReverseMap();
            Mapper.CreateMap<DmbenhTatYhctDto, DmbenhTatYhct>().ReverseMap();
            var benhanYHCT = _benhAnRepository._context.BenhAnKhamYhct.Include(x => x.DmBenhDanhTheoYHCT).Select(Mapper.Map<BenhAnKhamYhctDto>).FirstOrDefault(x => x.Idba == idba);
            benhanYHCT = benhanYHCT ?? new BenhAnKhamYhctDto();
            //TongKetBenhAn
            Mapper.CreateMap<BenhAnTongKetBenhAnDto, BenhAnTongKetBenhAn>().ReverseMap();
            Mapper.CreateMap<DmnhanVienDto, DmnhanVien>().ReverseMap();
            var tongKetBenhAn = _benhAnRepository._context.BenhAnTongKetBenhAn
                .Include(x => x.DmBsDieutri)
                .Include(x => x.DmNguoiGiao)
                .Include(x => x.DmNguoiNhan)
                .Select(Mapper.Map<BenhAnTongKetBenhAnDto>)
                .FirstOrDefault(x => x.Idba == idba);
            tongKetBenhAn = tongKetBenhAn ?? new BenhAnTongKetBenhAnDto();

            List<string> fields = new List<string>(){
                "BenhVien",
                "SoYTe",
                "Khoa",
                "Buong",
                "SoVVien",
                "SoLuuTru",
                "MaNgBenh",
                "MaNB",
                "MaYTe",
                "HoVaTen",
                "DanToc",
                "NgheNghiep",
                "QuocTich",
                "SoNha",
                "Thon",
                "Xa",
                "Huyen",
                "Tinh",
                "NoiLamViec",
                "GiaTriBaoHiemYTe",
                "LienHe",
                "SoDienThoai",
                "Nam",
                "Nu",
                "VaoVienGioPhut",
                "VaoVienNgayThang",
                "ChuyenDen",
                "RaVienGioPhut",
                "RaVienNgayThang",
                "NoiChuyenDenYHHD",
                "BenhChinhYHHD",
                "BenhKemTheoYHHD_1",
                "BenhKemTheoYHHD_2",
                "BenhKemTheoYHHD_3",
                "ThuThuatYHHD",
                "PhauThuatYHHD",
                "RaVienBenhChinhYHHD",
                "RaVienBenhKemTheoYHHD",
                "RaVienTaiBien",
                "RaVienBienChung",
                "NoiChuyenDenYHCT",
                "BenhChinhYHCT",
                "BenhKemTheoYHCT_1",
                "BenhKemTheoYHCT_2",
                "BenhKemTheoYHCT_3",
                "ThuThuatYHCT",
                "PhauThuatYHCT",
                "RaVienTaiBien",
                "RaVienBienChung",
                "RaVienBenhChinhYHCT",
                "RaVienBenhKemTheoYHCT",
                "GiamDoc",
                "TruongKhoa",
                "BsKhamBenh",
                "LyDoVaoVien",
                "BenhSu",
                "BanThan",
                "DacDiem",
                "GiaDinh",
                "ToanThan",
                // bang mach
                "mach",
                "nhietdo",
                "huyetap",
                "nhiptho",
                "cannang",
                "chieucao",
                "bmi",
                "TuanHoan",
                "HoHap",
                "TieuHoa",
                "TietNieu",
                "ThanKinh",
                "CoXuongKhop",
                "TaiMuiHong",
                "RangHamMat",
                "Mat",
                "NoiTiet",
                "TomTatCls",
                "TomTatBenhAn",
                "BenhKemTheoYHHD",
                "PhanBiet",
                "VongChan",
                "VanChan",
                "VaanChan",
                "XuaChan",
                "MachTayTrai",
                "MachTayPhai",
                "TomTatTuChuan",
                "BienChuong",
                "BenhDanh",
                "BatCuong",
                "NguyenNhan",
                "TangPhu",
                "KinhMach",
                "DinhViBenh",
                "PhapDieuTri",
                "PhuongDuoc",
                "KhongDungThuoc",
                "HuongDieuTri",
                "DuHau",
                "TongKetLyDoVaoVien",
                "TongKetQuaTrinhBenh",
                "KetQuaXetNghiemCanLamSang",
                "TinhTrangRaVien",
                "HuongDieuTriCheDoTiep",
                "TongKetBenhAnPPDTYHHD",
                "TongKetBenhAnPPDTYHCT",
                "VongChanMTKhac",
                "VanChanMoTaKhac",
                "VaanChanMTKhac",
                "ThietChanMTKhac",
                "PhuongPhapDieuTriKhongDungThuoc",
                "PhuongPhapKhac",
                "YHocHienDai",
                "NgayTuVong",
                "NguyenNhanTuVong",
                "KhamNghiemTuThi",
                "ChanDoanGiaiPhauTuThi",
				/*tổng kết bệnh án*/
				"soTongKetXQuang",
                "soTongKetCT",
                "soTongKetMRI",
                "soToSieuAm",
                "soToXetNghiem",
                "tongKetKhac",
                "soTongKetKhac",
                "soTongKetToanHoSo",
                "HoTenNguoiGiao",
                "HoTenNhan",
                "HoTenThay",
                "NgayThangTongKet",
                "soToSieuAm",
                "soToXetNghiem",
				/*thông tin chung bệnh án*/
                "NgayKy",
                "NgayGioLamBenh",
                "ThayThuocLamBenh",
                "BenhChinh",
                "VaoVienLanThu",
                "solanPT",
                "NgayThang",
                "TongSoNgayDtsauPT",
                "KhoaKhamBenhYHCT",
                "KhoaKhamBenhYHHD",
                "MoTa",
                //yhct
                "MoTaKhac",
                "MoTaKhac2",
                "MoTaKhac3",
                "XucChan",
                "TangPhu",
                "KinhMach",
                "Bhyt"

            };
            List<string> values = new List<string>(){
                PrintSetting.BenhVien,
                PrintSetting.SoYTe,
                benhAnKhoaDieuTri?.Khoa?.TenKhoa,
                benhAn?.Buong?.TenBuong,
                benhAn?.BenhAn?.SoVaoVien,
                benhAn?.BenhAn?.SoLuuTru,
                benhAn?.BenhNhan?.MaBn,
                benhAn?.BenhNhan?.MaBn,
                benhAn?.BenhAn?.MaYt,
                benhAn?.BenhNhan?.HoTen?.ToUpper(),
                benhAn?.DanToc?.TenDanToc,
                benhAn?.NgheNghiep?.TenNn,
                benhAn?.QuocGia?.TenQg,
                benhAn?.BenhNhan?.SoNha,
                benhAn?.BenhNhan?.Thon,
                benhAn?.PhuongXa?.TenPxa,
                benhAn?.QuanHuyen?.TenQh,
                benhAn?.Tinh?.TenTinh,
                benhAn?.BenhNhan?.NoiLamViec,
                benhAn?.BenhNhan?.Gtbhytdn?.ToString("dd/MM/yyyy"),
                benhAn?.BenhNhan?.LienHe,
                benhAn?.BenhNhan?.SoDienThoai,
                benhAn?.BenhNhan?.GioiTinh == 1 ? "x" : null,
                benhAn?.BenhNhan?.GioiTinh == 2 ? "x": null,
                PrintHelper.TimeText(benhAn?.BenhAn?.NgayVv),
                PrintHelper.DateText(benhAn?.BenhAn?.NgayVv),
                benhAn?.benhVienChuyenDen?.TenBv,
                PrintHelper.TimeText(benhAn?.BenhAn?.NgayRv),
                PrintHelper.DateText(benhAn?.BenhAn?.NgayRv),
                benhAn?.BenhTatNoiChuyenDen?.TenBenh,
                !String.IsNullOrEmpty(benhAn?.BenhAn?.TenBenhChinhVv) ? PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhChinhVv) : PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhChinhVv),

                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemVv1),
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemVv2),
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemVv3),
                benhAn?.BenhAn?.ThuThuatYhhd == 1 ? "x" : "",
                benhAn?.BenhAn?.PhauThuatYhhd == 1 ? "x" : "",
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhChinhRv),
                PrintHelper.ConcatStringArr((object)"\n", PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemRv1), PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemRv2), PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemRv3)),
                benhAn?.BenhAn?.TaiBienYhhd == 1 ? "x" : "",
                benhAn?.BenhAn?.BienChungYhhd == 1 ? "x" : "",
                PrintHelper.Trimmer(benhAn?.BenhNoiChuyenDenYHCT?.TenBenhIcd),
                !String.IsNullOrEmpty(benhAn?.BenhAn?.TenBenhChinhVvyhct) ? PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhChinhVvyhct) : PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhChinhVvyhct),
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemVv1yhct),
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemVv2yhct),
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemVv3yhct),
                benhAn?.BenhAn?.ThuThuatYhct == 1 ? "x" : "",
                 benhAn?.BenhAn?.PhauThuatYhct == 1 ? "x" : "",
                 benhAn?.BenhAn?.TaiBienYhct == 1 ? "x" : "",
                 benhAn?.BenhAn?.BienChungYhct == 1 ? "x" : "",
                 benhAn?.BenhAn?.TenBenhChinhRvyhct,
                PrintHelper.ConcatStringArr((object)"\n", PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemRv1yhct), PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemRv2yhct), PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhKemRv3yhct)),
                benhAn?.GiamDoc?.HoTen,
                benhAn?.TruongKhoa?.HoTen,
                benhAn?.BsDieutri?.HoTen,
                benhanYHHD?.LyDoVv,
                benhanYHHD?.BenhSu, //BenhSu
				tienSuBenh?.TienSuBanThan,
                tienSuBenh?.DacDiemLienQuanBenh, //DacDiem
				tienSuBenh?.TienSuGiaDinh,
                benhanYHHD?.ToanThan,
                benhanYHHD?.Mach?.ToString(),
                benhanYHHD?.NhietDo?.ToString(),
                benhanYHHD?.HuyetAp?.ToString(),
                benhanYHHD?.NhipTho?.ToString(),
                ((int?)benhanYHHD?.CanNang)?.ToString(),
                ((int?)benhanYHHD?.ChieuCao)?.ToString(),
                benhanYHHD?.Bmi?.ToString("0.00"),
                benhanYHHD?.TuanHoan,
                benhanYHHD?.HoHap,
                benhanYHHD?.TieuHoa,
                benhanYHHD?.ThanTnieuSduc, //TietNieu
				benhanYHHD?.ThanKinh,
                benhanYHHD?.XuongKhop,
                benhanYHHD?.TaiMuiHong,
                benhanYHHD?.RangHamMat,
                benhanYHHD?.Mat,
                benhanYHHD?.NoiTietDd,
                benhanYHHD?.CanLamSang,
                benhanYHHD?.TomTatBenhAn,
                "",//BenhKemTheoYHHD
				benhanYHHD?.DmBenhPhanBiet != null ? $"{benhanYHHD?.DmBenhPhanBiet?.MaBenh} - {benhanYHHD?.DmBenhPhanBiet?.TenBenh}" : "", //PhanBiet
				benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MtvaanChan,
                benhanYHCT?.MtthietChan,
                benhanYHCT?.MachTrai,
                benhanYHCT?.MachPhai,
                benhanYHCT?.TomTatTuChan,
                benhanYHCT?.BienChungLuanTri,
                $"{benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh} - {benhanYHCT?.DmBenhDanhTheoYHCT?.TenBenh}",
                benhanYHCT?.BatCuong,
                benhanYHCT?.MoTaNguyenNhan,
                benhanYHCT?.MoTaTangPhu,
                benhanYHCT?.MoTaKinhMach,
                benhanYHCT?.MoTaDinhViBenhTheo,
                benhanYHCT?.Ppdtyhct,
                benhanYHCT?.PhuongDuoc,
                benhanYHCT?.PpdtkhongDungThuoc,
                benhanYHHD?.HuongDtyhhd, //Hướng điều trị
				benhanYHHD?.TienLuong, //Dư hậu,
				tongKetBenhAn?.LyDoVv,
                tongKetBenhAn?.QuaTrinhBenhLy,
                tongKetBenhAn?.TomTatKetQuaCls,
                tongKetBenhAn?.TinhTrangBnrv,
                tongKetBenhAn?.HuongDt,
                tongKetBenhAn?.PpdttheoYhhd,
                tongKetBenhAn?.PpdttheoYhct,
                benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MtvaanChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MoTaXucChan,
                benhanYHCT?.PpdtkhongDungThuoc,
                benhanYHCT?.Ppkhac,
                benhanYHHD?.Ppdtyhhd,
                PrintHelper.DateTimeText(benhAn?.BenhAn?.NgayTuVong),
                benhAn?.BenhAn?.NguyenNhanTuVong,
                benhAn?.BenhAn?.KhamNghiemTuThi == 1 ? "x" : "",
                benhAn?.BenhGPTuThis?.TenBenh,
                tongKetBenhAn?.SoToXquang?.ToString(),
                tongKetBenhAn?.SoToCt?.ToString(),
                tongKetBenhAn?.SoToMri?.ToString(),
                tongKetBenhAn?.SoToSa?.ToString(),
                tongKetBenhAn?.SoToXn?.ToString(),
                String.IsNullOrEmpty(tongKetBenhAn?.Khac) ? "Khác ...." : tongKetBenhAn?.Khac,
                tongKetBenhAn?.SoToKhac?.ToString(),
                tongKetBenhAn?.SoToToanBoHs?.ToString(),
                tongKetBenhAn?.DmNguoiGiao?.HoTen,
                tongKetBenhAn?.DmNguoiNhan?.HoTen,
                tongKetBenhAn?.DmBsDieutri?.HoTen,
                PrintHelper.DateText(tongKetBenhAn?.NgayKy),
                tongKetBenhAn?.SoToSa?.ToString(),
                tongKetBenhAn?.SoToXn?.ToString(),
                PrintHelper.DateText(benhAn?.BenhAn?.NgayKy),
                //yhhd
                PrintHelper.DateTimeText(benhanYHHD?.NgayKham), //NgayGioLamBenh
				benhanYHHD?.DmBskham?.HoTen, //ThayThuocLamBenh
                PrintHelper.Trimmer(benhAn?.BenhAn?.TenBenhChinhVv),
                benhAn?.BenhAn?.Vvlan?.ToString(),
                benhAn?.BenhAn?.TongSoLanPt.ToString(),
                benhAn?.BenhAn?.TongSoNgayDtsauPt?.ToString(),
                PrintHelper.DateText(benhAn?.BenhAn?.NgayKy),
                benhAn?.BenhKKBYHCT?.TenBenhIcd,
                benhAn?.BenhKKBYHHD?.TenBenh,
                tienSuBenh?.MoTaTienSu,
                //yhct
                //MoTaKhac
                benhanYHCT?.MoTaVongChan,
                benhanYHCT?.MoTaVanChan,
                benhanYHCT?.MtthietChan,
                benhanYHCT?.MoTaXucChan,
                benhanYHCT?.MoTaTangPhu,
                benhanYHCT?.MoTaKinhMach,
                benhAn?.BenhNhan?.SoTheBhyt
            };

            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.NgheNghiep?.MaNn, "Nghenghiep", 2, '0', true);
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.DanToc?.MaDanToc, "dantoc", 2, '0', true);
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.QuanHuyen?.MaBhxh, "huyen", 3, '0', true);
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.PhuongXa?.MaBhxh, "xaphuong", 5, '0', true);
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.Tinh?.MaTinh, "thanhpho", 2, '0', true);

            var combo_ds = _benhAnRepository._context.DmbaCombods.Where(x => maDsCombo.Contains(x.MaParent)).OrderBy(x => x.MaParent).ThenBy(x => x.Ma).ToList();
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, PrintHelper.BirthText(benhAn?.BenhNhan?.NgaySinh), "NgaySinh", 8, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhNhan?.Tuoi?.ToString(), "Tuoi", 2, '0');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.TongSoNgayDt.ToString(), "TongSoNgayDT", 2, '0');
            PrintHelper.TextboxFieldDoiTuongHanlder(ref fields, ref values, benhAn?.DoiTuong?.MaDt);
            PrintHelper.TextboxFieldBHYTHanlder(ref fields, ref values, benhAn?.BenhNhan?.SoTheBhyt, "Bhyt", false);
            //PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhNhan?.SoTheBhyt, "Bhyt", 1, ' ');
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "tructiepvao", benhAn?.BenhAn?.TrucTiepVao, new string[] { "1", "2", "3" });
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "NoiGioiThieu", benhAn?.BenhAn?.NoiGt, new string[] { "1", "2", "3" });
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "ChuyenVien", benhAn?.BenhAn?.ChuyenVien, new string[] { "1", "2", "3" });
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "RaVien", benhAn?.BenhAn?.HtraVien, new string[] { "1", "2", "3", "4" });
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "nguyennhan", benhAn?.BenhAn?.NguyenNhanTBBC, new string[] { "1", "2", "3", "4" });
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhTatNoiChuyenDen?.MaBenh, "Noichuyenden", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhKKBYHHD?.MaBenh, "KhoaKhamBenh", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhChinhVv, "BenhChinh", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemVv1, "BenhKemTheo_1", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemVv2, "BenhKemTheo_2", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemVv3, "BenhKemTheo_3", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhChinhRv, "MaBenhChinhRv", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemRv1, "RaVienBenhKemTheoYHHD_1", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemRv2, "RaVienBenhKemTheoYHHD_2", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemRv3, "RaVienBenhKemTheoYHHD_3", 5, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhNoiChuyenDenYHCT?.MaBenh, "NoichuyendenYHCT", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhKKBYHCT?.MaBenh, "KhoaKhamBenhYHCT", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhChinhVvyhct, "BenhChinhYHCT", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemVv1yhct, "BenhKemTheoYHCT_1", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemVv2yhct, "BenhKemTheoYHCT_2", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemVv3yhct, "BenhKemTheoYHCT_3", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhChinhRvyhct, "RaVienBenhChinh", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhanYHCT?.DmBenhDanhTheoYHCT?.MaBenh, "ChanDoanBenhDanh", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemRv1yhct, "RaVienBenhKeoTheo_1", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemRv2yhct, "RaVienBenhKeoTheo_2", 6, ' ');
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhAn?.MaBenhKemRv3yhct, "RaVienBenhKeoTheo_3", 6, ' ');
            PrintHelper.OptionFieldValueHanlder(ref fields, ref values, "KQDT", benhAn?.BenhAn?.Kqdt, new string[] { "1", "2", "3", "4", "5" });
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "KQDT_TK", tongKetBenhAn.Kqdt, 1, getDsCombo(combo_ds, "130"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanHinhThai", benhanYHCT.HinhThaiVongChan, 3, getDsCombo(combo_ds, "045"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanThan", benhanYHCT.ThanVongChan, 2, getDsCombo(combo_ds, "046"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanSac", benhanYHCT.SacVongChan, 3, getDsCombo(combo_ds, "047"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanTrach", benhanYHCT.TrachVongChan, 2, getDsCombo(combo_ds, "048"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanLuoi", benhanYHCT.HinhThaiLuoi, 2, getDsCombo(combo_ds, "049"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanChatLuoi", benhanYHCT.ChatLuoi, 3, getDsCombo(combo_ds, "050")); //old 6
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VongChuanReuLuoi", benhanYHCT.ReuLuoi, 3, getDsCombo(combo_ds, "051")); //old 6
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAmThanhTiengNoi", benhanYHCT.GiongNoi, 5, getDsCombo(combo_ds, "056"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAmThanhHoiTho", benhanYHCT.HoiTho, 5, getDsCombo(combo_ds, "057"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoAmThanhHo", PrintHelper.HanlderBooleanType(benhanYHCT.CoHo), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAmThanhHo", benhanYHCT.Ho, 3, getDsCombo(combo_ds, "059"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAmThanhO", PrintHelper.HanlderBooleanType(benhanYHCT.OamThanh), 1);
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAmThanhNac", PrintHelper.HanlderBooleanType(benhanYHCT.NacAmThanh), 1);
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoMui", PrintHelper.HanlderBooleanType(benhanYHCT.CoMui), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanMui", benhanYHCT.KieuMui, 3, getDsCombo(combo_ds, "063"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanCoChatThai", PrintHelper.HanlderBooleanType(benhanYHCT.CoChatThaiBenhLy), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanChatThai", benhanYHCT.KieuChatThai, 3, getDsCombo(combo_ds, "065"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoHanNhiet", PrintHelper.HanlderBooleanType(benhanYHCT.BhhanNhiet), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanHanNhiet", benhanYHCT.Hannhiet, 3, getDsCombo(combo_ds, "067"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanMoHoi", benhanYHCT.MoHoi, 3, getDsCombo(combo_ds, "068"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoDauMatCo", PrintHelper.HanlderBooleanType(benhanYHCT.BhdauMatCo), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoDauDau", benhanYHCT.DauDau, 3, getDsCombo(combo_ds, "070"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanDauMatCoHoaMatChongMat", PrintHelper.HanlderBooleanType(benhanYHCT.HoaMat), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoMat", benhanYHCT.Mat, 3, getDsCombo(combo_ds, "072"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoTai", benhanYHCT.Tai, 3, getDsCombo(combo_ds, "073"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoMui", benhanYHCT.Mui, 3, getDsCombo(combo_ds, "074"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoHong", benhanYHCT.Hong, 3, getDsCombo(combo_ds, "075"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanDauMatCoCoVai", benhanYHCT.CoVai, 3, getDsCombo(combo_ds, "076"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoLung", PrintHelper.HanlderBooleanType(benhanYHCT.Bhlung), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanLung", benhanYHCT.Lung, 4, getDsCombo(combo_ds, "078"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanCoNguc", PrintHelper.HanlderBooleanType(benhanYHCT.Bhnguc), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanNguc", benhanYHCT.Nguc, 6, getDsCombo(combo_ds, "080"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanBung", PrintHelper.HanlderBooleanType(benhanYHCT.Bhbung), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanBung", benhanYHCT.Bung, 6, getDsCombo(combo_ds, "082"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChanChanTay", PrintHelper.HanlderBooleanType(benhanYHCT.BhchanTay), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChanChanTay", benhanYHCT.ChanTay, 6, getDsCombo(combo_ds, "084"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanAn", PrintHelper.HanlderBooleanType(benhanYHCT.Bhan), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanAn", benhanYHCT.An, 3, getDsCombo(combo_ds, "086"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanUong", PrintHelper.HanlderBooleanType(benhanYHCT.Bhuong), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanUong", benhanYHCT.Uong, 3, getDsCombo(combo_ds, "088"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanDaiTieuTien", PrintHelper.HanlderBooleanType(benhanYHCT.BhdaiTieuTien), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanTieuTien", benhanYHCT.TieuTien, 3, getDsCombo(combo_ds, "090"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanDaiTien", benhanYHCT.DaiTien, 3, getDsCombo(combo_ds, "091"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanNgu", PrintHelper.HanlderBooleanType(benhanYHCT.Bhngu), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanNgu", benhanYHCT.Ngu, 3, getDsCombo(combo_ds, "093"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSan", PrintHelper.HanlderBooleanType(benhanYHCT.RlknsinhDuc), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNam", benhanYHCT.Rlnam, 3, getDsCombo(combo_ds, "095"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNu", benhanYHCT.Rlnu, 3, getDsCombo(combo_ds, "096"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuKinhNguyet", PrintHelper.HanlderBooleanType(benhanYHCT.Bhkn), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuKinhNguyet", benhanYHCT.Rlkinhnguyet, 2, getDsCombo(combo_ds, "098"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuThongKinh", benhanYHCT.ThongKinh, 2, getDsCombo(combo_ds, "099"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuDoiHa", PrintHelper.HanlderBooleanType(benhanYHCT.BhdoiHa), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "VanChuanSinhSanNuDoiHa", benhanYHCT.DoiHa, 2, getDsCombo(combo_ds, "101"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "VanChuanLienQuan", PrintHelper.HanlderBooleanType(benhanYHCT.Dkxhbenh), 1);
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuan", PrintHelper.HanlderBooleanType(benhanYHCT.BhxucChan), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanDa", benhanYHCT.XucChanDa, 3, getDsCombo(combo_ds, "105"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanCoXuong", PrintHelper.HanlderBooleanType(benhanYHCT.BhcoXuongKhop), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanCoXuong", benhanYHCT.XucChanCoXuongKhop, 3, getDsCombo(combo_ds, "107"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanBung", PrintHelper.HanlderBooleanType(benhanYHCT.BhbungXucChan), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanBung", benhanYHCT.XucChanBung, 3, getDsCombo(combo_ds, "109"));
            PrintHelper.OptionFieldMultipleHanlder(ref fields, ref values, "ThietChuanMoHoi", PrintHelper.HanlderBooleanType(benhanYHCT.BhmoHoi), 1);
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMoHoi", benhanYHCT.XucChanMoHoi, 3, getDsCombo(combo_ds, "111"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuan", benhanYHCT.MachChan, 3, getDsCombo(combo_ds, "112"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanTongKhamPhai", benhanYHCT.TongKhanPhai, 3, getDsCombo(combo_ds, "113"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanTongKhamTrai", benhanYHCT.TongKhanTrai, 3, getDsCombo(combo_ds, "114"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiThon", benhanYHCT.ViKhanTraiThon, 3, getDsCombo(combo_ds, "115"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiQuan", benhanYHCT.ViKhanTraiQuan, 3, getDsCombo(combo_ds, "116"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanTraiXich", benhanYHCT.ViKhanTraiXich, 3, getDsCombo(combo_ds, "117"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiThon", benhanYHCT.ViKhanPhaiThon, 3, getDsCombo(combo_ds, "118"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiQuan", benhanYHCT.ViKhanPhaiQuan, 3, getDsCombo(combo_ds, "119"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ThietChuanMachChuanViKhanPhaiXich", benhanYHCT.ViKhanPhaiXich, 3, getDsCombo(combo_ds, "120"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanBatCuong", benhanYHCT.BhbatCuong, 5, getDsCombo(combo_ds, "121"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanNguyenNhan", benhanYHCT.NguyenNhan, 1, getDsCombo(combo_ds, "122"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanTangPhu", benhanYHCT.TangPhu, 4, getDsCombo(combo_ds, "123"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanKinhMach", benhanYHCT.KinhMach, 4, getDsCombo(combo_ds, "124"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "ChanDoanDinhViBenh", benhanYHCT.DinhViBenhTheo, 4, getDsCombo(combo_ds, "125"));

            //tinh trang ra vien
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVKQDT", benhAn?.BenhAn?.Kqdt, PrintHelper.CreateArrayIncreate(5));
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "GiaiPhauBenh", benhAn?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVTTTV", benhAn?.BenhAn?.TinhTrangTuVong, PrintHelper.CreateArrayIncreate(6));
            PrintHelper.OptionFieldHanlder(ref fields, ref values, "TTRVGPB", benhAn?.BenhAn?.GiaiPhauBenh, PrintHelper.CreateArrayIncreate(3));
            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn?.BenhGPTuThis?.MaBenh, "ChuanDoanGiaiPhau", 5, ' ');


            PrintHelper.OptionFieldSyncMultipleHanlder(ref fields, ref values, "BanThan", tienSuBenh.MaTienSu, 6, getDsCombo(combo_ds, "017"));


            fields.Add("Khoa");
            fields.Add("VaoKhoaNgayGio");
            if (benhAnKhoaDieuTri != null)
            {
                values.Add(benhAnKhoaDieuTri.Khoa.TenKhoa);
                values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTri.NgayVaoKhoa));
            }
            else
            {
                values.Add("");
                values.Add("");
                values.Add("");
            }
            // to dieu tri
            var toDieuTriService = new BenhAnToDieuTriService();

            var dataset = new DataSet();
            var list = new List<DictionaryEntry>();
            if (benhAn.LoaiBenhAn.MaLoaiBa == 3)
            {
                ManTinhPrint(ref fields, ref values, benhAn, benhAnKhoaDieuTris, phieukhamvv, tienSuBenh, benhanYHHD, benhanYHCT, tongKetBenhAn, combo_ds);

            }
            else
            {
                for (int i = 1; i < benhAnKhoaDieuTris.Count; i++)
                {
                    fields.Add($"KhoaChuyen{i}");
                    values.Add(benhAnKhoaDieuTris[i].Khoa.TenKhoa);
                    fields.Add($"NgayGioKhoaChuyen{i}");
                    values.Add(PrintHelper.DateShortTimeText(benhAnKhoaDieuTris[i].NgayVaoKhoa));
                    var compareDate = i != (benhAnKhoaDieuTris.Count - 1) ? PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i].NgayVaoKhoa, benhAnKhoaDieuTris[i + 1].NgayVaoKhoa, 1) : PrintHelper.CompareTwoDate(benhAnKhoaDieuTris[i].NgayVaoKhoa, benhAn?.BenhAn?.NgayRv, 1);

                    PrintHelper.TexboxFieldHanlder(ref fields, ref values, compareDate, $"MaKhoaChuyen{i}", 2, '0');
                }
            }
            string path = PrintHelper.PrintFileWithTable(null, _hostingEnvironment, template, null, null, fields, values);
            return path;
        }
        //hanlder mãn tính 
        private void ManTinhPrint(ref List<string> fields, ref List<string> values, DetailBenhAnDto benhAn, List<BenhAnKhoaDieuTriDto> benhAnKhoaDieuTris, BenhAnKhamVaoVien phieukhamvv, BenhAnTienSuBenh tienSuBenh, BenhAnKhamYhhd benhanYHHD, BenhAnKhamYhct benhanYHCT, BenhAnTongKetBenhAn tongKetBenhAn, List<DmbaCombods> combo_ds)
        {
            List<string> field_more = new List<string>()
            {
                "SoYTe",
                "NgaySinh",
                "Tuoi",
                "DiaChi",
                "DoiTuong",
                "SoTheBHYT",
                "HinhThucRaVien",
                "NoiGioiThieu",
                "TongSoNgayDT",
                "TuyenDuoi",
                "CapCuuKhoaKhamBenh",
                "VaoKhoaBenhChinh",
                "VaoKhoaBenhKemTheo",
                "RaVienBenhChinh",
                "RaVienKemTheo",
                "BienChungTaiBien",
                "GiaiPhauBenh_MT",
                "KhamNghiemTuThiManTinh",
                "KetQuaDieuTriRV",
                "TinhHinhTuVong",
                "TinhHinhSauVaoVien",
                "SauPhauThuat",
                "HutThuocLa",
                "SoDieu",
                "SoBao",
                "UongRuou",
                "LuongUongRuou",
                "CacYeuToKhac",
                "BenhLyKhac",
                "BenhTangHuyetApPhatHienKhiNao",
                "BenhTangHuyetApTuDieuTriHoacDieuTriODau",
                "BenhTangHuyetApCoDieuTriThuongXuyenKhong",
                "BenhTangHuyetApDonTriLieuHayDaTriLieu",
                "ChiSoHA",
                "BenhDaiThaoDuongPhatHienKhiNao",
                "BenhDaiThaoDuongTuDieuTri",
                "BenhDaiThaoDuongDieuTriThuongXuyen",
                "BenhDaiThaoDuongDonTriLieuHayDaTriLieu",
                "ChiSoDuongHuyet",
                "BenhSu",
                "HA",
                "NhipTim",
                "ChieuCao",
                "CanNang",
                "ChiSoBMI",
                "TrieuChung",
                "KetQuaXetNghiemMau",
                "KetQuaXetNghiemNuocTieu",
                "KetQuaChanDoanHinhAnh",
                "ChanDoanXacDinh",
                "MoTaVongChan",
                "MoTaVanChan",
                "MoTaXucChan",
                "MachChanTayTrai",
                "MachChanTayPhai",
                "TomTatTuChan",
                "BienChungLuanTri",
                "BenhDanh",
                "BatCuong",
                "TangPhuKinhLac",
                "NguyenNhan",
                "DieuTriDonThuanYHCT",
                "PhapDieuTri",
                "PhuongThuoc",
                "PhuongHuyet",
                "XoaBopDamHuyet",
                "DieuTriKhac",
                "CoDieuTriKetHopYHHD",
                "DieuTriKetHopYHHD",
                "KetQuaDieuTri",
                "HuongDieuTriVaCacCheDoTiep",
                "NgayHenKhamLai",
                "NgayHenXetNghiemLai",
                "DuHauTL",
                "NgayKy_MT"
            };
            List<string> value_more = new List<string>()
            {
                PrintSetting.SoYTe,
                PrintHelper.DateTextShortest(benhAn.BenhNhan.NgaySinh),
                benhAn.BenhNhan.Tuoi.ToString(),
                PrintHelper.HanlderDiaChiText(benhAn?.BenhNhan?.SoNha, benhAn?.BenhNhan?.Thon, benhAn?.PhuongXa?.TenPxa, benhAn?.QuanHuyen?.TenQh, benhAn?.Tinh?.TenTinh),
                benhAn.DoiTuong.MaDt,
                benhAn.BenhNhan.SoTheBhyt,
                benhAn.BenhAn.HtraVien,
                benhAn.BenhAn.NguyenNhanTBBC,
                benhAn.BenhAn.NoiGt,
                PrintHelper.HanlderUnknowsType<int>(benhAn.BenhAn.TongSoNgayDt),
                "", //TuyenDuoi
				benhAn.BenhKKBYHHD.TenBenh, //CapCuuKhoaKhamBenh
				benhAn.BenhAn.TenBenhChinhVv,
                benhAn.BenhAn.TenBenhKemVv1,
                benhAn.BenhAn.TenBenhChinhRv,
                benhAn.BenhAn.TenBenhKemRv1,
                (benhAn.BenhAn.TaiBienYhhd == 1 || benhAn.BenhAn.BienChungYhhd == 1) ? "1" : "2",
                benhAn.BenhAn.GiaiPhauBenh,
                benhAn.BenhAn.KhamNghiemTuThi?.ToString(),
                benhAn.BenhAn.Kqdt,
                benhAn.BenhAn.TinhTrangTuVong == "1" || benhAn.BenhAn.TinhTrangTuVong == "2" ? benhAn.BenhAn.TinhTrangTuVong:"",
                benhAn.BenhAn.TinhTrangTuVong == "4" ? benhAn.BenhAn.TinhTrangTuVong:"",
                benhAn.BenhAn.TinhTrangTuVong == "5" ? benhAn.BenhAn.TinhTrangTuVong:"",
                benhAn.BenhAn.TinhTrangTuVong == "6" ? benhAn.BenhAn.TinhTrangTuVong:"",
               tienSuBenh.HutThuoc?.ToString(),
                tienSuBenh?.SoDieu?.ToString(),
                tienSuBenh?.SoBao?.ToString(),
                tienSuBenh?.UongRuou?.ToString(),
                ((int?)tienSuBenh?.LuongRuou)?.ToString(),
                tienSuBenh.YeuToNguyCoKhac,
                tienSuBenh.BenhPhoiHopBenhLyKhac,
                tienSuBenh.ThoiDiemPhatHienTangHa,
                tienSuBenh.NoiDieuTriTangHa,
                tienSuBenh.DieuTriTangHathuongXuyen,
                tienSuBenh.DonDaTriLieuHa,
                tienSuBenh.ChiSoHamax,
                tienSuBenh.ThoiDiemPhatHienDtd,
                tienSuBenh.NoiDieuTriDtd,
                tienSuBenh.DieuTriDtdthuongXuyen,
                tienSuBenh.DonDaTriLieuDtd,
                tienSuBenh.Dtdmax,
                benhanYHHD.BenhSu,
                benhanYHHD.HuyetAp,
                benhanYHHD?.NhipTim?.ToString(),
                ((int?)benhanYHHD.ChieuCao)?.ToString(),
                ((int?)benhanYHHD?.CanNang)?.ToString(),
                benhanYHHD.Bmi?.ToString("0.00"),
                benhanYHHD.TrieuChung,// Triệu chứng
				benhanYHHD.Kqxnmau,
                benhanYHHD.KqxnnuocTieu,
                benhanYHHD.Kqcdha,
                $"{benhAn.BenhAn.MaBenhChinhVv} - {benhAn.BenhAn.TenBenhChinhVv}",//ChanDoanXacDinh
                benhanYHCT.MoTaVongChan,
                benhanYHCT.MoTaVanChan,
                benhanYHCT.MoTaXucChan,
                benhanYHCT.MachTrai,
                benhanYHCT.MachPhai,
                benhanYHCT.TomTatTuChan,
                benhanYHCT.BienChungLuanTri,
                benhanYHCT.DmBenhDanhTheoYHCT != null ? benhanYHCT.DmBenhDanhTheoYHCT.TenBenhIcd : null,
                benhanYHCT.BatCuong,
                benhanYHCT.MoTaTangPhu,
                benhanYHCT.NguyenNhan,
                benhanYHCT.DtdonThuanYhct == 1 ? "x" : "",
                benhanYHCT.Ppdtyhct,
                benhanYHCT.PhuongDuoc,
                benhanYHCT.PhuongHuyet,
                benhanYHCT.XoaBopBamHuyet,
                benhanYHCT.Ppkhac,
                benhanYHHD.DtketHopYhhd == 1 ? "x" : "",
                benhanYHHD.Ppdtyhhd,
                benhanYHHD.Kqdt,
                benhanYHHD.HuongDtyhhd,
                PrintHelper.DateTextShortest(benhanYHHD.NgayHenKhamLai),
                PrintHelper.DateTextShortest(benhanYHHD.NgayHenXnlai),
                benhanYHHD.TienLuong,
                PrintHelper.DateText(tongKetBenhAn.NgayKy)
            };

            PrintHelper.TexboxFieldHanlder(ref field_more, ref value_more, benhAn.BenhAn.MaBenhChinhVv, "VaoKhoaBenhChinh", 4, ' ');
            PrintHelper.TexboxFieldHanlder(ref field_more, ref value_more, benhAn.BenhAn.MaBenhChinhRv, "RaVienBenhChinh", 4, ' ');
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopTangHa, "TangHuyetAp");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopDtd, "DaiThaoDuong");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopGout, "Gout");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopKhopManTinh, "KhopManTinh");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopRlchLipid, "RLCHLipit");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopThan, "BenhThan");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopMachVanh, "BenhMachVanh");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhPhoiHopNoiTietKhac, "BenhNoiTiet");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.NamMacTmHaDtd, "TienSuGiaDinh_Nam");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.NuMacTmHaDtd, "TienSuGiaDinh_Nu");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhTangHa, "BenhTangHuyetAp");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.BenhDtd, "BenhDaiThaoDuong");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.ThucHienCdanDtd, "TieuDuongAnKieng");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, tienSuBenh.UongThuocDtd, "ThucUong");
            //PrintHelper.HanlderBooleanTypeOption(ref field_more, ref value_more, tienSuBenh.LoaiThuocDtd, "ThucUong");
            PrintHelper.HanlderBooleanNumberTypeOption(ref field_more, ref value_more, benhanYHHD.NhipTimDeu, "NhipTimDeu");
            PrintHelper.OptionFieldSyncMultipleHanlder(ref field_more, ref value_more, "CheDoDinhDuong", benhanYHHD.CddinhDuong, 3, getDsCombo(combo_ds, "128"));
            PrintHelper.OptionFieldSyncMultipleHanlder(ref field_more, ref value_more, "KQDT_MT", tongKetBenhAn.Kqdt, 1, getDsCombo(combo_ds, "131"));
            PrintHelper.OptionFieldSyncHanlder(ref field_more, ref value_more, "LoaiThuocUong", tienSuBenh.LoaiThuocDtd, 5, getDsCombo(combo_ds, "192"));
            PrintHelper.OptionFieldMultipleHanlder(ref field_more, ref value_more, "CheDoChamSoc", benhanYHHD.CdchamSoc, 1);

            var benhPhoiHop = new string[]{
               !String.IsNullOrEmpty(benhAn.BenhAn.MaBenhKemVv1) ?  $"{benhAn.BenhAn.MaBenhKemVv1} - {benhAn.BenhAn.TenBenhKemVv1}" : "",
               !String.IsNullOrEmpty(benhAn.BenhAn.MaBenhKemVv2) ?  $"{benhAn.BenhAn.MaBenhKemVv2} - {benhAn.BenhAn.TenBenhKemVv2}" : "",
               !String.IsNullOrEmpty(benhAn.BenhAn.MaBenhKemVv3) ?  $"{benhAn.BenhAn.MaBenhKemVv3} - {benhAn.BenhAn.TenBenhKemVv3}" : "",
            };
            //benh phoi hop
            field_more.Add("BenhPhoiHop");
            value_more.Add(string.Join(",", Array.FindAll(benhPhoiHop, x => !string.IsNullOrEmpty(x))));
            //chuyen khoa
            field_more.Add("ChuyenKhoa");
            var chuyenKhoa = new List<string>();
            for (int i = 1; i < benhAnKhoaDieuTris.Count; i++)
            {
                chuyenKhoa.Add(benhAnKhoaDieuTris[i].Khoa.TenKhoa);
            }
            value_more.Add(string.Join(",", Array.FindAll(chuyenKhoa.ToArray(), x => !string.IsNullOrEmpty(x))));
            for (int i = 0; i < field_more.Count; i++)
            {
                fields.Add(field_more[i]);
                values.Add(value_more[i]);
            }
            //tongket chẩn đoán 
            fields.Add("BenhChinhYHHDVV");
            if (benhAnKhoaDieuTris.Count > 0)
            {
                values.Add(PrintHelper.ConcatStringArr(
                 !string.IsNullOrEmpty(benhAnKhoaDieuTris[0].BenhChinh.MaBenh) ? $"{benhAnKhoaDieuTris[0].BenhChinh.MaBenh} - {benhAnKhoaDieuTris[0].BenhChinh.TenBenh}" : "",
                 !string.IsNullOrEmpty(benhAnKhoaDieuTris[0].BenhKem1.MaBenh) ? $"{benhAnKhoaDieuTris[0].BenhKem1.MaBenh} - {benhAnKhoaDieuTris[0].BenhKem1.TenBenh}" : "",
                 !string.IsNullOrEmpty(benhAnKhoaDieuTris[0].BenhKem2.MaBenh) ? $"{benhAnKhoaDieuTris[0].BenhKem2.MaBenh} - {benhAnKhoaDieuTris[0].BenhKem2.TenBenh}" : "",
                 !string.IsNullOrEmpty(benhAnKhoaDieuTris[0].BenhKem3.MaBenh) ? $"{benhAnKhoaDieuTris[0].BenhKem3.MaBenh} - {benhAnKhoaDieuTris[0].BenhKem3.TenBenh}" : ""
                ));
            }
            else
            {
                values.Add("");
            }
            fields.Add("BenhChinhYHCTVV");
            var BenhChinhVvyhct = _benhAnRepository._context.DmbenhTatYhct.Where(x => x.MaBenh == benhAn.BenhAn.MaBenhChinhVvyhct).Select(x => $"{x.MaBenhIcd} - {x.TenBenhIcd}").FirstOrDefault();

            values.Add(PrintHelper.ConcatStringArr(BenhChinhVvyhct,
                 !string.IsNullOrEmpty(benhAn.BenhAn.MaBenhKemVv1yhct) ? $"{benhAn.BenhAn.MaBenhKemVv1yhct} - {benhAn.BenhAn.TenBenhKemVv1yhct}" : "",
                 !string.IsNullOrEmpty(benhAn.BenhAn.MaBenhKemVv2yhct) ? $"{benhAn.BenhAn.MaBenhKemVv2yhct} - {benhAn.BenhAn.TenBenhKemVv2yhct}" : "",
                 !string.IsNullOrEmpty(benhAn.BenhAn.MaBenhKemVv3yhct) ? $"{benhAn.BenhAn.MaBenhKemVv3yhct} - {benhAn.BenhAn.TenBenhKemVv3yhct}" : ""));


            PrintHelper.TexboxFieldHanlder(ref fields, ref values, benhAn.BenhAn.MaBenhKemRv1, "RaVienBenhKem", 4, ' ');
            var benhanshort = _benhAnRepository._context.BenhAn.Where(x => x.Idba == benhAn.BenhAn.Idba).Select(x => new
            {
                benhChinhRv = x.DmBenhChinhRV != null ? $"{x.DmBenhChinhRV.MaBenh} - {x.DmBenhChinhRV.TenBenh}" : null,
                DmBenhKemRV1 = x.DmBenhKemRV1 != null ? $"{x.DmBenhKemRV1.MaBenh} - {x.DmBenhKemRV1.TenBenh}" : null,
                DmBenhKemRV2 = x.DmBenhKemRV2 != null ? $"{x.DmBenhKemRV2.MaBenh} - {x.DmBenhKemRV2.TenBenh}" : null,
                DmBenhKemRV3 = x.DmBenhChinhRV != null ? $"{x.DmBenhKemRV3.MaBenh} - {x.DmBenhKemRV3.TenBenh}" : null,
                benhChinhRvyhct = x.DmBenhChinhRVYHCT != null ? $"{x.DmBenhChinhRVYHCT.MaBenhIcd} - {x.DmBenhChinhRVYHCT.TenBenhIcd}" : null,
                DmBenhKemRV1YHCT = x.DmBenhKemRV1YHCT != null ? $"{x.DmBenhKemRV1YHCT.MaBenhIcd} - {x.DmBenhKemRV1YHCT.TenBenhIcd}" : null,
                DmBenhKemRV2YHCT = x.DmBenhKemRV2YHCT != null ? $"{x.DmBenhKemRV2YHCT.MaBenhIcd} - {x.DmBenhKemRV2YHCT.TenBenhIcd}" : null,
                DmBenhKemRV3YHCT = x.DmBenhKemRV3YHCT != null ? $"{x.DmBenhKemRV3YHCT.MaBenhIcd} - {x.DmBenhKemRV3YHCT.TenBenhIcd}" : null,
            }).FirstOrDefault();
            fields.Add("ChanDoanRVYHHD");
            values.Add(benhanshort == null ? "" : PrintHelper.ConcatStringArr(benhanshort.benhChinhRv, benhanshort.DmBenhKemRV1, benhanshort.DmBenhKemRV2, benhanshort.DmBenhKemRV3));
            fields.Add("ChanDoanRVYHCT");
            values.Add(benhanshort == null ? "" : PrintHelper.ConcatStringArr(benhanshort.benhChinhRvyhct, benhanshort.DmBenhKemRV1YHCT, benhanshort.DmBenhKemRV2YHCT, benhanshort.DmBenhKemRV3YHCT));

        }

        public string GiayRaVienPrint(decimal idba)
        {

            var benhAn = Detail(idba);
            var tongKetBenhAn = _benhAnRepository._context.BenhAnTongKetBenhAn
                    .Include(x => x.DmBsDieutri)
                    .Include(x => x.DmNguoiGiao)
                    .Include(x => x.DmNguoiNhan)
                    .Select(Mapper.Map<BenhAnTongKetBenhAnDto>)
                    .FirstOrDefault(x => x.Idba == idba);
            tongKetBenhAn = tongKetBenhAn ?? new BenhAnTongKetBenhAnDto();
            List<string> fields = new List<string>(){
                "SoYTe",
                "BenhVien",
                "SoLuuTru",
                "MaYTe",
                "HoTen",
                "Tuoi",
                "GioiTinh",
                "MaBN",
                "DanToc",
                "NgheNghiep",
                "Bhyt",
                "DiaChi",
                "GioVaoVien",
                "NgayVaoVien",
                "GioRaVien",
                "NgayRaVien",
                "ChanDoan",
                "PhuongPhapDieuTri",
                "GhiChu",
                "NgayGioDongDau",
                "NgayGioDieuTri",
                "GiamDoc",
                "TruongKhoaDieuTri"
            };
            List<string> values = new List<string>(){
                PrintSetting.SoYTe,
                PrintSetting.BenhVien,
                benhAn?.SoLuuTru,
                benhAn?.MaYt,
                benhAn?.BenhNhan?.HoTen?.ToUpper(),
                benhAn?.BenhNhan?.Tuoi?.ToString(),
                benhAn?.BenhNhan?.GioiTinh == 1 ? "Nam" : "Nữ",
                benhAn?.BenhNhan?.MaBn,
                benhAn?.BenhNhan?.DanToc?.TenDanToc,
                benhAn.BenhNhan.NgheNghiep.TenNn,
                benhAn.BenhNhan.SoTheBhyt,
                PrintHelper.HanlderDiaChiText(benhAn?.BenhNhan?.SoNha, benhAn?.BenhNhan?.Thon, benhAn?.BenhNhan?.PhuongXa?.TenPxa, benhAn?.BenhNhan?.QuanHuyen?.TenQh, benhAn?.BenhNhan?.Tinh?.TenTinh),
                PrintHelper.TimeText(benhAn.NgayVv),
                PrintHelper.DateText(benhAn.NgayVv),
                PrintHelper.TimeText(benhAn.NgayRv),
                PrintHelper.DateText(benhAn.NgayRv),
                benhAn.TenBenhChinhRv,
                tongKetBenhAn.HuongDt,
                "",
                PrintHelper.DateText(tongKetBenhAn.NgayKy),
                PrintHelper.DateText(benhAn.NgayTruongKhoaKy),
                benhAn.Giamdoc.HoTen,
                benhAn.BsDieutri.HoTen
            };
            string path = PrintHelper.PrintFile<BenhAn>(_hostingEnvironment, "giay-ra-vien.doc", fields, values);
            return path;
        }

        public string[] getDsCombo(List<DmbaCombods> combos, string maParent)
        {
            var result = combos.Where(x => x.MaParent == maParent).Select(x => x.Ma).ToArray();
            return result;
        }
        private string[] maDsCombo = new string[] {
            "017",
            "045",
            "046",
            "047",
            "048",
            "049",
            "050",
            "051",
            "056",
            "057",
            "059",
            "063",
            "065",
            "067",
            "068",
            "070",
            "072",
            "073",
            "074",
            "075",
            "076",
            "078",
            "080",
            "082",
            "084",
            "086",
            "088",
            "091",
            "090",
            "093",
            "095",
            "096",
            "098",
            "099",
            "101",
            "105",
            "107",
            "109",
            "111",
            "112",
            "114",
            "113",
            "115",
            "116",
            "117",
            "118",
            "119",
            "120",
            "124",
            "121",
            "122",
            "123",
            "125",
            "128",
            "192",
            "130",
            "131",
            "052",
            "053",
            "054",
            "055",
            "104",
            "201",
            "202",
            "203"

        };

        //public void WriteLog(String log)
        //{
        //    #region ghi log dang nhap
        //    var u = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");

        //    //string constr = ConfigurationManager.AppSettings["MongoDBconnectionString"];
        //    //string constr = _config.GetValue<string>("Medyx_BCADatabase:ConnectionString");
        //    string constr = u.MongoDBConnectionString;
        //    var client = new MongoClient(constr);
        //    //IMongoDatabase db = client.GetDatabase("Medyx_BCA");
        //    //IMongoDatabase dbm = client.GetDatabase(ConfigurationManager.AppSettings["MongoDBDataBase"]);
        //    //IMongoDatabase dbm = client.GetDatabase(_config.GetValue<string>("Medyx_BCADatabase:DatabaseName"));
        //    IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
        //    IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
        //    string MaMay = this.GetLocalIPAddress();
        //    TraceLogMongo emp = new TraceLogMongo();
        //    emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        //    emp.TenBang = "Log";
        //    emp.KieuTacDong = "Error";
        //    emp.NguoiSD = u.Pub_sNguoiSD;
        //    emp.MaMay = MaMay;
        //    emp.NoiDungSD = "Loi API: " + log;
        //    collection.InsertOne(emp);
        //    #endregion
        //}
        //protected string GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }

        //    string ips = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (string.IsNullOrEmpty(ips))
        //    {
        //        ips = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    return ips;
        //    //string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        //    //return ip;
        //}
    }
}
