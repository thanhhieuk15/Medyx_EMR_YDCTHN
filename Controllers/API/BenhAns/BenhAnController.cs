using AutoMapper;
using Medyx_EMR_BCA.ApiAssets.AttributeCustom;
using Medyx_EMR_BCA.ApiAssets.Dto;
using Medyx_EMR_BCA.ApiAssets.Enums;
using Medyx_EMR_BCA.ApiAssets.Helpers;
using Medyx_EMR_BCA.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.Models.Session;
using Medyx_EMR_BCA.ApiAssets.QueryParameters;
using Medyx_EMR_BCA.ApiAssets.Repository;
using Medyx_EMR_BCA.ApiAssets.ResponseHandler;
using Medyx_EMR_BCA.ApiAssets.Services;
using Medyx_EMR_BCA.ApiAssets.ViewModels;
using Medyx_EMR_BCA.Models.DanhMuc;
using Medyx_EMR_BCA.Models.DBConText;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
using Medyx_EMR.Models.DanhMuc;
using Path = System.IO.Path;
using System.CodeDom;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Writers;
using System.IO.Compression;
using System.Collections.Generic;
using Medyx_EMR_BCA.Models;
using MongoDB.Driver;
using System.Windows.Interop;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Spire.Pdf.Fields;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.EntityFrameworkCore;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System.Net.Http;
using System.Threading.Tasks;
using iTextSharp.text;
using Spire.Pdf.Actions;
using System.Windows;
using PdfSharp.Pdf.Annotations;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.Extensions.Caching.Memory;
namespace Medyx_EMR_BCA.Controllers.API.BenhAns
{
    [Route("api/benh-an")]
    [ApiController]
    //[SessionFilter]
    //public static class MaBaStorage 
    //{
    //    public static string MaBaValue { get; set; }
    //}
    public class BenhAnController : ControllerBase
    {
        private readonly BenhAnService _benhAnService;
        private UploadFileRespository uploadFileRespository = null;
        private readonly IRepository<BenhAn> _benhAnRepository = null;
        private readonly IRepository<ThongTinBn> ThongTinBn = null;
        private readonly IRepository<BenhAnClsKq> _BenhAnClsKq = null;
        private readonly IRepository<DM_HSBA> _dmHSBARepository = null;
        private readonly IRepository<DmbaLoaiTaiLieu> _dmbaLoaiTaiLieuRepository = null;
        private IHttpContextAccessor _accessor = null;
        private readonly IConfiguration _config;
        public BenhAnController(BenhAnService benhAnService, IHttpContextAccessor accessor)
        {
            _benhAnService = benhAnService;
            _benhAnRepository = new GenericRepository<BenhAn>(accessor);
            ThongTinBn = new GenericRepository<ThongTinBn>(accessor);
            _BenhAnClsKq = new GenericRepository<BenhAnClsKq>(accessor);
            _dmHSBARepository = new GenericRepository<DM_HSBA>(accessor);
            _dmbaLoaiTaiLieuRepository = new GenericRepository<DmbaLoaiTaiLieu>(accessor);
            uploadFileRespository = new UploadFileRespository();
        }
        //private readonly IConfiguration _config; //(đoạn này chính ra đéo comment đâu vì phải dùng  api cùi bắp của thằng nampe)
        // GET: api/<BenhAnController>
        [HttpGet]
        // [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        [SetActionContextItem(ActionType.PagedList)]
        public Response<BenhAnDto> Get([FromQuery] BenhAnParameters parameters)
        {
            var user = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserSession>(HttpContext.Session, "UserProfileSessionData");
            IQueryable<BenhAnDto> benhAnQuery = _benhAnService.Get(parameters, user);
            //return Res<BenhAnDto>.Get(benhAnQuery, parameters.PageNumber, parameters.PageSize);
            var kq = Res<BenhAnDto>.Get(benhAnQuery, parameters.PageNumber, parameters.PageSize);
            return kq;
        }
        // GET api/<BenhAnController>/5
        // Phụ trách hiển thị dữ liệu chi tiết thông tin bệnh nhân trong mục thông tin chung bệnh án
        [HttpGet("{id}")]
        public BenhAnDetailDto Detail(decimal id)
        {
            return _benhAnService.Detail(id);
        }
        // show toàn bộ dữ liệu của tờ bệnh án
        [HttpGet("{id}/detail")]
        public BenhAn Show(decimal id)
        {
            //BenhAn kq = new BenhAn();
            var data = _benhAnService.Show(id);
            return _benhAnService.Show(id);
            //lây dl từ store
            //HL7CoreDataDataContext db = new HL7CoreDataDataContext();
            //var data = db.BenhAn_DetailDTOGetList(id);
            ////ktra data co bang hay khong?
            //if (data != null && data.Tables.Count > 0)
            //{
            //    //ktra bang so 0 trong store co dl?
            //    if (data.Tables[0].Rows.Count > 0)
            //    {
            //        DateTime d;
            //        kq.Idba = decimal.Parse(data.Tables[0].Rows[0]["IDBA"].ToString());
            //        kq.MaBa = data.Tables[0].Rows[0]["MaBa"].ToString();
            //        kq.TenDvcq = data.Tables[0].Rows[0]["TenDvcq"].ToString();
            //        kq.MaBv = data.Tables[0].Rows[0]["MaBv"].ToString();
            //        kq.TenBv = data.Tables[0].Rows[0]["TenBv"].ToString();
            //        kq.SoVaoVien = data.Tables[0].Rows[0]["SoVaoVien"].ToString();
            //        kq.SoLuuTru = data.Tables[0].Rows[0]["SoLuuTru"].ToString();
            //        kq.MaYt = data.Tables[0].Rows[0]["MaYt"].ToString();
            //        kq.Huy = bool.Parse(data.Tables[0].Rows[0]["Huy"].ToString());
            //        if (DateTime.TryParse(data.Tables[0].Rows[0]["NgayVv"].ToString(), out d))
            //            kq.NgayVv = d;
            //        if (DateTime.TryParse(data.Tables[0].Rows[0]["NgayRv"].ToString(), out d))
            //            kq.NgayRv = d;
            //        kq.TrucTiepVao = data.Tables[0].Rows[0]["TrucTiepVao"].ToString();
            //        kq.NoiGt = data.Tables[0].Rows[0]["NoiGt"].ToString();
            //        kq.ChuyenVien = data.Tables[0].Rows[0]["ChuyenVien"].ToString();
            //        kq.MaBvChuyenDen = data.Tables[0].Rows[0]["MaBvChuyenDen"].ToString();
            //        kq.HtraVien = data.Tables[0].Rows[0]["HtraVien"].ToString();
            //        kq.TongSoNgayDt = int.Parse(data.Tables[0].Rows[0]["TongSoNgayDt"].ToString());
            //        kq.MaBenhChinhVv = data.Tables[0].Rows[0]["MaBenhChinhVv"].ToString();
            //        kq.TenBenhChinhVv = data.Tables[0].Rows[0]["TenBenhChinhVv"].ToString();
            //        kq.ThuThuatYhhd = byte.Parse(data.Tables[0].Rows[0]["ThuThuatYhhd"].ToString());
            //        kq.PhauThuatYhhd = byte.Parse(data.Tables[0].Rows[0]["PhauThuatYhhd"].ToString());
            //        kq.MaBenhChinhRv = data.Tables[0].Rows[0]["MaBenhChinhRv"].ToString();
            //        kq.TenBenhChinhRv = data.Tables[0].Rows[0]["TenBenhChinhRv"].ToString();
            //        kq.TaiBienYhhd = byte.Parse(data.Tables[0].Rows[0]["TaiBienYhhd"].ToString());
            //        kq.BienChungYhhd = byte.Parse(data.Tables[0].Rows[0]["BienChungYhhd"].ToString());
            //        kq.MaBenhChinhVvyhct = data.Tables[0].Rows[0]["MaBenhChinhVvyhct"].ToString();
            //        kq.TenBenhChinhVvyhct = data.Tables[0].Rows[0]["TenBenhChinhVvyhct"].ToString();
            //        kq.MaBenhChinhRvyhct = data.Tables[0].Rows[0]["MaBenhChinhRvyhct"].ToString();
            //        kq.TenBenhChinhRvyhct = data.Tables[0].Rows[0]["TenBenhChinhRvyhct"].ToString();
            //        kq.ThuThuatYhct = byte.Parse(data.Tables[0].Rows[0]["ThuThuatYhct"].ToString());
            //        kq.PhauThuatYhct = byte.Parse(data.Tables[0].Rows[0]["PhauThuatYhct"].ToString());
            //        kq.TaiBienYhct = byte.Parse(data.Tables[0].Rows[0]["TaiBienYhct"].ToString());
            //        kq.BienChungYhct = byte.Parse(data.Tables[0].Rows[0]["BienChungYhct"].ToString());
            //        kq.Kqdt = data.Tables[0].Rows[0]["Kqdt"].ToString();
            //        kq.GiaiPhauBenh = data.Tables[0].Rows[0]["GiaiPhauBenh"].ToString();
            //        if (DateTime.TryParse(data.Tables[0].Rows[0]["NgayTuVong"].ToString(), out d))
            //            kq.NgayTuVong = d;
            //        kq.NguyenNhanTuVong = data.Tables[0].Rows[0]["NguyenNhanTuVong"].ToString();
            //        kq.KhamNghiemTuThi = byte.Parse(data.Tables[0].Rows[0]["KhamNghiemTuThi"].ToString());
            //        if (DateTime.TryParse(data.Tables[0].Rows[0]["NgayKy"].ToString(), out d))
            //            kq.NgayKy = d; ;
            //        if (DateTime.TryParse(data.Tables[0].Rows[0]["NgayTruongKhoaKy"].ToString(), out d))
            //            kq.NgayTruongKhoaKy = d;
            //        kq.TinhTrangTuVong = data.Tables[0].Rows[0]["TinhTrangTuVong"].ToString();
            //        kq.Vvlan = byte.Parse(data.Tables[0].Rows[0]["Vvlan"].ToString());
            //    }
            //}
            ////kq.

            //return kq;
        }
        // show combo các loại bệnh
        [HttpGet("{id}/loai-benh-an")]
        public object GetLoaiBA(decimal id)
        {
            return _benhAnService.GetLoaiBA(id);
        }
        [HttpGet("thongtinbn/{maba}")]
        public object GetDateByMaBA(string maba)
        {
            var reqBa = _benhAnRepository.Table.Where(x => x.MaBa == maba).FirstOrDefault();
            if (reqBa != null)
            {
                return ThongTinBn.Table.Where(x => x.MaBn == reqBa.MaBn).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        // POST api/<BenhAnController>  
        [HttpPost]
        [SetActionContextItem(ActionType.Create)]
        [SessionMiddlewareFilter("HSBA/thongtinbenhan/create")]
        public ActionResult Post([FromBody] BenhAn benhAn)
        {
            if (ModelState.IsValid)
            {
                _benhAnService.Store(benhAn);
            }
            return Ok();
        }
        // PUT api/<BenhAnController>/5
        [HttpPut("{id}")]
        [SetActionContextItem(ActionType.Update)]
        [SessionMiddlewareFilter("HSBA/thongtinbenhan/modify")]
        public ActionResult Update(decimal id, [FromBody] BenhAn benhAn)
        {
            if (ModelState.IsValid)
            {
                _benhAnService.Update(id, benhAn);
            }
            return Ok();
        }
        [HttpPost("them-moi-thong-tin-benh-an")]
        [SetActionContextItem(ActionType.Create)]
        [SessionMiddlewareFilter("HSBA/thongtinbenhan/create")]
        public ActionResult ThongTinBenhAnCreate([FromBody] BenhAnDetailDto benhAn)
        {
            if (ModelState.IsValid)
            {
                _benhAnService.ThongTinBnCreate(benhAn);
            }
            return Ok();
        }
        // Show file pdf đã được ký lên cho mấy cu em lãnh đạo xem


        //[HttpPost("get-file-pdf")]
        //[SetActionContextItem(ActionType.Create)]
        //[SessionMiddlewareFilter("HSBA/thongtinbenhan/create")]
        //public ActionResult GetFilePDF()
        //{
        //    string directory = Path.Combine(Directory.GetCurrentDirectory(), "PDF");
        //    if (!Directory.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }
        //    string tenFile = "202304101416212396_signed";
        //    string urlfile = Path.Combine(directory, tenFile + ".pdf");
        //    return Ok(new { FilePath = urlfile });
        //}
        [HttpPost("{idba}/cap-nhap-thong-tin-benh-an")]
        [SessionMiddlewareFilter("HSBA/thongtinbenhan/modify")]
        [SetActionContextItem(ActionType.Update)]
        public ActionResult ThongTinBenhAnUpdate([FromBody] BenhAnDetailDto benhAn, decimal idba)
        {
            if (ModelState.IsValid)
            {
                _benhAnService.ThongTinBnUpdate(benhAn, idba);
            }
            return Ok();
        }
        [HttpPost("{idba}/dong-thong-tin-benh-an")]
        [SessionMiddlewareFilter("HSBA/thongtinbenhan/close")]
        [SetActionContextItem(ActionType.Update)]
        public ActionResult DongBenhAn([FromBody] BenhAnDetailDto benhAn, decimal idba)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Dữ liệu không hợp lệ",
                        status = 400
                    });
                }
                var user = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserSession>(HttpContext.Session, "UserProfileSessionData");
                if (user.Pub_bApplicationRole != "15")
                {
                    int checkFile = CheckFileChuaKy(idba);
                    if (checkFile > 0)
                    {
                        return Ok(new { message = "Đóng bệnh án thất bại! Vui lòng ký hết để đóng được bệnh án!", status = 500 });
                    }
                }
                _benhAnService.ThongTinBnUpdate(benhAn, idba, true);

