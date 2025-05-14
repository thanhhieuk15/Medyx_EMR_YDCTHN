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
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        private readonly IRepository<DM_HSBA> _dmHSBARepository = null;
        private readonly IRepository<DmbaLoaiTaiLieu> _dmbaLoaiTaiLieuRepository = null;
        private IHttpContextAccessor _accessor = null;
        private readonly IConfiguration _config;
        public BenhAnController(BenhAnService benhAnService, IHttpContextAccessor accessor)
        {
            _benhAnService = benhAnService;
            _benhAnRepository = new GenericRepository<BenhAn>(accessor);
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
            var reuslt = kq;
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
            //spDMTrangThaiKy_GetChuaKy
            try
            {
                int checkFile = CheckFileChuaKy(idba);
                if (checkFile > 0)
                {
                    return Ok(new { message = "Đóng bệnh án thất bại! Vui lòng ký hết để đóng được bệnh án!", status = 500 });
                }
                if (ModelState.IsValid)
                {
                    _benhAnService.ThongTinBnUpdate(benhAn, idba, true);
                }
                return Ok(new { message = "Đóng bệnh án thành công !", status = 200 });
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
                WriteLog("reportfilename: "+ reportfilename);
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
                WriteLog("stream2 Error: " + stream2+reqContext);
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
                ReportDocument rpt = new ReportDocument();

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


        [HttpGet("xuat-file-Da-Ky/{id}")]
        public IActionResult ExportPDFDaKy(decimal id)
        {
            string mss = "";
            try
            {
                DownloadFileZIP(id);
                // Lấy thông tin bệnh án
                var benhAn = _benhAnRepository.GetById(id);
                if (benhAn == null)
                {
                    return NotFound("Bệnh án không tồn tại.");
                }
                mss += "MaBenhAn:" + benhAn.MaBa + "/";

                // Lấy thông tin hồ sơ bệnh án
                var dmHoSoBenhAn = _dmHSBARepository.GetAll().OrderBy(x => x.Id).ToList();
                mss += "dmHoSoBenhAn:" + dmHoSoBenhAn.Count() + "/";

                var baseFolder = $"Storage\\DownloadFolder";
                mss += "baseFolder:" + baseFolder + "/";

                using (var outputDocument = new PdfDocument())
                {
                    foreach (var hoSoBenhAn in dmHoSoBenhAn)
                    {
                        mss += "hoSoBenhAn:" + hoSoBenhAn.Id + "/";
                        var dmLoaiTaiLieu = _dmbaLoaiTaiLieuRepository._context.DmbaLoaiTaiLieu
                                            .Where(x => x.MaNhomTaiLieu == (byte?)hoSoBenhAn.Id)
                                            .Select(x => x.MaLoaiTaiLieu).ToList();
                        mss += "dmLoaiTaiLieu:" + dmLoaiTaiLieu.Count() + "/";

                        foreach (var item in dmLoaiTaiLieu)
                        {
                            if (item != 0)
                            {
                                string destinationFolder = Path.Combine(baseFolder, benhAn.MaBa);
                                mss += "destinationFolder:" + destinationFolder + "/";

                                if (Directory.Exists(destinationFolder))
                                {
                                    string[] subDirectories = Directory.GetDirectories(destinationFolder);
                                    mss += "subDirectories:" + subDirectories.Length + "/";

                                    foreach (string subDirectory in subDirectories)
                                    {
                                        string subDirectoryName = Path.GetFileName(subDirectory);
                                        mss += "subDirectoryName:" + subDirectoryName + "/";

                                        if (string.Equals(subDirectoryName, item.ToString(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            string[] pdfFiles = Directory.GetFiles(subDirectory, "*.pdf");
                                            mss += "pdfFiles:" + pdfFiles.Length + "/";
                                            Array.Sort(pdfFiles);

                                            foreach (string pdfFile in pdfFiles)
                                            {
                                                try
                                                {
                                                    mss += "pdfFile:" + pdfFile + "/";
                                                    byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFile);
                                                    using (var pdfStream = new MemoryStream(pdfBytes))
                                                    {
                                                        using (var inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
                                                        {
                                                            foreach (var page in inputDocument.Pages)
                                                            {
                                                                outputDocument.AddPage(page);
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    WriteLog("Error processing PDF file: " + pdfFile + " - " + ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    string outputPath = Path.Combine(baseFolder, "HSBA_PDF_" + benhAn.MaBa + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf");
                    outputDocument.Save(outputPath);

                    var fileBytes = System.IO.File.ReadAllBytes(outputPath);
                    var contentType = "application/pdf";
                    DownloadFileResult downloadFileResult = uploadFileRespository.Download(outputPath, true, true);
                    return File(downloadFileResult.FileBytes, contentType);
                }
            }
            catch (Exception ex)
            {
                WriteLog("Exception: " + ex.Message + " - mss: " + mss);
                return StatusCode(500, "Internal server error. Please check the server logs for details.");
            }
        }


        [HttpGet("DownloadFileZip/{id}")]
        public IActionResult DownloadFileZIP(decimal id)
        {
            try
            {
                var benhAn = _benhAnRepository.GetById(id);
                if (benhAn == null)
                {
                    return NotFound();
                }

                string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
                string tenStore = "spGetDMTrangThaiKyByMaBA";
                List<DMTrangThaiKy> pdfFilePaths = new List<DMTrangThaiKy>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(tenStore, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@MaBA_NoiTru", benhAn.MaBa));

                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                DMTrangThaiKy pdfFilePath = new DMTrangThaiKy
                                {
                                    DuongDanFile = reader["DuongDanLink"].ToString(),
                                    LoaiGiayTo = reader["MaNhomTaiLieu"].ToString()
                                };
                                pdfFilePaths.Add(pdfFilePath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while executing SQL query: " + ex.Message);
                        return StatusCode(500, "Internal server error");
                    }
                }

                string downloadFolder = Path.Combine("Storage", "DownloadZip", benhAn.MaBa);
                string baseFolder = Path.Combine("Storage", "DownloadFolder");
                if (Directory.Exists(downloadFolder))
                {
                    Directory.Delete(downloadFolder, true);
                }
                Directory.CreateDirectory(downloadFolder);

                foreach (var item in pdfFilePaths)
                {
                    try
                    {
                        string fileName = Path.GetFileName(item.DuongDanFile);
                        string destinationFolder = Path.Combine(downloadFolder, item.LoaiGiayTo);
                        string destinationFilePath = Path.Combine(destinationFolder, fileName);

                        // Create folder if it doesn't exist
                        Directory.CreateDirectory(destinationFolder);
                        // Copy file to destination folder
                        using (var sourceStream = new FileStream(item.DuongDanFile, FileMode.Open, FileAccess.Read))
                        {
                            using (var destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
                            {
                                sourceStream.CopyTo(destinationStream);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying file: {ex.Message}");
                        // Handle or log the error
                    }
                }

                string renamedFolder = Path.Combine(baseFolder, $"{benhAn.MaBa}");
                if (Directory.Exists(renamedFolder))
                {
                    Directory.Delete(renamedFolder, true);
                }
                Directory.Move(downloadFolder, renamedFolder);

                // Zip the renamed folder
                string zipFileName = $"HSBA_{benhAn.MaBa}_{DateTime.Now:ddMMyyyyHHmmssfff}.zip";
                string tempZipPath = Path.Combine(baseFolder, zipFileName);
                ZipFile.CreateFromDirectory(renamedFolder, tempZipPath);

                // Read zip file bytes and return as response
                byte[] fileBytes = System.IO.File.ReadAllBytes(tempZipPath);
                return File(fileBytes, "application/zip", zipFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
