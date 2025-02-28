using System;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
using Medyx_EMR_BCA.Models.DBConText;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Medyx_EMR_BCA.Models.DanhMuc;
using Excel;
using MongoDB.Driver;
using Medyx_EMR_BCA.Models;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Spire.Pdf.Exporting.XPS.Schema;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using PdfSharp.Pdf;
using System.Net.Http;
using Path = System.IO.Path;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Text.RegularExpressions;
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
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using SessionHelper = Medyx_EMR_BCA.ApiAssets.Helpers.SessionHelper;
using System.Windows.Interop;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace HTC.WEB.NIOEH.Areas.Client.DanhMuc
{
    public class DMTrangThaiKyController : BCAController
    {
        #region Khởi tạo
        private IMemoryCache cache;
        private readonly IConfiguration _config;
        private readonly IRepository<DmbaLoaiTaiLieu> _dmbaLoaiTaiLieuRepository = null;
        private IHttpContextAccessor _accessor = null;
        //public readonly ISession session;
        public DMTrangThaiKyController(IMemoryCache cache, IConfiguration config, IHttpContextAccessor accessor, DmbaLoaiTaiLieuService dmbaLoaiTaiLieuService)
        {
            this.cache = cache;
            _config = config;
            _dmbaLoaiTaiLieuRepository = new GenericRepository<DmbaLoaiTaiLieu>(accessor);
            //this.session = HttpContext.Session;
        }
        private void SaveToDatabase(string base64URL, decimal Idbaemr, string stt, string Loaigiayto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SpInsertfilebase64DMTrangThaiKy", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@base64URL", base64URL);
                        command.Parameters.AddWithValue("@Idbaemr", Idbaemr);
                        command.Parameters.AddWithValue("@stt", stt);
                        command.Parameters.AddWithValue("@Loaigiayto", Loaigiayto);
                        //command.Parameters.AddWithValue("@IDBAMaBa", IDBAMaBa);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public ActionResult Index()
        {
            return Khoitao(cache);
        }
        #endregion Khởi tạo

        #region Load danh sách trạng thái ký  paging

        [HttpGet]
        public JsonResult LoadData(int stt, string idBA, string maBN, string maBS, string tenBS, string trangthaiKy, int lanKy, string duongdanFile, string maMay, string ngayLap, string ngaySD, string nguoiSD, string ngayHuy, string nguoiHuy, int pageIndex, int pageSize, int add)
        {
            Pagination db = new Pagination();

            if (string.IsNullOrEmpty(idBA))
                idBA = "";
            if (string.IsNullOrEmpty(maBN))
                maBN = "";
            if (string.IsNullOrEmpty(maBS))
                maBS = "";
            if (string.IsNullOrEmpty(tenBS))
                tenBS = "";
            if (string.IsNullOrEmpty(trangthaiKy))
                trangthaiKy = "";
            if (string.IsNullOrEmpty(duongdanFile))
                duongdanFile = "";
            if (string.IsNullOrEmpty(maMay))
                maMay = "";
            if (string.IsNullOrEmpty(ngayLap))
                ngayLap = "";
            if (string.IsNullOrEmpty(ngaySD))
                ngaySD = "";
            if (string.IsNullOrEmpty(nguoiSD))
                nguoiSD = "";
            if (string.IsNullOrEmpty(ngayHuy))
                ngayHuy = "";
            if (string.IsNullOrEmpty(nguoiHuy))
                nguoiHuy = "";
            var response = db.DMTrangThaiKyGetListPaging(stt, idBA, maBN, maBS, tenBS, trangthaiKy, lanKy, duongdanFile, maMay, ngayLap, ngaySD, nguoiSD, ngayHuy, nguoiHuy, pageIndex, pageSize, add);
            return CreateJsonJsonResult(() =>
            {
                return Json(response);
            });
        }
        [HttpPost]
        public ActionResult getSignFilePath(string path)
        {
            if (path.Length == 0)
            {
                return Json(new { success = false, message = "IDBA không thể rỗng! Không tìm thấy file", status = 500 });
            }
            else
            {
                Pagination db = new Pagination();
                string[] arr = path.Split('_');
                if (arr.Length > 0)
                {
                    string mss = "";
                    try
                    {
                        int loaitailieu = int.Parse(arr[0]);
                        mss += "loaitailieu:" + loaitailieu.ToString() + "/";
                        decimal idba = decimal.Parse(arr[1]);
                        mss += "idba:" + idba.ToString() + "/";
                        string response = db.getSignFilePath(loaitailieu, idba);
                        mss += "response:" + response + "/";
                        string webRootPath = _config.GetValue<string>("HSBADirectory").Replace("Storage\\HSBA", "");
                        mss += "webRootPath:" + webRootPath + "/";
                        string logFilePath = System.IO.Path.Combine(webRootPath, response);
                        mss += "logFilePath:" + logFilePath + "/";
                        string config = System.IO.File.ReadAllText(logFilePath);
                        byte[] configBytes = Encoding.UTF8.GetBytes(config);
                        string base64String = Convert.ToBase64String(configBytes);
                        mss += "base64String:" + base64String + "/";
                        return CreateJsonJsonResult(() =>
                        {
                            return Json(new { success = true, message = logFilePath + "base64String", status = 200 });
                        });
                    }
                    catch (Exception ex)
                    {
                        #region ghi log dang nhap
                        UserProfileSessionData u = new UserProfileSessionData();
                        u.ListRoleSession = db.GetActionByRoleID(u.Pub_sAccount);
                        u.MongoDBConnectionString = _config.GetValue<string>("Medyx_BCADatabase:ConnectionString");
                        u.MongoDBDataBaseName = _config.GetValue<string>("Medyx_BCADatabase:DatabaseName"); 
                        string constr = u.MongoDBConnectionString;
                        var client = new MongoClient(constr);
                        IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                        IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                        string MaMay = this.GetLocalIPAddress();
                        TraceLogMongo emp = new TraceLogMongo();
                        emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        emp.TenBang = "Trang thai ky";
                        emp.KieuTacDong = "get base 64";
                        emp.NguoiSD = "admin";
                        emp.MaMay = MaMay;
                        emp.NoiDungSD = mss;
                        collection.InsertOne(emp);
                        #endregion
                        return Json(new { success = false, message = "Không tìm thấy file", status = 404 });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy file", status = 500 });
  
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateSignedFile(string path, string base64string)
        {
            if (path.Length == 0)
            {
                return Json(new { success = false, message = "IDBA không thể rỗng! Không tìm thấy file", status = 500 });
            }
            else
            {
                Pagination db = new Pagination();
                string[] arr = path.Split('_');
                if (arr.Length > 0)
                {
                    int loaitailieu = int.Parse(arr[0]);
                    decimal idba = decimal.Parse(arr[1]);
                    string response = db.getSignFilePath(loaitailieu, idba);

                    string base64Data = base64string.ToString();
                    byte[] dataBytes = Convert.FromBase64String(base64string);
                    //Byte[] dataBytes = Convert.FromBase64String(base64string);
                    string xmlText = Encoding.UTF8.GetString(dataBytes);

                    string webRootPath = _config.GetValue<string>("HSBADirectory").Replace("Storage\\HSBA", "");
                    string logFileName = response;
                    string directory = AppDomain.CurrentDomain.BaseDirectory;
                    string logFilePath = System.IO.Path.Combine(webRootPath, response);

                    string config = System.IO.File.ReadAllText(logFilePath);
                    byte[] configBytes = Encoding.UTF8.GetBytes(config);
                    string base64String = Convert.ToBase64String(configBytes);
                    System.IO.File.WriteAllText(logFilePath, xmlText);
                    System.IO.File.WriteAllBytes(logFilePath, dataBytes);

                    return CreateJsonJsonResult(() =>
                    {
                        return Json(new { success = true, message = "Lưu file thành công", status = 200 });

                    });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy file", status = 500 });
                }
            }
        }
        #endregion

        #region View thực hiện update trạng thái ký 



        // chuyển api 



        [HttpGet]
        public ActionResult SaveToDatabase(string base64URL, string stt, string Loaigiayto)
        {

            using (HttpClient client = new HttpClient())
            {
                string fileName = Path.GetFileName(base64URL);

                // Gán IDBA bằng tên của tệp
                string IDBAEMR = fileName.Replace("_1.pdf", "");
                var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
                // Tải tệp PDF từ URL và đợi cho việc tải hoàn tất
                byte[] pdfData = client.GetByteArrayAsync(base64URL).Result;

                // Chuyển đổi dữ liệu sang dạng Base64
                string base64String = Convert.ToBase64String(pdfData);
                string dk = u.Pub_sNguoiSD;
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SpInsertfilebase64DMTrangThaiKy", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@base64URL", base64String);
                            command.Parameters.AddWithValue("@Idbaemr", IDBAEMR);
                            command.Parameters.AddWithValue("@stt", stt);
                            command.Parameters.AddWithValue("@Loaigiayto", Loaigiayto);
                            command.Parameters.AddWithValue("@MaBS", dk);
                            //command.Parameters.AddWithValue("@IDBAMaBa", IDBAMaBa);
                            command.ExecuteNonQuery();
                            return Ok("thành công");

                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }


        }

        [HttpGet]

        //public ActionResult urlSgintFilePath(string base64URL)

        //{
        //    string fileName = Path.GetFileName(base64URL);
        //    string url = "http://192.168.1.14:7000/Upload/";
        //    // Gán IDBA bằng tên của tệp
        //    string IDBAEMR = fileName.Replace(".pdf", "");
        //    var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
        //    string dk = u.Pub_sNguoiSD;
        //    string urlFilesign =  url + IDBAEMR + ".signed.pdf.pdf";
        //    string LtL = "1";
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("spTrangThaiKy_linkemr", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.AddWithValue("@IDBAEMR", IDBAEMR);
        //            command.Parameters.AddWithValue("@DuongDanFile", urlFilesign);                 
        //            command.Parameters.AddWithValue("@MaBS", dk);
        //            command.Parameters.AddWithValue("@LoaigiayTo", LtL);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    return Ok(urlFilesign);
        //}





        public ActionResult urlSgintFilePath(string base64URL)
        {
            string fileName = Path.GetFileName(base64URL);
            string folderName = Path.GetDirectoryName(base64URL);

            // Trích xuất tên thư mục đầu tiên
            string[] folders = folderName.Split(Path.DirectorySeparatorChar);
            string firstFolder = folders.LastOrDefault();
            string[] segments = base64URL.TrimEnd('/').Split('/');
            //string fileNames = segments[segments.Length - 1];
            string IDBAEMR = "";
            string LtL = "";
            // Kiểm tra nội dung của tên thư mục đầu tiên
            if (!string.IsNullOrEmpty(firstFolder))

            {
                string url = "http://192.168.1.14:7000/Upload/";
                // Gán IDBA bằng tên của tệp
                string fileold = fileName.Replace(".pdf", "");
                var match = Regex.Match(fileName, @"-(.+)\.pdf");
                string idba_stt = "";
                if (match.Success)
                {
                     idba_stt = match.Groups[1].Value;
                }
                //string IDBAEMR = fileName.Replace("_1.pdf", "");
                //string[] parts = fileNames.Split('_');
                var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
                string dk = u.Pub_sNguoiSD;
                string urlFilesign = url + fileold + ".signed.pdf.pdf";
                IDBAEMR = idba_stt;
                //if (parts.Length >= 2)
                //{
                //    //IDBAEMR = parts[0];
                //    LtL = parts[1].Split('.')[0];
                //}
                // Thực hiện kết nối CSDL và thực thi lệnh nếu thỏa mãn điều kiện
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SpInsertURLDMTrangThaiKy", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IDBA", IDBAEMR);
                        command.Parameters.AddWithValue("@DuongDanFile", urlFilesign);
                        //command.Parameters.AddWithValue("@LoaiGiayTo", LtL);
                        command.Parameters.AddWithValue("@MaBS", dk);
                        command.ExecuteNonQuery();
                    }
                }
                return Ok(urlFilesign);
            }
            else
            {
                // Nếu không thỏa mãn điều kiện, trả về BadRequest hoặc thông báo lỗi tùy thuộc vào yêu cầu của bạn
                return BadRequest("Folder không chứa nội dung '1'.");
            }
        }

        [HttpGet]
        public ActionResult ShowPdfhis(string base64URL)
        {

            string message = "";
            int lanky = 0;
            int lanDaKy = 0;
            int lanDuocKy = 0;
            string FilePathGoc = "";
            string FilePathKy = "";
            string tenBSNames = "";
            string MaBS = "";
            string maChucVu = "";
            bool isCheckKy = false;
            string fileName = Path.GetFileName(base64URL);
            string IDBAEMR = fileName.Replace(".pdf", "");
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            string dk = u.Pub_sNguoiSD;
            string ltl = "";
            string[] segments = base64URL.TrimEnd('/').Split('/');
            string fileNames = segments[segments.Length - 1]; // Lấy phần tử cuối cùng của mảng segments
            // Tách phần số sau dấu gạch trong fileName
            string[] parts = fileNames.Split('_');
            if (parts.Length >= 2)
            {
                ltl = parts[1].Split('.')[0]; // Lấy phần sau dấu _ và loại bỏ phần mở rộng .pdf
            }
            string tenStore = "spGetPdfHistoemr";
            string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                using (SqlCommand Command = new SqlCommand(tenStore, Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@IDBAEMR", IDBAEMR);
                    Command.Parameters.AddWithValue("@MaBS", dk);
                    Command.Parameters.AddWithValue("@LoaiGiayTo", ltl);
                    SqlDataAdapter dp = new SqlDataAdapter(Command);
                    DataSet dr = new DataSet();
                    dp.Fill(dr);
                    if (dr != null && dr.Tables.Count > 0)
                    {
                        if (dr.Tables[0].Rows.Count > 0)
                        {
                            //set lan ky
                            lanky = int.Parse(dr.Tables[0].Rows[0]["LanKy"].ToString());
                            //set lan Da Ky
                            lanDaKy = int.Parse(dr.Tables[0].Rows[0]["LanDaKy"].ToString());
                            FilePathGoc = dr.Tables[0].Rows[0]["DuongDanFile"].ToString();
                         
                        }
                        if (dr.Tables[1].Rows.Count > 0)
                        {
                            //xuất ra  lần được ký của loại tài liệu  đó 
                            lanDuocKy = int.Parse(dr.Tables[1].Rows[0][0].ToString());
                        }
                        if (dr.Tables[2].Rows.Count > 0)
                        {
                            //danh sách bác sĩ đã ký 
                            for (int i = 0; i < dr.Tables[2].Rows.Count; i++)
                            {
                                tenBSNames += dr.Tables[2].Rows[i]["TenBS"].ToString() + ", ";
                            }
                        }
                        if (dr.Tables[3].Rows.Count > 0)
                        {
                           
                            maChucVu = dr.Tables[3].Rows[0]["MaChucVu"].ToString();
                        }
                    }
                    if (lanDuocKy > 0 && lanDaKy >= lanDuocKy)
                    {
                        if (dr.Tables[2].Rows.Count > 0)
                        {
                            isCheckKy = true;
                            message = "Giấy tờ này đã được ký đủ bởi " + tenBSNames;
                           
                        }
                        else if (maChucVu == "001" || maChucVu == "002" || maChucVu == "003" || maChucVu == "004")
                        {
                            message = "Giấy tờ này đã được ký bởi " + tenBSNames;
                        }
                        else
                        {
                            isCheckKy = true;
                            message = "Giấy tờ này đã ký đủ, các bác sĩ đã ký: "+ tenBSNames;
                        }
                    }
                    else if (lanky == 0)
                    {
                        message = "Giấy tờ này bạn chưa ký!";
                    }
                    else
                    {
                        message = "Giấy tờ này bạn đã ký bởi bác sĩ:"+ tenBSNames +"!";
                    }

                    //string pdfFilePath = Command.ExecuteScalar() as string;
                    //if (!string.IsNullOrEmpty(pdfFilePath))
                    //{

                    //    string fullPdfUrl = pdfFilePath.Replace(".pdf.pdf", ".pdf");
                    //}
                    return Ok(new {DuongDanFile = FilePathGoc, Message = message, IsCheckKy = isCheckKy });

                }
            }

        }


        [HttpGet]
        public ActionResult ShowFileDaKY(string base64URL)
        {
            string mss = "";
            try
            {
                mss += "base64URL:" + base64URL + "/";
                string fileName = Path.GetFileName(base64URL);
                mss += "fileName:" + fileName + "/";
                string LoaiGiayTo = fileName.Replace(".pdf", "");
                mss += "LoaiGiayTo:" + LoaiGiayTo + "/";
                var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
                string dk = u.Pub_sNguoiSD;
                mss += "dk:" + dk + "/";
                string pattern = @"([^/]+)_idba/([^/]+)_stt/([^/]+)_sttKhoa/([^/]+)_ngayYLenh(?:/([^/]+)_LoaiGiayTo)?";
                Match match = Regex.Match(base64URL, pattern);
                string[] arr = base64URL.TrimEnd('/').Split('/');

                mss += "match : " + match + "/";
                string IDBAEMR = arr[5];
                string stt = arr[6];
                string sttKhoa = arr[7];
                string ngayYLenh = arr[8].Replace("-", "/");
                string LoaiGiayToErm = arr[9]; ;
                string ltl = "";
               
                if (LoaiGiayToErm == null || LoaiGiayToErm == "")
                {
                    var MaGiayTo = _dmbaLoaiTaiLieuRepository._context.DmbaLoaiTaiLieu.Where(x => x.MaLoaiGiayTo.Trim().Equals(LoaiGiayTo)).FirstOrDefault()?.MaLoaiTaiLieu;
                    mss += "MaGiayTo:" + MaGiayTo + "/";
                    ltl = MaGiayTo.ToString();
                    mss += "ltl:" + ltl + "/";
                }
                else
                {
                    ltl = LoaiGiayToErm.ToString();
                    mss += "ltl527:" + ltl + "/";
                }


                string tenStore = "spShowFileDaKY";
                string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    mss += "Looxi keest nois/";
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand(tenStore, Connection))
                    {
                        mss += "Command:" + Command + "/";
                        Command.CommandType = CommandType.StoredProcedure;
                        mss += "IDBAEMR:" + IDBAEMR + "/";
                        Command.Parameters.AddWithValue("@IDBAEMR", IDBAEMR);
                        mss += "dk:" + dk+ "/";
                        Command.Parameters.AddWithValue("@MaBS", dk);
                        mss += "ltl:" + ltl + "/";
                        Command.Parameters.AddWithValue("@LoaiGiayTo", ltl);
                        mss += "stt:" + stt + "/";
                        Command.Parameters.AddWithValue("@stt", stt);
                        mss += "sttKhoa:" + sttKhoa + "/";
                        Command.Parameters.AddWithValue("@sttKhoa", sttKhoa);
                        mss += "ngayYLenh:" + ngayYLenh + "/";
                        string nyl = ngayYLenh ?? ngayYLenh.ToString();
                        mss += "ngayYLenh:" + nyl + "/";
                        Command.Parameters.AddWithValue("@ngayYLenh", nyl);
                        mss += "ExecuteScalar:" + Command.ExecuteScalar() + "/";
                        string pdfFilePath = Command.ExecuteScalar() as string;
                        mss += "pdfFilePath:" + pdfFilePath + "/";
                        if (!string.IsNullOrEmpty(pdfFilePath))
                        {

                            string fullPdfUrl = pdfFilePath.Replace(".pdf.pdf", ".pdf");
                        }
                        WriteLog("Loi: " + " mss: " + mss);
                        return Json(new { success = true, data = pdfFilePath, status = 200 });

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " mss: " + mss);
                WriteLog("Loi: " + ex.Message.ToString() + " mss: " + mss);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        public void WriteLog(String log)
        {
            #region ghi log dang nhap
            var u = Medyx_EMR_BCA.ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            string constr = u.MongoDBConnectionString;
            var client = new MongoClient(constr);
            IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
            IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
            string MaMay = this.GetLocalIPAddress();
            TraceLogMongo emp = new TraceLogMongo();
            emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            emp.TenBang = "Log";
            emp.KieuTacDong = "Error";
            emp.NguoiSD = u.Pub_sNguoiSD;
            emp.MaMay = MaMay;
            emp.NoiDungSD = "Loi Show File Ký: " + log;
            collection.InsertOne(emp);
            #endregion
        }

        [HttpPost]
        public ActionResult Modify(DMTrangThaiKy ttk)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/DMTrangThaiKy/Index/Modify").Count() > 0;
            if (Quyen)
            {
                string MaMay = this.GetLocalIPAddress();
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                #region ghi log
                string constr = u.MongoDBConnectionString;
                var client = new MongoClient(constr);
                IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                TraceLogMongo emp = new TraceLogMongo();
                emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                emp.TenBang = "BenhAn_TrangThaiKy";
                emp.KieuTacDong = "Update";
                emp.NguoiSD = u.Pub_sNguoiSD;
                emp.MaMay = MaMay;
                emp.NoiDungSD = JsonConvert.SerializeObject(ttk);
                collection.InsertOne(emp);
                #endregion
                db.spDMTRANGTHAIKY_UPDATE(ttk.STT.ToString(), ttk.IDBA, ttk.TenBS, ttk.TrangThaiKy, ttk.LanKy.ToString(), ttk.DuongDanFile, MaMay, u.Pub_sNguoiSD, ttk.NguoiHuy, ttk.Huy);
                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền sửa trạng thái ký  !", status = 500 });

        }

        #endregion

        #region View delete trạng thái ký 

        [HttpPost]
        public ActionResult DeleteTrangThaiKy(string IDBA, string STT)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/DMTrangThaiKy/Index/Delete").Count() > 0;
            if (Quyen)
            {
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string MaMay = this.GetLocalIPAddress();
                #region ghi log
                string constr = u.MongoDBConnectionString;
                var client = new MongoClient(constr);
                IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                TraceLogMongo emp = new TraceLogMongo();
                emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                emp.TenBang = "BenhAn_TrangThaiKy";
                emp.KieuTacDong = "Delete";
                emp.NguoiSD = u.Pub_sNguoiSD;
                emp.MaMay = MaMay;
                emp.NoiDungSD = "Delete IDBA: " + IDBA;
                collection.InsertOne(emp);
                #endregion
                db.spDMTRANGTHAIKY_DELETED(STT, IDBA, MaMay, u.Pub_sNguoiSD);

                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền xóa trạng thái ký  !", status = 500 });
        }
        [HttpPost]
        public ActionResult DeleteTrangThaiKyUrl(string base64URL, string STT)
        {
            string mss = "";
            string fileName = Path.GetFileName(base64URL);
            string IDBAEMR = fileName.Replace(".pdf", "");
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/DMTrangThaiKy/Index/Delete").Count() > 0;
            if (Quyen)
            {
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string MaMay = this.GetLocalIPAddress();
                #region ghi log
                string constr = u.MongoDBConnectionString;
                var client = new MongoClient(constr);
                IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                TraceLogMongo emp = new TraceLogMongo();
                emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                emp.TenBang = "BenhAn_TrangThaiKy";
                emp.KieuTacDong = "Delete";
                emp.NguoiSD = u.Pub_sNguoiSD;
                emp.MaMay = MaMay;
                emp.NoiDungSD = "Delete IDBA: " + IDBAEMR;
                collection.InsertOne(emp);
                #endregion
                //db.spDMTRANGTHAIKY_DELETED(STT, IDBAEMR, MaMay, u.Pub_sNguoiSD);

                string dk = u.Pub_sNguoiSD;
                string ltl = "1";
                string tenStore = "spDMTrangThaiKy_Delete";
                string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    Connection.Open();
                    using (SqlCommand Command = new SqlCommand(tenStore, Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;

                        Command.Parameters.AddWithValue("@IDBA", IDBAEMR);
                        Command.Parameters.AddWithValue("@MaBS", emp.NguoiSD);
                        Command.Parameters.AddWithValue("@ltl", ltl);
                        object result = Command.ExecuteScalar();
                        string pdfFilePath = result != null ? result.ToString() : null;
                        mss += "result :" + result;
                        mss += "pdfFilePath :" + pdfFilePath;
                        WriteLog("Loi: " + mss + " + mss");
                        if (pdfFilePath == "1")

                            return Json(new { success = true, message = "Huỷ file ký thành công!", status = 200 });
                        else
                            return Json(new { success = false, message = "Huỷ trạng thái ký không thành công !", status = 500 });



                    }
                }
            }
            else
                return Json(new { success = false, message = "Không có quyền xóa trạng thái ký  !", status = 500 });

        }





        #endregion

        #region View thêm mới trangthaiKy
        [HttpPost]
        public ActionResult CreateTrangThaiKy(string maBn, string maBS, string tenBS, string trangthaiKy, string lanKy, string duongdanFile, string nguoiHuy)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/DMTrangThaiKy/Index/Create").Count() > 0;
            if (Quyen)
            {
                string MaMay = this.GetLocalIPAddress();
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                #region ghi log
                string constr = u.MongoDBConnectionString;
                var client = new MongoClient(constr);
                IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                TraceLogMongo emp = new TraceLogMongo();
                emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                emp.TenBang = "BenhAn_TrangThaiKy";
                emp.KieuTacDong = "Create";
                emp.NguoiSD = u.Pub_sNguoiSD;
                emp.MaMay = MaMay;
                emp.NoiDungSD = "Create tenBS: " + tenBS;
                collection.InsertOne(emp);
                #endregion
                db.spDMTRANGTHAIKY_CREATE(maBn, maBS, tenBS, trangthaiKy, lanKy, duongdanFile, MaMay, u.Pub_sNguoiSD, nguoiHuy);

                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới trạng thái ký !", status = 500 });
        }
        #endregion

        //#region Load danh mục trạng thái ký  get all
        //public JsonResult DMTrangThaiKyGetAll()
        //{
        //    var response = _chucDanhService.DMTrangThaiKyGetAll();
        //    return CreateJsonJsonResult(() =>
        //    {
        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    });
        //}
        //#endregion

        #region Export dữ liệu theo các cột được chọn
        [HttpGet]
        public ActionResult ExportExcel(string obj)
        {
            dynamic data = JObject.Parse(obj);
            string key = data.key;
            string columns = data.columns;
            //HttpContext.Server.ScriptTimeout = 90;
            //var dataSource = _chucVuService.ExportDMChucVu(key, columns);
            HL7CoreDataDataContext db = new HL7CoreDataDataContext();
            var dataSource = db.ExportDMTRANGTHAIKY(key, columns);
            string filename = "DMTRANGTHAIKY " + DateTime.Now.ToString() + ".xlsx";
            //MVC4 mVC4 = new MVC4();
            //mVC4.
            ExportToExcel(filename, dataSource);
            return RedirectToAction("/Index");
        }
        #endregion

        #region Import data
        [HttpPost]
        public ActionResult ImportDMTRANGTHAIKY()
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/DMTrangThaiKy/Index/ImportDMTRANGTHAIKY").Count() > 0;
            if (Quyen)
            {
                var file = Request.Form.Files[0];
                //HttpPostedFileBase fileName = (HttpPostedFileBase)Request.Form.Files[0];
                string ins = Request.Form["insert"];
                bool insert = Convert.ToBoolean(ins);
                //if (ModelState.IsValid)
                //{
                if (file != null && file.Length > 0)
                {
                    //ExcelDataReader works on binary excel file
                    Stream stream = file.OpenReadStream();
                    //We need to written the Interface.
                    IExcelDataReader reader = null;
                    if (file.FileName.EndsWith(".xls"))
                    {
                        //reads the excel file with .xls extension
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        //reads excel file with .xlsx extension
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        //Shows error if uploaded file is not Excel file
                        ModelState.AddModelError("File", "This file format is not supported");
                        return RedirectToAction("Index");
                    }
                    //treats the first row of excel file as Coluymn Names
                    reader.IsFirstRowAsColumnNames = true;
                    //Adding reader data to DataSet()
                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    result.Tables[0].TableName = "BenhAn_TrangThaiKy";
                    for (int i = 0; i < result.Tables[0].Columns.Count; i++)
                    {
                        if (i == 5)
                        {
                            DataColumn newcolumn = new DataColumn("temporary", typeof(DateTime));
                            result.Tables[0].Columns.Add(newcolumn);
                            foreach (DataRow row in result.Tables[0].Rows)
                            {
                                try
                                {
                                    row["temporary"] = Convert.ChangeType(row[i], typeof(DateTime));
                                }
                                catch
                                {
                                }
                            }
                            result.Tables[0].Columns.Remove("NgaySD");
                            newcolumn.ColumnName = "NgaySD";
                        }
                        else
                            result.Tables[0].Columns[i].DataType = typeof(String);
                    }
                    HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                    #region ghi log
                    string MaMay = this.GetLocalIPAddress();
                    string constr = u.MongoDBConnectionString;
                    var client = new MongoClient(constr);
                    IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                    IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                    TraceLogMongo emp = new TraceLogMongo();
                    emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    emp.TenBang = "BenhAn_TrangThaiKy";
                    emp.KieuTacDong = "Import";
                    emp.NguoiSD = u.Pub_sNguoiSD;
                    emp.MaMay = MaMay;
                    emp.NoiDungSD = JsonConvert.SerializeObject(result.Tables[0]);
                    collection.InsertOne(emp);
                    #endregion
                    db.ImportDMTRANGTHAIKY(result.Tables[0], insert);
                    //_chucVuService.ImportDMChucVu(result.Tables[0], insert);
                    return RedirectToAction("Index");
                }
                //}
                //else
                //{
                //    ModelState.AddModelError("File", "Please upload your file");
                //}
                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới trạng thái   ký !", status = 500 });
        }
        #endregion
    }
    [ApiController]
    [Route("api/[controller]")]
    public class PdfToBase64Controller : ControllerBase
    {
        [HttpPost("{pdfUrl}")]
        public IActionResult ConvertPdfToBase64(string pdfUrl, string IDBA)
        {
            try
            {
                // Tải dữ liệu PDF từ URL
                string pdfData;
                using (WebClient client = new WebClient())
                {
                    pdfData = "http://192.168.1.14:7000/Upload";
                }

                // Chuyển đổi dữ liệu PDF thành chuỗi Base64
                byte[] pdfData64 = System.IO.File.ReadAllBytes(pdfData);

                // Chuyển đổi dữ liệu sang dạng Base64
                string base64String = Convert.ToBase64String(pdfData64);
                string stt = "1";
                string Loaigiayto = "1";
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SpInsertfilebase64DMTrangThaiKyEMR", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@base64URL", base64String);
                        command.Parameters.AddWithValue("@Idbaemr", IDBA);
                        command.Parameters.AddWithValue("@stt", stt);
                        command.Parameters.AddWithValue("@Loaigiayto", Loaigiayto);
                        //command.Parameters.AddWithValue("@IDBAMaBa", IDBAMaBa);
                        command.ExecuteNonQuery();
                    }
                }
                // Trả về kết quả dưới dạng JSON
                return Ok(new { success = true, base64String });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi chuyển đổi PDF thành chuỗi Base64." });
            }
        }
    }

}