                return Ok(new
                {
                    message = "Đóng bệnh án thành công!",
                    status = 200
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private int CheckFileChuaKy(decimal IDBA)
        {
            string tenStore = "spDMTrangThaiKy_GetChuaKy";
            string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            var benhan = _benhAnService.Detail(IDBA);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(tenStore, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@maba", benhan.MaBa);
                    command.Parameters.AddWithValue("@mabs", "");
                    command.Parameters.AddWithValue("@dk", "");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count > 1)
                        {
                            return dataSet.Tables[1].Rows.Count;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }
        [HttpPost("{idba}/cap-nhap-to-benh-an")]
        [SetActionContextItem(ActionType.Update)]
        [SessionMiddlewareFilter("HSBA/tobenhan/modify")]
        public ActionResult CapNhapToBenhAn(ToBenhAnVM info, decimal idba)
        {

            if (ModelState.IsValid)
            {
                _benhAnService.ThongTinToBenhAnCreateUpdate(info, idba);
            }
            return Ok();
        }
        // DELETE api/<BenhAnController>/5
        [HttpDelete("{id}")]
        [SetActionContextItem(ActionType.Delete)]
        public ActionResult Delete(decimal id)
        {
            _benhAnService.Destroy(id);
            return Ok();
        }
        [HttpGet("{id}/print-ba-file/{maba}.pdf")]
        [SessionMiddlewareFilter("HSBA/thongtinbenhan/export", "HSBA/tobenhan/export")]
        public ActionResult Print(decimal id, [FromQuery] PrintParameters parameters)
        {
            var path = _benhAnService.Print(id);

            if (parameters.ShouldReturnPath)
                return new JsonResult(new { path });
            DownloadFileResult downloadFileResult = uploadFileRespository.Download(path, true, true);
            return File(downloadFileResult.FileBytes, downloadFileResult.contentType);
        }
        public DataSet GetInforBenhAn(decimal Idba, string stt)
        {
            DataSet dr = new DataSet();
            string tenStore = "";
            switch (stt)
            {
                case "1":
                    tenStore = "sp_GetBenhAnFile1";
                    break;
                case "2":
                    tenStore = "sp_GetBenhAnFile2";
                    break;
                case "3":
                    tenStore = "sp_GetBenhAnFile3";
                    break;
                default:
                    break;
            }
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
        private string GetPathReport(string loai)
        {
            string tenStore = "sp_GetPathReport";
            string connectionString = ConfigurationManager.ConnectionStrings["Medyx_EMR_BCA_SQLConnectionString"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                using (SqlCommand Command = new SqlCommand(tenStore, Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Loai", loai);
                    string pdfFilePath = Command.ExecuteScalar() as string;
                    if (!string.IsNullOrEmpty(pdfFilePath))
                    {
                        return pdfFilePath;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }
        public FileStreamResult PrintPDF(string reportpath, string filename, DataSet dt)
        {
            try
            {
                //string webRootPath = AppDomain.CurrentDomain;
                //var webRootPath = Directory.GetCurrentDirectory()+"\\ReportHSBA";
                var webRootPath = ConfigurationManager.AppSettings["ReportDirectory"].Replace("\\\\", "\\");
                //WriteLog("bat dau");
                //string webRootPath = _config.GetValue<string>("ReportDirectory");
                WriteLog("webRootPath: " + webRootPath);
                ReportDocument rpt = new ReportDocument();
                string reportfilename = webRootPath + reportpath;
                WriteLog("reportfilename: " + reportfilename);
                rpt.Load(reportfilename);//Crystal Report Path
                rpt.SetDataSource(dt);//Get data source. All the data can be read in SQL.
                //string path = webRootPath + "\\temp\\" + DateTime.Now.AddDays(-1).Month.ToString() + DateTime.Now.AddDays(-1).Day.ToString();
                string path = webRootPath.Replace("\\ReportHSBA", "") + "\\";
                //WriteLog("path: " + path);
                string fileName = path + filename;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                CrystalDecisions.Shared.ExportRequestContext reqContext = new CrystalDecisions.Shared.ExportRequestContext();
                var exportOptions2 = new CrystalDecisions.Shared.ExportOptions
                {
                    ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                    FormatOptions = new CrystalDecisions.Shared.PdfFormatOptions { UsePageRange = false, FirstPageNumber = 1, LastPageNumber = 1 }
                };
                reqContext.ExportInfo = exportOptions2;
                var stream2 = rpt.FormatEngine.ExportToStream(reqContext);
                WriteLog("stream2 Error: " + stream2 + reqContext);
                stream2.Seek(0, SeekOrigin.Begin);
                var combinedPdf = new PdfSharp.Pdf.PdfDocument();
                int i = 0;
                foreach (PdfSharp.Pdf.PdfPage page in PdfSharp.Pdf.IO.PdfReader.Open(stream2, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import).Pages)
                {
                    combinedPdf.AddPage(page);
                    i++;

                }
                combinedPdf.Save(fileName);
                rpt.Close();
                rpt.Dispose();

                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return File(fs, "application/pdf");
                //return fileName;
            }
            catch (Exception ex)
            {
                WriteLog("PrintPDF Error: " + ex.ToString());
                return null;
            }
        }
        //API in tờ thứ 3
        [HttpGet("{id}/print-ba-file3/{maba}.pdf")]
        public ActionResult Print3(decimal id, [FromQuery] PrintParameters parameters)
        {
            string mess = "";
            try
            {
                string stt = "3";
                string Loaigiayto = "3";
                DataSet dt = GetInforBenhAn(id, stt);
                ReportDocument rpt = new ReportDocument();
                string reportfile = dt.Tables[0].Rows[0]["ReportPath"].ToString();
                string time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string idba_stt = id + "_3";
                string fileName = $"Storage/Print/{time}-{idba_stt}.pdf";

                return PrintPDF(reportfile, fileName, dt);


            }
            catch (Exception ex)
            {
                WriteLog("\nError processing PDF file: " + ex.Message + "- mess:" + mess);
                throw ex;
            }
        }
        //API in tờ bệnh án thứ hai
        [HttpGet("{id}/print-ba-file2/{maba}.pdf")]
        public ActionResult Print2(decimal id, [FromQuery] PrintParameters parameters)
        {
            string mess = "";
            try
            {
                string stt = "2";
                string Loaigiayto = "2";

                DataSet dt = GetInforBenhAn(id, stt);
                ReportDocument rpt = new ReportDocument();
                string reportfile = dt.Tables[0].Rows[0]["ReportPath"].ToString();
                string time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string idba_stt = id + "_2";
                string fileName = $"Storage/Print/{time}-{idba_stt}.pdf";

                return PrintPDF(reportfile, fileName, dt);


            }
            catch (Exception ex)
            {
                WriteLog("\nError processing PDF file: " + ex.Message + "- mess:" + mess);
                throw ex;
            }
        }
        [HttpGet("{id}/print-ba-file1/{maba}_1.pdf")]
        //[SessionMiddlewareFilter("HSBA/thongtinbenhan/export", "HSBA/tobenhan/export")]
        public ActionResult Print1(decimal id, [FromQuery] PrintParameters parameters)
        {
            string mess = "";
            try
            {
                string stt = "1";
                string Loaigiayto = "1";

                DataSet dt = GetInforBenhAn(id, stt);
                //ReportDocument rpt = new ReportDocument();

                //string reportfile = GetPathReport("ToBenhAn1");
                string reportfile = dt.Tables[0].Rows[0]["ReportPath"].ToString();
                string time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string idba_stt = id + "_1";
                string fileName = $"Storage/Print/{time}-{idba_stt}.pdf";

                return PrintPDF(reportfile, fileName, dt);
            }
            catch (Exception ex)
            {
                WriteLog("\nError processing PDF file: " + ex.Message + "- mess:" + mess);
                throw ex;
            }
        }
        //[HttpGet("PrintView1/{id}")]
        //public ActionResult PrintView1(decimal id, [FromQuery] PrintParameters parameters)
        //{
        //    try
        //    {
        //        var path = _benhAnService.Print1(id);

        //        if (!System.IO.File.Exists(path))
        //        {
        //            return NotFound(new { message = "File not found", path });
        //        }
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(path);
        //        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        //        string contentType = MimeMapping.GetMimeMapping(path);
        //        return File(fs, contentType, $"{id}_report.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog("\nError processing PDF file: " + ex.Message);
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpGet("xuat-file/{id}")]
        public IActionResult ExportPDF(decimal id)
        {
            var benhAn = _benhAnRepository.GetById(id);
            var dmHoSoBenhAn = _dmHSBARepository.GetAll().OrderBy(x => x.Id);
            var baseFolder = $"Storage\\HSBA\\{benhAn.MaBa}";
            PdfDocument outputDocument = new PdfDocument();
            foreach (var hoSoBenhAn in dmHoSoBenhAn)
            {
                string destinationFolder = Path.Combine(baseFolder, hoSoBenhAn.MaHS);

                if (Directory.Exists(destinationFolder))
                {
                    string[] pdfFiles = Directory.GetFiles(destinationFolder, "*.pdf");

                    Array.Sort(pdfFiles);

                    foreach (string pdfFile in pdfFiles)
                    {
                        try
                        {
                            byte[] pdfBytes = ConvertPdfToByteArray(pdfFile);
                            using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
                            {
                                PdfDocument inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
                                foreach (PdfPage page in inputDocument.Pages)
                                {
                                    outputDocument.AddPage(page);
                                }
                            }
                        }
                        catch (PdfSharp.Pdf.IO.PdfReaderException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }

            string outputPath = "HSBA_PDF_" + benhAn.Idba + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
            var strUrl = baseFolder + "\\" + outputPath;
            outputDocument.Save(strUrl);
            outputDocument.Dispose();
            DownloadFileResult downloadFileResult = uploadFileRespository.Download(strUrl, true, true);
            return File(downloadFileResult.FileBytes, downloadFileResult.contentType);
        }
        private byte[] ConvertPdfToByteArray(string filePath)
        {
            byte[] fileBytes;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }

            return fileBytes;
        }
        //[HttpGet("xuat-file-Da-Ky/{id}")]
        #region
        //public async Task<IActionResult> ExportPDFDaKy(decimal id)
        //{
        //    try
        //    {
        //        await DownloadFileZIP(id); // Gọi hàm chuẩn bị file
        //        var benhAn = _benhAnRepository.GetById(id);
        //        if (benhAn == null)
        //            return NotFound("Bệnh án không tồn tại.");
        //        var dmHoSoBenhAn = _dmHSBARepository.GetAll().OrderBy(x => x.Id).ToList();
        //        string baseFolder = Path.Combine("Storage", "DownloadFolder");
        //        string maBA = benhAn.MaBa;
        //        string benhAnFolder = Path.Combine(baseFolder, maBA);
        //        var outputDocument = new PdfDocument();
        //        foreach (var hoSo in dmHoSoBenhAn)
        //        {
        //            var maLoai = _dmbaLoaiTaiLieuRepository._context.DmbaLoaiTaiLieu
        //                                    .Where(x => x.MaNhomTaiLieu == (long)hoSo.Id)
        //                                    .Select(x => x.MaNhomTaiLieu)
        //                                    .FirstOrDefault();
        //            if (maLoai == 0) continue;
        //            string subFolder = Path.Combine(benhAnFolder, maLoai.ToString());
        //            if (!Directory.Exists(subFolder)) continue;
        //            var pdfFiles = Directory.GetFiles(subFolder, "*.pdf").OrderBy(f => f).ToList();
        //            //foreach (var pdfFile in pdfFiles)
        //            //{
        //            //    try
        //            //    {
        //            //        var pdfStream = System.IO.File.OpenRead(pdfFile);
        //            //        var inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
        //            //        foreach (var page in inputDocument.Pages)
        //            //            outputDocument.AddPage(page);
        //            //    }
        //            //    catch (Exception ex)
        //            //    {
        //            //        WriteLog($"Lỗi xử lý file PDF {pdfFile}: {ex.Message}");
        //            //    }
        //            //}
        //            foreach (var pdfFile in pdfFiles)
        //            {
        //                try
        //                {
        //                    // Sử dụng using để tự động đóng và giải phóng file kể cả khi có lỗi xảy ra
        //                    using (var pdfStream = System.IO.File.OpenRead(pdfFile))
        //                    using (var inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
        //                    {
        //                        foreach (var page in inputDocument.Pages)
        //                        {
        //                            outputDocument.AddPage(page);
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Khi lỗi xảy ra, nó sẽ nhảy vào đây, ghi log và tiếp tục vòng lặp tới file tiếp theo
        //                    WriteLog($"Lỗi xử lý file PDF {pdfFile}: {ex.Message}");

        //                    // Bạn có thể thêm lệnh continue; ở đây cho rõ ràng, dù không có nó thì vòng lặp vẫn tiếp tục
        //                    continue;
        //                }
        //            }
        //        }
        //        // ================= LẤY LINK PACS/LIS =================
        //        var query = _BenhAnClsKq.Table.AsQueryable();
        //        var listLinkPacs = query
        //                            .Where(x => x.MaBa == maBA)
        //                            .Select(x => x.LinkPacsLis)
        //                            .ToList();
        //        // ================= ADD TRANG LINK =================
        //        if (listLinkPacs != null && listLinkPacs.Any(x => !string.IsNullOrWhiteSpace(x)))
        //        {
        //            PdfPage page = outputDocument.AddPage();
        //            XGraphics gfx = XGraphics.FromPdfPage(page);
        //            XFont titleFont = new XFont("Arial", 14, XFontStyle.Bold);
        //            XFont linkFont = new XFont("Arial", 12, XFontStyle.Regular);
        //            double yPoint = 40;
        //            gfx.DrawString("DANH SÁCH LINK PACS/LIS",
        //                titleFont,
        //                XBrushes.Black,
        //                new XRect(40, yPoint, page.Width - 80, page.Height),
        //                XStringFormats.TopLeft);
        //            yPoint += 30;
        //            int index = 1;
        //            foreach (var link in listLinkPacs.Where(x => !string.IsNullOrWhiteSpace(x)))
        //            {
        //                string text = $"{index}. {link}";
        //                gfx.DrawString(text,
        //                    linkFont,
        //                    XBrushes.Blue,
        //                    new XRect(40, yPoint, page.Width - 80, page.Height),
        //                    XStringFormats.TopLeft);
        //                XRect xRect = new XRect(40, yPoint, page.Width - 80, 15);
        //                PdfRectangle pdfRect = new PdfRectangle(xRect);
        //                PdfLinkAnnotation linkAnnotation =
        //                    PdfLinkAnnotation.CreateWebLink(pdfRect, link);
        //                page.Annotations.Add(linkAnnotation);
        //                yPoint += 20;
        //                index++;
        //            }
        //        }
        //        // ================= SAVE FILE =================
        //        if (outputDocument.PageCount == 0)
        //        {
        //            outputDocument.AddPage();
        //        }
        //        string outputFileName = $"HSBA_PDF_{maBA}_{DateTime.Now:ddMMyyyyHHmmssfff}.pdf";
        //        string outputPath = Path.Combine(baseFolder, outputFileName);
        //        outputDocument.Save(outputPath);
        //        outputDocument.Close();
        //        var downloadResult = uploadFileRespository.Download(outputPath, deleteAfterDownload: true, true);
        //        return File(downloadResult.FileBytes, "application/pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog($"Lỗi tổng quát trong ExportPDFDaKy: {ex.Message}");
        //        return StatusCode(500, "Lỗi hệ thống. Vui lòng kiểm tra log.");
        //    }
        //}
        //public async Task<IActionResult> ExportPDFDaKy(decimal id)
        //{
        //    try
        //    {
        //        //await DownloadFileZIP(id); // Gọi hàm chuẩn bị file
        //        var benhAn = _benhAnRepository.GetById(id);
        //        if (benhAn == null)
        //            return NotFound("Bệnh án không tồn tại.");
        //        var dmHoSoBenhAn = _dmHSBARepository.GetAll().OrderBy(x => x.Id).ToList();
        //        string baseFolder = Path.Combine("Storage", "DownloadFolder");
        //        string maBA = benhAn.MaBa;
        //        string benhAnFolder = Path.Combine(baseFolder, maBA);
        //        var outputDocument = new PdfDocument();
        //        //lay danh sach file từ store spGetDMTrangThaiKyByMaBA
        //        string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString +
        //                                  "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
        //        using (var connection = new SqlConnection(connectionString))
        //        using (var command = new SqlCommand("spGetDMTrangThaiKyByMaBA", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@MaBA_NoiTru", maBA);
        //            connection.Open();
        //            using (var reader = command.ExecuteReader())
        //            {
        //                string linkApiBase = "";
        //                bool isFirstRow = true;

        //                while (reader.Read())
        //                {
        //                    // Lấy link API từ cột gộp bảng 2 ở dòng đầu tiên
        //                    if (isFirstRow)
        //                    {
        //                        linkApiBase = reader["LinkEMR"]?.ToString() ?? "";
        //                        isFirstRow = false;
        //                    }
        //                    string pdfFile = reader["DuongDanLink"]?.ToString();
        //                    if (string.IsNullOrEmpty(pdfFile)) continue;
        //                    try
        //                    {
        //                        if (System.IO.File.Exists(pdfFile))
        //                        {
        //                            // --- TRƯỜNG HỢP 1: XỬ LÝ FILE CỤC BỘ ---
        //                            try
        //                            {
        //                                using (var pdfStream = new FileStream(pdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                                using (var inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
        //                                {
        //                                    foreach (PdfPage page in inputDocument.Pages)
        //                                    {
        //                                        outputDocument.AddPage(page);
        //                                    }
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                WriteLog($"File cục bộ lỗi (Bỏ qua): {pdfFile}. Chi tiết: {ex.Message}");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            // --- TRƯỜNG HỢP 2: GỌI API NẾU FILE CỤC BỘ KHÔNG CÓ ---
        //                            if (!string.IsNullOrEmpty(linkApiBase))
        //                            {
        //                                try
        //                                {
        //                                    string url = linkApiBase + System.Web.HttpUtility.UrlEncode(pdfFile);
        //                                    using (HttpClient client = new HttpClient())
        //                                    {
        //                                        // Giới hạn thời gian chờ để tránh treo tiến trình
        //                                        client.Timeout = TimeSpan.FromSeconds(30);
        //                                        HttpResponseMessage response = await client.GetAsync(url);

        //                                        if (response.IsSuccessStatusCode)
        //                                        {
        //                                            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

        //                                            // Kiểm tra xem có phải file PDF hợp lệ không (ít nhất phải > 100 byte)
        //                                            if (fileBytes != null && fileBytes.Length > 100)
        //                                            {
        //                                                using (MemoryStream ms = new MemoryStream(fileBytes))
        //                                                using (var inputDocument = PdfReader.Open(ms, PdfDocumentOpenMode.Import))
        //                                                {
        //                                                    foreach (PdfPage page in inputDocument.Pages)
        //                                                    {
        //                                                        outputDocument.AddPage(page);
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                WriteLog($"API trả về file trống hoặc lỗi (Bỏ qua): {pdfFile}");
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    WriteLog($"Lỗi khi gọi API hoặc giải mã PDF từ API (Bỏ qua): {pdfFile}. Chi tiết: {ex.Message}");
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        // Catch tổng cho mỗi vòng lặp để đảm bảo luôn sang file tiếp theo
        //                        WriteLog($"Lỗi không xác định khi xử lý bản ghi {pdfFile}: {ex.Message}");
        //                    }
        //                }
        //            }
        //        }
        //        // ================= LẤY LINK PACS/LIS =================
        //        var query = _BenhAnClsKq.Table.AsQueryable();
        //        var listLinkPacs = query
        //                            .Where(x => x.MaBa == maBA)
        //                            .Select(x => x.LinkPacsLis)
        //                            .ToList();
        //        // ================= ADD TRANG LINK =================
        //        if (listLinkPacs != null && listLinkPacs.Any(x => !string.IsNullOrWhiteSpace(x)))
        //        {
        //            PdfPage page = outputDocument.AddPage();
        //            XGraphics gfx = XGraphics.FromPdfPage(page);
        //            XFont titleFont = new XFont("Arial", 14, XFontStyle.Bold);
        //            XFont linkFont = new XFont("Arial", 12, XFontStyle.Regular);
        //            double yPoint = 40;
        //            gfx.DrawString("DANH SÁCH LINK PACS/LIS",
        //                titleFont,
        //                XBrushes.Black,
        //                new XRect(40, yPoint, page.Width - 80, page.Height),
        //                XStringFormats.TopLeft);
        //            yPoint += 30;
        //            int index = 1;
        //            foreach (var link in listLinkPacs.Where(x => !string.IsNullOrWhiteSpace(x)))
        //            {
        //                string text = $"{index}. {link}";
        //                gfx.DrawString(text,
        //                    linkFont,
        //                    XBrushes.Blue,
        //                    new XRect(40, yPoint, page.Width - 80, page.Height),
        //                    XStringFormats.TopLeft);
        //                XRect xRect = new XRect(40, yPoint, page.Width - 80, 15);
        //                PdfRectangle pdfRect = new PdfRectangle(xRect);
        //                PdfLinkAnnotation linkAnnotation =
        //                    PdfLinkAnnotation.CreateWebLink(pdfRect, link);
        //                page.Annotations.Add(linkAnnotation);
        //                yPoint += 20;
        //                index++;
        //            }
        //        }
        //        // ================= SAVE FILE =================
        //        if (outputDocument.PageCount == 0)
        //        {
        //            outputDocument.AddPage();
        //        }
        //        string outputFileName = $"HSBA_PDF_{maBA}_{DateTime.Now:ddMMyyyyHHmmssfff}.pdf";
        //        string outputPath = Path.Combine(baseFolder, outputFileName);
        //        outputDocument.Save(outputPath);
        //        outputDocument.Close();
        //        var downloadResult = uploadFileRespository.Download(outputPath, deleteAfterDownload: true, true);
        //        return File(downloadResult.FileBytes, "application/pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog($"Lỗi tổng quát trong ExportPDFDaKy: {ex.Message}");
        //        return StatusCode(500, "Lỗi hệ thống. Vui lòng kiểm tra log.");
        //    }
        //}

        //[HttpGet("xuat-file-Da-Ky/{id}")]
        //public async Task<IActionResult> ExportPDFDaKy(decimal id)
        //{
        //    try
        //    {
        //        // 1. Chạy hàm chuẩn bị: Tải file từ SQL/API và lưu vào Storage/TempPDF/{maBA}
        //        await DownloadFileZIP1(id);

        //        var benhAn = _benhAnRepository.GetById(id);
        //        if (benhAn == null)
        //            return NotFound("Bệnh án không tồn tại.");

        //        string maBA = benhAn.MaBa;
        //        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        //        // Cấu hình đường dẫn khớp với hàm DownloadFileZIP mới
        //        string tempFolder = Path.Combine(baseDir, "Storage", "TempPDF", maBA);
        //        string downloadFolder = Path.Combine(baseDir, "Storage", "DownloadFolder");
        //        if (!Directory.Exists(downloadFolder)) Directory.CreateDirectory(downloadFolder);

        //        if (!Directory.Exists(tempFolder))
        //            return BadRequest("Thư mục tạm không tồn tại hoặc chuẩn bị file thất bại.");

        //        var outputDocument = new PdfDocument();

        //        // 2. DUYỆT VÀ GHÉP CÁC FILE PDF ĐÃ TẢI SẴN (Trong cùng 1 folder, không chia subfolder)
        //        // OrderBy theo tên file để đảm bảo đúng thứ tự đã đánh số (001, 002...)
        //        var files = Directory.GetFiles(tempFolder, "*.pdf")
        //                             .OrderBy(f => Path.GetFileName(f)).ToList();

        //        foreach (var file in files)
        //        {
        //            try
        //            {
        //                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                using (var inputDoc = PdfReader.Open(fs, PdfDocumentOpenMode.Import))
        //                {
        //                    foreach (PdfPage page in inputDoc.Pages)
        //                    {
        //                        outputDocument.AddPage(page);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                WriteLog($"Lỗi ghép file lẻ {Path.GetFileName(file)}: {ex.Message}");
        //            }
        //        }

        //        // 3. ================= LẤY LINK PACS/LIS =================
        //        var query = _BenhAnClsKq.Table.AsQueryable();
        //        var listLinkPacs = query
        //                            .Where(x => x.MaBa == maBA)
        //                            .Select(x => x.LinkPacsLis)
        //                            .ToList();

        //        // 4. ================= ADD TRANG LINK (VỚI LOGIC NGẮT TRANG) =================
        //        if (listLinkPacs != null && listLinkPacs.Any(x => !string.IsNullOrWhiteSpace(x)))
        //        {
        //            PdfPage page = outputDocument.AddPage();
        //            XGraphics gfx = XGraphics.FromPdfPage(page);
        //            XFont titleFont = new XFont("Arial", 14, XFontStyle.Bold);
        //            XFont linkFont = new XFont("Arial", 11, XFontStyle.Regular);
        //            double yPoint = 40;

        //            gfx.DrawString("DANH SÁCH LINK PACS/LIS", titleFont, XBrushes.Black,
        //                new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
        //            yPoint += 30;

        //            int idxCount = 1;
        //            foreach (var link in listLinkPacs.Where(x => !string.IsNullOrWhiteSpace(x)))
        //            {
        //                // Tự động thêm trang mới nếu danh sách link dài quá khổ giấy
        //                if (yPoint > page.Height - 50)
        //                {
        //                    page = outputDocument.AddPage();
        //                    gfx = XGraphics.FromPdfPage(page);
        //                    yPoint = 40;
        //                }

        //                string text = $"{idxCount}. {link}";
        //                gfx.DrawString(text, linkFont, XBrushes.Blue,
        //                    new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);

        //                // Tạo liên kết xanh click được
        //                XRect xRect = new XRect(40, yPoint, page.Width - 80, 15);
        //                page.Annotations.Add(PdfLinkAnnotation.CreateWebLink(new PdfRectangle(xRect), link));

        //                yPoint += 20;
        //                idxCount++;
        //            }
        //        }

        //        if (outputDocument.PageCount == 0) outputDocument.AddPage();

        //        // 5. ================= LƯU FILE TỔNG =================
        //        string outputFileName = $"HSBA_Full_{maBA}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
        //        string outputPath = Path.Combine(downloadFolder, outputFileName);

        //        outputDocument.Save(outputPath);
        //        outputDocument.Close();
        //        outputDocument.Dispose();

        //        // 6. ================= XÓA FOLDER TẠM (Rác từ DownloadFileZIP) =================
        //        try
        //        {
        //            if (Directory.Exists(tempFolder))
        //            {
        //                Directory.Delete(tempFolder, true);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLog($"Không thể xóa thư mục tạm {tempFolder}: {ex.Message}");
        //        }

        //        // 7. ================= TRẢ VỀ TRÌNH DUYỆT QUA REPO CŨ =================
        //        var downloadResult = uploadFileRespository.Download(outputPath, true, true);

        //        // Thêm Header Inline để trình duyệt ưu tiên mở trực tiếp
        //        Response.Headers.Add("Content-Disposition", "inline; filename=" + outputFileName);
        //        return File(downloadResult.FileBytes, "application/pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog($"Lỗi tổng quát tại ExportPDFDaKy: {ex.Message}");
        //        return StatusCode(500, "Lỗi hệ thống. Vui lòng kiểm tra log.");
        //    }
        //}

        //[HttpGet("xuat-file-Da-Ky/{id}")]
        //public IActionResult ExportPDFDaKy(decimal id)
        //{
        //    var benhAn = _benhAnRepository.GetById(id);
        //    if (benhAn == null) return NotFound();
        //    string maBA = benhAn.MaBa;
        //    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        //    string downloadFolder = Path.Combine(baseDir, "Storage", "DownloadFolder");
        //    if (!Directory.Exists(downloadFolder)) Directory.CreateDirectory(downloadFolder);
        //    string outputFileName = string.Format("HSBA_Final_{0}.pdf", id);
        //    string outputPath = Path.Combine(downloadFolder, outputFileName);
        //    string lockFile = outputPath + ".lock";
        //    // TRƯỜNG HỢP 1: XỬ LÝ XONG -> TRẢ FILE
        //    if (!System.IO.File.Exists(lockFile) && System.IO.File.Exists(outputPath))
        //    {
        //        try
        //        {
        //            byte[] fileBytes;
        //            using (var fs = new FileStream(outputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //            {
        //                fileBytes = new byte[fs.Length];
        //                fs.Read(fileBytes, 0, (int)fs.Length);
        //            }
        //            // Xóa file sau khi nạp vào RAM thành công để sạch server
        //            if (System.IO.File.Exists(outputPath)) System.IO.File.Delete(outputPath);
        //            Response.Headers.Add("Content-Disposition", "inline; filename=" + outputFileName);
        //            return File(fileBytes, "application/pdf");
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLog("Lỗi đọc file: " + ex.Message);
        //            // Nếu bị lock thì rơi xuống dưới để tiếp tục hiện trang chờ
        //        }
        //    }
        //    // TRƯỜNG HỢP 2: ĐANG XỬ LÝ -> TRANG CHỜ (Đã sửa Font và Gộp folder phẳng)
        //    if (System.IO.File.Exists(lockFile))
        //    {
        //        string html = "<html><head><meta charset='utf-8'><meta http-equiv='refresh' content='5'></head>" +
        //                      "<body style='font-family:Arial; text-align:center; padding-top:100px; background:#f4f4f9;'>" +
        //                      "<div style='background:white; display:inline-block; padding:30px; border-radius:10px; box-shadow:0 2px 10px rgba(0,0,0,0.1);'>" +
        //                      "<h2 style='color:#2c3e50;'>Đang xử lý hồ sơ bệnh án lớn</h2>" +
        //                      "<p style='color:#7f8c8d;'>Hệ thống đang ghép các tệp tin PDF, vui lòng đợi trong giây lát...</p>" +
        //                      "<div style='margin:20px auto; width:40px; height:40px; border:5px solid #f3f3f3; border-top:5px solid #3498db; border-radius:50%; animation:spin 1s linear infinite;'></div>" +
        //                      "<p style='font-size:12px; color:#bdc3c7;'>Trình duyệt sẽ tự động tải file khi hoàn tất.</p>" +
        //                      "</div>" +
        //                      "<style>@keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }</style>" +
        //                      "</body></html>";
        //        return Content(html, "text/html");
        //    }
        //    // TRƯỜNG HỢP 3: LẦN ĐẦU CHẠY -> KÍCH HOẠT CHẠY NGẦM
        //    System.IO.File.WriteAllText(lockFile, DateTime.Now.ToString());
        //    Task.Run(async () =>
        //    {
        //        try
        //        {
        //            // Tải file vào 1 folder duy nhất (DownloadFileZIP1 đã sửa ở câu trước để nạp List<string>)
        //            List<string> downloadedFiles = await DownloadFileZIP1(id);
        //            using (var outputDocument = new PdfDocument())
        //            {
        //                foreach (var pdfFile in downloadedFiles)
        //                {
        //                    try
        //                    {
        //                        if (!System.IO.File.Exists(pdfFile)) continue;
        //                        using (var pdfStream = System.IO.File.OpenRead(pdfFile))
        //                        using (var inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
        //                        {
        //                            foreach (PdfPage page in inputDocument.Pages)
        //                                outputDocument.AddPage(page);
        //                        }
        //                    }
        //                    catch (Exception ex) { WriteLog("Bỏ qua file PDF lỗi: " + pdfFile); }
        //                }
        //                // --- ADD TRANG LINK PACS/LIS ---
        //                var listLinkPacs = _BenhAnClsKq.Table.AsQueryable().Where(x => x.MaBa == maBA).Select(x => x.LinkPacsLis).ToList();
        //                if (listLinkPacs != null && listLinkPacs.Any(x => !string.IsNullOrWhiteSpace(x)))
        //                {
        //                    PdfPage page = outputDocument.AddPage();
        //                    XGraphics gfx = XGraphics.FromPdfPage(page);
        //                    XFont titleFont = new XFont("Arial", 14, XFontStyle.Bold);
        //                    XFont linkFont = new XFont("Arial", 11, XFontStyle.Regular);
        //                    double yPoint = 40;
        //                    gfx.DrawString("DANH SÁCH LINK PACS/LIS", titleFont, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
        //                    yPoint += 30;
        //                    int idx = 1;
        //                    foreach (var link in listLinkPacs.Where(l => !string.IsNullOrWhiteSpace(l)))
        //                    {
        //                        if (yPoint > page.Height - 50) { page = outputDocument.AddPage(); gfx = XGraphics.FromPdfPage(page); yPoint = 40; }
        //                        gfx.DrawString(idx + ". " + link, linkFont, XBrushes.Blue, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
        //                        page.Annotations.Add(PdfLinkAnnotation.CreateWebLink(new PdfRectangle(new XRect(40, yPoint, page.Width - 80, 15)), link));
        //                        yPoint += 20; idx++;
        //                    }
        //                }
        //                if (outputDocument.PageCount == 0) outputDocument.AddPage();
        //                outputDocument.Save(outputPath);
        //                outputDocument.Close();
        //                outputDocument.Dispose();
        //            }
        //            // Xóa folder TempPDF lẻ
        //            string tempFolder = Path.Combine(baseDir, "Storage", "TempPDF", maBA);
        //            if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
        //        }
        //        catch (Exception ex) { WriteLog("Lỗi chạy ngầm: " + ex.Message); }
        //        finally
        //        {
        //            if (System.IO.File.Exists(lockFile)) System.IO.File.Delete(lockFile);
        //        }
        //    });

        //    // Quay lại để rơi vào Trường hợp 2 (Trang chờ)
        //    return RedirectToAction("ExportPDFDaKy", new { id = id });
        //}
        #endregion
        //[HttpGet("xuat-file-Da-Ky/{id}")]
        //public async Task<ActionResult> ExportPDFDaKy(decimal id)
        //{
        //    // Sử dụng HttpRuntime.Cache có sẵn trong System.Web để lưu dữ liệu trên RAM
        //    string cacheKey = "PDF_FINAL_DATA_" + id;
        //    string lockKey = "PDF_PROCESS_LOCK_" + id;

        //    // TRƯỜNG HỢP 1: ĐÃ XỬ LÝ XONG -> TRẢ FILE ĐỂ HIỂN THỊ
        //    var cachedFile = HttpRuntime.Cache.Get(cacheKey) as byte[];
        //    if (cachedFile != null)
        //    {
        //        HttpRuntime.Cache.Remove(cacheKey); // Lấy xong xóa luôn để giải phóng RAM
        //        Response.Headers.Add("Content-Disposition", "inline; filename=HoSoBenhAn_Final.pdf");
        //        return File(cachedFile, "application/pdf");
        //    }

        //    // TRƯỜNG HỢP 2: ĐANG TRONG QUÁ TRÌNH XỬ LÝ -> HIỆN TRANG CHỜ
        //    if (HttpRuntime.Cache.Get(lockKey) != null)
        //    {
        //        string html = "<html><head><meta charset='utf-8'><meta http-equiv='refresh' content='5'></head>" +
        //                      "<body style='font-family:Arial; text-align:center; padding-top:100px; background:#f4f4f9;'>" +
        //                      "<div style='background:white; display:inline-block; padding:30px; border-radius:10px; box-shadow:0 2px 10px rgba(0,0,0,0.1);'>" +
        //                      "<h2 style='color:#2c3e50;'>Đang xử lý hồ sơ bệnh án lớn</h2>" +
        //                      "<p style='color:#7f8c8d;'>Hệ thống đang tải và ghép các tệp tin trực tiếp trên bộ nhớ, vui lòng đợi...</p>" +
        //                      "<div style='margin:20px auto; width:40px; height:40px; border:5px solid #f3f3f3; border-top:5px solid #3498db; border-radius:50%; animation:spin 1s linear infinite;'></div>" +
        //                      "<p style='font-size:12px; color:#bdc3c7;'>Trình duyệt sẽ tự động hiển thị PDF khi hoàn tất.</p>" +
        //                      "</div>" +
        //                      "<style>@keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }</style>" +
        //                      "</body></html>";
        //        return Content(html, "text/html");
        //    }

        //    // TRƯỜNG HỢP 3: LẦN ĐẦU CHẠY -> KÍCH HOẠT CHẠY NGẦM VÀ LƯU VÀO CACHE
        //    var benhAn = _benhAnRepository.GetById(id);
        //    if (benhAn == null) return NotFound();
        //    string maBA = benhAn.MaBa;

        //    // Đặt khóa Lock trong 10 phút để tránh chạy trùng lặp
        //    HttpRuntime.Cache.Insert(lockKey, "processing", null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
        //    // Chạy ngầm việc tải và gộp file
        //    Task.Run(async () =>
        //    {
        //        try
        //        {
        //            // 1. Lấy danh sách file và Tải vào RAM (byte[])
        //            var (fileList, linkApiBase) = await GetFileListFromStore(maBA);
        //            List<byte[]> pdfBytesList = await DownloadFilesToMemory(fileList, linkApiBase);

        //            // 2. Tiến hành gộp file bằng PdfSharp trên RAM
        //            using (MemoryStream outputStream = new MemoryStream())
        //            {
        //                using (PdfDocument outputDocument = new PdfDocument())
        //                {
        //                    foreach (var pdfByte in pdfBytesList)
        //                    {
        //                        try
        //                        {
        //                            using (MemoryStream msInput = new MemoryStream(pdfByte))
        //                            using (PdfDocument inputDocument = PdfReader.Open(msInput, PdfDocumentOpenMode.Import))
        //                            {
        //                                foreach (PdfPage page in inputDocument.Pages)
        //                                    outputDocument.AddPage(page);
        //                            }
        //                        }
        //                        catch { /* Bỏ qua file PDF lỗi định dạng */ }
        //                    }

        //                    // 3. Thêm trang Link PACS/LIS (viết hàm riêng ở dưới)
        //                    AddPacsPageToPdf(outputDocument, maBA);

        //                    if (outputDocument.PageCount == 0) outputDocument.AddPage();
        //                    outputDocument.Save(outputStream, false);
        //                }

        //                // 4. Lưu mảng byte cuối cùng vào Cache để trang chờ lấy ra
        //                byte[] finalData = outputStream.ToArray();
        //                HttpRuntime.Cache.Insert(cacheKey, finalData, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLog("Lỗi xử lý PDF ngầm: " + ex.Message);
        //        }
        //        finally
        //        {
        //            // Luôn xóa khóa lock khi kết thúc (dù thành công hay lỗi)
        //            HttpRuntime.Cache.Remove(lockKey);
        //        }
        //    });
        //    // Sau khi kích hoạt Task.Run, quay lại chính nó để rơi vào Case 2 (Trang chờ)
        //    return RedirectToAction("ExportPDFDaKy", new { id = id });
        //}
        [HttpGet("xuat-file-Da-Ky/{id}")]
        public async Task<ActionResult> ExportPDFDaKy(decimal id)
        {
            string cacheKey = "PDF_FINAL_DATA_" + id;
            string lockKey = "PDF_PROCESS_LOCK_" + id;
            // 1. TRẢ FILE PDF
            var cachedFile = HttpRuntime.Cache.Get(cacheKey) as byte[];
            if (cachedFile != null)
            {
                HttpRuntime.Cache.Remove(cacheKey);
                Response.Headers.Add("Content-Disposition", "inline; filename=HoSoBenhAn_Final.pdf");
                return File(cachedFile, "application/pdf");
            }
            // 2. TRANG CHỜ (Sửa lỗi CS1501 và mất định dạng)
            if (HttpRuntime.Cache.Get(lockKey) != null)
            {
                // Giả sử biến fileCount lấy từ giá trị Lock trong Cache như các bước trước
                int fileCount = 0;
                var lockValue = HttpRuntime.Cache.Get(lockKey);
                if (lockValue != null) int.TryParse(lockValue.ToString(), out fileCount);

                string html = $@"
<html>
<head>
    <meta charset='utf-8'>
    <meta http-equiv='refresh' content='5'>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; text-align: center; padding-top: 100px; background: #f0f2f5; color: #333; margin: 0; }}
        .card {{ background: white; display: inline-block; padding: 40px; border-radius: 15px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); max-width: 500px; }}
        .loader {{ margin: 20px auto; width: 60px; height: 60px; border: 6px solid #f3f3f3; border-top: 6px solid #007bff; border-radius: 50%; animation: spin 1s linear infinite; }}
        @keyframes spin {{ 0% {{ transform: rotate(0deg); }} 100% {{ transform: rotate(360deg); }} }}
        h2 {{ color: #1a73e8; margin-bottom: 10px; font-size: 24px; }}
        .info {{ color: #5f6368; font-size: 16px; line-height: 1.6; }}
        .highlight {{ color: #d93025; font-weight: bold; font-size: 18px; }}
        .countdown-box {{ margin-top: 20px; font-size: 14px; color: #9aa0a6; border-top: 1px solid #eee; padding-top: 20px; }}
        #timer {{ font-weight: bold; color: #007bff; }}
    </style>
</head>
<body>
    <div class='card'>
        <div class='loader'></div>
        <h2>Đang gộp hồ sơ bệnh án</h2>
        <div class='info'>
            Hệ thống đang xử lý <span class='highlight'>{(fileCount > 0 ? fileCount.ToString() : "danh sách")}</span> tệp tin ký số.<br>
            Vui lòng không đóng trình duyệt lúc này...
        </div>
        <div class='countdown-box'>
            Trình duyệt sẽ tự động làm mới sau <span id='timer'>5</span> giây
        </div>
    </div>
    <script>
        var timeLeft = 5;
        var downloadTimer = setInterval(function(){{
            if(timeLeft <= 0){{
                clearInterval(downloadTimer);
            }} else {{
                document.getElementById('timer').innerHTML = timeLeft;
            }}
            timeLeft -= 1;
        }}, 1000);
    </script>
</body>
</html>";

                // Trả về ContentResult truyền thống nhưng chỉ định rõ ContentType
                return new ContentResult { Content = html, ContentType = "text/html" };
            }

            // 3. LẤY DỮ LIỆU
            var benhAn = _benhAnRepository.GetById(id);
            if (benhAn == null) return NotFound();

            // Lấy danh sách file trước khi vào Task chạy ngầm
            var (fileList, linkApiBase) = await GetFileListFromStore(benhAn.MaBa);
            if (fileList == null || fileList.Count == 0) return Content("Không tìm thấy file tài liệu.");

            HttpRuntime.Cache.Insert(lockKey, fileList.Count, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);

            // 4. CHẠY NGẦM
            Task.Run(async () =>
            {
                try
                {
                    // Tải file có Semaphore và xử lý URL đặc biệt
                    var pdfBytesList = await DownloadFilesToMemory(fileList, linkApiBase);

                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        using (PdfDocument outputDocument = new PdfDocument())
                        {
                            foreach (var pdfByte in pdfBytesList)
                            {
                                if (pdfByte == null || pdfByte.Length < 100) continue;
                                try
                                {
                                    using (MemoryStream msInput = new MemoryStream(pdfByte))
                                    using (PdfDocument inputDocument = PdfReader.Open(msInput, PdfDocumentOpenMode.Import))
                                    {
                                        foreach (PdfPage page in inputDocument.Pages)
                                            outputDocument.AddPage(page);
                                    }
                                }
                                catch { continue; } // Bỏ qua file lỗi định dạng
                            }
                            try { AddPacsPageToPdf(outputDocument, benhAn.MaBa); } catch { }
                            if (outputDocument.PageCount == 0) outputDocument.AddPage();
                            outputDocument.Save(outputStream, false);
                        }
                        HttpRuntime.Cache.Insert(cacheKey, outputStream.ToArray(), null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                catch (Exception ex) { WriteLog("Lỗi gộp: " + ex.Message); }
                finally { HttpRuntime.Cache.Remove(lockKey); }
            });

            return RedirectToAction("ExportPDFDaKy", new { id = id });
        }


        // --- HÀM TẢI FILE SONG SONG CỦA BẠN (Đã sửa lỗi URL nối chuỗi) ---
        //private async System.Threading.Tasks.Task<System.Collections.Generic.List<byte[]>> DownloadFilesToMemory(System.Collections.Generic.List<string> filePaths, string apiBase)
        //{
        //    using (var client = new System.Net.Http.HttpClient())
        //    {
        //        client.Timeout = System.TimeSpan.FromMinutes(5);

        //        var downloadTasks = filePaths
        //            .Where(path => !string.IsNullOrEmpty(path))
        //            .Select(async path =>
        //            {
        //                try
        //                {
        //                    if (System.IO.File.Exists(path))
        //                    {
        //                        return await System.Threading.Tasks.Task.Run(() => System.IO.File.ReadAllBytes(path));
        //                    }
        //                    else if (!string.IsNullOrEmpty(apiBase))
        //                    {
        //                        // Sửa lỗi nối chuỗi URL: Đảm bảo không bị dư/thiếu dấu / và không Escape dấu / của đường dẫn
        //                        string baseUrl = (apiBase.EndsWith("/") || apiBase.EndsWith("=")) ? apiBase : apiBase + "/";
        //                        string relativePath = path.Replace("\\", "/").TrimStart('/');
        //                        string url = baseUrl + relativePath;

        //                        byte[] data = await client.GetByteArrayAsync(url);
        //                        return (data != null && data.Length > 100) ? data : null;
        //                    }
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    WriteLog($"Lỗi tải file {path}: {ex.Message}");
        //                }
        //                return null;
        //            });

        //        byte[][] results = await System.Threading.Tasks.Task.WhenAll(downloadTasks);
        //        return results.Where(data => data != null).Cast<byte[]>().ToList();
        //    }
        //}
        private async Task<List<byte[]>> DownloadFilesToMemory(List<string> filePaths, string apiBase)
        {
            // Semaphore giới hạn 10 luồng tải để tránh treo Server
            using (var semaphore = new System.Threading.SemaphoreSlim(10))
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(25); // Mỗi file chờ tối đa 25s

                var downloadTasks = filePaths
                    .Where(path => !string.IsNullOrEmpty(path))
                    .Select(async path =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            if (System.IO.File.Exists(path))
                            {
                                return await Task.Run(() => System.IO.File.ReadAllBytes(path));
                            }

                            string baseUrl = apiBase.EndsWith("/") || apiBase.EndsWith("=") ? apiBase : apiBase + "/";
                            string relativePath = path.Replace("\\", "/").TrimStart('/');

                            // XỬ LÝ KÝ TỰ ĐẶC BIỆT: Chỉ mã hóa tên file, giữ lại dấu gạch chéo /
                            var parts = relativePath.Split('/').Select(p => Uri.EscapeDataString(p));
                            string safePath = string.Join("/", parts);
                            string url = baseUrl + safePath;

                            return await client.GetByteArrayAsync(url);
                        }
                        catch { return null; } // Bỏ qua file nếu link lỗi (#, &, tên file quá dài...)
                        finally { semaphore.Release(); }
                    });

                var results = await Task.WhenAll(downloadTasks);
                return results.Where(data => data != null && data.Length > 100).ToList();
            }
        }


        private void SendRawJson(System.Web.HttpResponse response, string json, int statusCode = 200)
        {
            response.Clear();
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            response.Write(json);
            response.Flush();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        // HÀM TẠO TRANG PACS (ITEXTSHARP)
        private byte[] GeneratePacsPdfBytes(System.Collections.Generic.List<string> links)
        {
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4))
                    {
                        iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                        doc.Open();
                        var titleFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 14);
                        var linkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 11, iTextSharp.text.BaseColor.BLUE);

                        doc.Add(new iTextSharp.text.Paragraph("DANH SACH LINK PACS/LIS", titleFont));
                        foreach (var link in links.Where(l => !string.IsNullOrWhiteSpace(l)))
                        {
                            iTextSharp.text.Anchor anchor = new iTextSharp.text.Anchor(link, linkFont) { Reference = link };
                            doc.Add(new iTextSharp.text.Paragraph(anchor));
                        }
                        doc.Close();
                    }
                    return ms.ToArray();
                }
            }
            catch { return null; }
        }
        // HÀM TẢI FILE (HTTPCLIENT)
        private async System.Threading.Tasks.Task<byte[]> DownloadSingleFile(string fileName, string apiBase)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.Timeout = System.TimeSpan.FromSeconds(30);
                try
                {
                    string url = (apiBase.EndsWith("/") ? apiBase : apiBase + "/") + fileName.TrimStart('/');
                    return await client.GetByteArrayAsync(url);
                }
                catch { return null; }
            }
        }
        // Hàm render an toàn tránh lỗi JSON
        private System.Web.Mvc.ActionResult RenderWaitingHtml()
        {
            var response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "text/html";
            response.Write("<html><head><meta charset='utf-8'><meta http-equiv='refresh' content='5'></head><body style='font-family:Arial;text-align:center;padding-top:100px;'><h2>Đang xử lý hồ sơ...</h2><p>Vui lòng đợi giây lát.</p></body></html>");
            return new System.Web.Mvc.EmptyResult();
        }
        private byte[] AddPacsPageToPdf_iTextSharp(string maBA)
        {
            var listLinkPacs = _BenhAnClsKq.Table.AsQueryable().Where(x => x.MaBa == maBA).Select(x => x.LinkPacsLis).ToList();
            var validLinks = listLinkPacs.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (validLinks.Count == 0) return null;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4))
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                    doc.Open();
                    var titleFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA_BOLD, 14);
                    var linkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 11, iTextSharp.text.BaseColor.BLUE);
                    doc.Add(new iTextSharp.text.Paragraph("DANH SACH LINK PACS/LIS", titleFont));
                    foreach (var link in validLinks)
                    {
                        iTextSharp.text.Anchor anchor = new iTextSharp.text.Anchor(link, linkFont) { Reference = link };
                        doc.Add(new iTextSharp.text.Paragraph(anchor));
                    }
                    doc.Close();
                }
                return ms.ToArray();
            }
        }
        private async Task<(List<string> files, string apiBase)> GetFileListFromStore(string maBA)
        {
            var files = new List<string>();
            string apiBase = "";

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(maBA)) return (files, apiBase);

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString;

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            using (var command = new System.Data.SqlClient.SqlCommand("spGetDMTrangThaiKyByMaBA", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MaBA_NoiTru", maBA);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    // Lấy danh sách file (Kết quả thứ 1)
                    while (await reader.ReadAsync())
                    {
                        var path = reader["DuongDanLink"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            files.Add(path.Trim());
                        }
                    }

                    // Chuyển sang kết quả thứ 2 để lấy LinkEMR
                    if (await reader.NextResultAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            apiBase = reader["LinkEMR"]?.ToString() ?? "";
                            apiBase = apiBase.Trim();
                        }
                    }
                }
            }
            return (files, apiBase);
        }

        //private async Task<(List<string> files, string apiBase)> GetFileListFromStore(string maBA)
        //{
        //    var files = new List<string>();
        //    string apiBase = "";
        //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString;

        //    using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
        //    using (var command = new System.Data.SqlClient.SqlCommand("spGetDMTrangThaiKyByMaBA", connection))
        //    {
        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@MaBA_NoiTru", maBA);
        //        await connection.OpenAsync();
        //        using (var reader = await command.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //                files.Add(reader["DuongDanLink"]?.ToString());

        //            if (await reader.NextResultAsync() && await reader.ReadAsync())
        //                apiBase = reader["LinkEMR"]?.ToString() ?? "";
        //        }
        //    }
        //    return (files, apiBase);
        //}
        //private async Task<List<byte[]>> DownloadFilesToMemory(List<string> filePaths, string apiBase)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.Timeout = TimeSpan.FromMinutes(5);

        //        // Tạo danh sách các Task để chạy song song
        //        var downloadTasks = filePaths
        //            .Where(path => !string.IsNullOrEmpty(path))
        //            .Select(async path =>
        //            {
        //                try
        //                {
        //                    if (System.IO.File.Exists(path))
        //                    {
        //                        // Đọc file từ ổ đĩa (vẫn dùng Task.Run để không chặn luồng chính)
        //                        return await Task.Run(() => System.IO.File.ReadAllBytes(path));
        //                    }
        //                    else if (!string.IsNullOrEmpty(apiBase))
        //                    {
        //                        // Logic nối chuỗi chuẩn như code trước
        //                        string baseUrl = apiBase.EndsWith("/") || apiBase.EndsWith("=") ? apiBase : apiBase + "/";
        //                        string relativePath = path.Replace("\\", "/").TrimStart('/');
        //                        string url = baseUrl + Uri.EscapeDataString(relativePath);

        //                        byte[] data = await client.GetByteArrayAsync(url);
        //                        return (data != null && data.Length > 100) ? data : null;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    WriteLog($"Lỗi tải file song song tại {path}: {ex.Message}");
        //                }
        //                return null;
        //            });

        //        // Kích hoạt tất cả các Task cùng lúc và đợi cho đến khi tất cả xong
        //        byte[][] results = await Task.WhenAll(downloadTasks);

        //        // Lọc bỏ những file bị null (do lỗi hoặc không tìm thấy) và chuyển về List
        //        return results.Where(data => data != null).ToList();
        //    }
        //}
        //private async Task<List<byte[]>> DownloadFilesToMemory(List<string> filePaths, string apiBase)
        //{
        //    var result = new List<byte[]>();
        //    using (var client = new HttpClient())
        //    {
        //        client.Timeout = TimeSpan.FromMinutes(5);
        //        foreach (var path in filePaths)
        //        {
        //            if (string.IsNullOrEmpty(path)) continue;
        //            try
        //            {
        //                byte[] fileBytes = null;
        //                if (System.IO.File.Exists(path))
        //                {
        //                    // Đọc file từ ổ đĩa local không dùng ReadAllBytesAsync (tránh lỗi .NET cũ)
        //                    fileBytes = await Task.Run(() => System.IO.File.ReadAllBytes(path));
        //                }
        //                else if (!string.IsNullOrEmpty(apiBase))
        //                {
        //                    string baseUrl = apiBase.EndsWith("/") || apiBase.EndsWith("=") ? apiBase : apiBase + "/";
        //                    string relativePath = path.Replace("\\", "/").TrimStart('/');
        //                    string url = baseUrl + Uri.EscapeDataString(relativePath);
        //                    fileBytes = await client.GetByteArrayAsync(url);
        //                }

        //                if (fileBytes != null && fileBytes.Length > 100) result.Add(fileBytes);
        //            }
        //            catch { /* Log lỗi file đơn lẻ nếu cần */ }
        //        }
        //    }
        //    return result;
        //}
        private void AddPacsPageToPdf(PdfDocument outputDocument, string maBA)
        {
            var listLinkPacs = _BenhAnClsKq.Table.AsQueryable().Where(x => x.MaBa == maBA).Select(x => x.LinkPacsLis).ToList();
            if (listLinkPacs != null && listLinkPacs.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                PdfPage page = outputDocument.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 14, XFontStyle.Bold);
                XFont linkFont = new XFont("Arial", 11, XFontStyle.Regular);
                double yPoint = 40;
                gfx.DrawString("DANH SÁCH LINK PACS/LIS", titleFont, XBrushes.Black, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
                yPoint += 30;
                foreach (var link in listLinkPacs.Where(l => !string.IsNullOrWhiteSpace(l)))
                {
                    if (yPoint > page.Height - 50) { page = outputDocument.AddPage(); gfx = XGraphics.FromPdfPage(page); yPoint = 40; }
                    gfx.DrawString(link, linkFont, XBrushes.Blue, new XRect(40, yPoint, page.Width - 80, 20), XStringFormats.TopLeft);
                    page.Annotations.Add(PdfLinkAnnotation.CreateWebLink(new PdfRectangle(new XRect(40, yPoint, page.Width - 80, 15)), link));
                    yPoint += 25;
                }
            }
        }
        [HttpGet("DownloadFileZip1/{id}")]
        public async Task<List<string>> DownloadFileZIP1(decimal id)
        {
            List<string> downloadedFiles = new List<string>();
            try
            {
                var benhAn = _benhAnRepository.GetById(id);
                if (benhAn == null) return downloadedFiles;
                string maBA = benhAn.MaBa;
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString;
                var fileList = new List<DMTrangThaiKy>();
                string linkApiBase = "";
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("spGetDMTrangThaiKyByMaBA", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaBA_NoiTru", maBA);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fileList.Add(new DMTrangThaiKy
                            {
                                DuongDanFile = reader["DuongDanLink"]?.ToString(),
                                LoaiGiayTo = reader["MaNhomTaiLieu"]?.ToString()
                            });
                        }
                        // Bảng 2: Link API gốc (LinkEMR)
                        if (reader.NextResult() && reader.Read())
                        {
                            linkApiBase = reader["LinkEMR"]?.ToString() ?? "";
                        }
                    }
                }
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string tempFolder = Path.Combine(baseDir, "Storage", "TempPDF", maBA);
                if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
                Directory.CreateDirectory(tempFolder);
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    int index = 1;
                    string format = "D" + Math.Max(4, fileList.Count.ToString().Length);
                    foreach (var fileItem in fileList)
                    {
                        if (string.IsNullOrEmpty(fileItem.DuongDanFile)) continue;
                        string fileName = string.Format("{0}_{1}", index.ToString(format), Path.GetFileName(fileItem.DuongDanFile));
                        string targetPath = Path.GetFullPath(Path.Combine(tempFolder, fileName));
                        index++;
                        try
                        {
                            if (System.IO.File.Exists(fileItem.DuongDanFile))
                            {
                                System.IO.File.Copy(fileItem.DuongDanFile, targetPath, true);
                                downloadedFiles.Add(targetPath);
                            }
                            else if (!string.IsNullOrEmpty(linkApiBase))
                            {
                                string baseUrl = linkApiBase.EndsWith("/") || linkApiBase.EndsWith("=") ? linkApiBase : linkApiBase + "/";
                                string url = baseUrl + Uri.EscapeDataString(fileItem.DuongDanFile);
                                byte[] fileBytes = await client.GetByteArrayAsync(url);
                                if (fileBytes != null && fileBytes.Length > 100)
                                {
                                    System.IO.File.WriteAllBytes(targetPath, fileBytes);
                                    downloadedFiles.Add(targetPath);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(string.Format("Lỗi nạp file {0}: {1}", fileItem.DuongDanFile, ex.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Lỗi nghiêm trọng tại DownloadFileZIP1: " + ex.Message);
            }
            return downloadedFiles;
        }
        [HttpGet("DownloadFileZip/{id}")]
        public async Task<IActionResult> DownloadFileZIP(decimal id)
        {
            try
            {
                var benhAn = _benhAnRepository.GetById(id);
                if (benhAn == null)
                    return NotFound("Bệnh án không tồn tại.");

                string maBA = benhAn.MaBa;
                string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString +
                                          "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
                var fileList = new List<DMTrangThaiKy>();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("spGetDMTrangThaiKyByMaBA", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaBA_NoiTru", maBA);
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var loaiGiayTo = reader["MaNhomTaiLieu"].ToString();

                        DateTime? ngayYLenh = null;

                        // Chỉ xử lý ngày khi LoaiGiayTo = 5
                        if (loaiGiayTo == "5")
                        {
                            var ngayValue = reader["NgayYLenh"];
                            if (ngayValue != DBNull.Value)
                            {
                                if (DateTime.TryParse(ngayValue.ToString(), out DateTime temp))
                                {
                                    ngayYLenh = temp;
                                }
                                else
                                {
                                    WriteLog($"Warning: Ngày Y Lệnh không hợp lệ: {ngayValue}");
                                }
                            }
                        }
                        fileList.Add(new DMTrangThaiKy
                        {
                            DuongDanFile = reader["DuongDanLink"].ToString(),
                            LoaiGiayTo = reader["MaNhomTaiLieu"].ToString(),
                            NgayYLenh = ngayYLenh
                        });
                    }
                }
                WriteLog($"Lỗi copy file :" + fileList.Count);
                string downloadFolder = Path.Combine("Storage", "DownloadZip", maBA);
                string baseFolder = Path.Combine("Storage", "DownloadFolder");
                string finalFolder = Path.Combine(baseFolder, maBA);

                if (Directory.Exists(downloadFolder)) Directory.Delete(downloadFolder, true);
                Directory.CreateDirectory(downloadFolder);

                // ================= DOWNLOAD / COPY FILE =================

                // Sắp xếp fileList: loại 5 theo NgayYLenh, các loại khác giữ nguyên vị trí
                var finalList = fileList
                    .OrderBy(x => x.LoaiGiayTo == "5" ? x.NgayYLenh ?? DateTime.MaxValue : DateTime.MaxValue)
                    .ThenBy(x => x.LoaiGiayTo != "5" ? 1 : 0)
                    .ToList();

                int indexLoai5 = 1;
                int index = 1;
                int totalFiles = fileList.Count;
                // Tính xem tổng số file có bao nhiêu chữ số (vd: 100 file -> 3 chữ số)
                int digitCount = totalFiles.ToString().Length;
                string format = "D" + digitCount;
                foreach (var fileItem in finalList)
                {
                    try
                    {
                        //string originalName = Path.GetFileName(fileItem.DuongDanFile);
                        //string fileName;

                        //if (fileItem.LoaiGiayTo == "5")
                        //{
                        //    // Thêm index cho loại 5
                        //    fileName = $"{indexLoai5:D3}_{originalName}";
                        //    indexLoai5++;
                        //}
                        //else
                        //{
                        //    fileName = originalName;
                        //}
                        string fileName = $"{index.ToString(format)}_{Path.GetFileName(fileItem.DuongDanFile)}";
                        index++;
                        string targetFolder = Path.Combine(downloadFolder, fileItem.LoaiGiayTo);
                        string targetPath = Path.Combine(targetFolder, fileName);

                        Directory.CreateDirectory(targetFolder);

                        if (!string.IsNullOrEmpty(fileItem.DuongDanFile))
                        {
                            if (System.IO.File.Exists(fileItem.DuongDanFile))
                            {
                                // File tồn tại trong server
                                System.IO.File.Copy(fileItem.DuongDanFile, targetPath, true);
                            }
                            else
                            {
                                // File không có trên server → gọi API
                                string url = "http://14.248.94.181:6678/MedyxAPI/AGB/GetFile?filepath=" + fileItem.DuongDanFile;
                                using (HttpClient client = new HttpClient())
                                {
                                    HttpResponseMessage response = await client.GetAsync(url);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                                        System.IO.File.WriteAllBytes(targetPath, fileBytes);
                                    }
                                    else
                                    {
                                        WriteLog($"Lỗi gọi API lấy file {fileItem.DuongDanFile}: {response.StatusCode}");
                                    }
                                };

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"Lỗi copy file {fileItem.DuongDanFile}: {ex.Message}");
                    }
                }

                // ================= ZIP =================

                if (Directory.Exists(finalFolder)) Directory.Delete(finalFolder, true);
                Directory.Move(downloadFolder, finalFolder);

                string zipFileName = $"HSBA_{maBA}_{DateTime.Now:ddMMyyyyHHmmssfff}.zip";
                string zipPath = Path.Combine(baseFolder, zipFileName);
                ZipFile.CreateFromDirectory(finalFolder, zipPath);

                byte[] zipBytes = System.IO.File.ReadAllBytes(zipPath);
                return File(zipBytes, "application/zip", zipFileName);
            }
            catch (Exception ex)
            {
                WriteLog($"Lỗi tổng quát trong DownloadFileZIP: {ex.Message}");
                return StatusCode(500, "Lỗi hệ thống.");
            }
        }
        public void WriteLog(String log)
        {
            #region ghi log dang nhap
            var u = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");

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
            emp.NoiDungSD = "Loi File PDF: " + log;
            collection.InsertOne(emp);
            #endregion
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
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
        }
        [HttpGet("{id}/print-ba-file/giay-ra-vien/{maba}.pdf")]
        public ActionResult GiayRaVienPrint(decimal id, [FromQuery] PrintParameters parameters)
        {
            var path = _benhAnService.GiayRaVienPrint(id);
            if (parameters.ShouldReturnPath)
                return new JsonResult(new { path });
            DownloadFileResult downloadFileResult = uploadFileRespository.Download(path, true, true);
            return File(downloadFileResult.FileBytes, downloadFileResult.contentType);
        }
        [HttpGet("print-test")]
        public ActionResult KyHSBABA1([FromQuery] PrintParameters parameters, decimal id)
        {
            string IDBA = _benhAnService.Print1(id);
            string ltl = "1";
            string pdfFilePath = ShowPdf(IDBA, ltl);
            if (!string.IsNullOrEmpty(pdfFilePath))
            {
                // Trả về mã JavaScript để mở tệp PDF trong một tab mới
                return Content($"<script>window.open('{pdfFilePath}','_blank');</script>");
            }
            else
            {
                // Xử lý trường hợp không tìm thấy tệp PDF
                return Content("PDF not found");
            }
        }
        //[HttpPost]
        //public IActionResult SaveToDatabaseEMR(string idbahis, string Idbaemr, string LoaiGiayTo, string IDchuoi, string stt, string sttkhoa, string MaBA)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
        //        {
        //            connection.Open();
        //            using (SqlCommand command = new SqlCommand("SpInsertURLDMTrangThaiKyEMR", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@IDBA", idbahis);
        //                command.Parameters.AddWithValue("@Idbaemr", Idbaemr);
        //                command.Parameters.AddWithValue("@LoaiGiayTo", LoaiGiayTo);
        //                command.Parameters.AddWithValue("@IDchuoi", IDchuoi);
        //                command.Parameters.AddWithValue("@stt", stt);
        //                command.Parameters.AddWithValue("@sttkhoa", sttkhoa);
        //                command.Parameters.AddWithValue("@MaBA", MaBA);
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        return Ok("Dữ liệu đã được lưu thành công.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi cơ sở dữ liệu, ghi log hoặc xử lý theo cần thiết
        //        return StatusCode(500, "Đã xảy ra lỗi khi lưu dữ liệu vào cơ sở dữ liệu.");
        //    }
        //}
        private string ShowPdf(string IDBA, string ltl)
        {
            string tenStore = "spGetPdfDataEMR";
            string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                using (SqlCommand Command = new SqlCommand(tenStore, Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@IDBAEMR", IDBA);
                    Command.Parameters.AddWithValue("@LoaiGiayTo", ltl);
                    string pdfFilePath = Command.ExecuteScalar() as string;
                    if (!string.IsNullOrEmpty(pdfFilePath))
                    {
                        return pdfFilePath;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }
    }
}
