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

namespace HTC.WEB.NIOEH.Areas.Client.DanhMuc
{
    public class DMTrangThaiKyController : BCAController
    {
        #region Khởi tạo
        private IMemoryCache cache;
        private readonly IConfiguration _config;
        //public readonly ISession session;
        public DMTrangThaiKyController(IMemoryCache cache, IConfiguration config)
        {
            this.cache = cache;
            _config = config;
            //this.session = HttpContext.Session;
        }
         private void SaveToDatabase(string base64URL, decimal Idbaemr, string stt ,string Loaigiayto)
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
        public JsonResult LoadData(int stt, string idBA, string maBN, string maBS,string tenBS , string trangthaiKy, int  lanKy, string duongdanFile, string maMay, string ngayLap, string ngaySD, string nguoiSD, string ngayHuy, string nguoiHuy, int pageIndex, int pageSize, int add)
        {
            //var response = _chucDanhService.DMTrangThaiKyGetListPaging(maCD, tenCD, maMay, ngaySD, nguoiSD, pageIndex, pageSize, add);
            //return CreateJsonJsonResult(() =>
            //{
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //});
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
            var response = db.DMTrangThaiKyGetListPaging(stt,idBA, maBN, maBS,tenBS, trangthaiKy,lanKy, duongdanFile, maMay, ngayLap, ngaySD, nguoiSD, ngayHuy, nguoiHuy, pageIndex, pageSize, add);
            return CreateJsonJsonResult(() =>
            {
                return Json(response);
            });
        }
        [HttpPost]
        public ActionResult getSignFilePath(string path)
        {
            //var response = _chucDanhService.DMTrangThaiKyGetListPaging(maCD, tenCD, maMay, ngaySD, nguoiSD, pageIndex, pageSize, add);
            //return CreateJsonJsonResult(() =>
            //{
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //});
            if (path.Length == 0)
            {
                return Json(new { success = false, message = "IDBA không thể rỗng! Không tìm thấy file", status = 500 });
                //return "IDBA không thể rỗng! Không tìm thấy file";
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
                        //string logFileName = response;
                        //string directory = AppDomain.CurrentDomain.BaseDirectory;
                        string logFilePath = System.IO.Path.Combine(webRootPath, response);
                        mss += "logFilePath:" + logFilePath + "/";
                        string config = System.IO.File.ReadAllText(logFilePath);
                        byte[] configBytes = Encoding.UTF8.GetBytes(config);
                        string base64String = Convert.ToBase64String(configBytes);
                        //Byte[] configBytes = System.IO.File.ReadAllBytes(logFilePath);
                        //String base64String = Convert.ToBase64String(configBytes);
                        mss += "base64String:" + base64String + "/";
                        return CreateJsonJsonResult(() =>
                        {
                            //return Json(base64String);
                            //return Json(response);
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
                        emp.TenBang = "Trang thai ky";
                        emp.KieuTacDong = "get base 64";
                        emp.NguoiSD = "admin";
                        emp.MaMay = MaMay;
                        emp.NoiDungSD = mss;
                        collection.InsertOne(emp);
                        #endregion
                        return Json(new { success = false, message = "Không tìm thấy file", status = 404 });
                    }
                    //return base64String;
                    //return "file ok";
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy file", status = 500 });
                    //return "Không tìm thấy file";
                }
            }
        }
        //public JsonResult getValueString(string value)
        //{
        //    if (value.Length == 0)
        //    {
        //        return Json(new { success = false, message = "", status = 500 });
        //    }
        //}
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
                string IDBAEMR = fileName.Replace(".pdf","");
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
                db.spDMTRANGTHAIKY_UPDATE(ttk.STT.ToString(),ttk.IDBA,ttk.TenBS,ttk.TrangThaiKy,ttk.LanKy.ToString(),ttk.DuongDanFile, MaMay, u.Pub_sNguoiSD,ttk.NguoiHuy,ttk.Huy);
                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền sửa trạng thái ký  !", status = 500 });

        }

        #endregion 
          
        #region View delete trạng thái ký 

        [HttpPost]
        public ActionResult DeleteTrangThaiKy(string IDBA,string STT)
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
                db.spDMTRANGTHAIKY_DELETED(STT,IDBA, MaMay,u.Pub_sNguoiSD);

                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền xóa trạng thái ký  !", status = 500 });
        }

        #endregion 

        #region View thêm mới trangthaiKy
        [HttpPost]
        public ActionResult CreateTrangThaiKy(string maBn,string maBS,string tenBS,string trangthaiKy,string lanKy,string duongdanFile,string nguoiHuy)
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
                db.spDMTRANGTHAIKY_CREATE(maBn, maBS, tenBS, trangthaiKy,lanKy,duongdanFile, MaMay, u.Pub_sNguoiSD,nguoiHuy);

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
        public IActionResult ConvertPdfToBase64(string pdfUrl,string IDBA)
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