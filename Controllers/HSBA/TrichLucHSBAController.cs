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
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using HTC.WEB.NIOEH.Areas.Client.DanhMuc;
using System.Configuration;
using System.Data.SqlClient;
using Medyx_EMR.Models.DanhMuc;
using SharpCompress.Common;
using Medyx_EMR_BCA.Models;
using MongoDB.Driver;
namespace Medyx_EMR_BCA.Controllers.HSBA
{
    public class TrichLucHSBAController : BCAController
    {
        private readonly IConfiguration _config;
        #region Khởi tạo
        private IMemoryCache cache;
        //public readonly ISession session;
        public TrichLucHSBAController(IMemoryCache cache, IConfiguration config)
        {
            this.cache = cache;
            _config = config;
            //this.session = HttpContext.Session;
        }
        public ActionResult DeNghiTrichLuc()
        {
            return Khoitao(cache);
        }
        #endregion Khởi tạo
        #region Load danh sách benh nhan

        [HttpGet]
        public JsonResult FindHSBA(string hotenbn, string tungay, string denngay, string khoa)
        {
            //var response = _chucDanhService.DMChucDanhGetListPaging(maCD, tenCD, maMay, ngaySD, nguoiSD, pageIndex, pageSize, add);
            //return CreateJsonJsonResult(() =>
            //{
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //});
            Pagination db = new Pagination();
            if (string.IsNullOrEmpty(hotenbn))
                hotenbn = "";
            if (string.IsNullOrEmpty(tungay))
                tungay = "";
            if (string.IsNullOrEmpty(denngay))
                denngay = "";
            if (string.IsNullOrEmpty(khoa))
                khoa = "";
            var response = db.FindHSBA(hotenbn, tungay, denngay, khoa);
            return CreateJsonJsonResult(() =>
            {
                return Json(response);
            });
        }

        #endregion
        #region Load danh sách đề nghị trích lục paging

        [HttpGet]
        public JsonResult LoadData(string sophieu, string tenLoaiDeNghi, string maba, string tungay, string denngay, bool Duyet, string maMay, string ngaySD, string nguoiSD, int pageIndex, int pageSize, int add)
        {
            //var response = _chucDanhService.DMChucDanhGetListPaging(maCD, tenCD, maMay, ngaySD, nguoiSD, pageIndex, pageSize, add);
            //return CreateJsonJsonResult(() =>
            //{
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //});
            Pagination db = new Pagination();
            if (string.IsNullOrEmpty(sophieu))
                sophieu = "0";
            if (string.IsNullOrEmpty(tenLoaiDeNghi))
                tenLoaiDeNghi = "";
            if (string.IsNullOrEmpty(maba))
                maba = "";
            if (string.IsNullOrEmpty(maMay))
                maMay = "";
            if (string.IsNullOrEmpty(ngaySD))
                ngaySD = "";
            if (string.IsNullOrEmpty(nguoiSD))
                nguoiSD = "";
            var response = db.DeNghiTrichLucGetListPaging(sophieu, tenLoaiDeNghi, maba, tungay, denngay, Duyet, maMay, ngaySD, nguoiSD, pageIndex, pageSize, add);
            return CreateJsonJsonResult(() =>
            {
                return Json(response);
            });
        }

        #endregion
        #region View thêm mới đề nghị trích lục
        [HttpPost]
        public ActionResult CreateOrUpdateDN(string LoaiDeNghi, string MaBA, string HoTenNguoiDeNghi, string NgaySinhNguoiDN, string SoCMT, string NgayCapCMT, string NoiCapCMT, string QHBN, string HoTenBN, string DiaChi, string TGVQ, string MaKhoaDT, string TuNgayDT, string DenNgayDT, string LyDo, decimal sophieu, bool Duyet, string ngayduyet, bool huy, string dvct, string NhamLan, string CMNhamLan, string MatRach)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/Create").Count() > 0;
            if (Quyen)
            {
                byte loaidn = 0;
                if (!string.IsNullOrEmpty(LoaiDeNghi))
                    loaidn = byte.Parse(LoaiDeNghi);
                if (string.IsNullOrEmpty(MaBA))
                    MaBA = "";
                if (string.IsNullOrEmpty(HoTenNguoiDeNghi))
                    HoTenNguoiDeNghi = "";
                if (string.IsNullOrEmpty(NgaySinhNguoiDN))
                    NgaySinhNguoiDN = "";
                if (string.IsNullOrEmpty(SoCMT))
                    SoCMT = "";
                if (string.IsNullOrEmpty(NgayCapCMT))
                    NgayCapCMT = "";
                if (string.IsNullOrEmpty(NoiCapCMT))
                    NoiCapCMT = "";
                if (string.IsNullOrEmpty(QHBN))
                    QHBN = "";
                if (string.IsNullOrEmpty(HoTenBN))
                    HoTenBN = "";
                if (string.IsNullOrEmpty(DiaChi))
                    DiaChi = "";
                if (string.IsNullOrEmpty(TGVQ))
                    TGVQ = "";
                if (string.IsNullOrEmpty(MaKhoaDT))
                    MaKhoaDT = "";
                if (string.IsNullOrEmpty(LyDo))
                    LyDo = "";
                if (string.IsNullOrEmpty(ngayduyet))
                    ngayduyet = "";
                if (string.IsNullOrEmpty(dvct))
                    dvct = "";
                if (string.IsNullOrEmpty(NhamLan))
                    NhamLan = "";
                if (string.IsNullOrEmpty(CMNhamLan))
                    CMNhamLan = "";
                if (string.IsNullOrEmpty(MatRach))
                    MatRach = "";
                string MaMay = this.GetLocalIPAddress();
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                decimal sp = db.spBenhAn_DeNghiTrichLuc_CREATEOrUpdate(loaidn, MaBA, HoTenNguoiDeNghi, NgaySinhNguoiDN, SoCMT, NgayCapCMT, NoiCapCMT, QHBN, HoTenBN, DiaChi, TGVQ, MaKhoaDT, TuNgayDT, DenNgayDT, LyDo, MaMay, u.Pub_sNguoiSD, sophieu, Duyet, ngayduyet, huy, dvct, NhamLan, CMNhamLan, MatRach);
                return RedirectToAction("DeNghiTrichLuc");
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới !", status = 500 });
        }
        [HttpGet]
        public ActionResult CreateOrUpdateDNPrint(string LoaiDeNghi, string MaBA, string HoTenNguoiDeNghi, string NgaySinhNguoiDN, string SoCMT, string NgayCapCMT, string NoiCapCMT, string QHBN, string HoTenBN, string DiaChi, string TGVQ, string MaKhoaDT, string TuNgayDT, string DenNgayDT, string LyDo, decimal sophieu, bool Duyet, string ngayduyet, bool huy, string dvct, string NhamLan, string CMNhamLan, string MatRach)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/Create").Count() > 0;
            if (Quyen)
            {
                byte loaidn = 0;
                if (!string.IsNullOrEmpty(LoaiDeNghi))
                    loaidn = byte.Parse(LoaiDeNghi);
                if (string.IsNullOrEmpty(MaBA))
                    MaBA = "";
                if (string.IsNullOrEmpty(HoTenNguoiDeNghi))
                    HoTenNguoiDeNghi = "";
                if (string.IsNullOrEmpty(NgaySinhNguoiDN))
                    NgaySinhNguoiDN = "";
                if (string.IsNullOrEmpty(SoCMT))
                    SoCMT = "";
                if (string.IsNullOrEmpty(NgayCapCMT))
                    NgayCapCMT = "";
                if (string.IsNullOrEmpty(NoiCapCMT))
                    NoiCapCMT = "";
                if (string.IsNullOrEmpty(QHBN))
                    QHBN = "";
                if (string.IsNullOrEmpty(HoTenBN))
                    HoTenBN = "";
                if (string.IsNullOrEmpty(DiaChi))
                    DiaChi = "";
                if (string.IsNullOrEmpty(TGVQ))
                    TGVQ = "";
                if (string.IsNullOrEmpty(MaKhoaDT))
                    MaKhoaDT = "";
                if (string.IsNullOrEmpty(LyDo))
                    LyDo = "";
                if (string.IsNullOrEmpty(ngayduyet))
                    ngayduyet = "";
                if (string.IsNullOrEmpty(dvct))
                    dvct = "";
                if (string.IsNullOrEmpty(NhamLan))
                    NhamLan = "";
                if (string.IsNullOrEmpty(CMNhamLan))
                    CMNhamLan = "";
                if (string.IsNullOrEmpty(MatRach))
                    MatRach = "";
                string MaMay = this.GetLocalIPAddress();
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                decimal sp = db.spBenhAn_DeNghiTrichLuc_CREATEOrUpdate(loaidn, MaBA, HoTenNguoiDeNghi, NgaySinhNguoiDN, SoCMT, NgayCapCMT, NoiCapCMT, QHBN, HoTenBN, DiaChi, TGVQ, MaKhoaDT, TuNgayDT, DenNgayDT, LyDo, MaMay, u.Pub_sNguoiSD, sophieu, Duyet, ngayduyet == null ? "" : ngayduyet, huy, dvct, NhamLan == null ? "" : NhamLan, CMNhamLan == null ? "" : CMNhamLan, MatRach == null ? "" : MatRach);
                string reportfile = "\\Report\\CRDeNghiTrichLuc.rpt";
                string fileName = "\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "DonXinTrichLuc.pdf";
                DataTable dt = db.spBenhAn_DeNghiTrichLuc_Print(sp.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    //string filepath = 
                    try
                    {
                        return PrintPDF(reportfile, fileName, dt, _config);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Error: " + ex.ToString() + "!", status = 500 });
                    }

                    //return RedirectToAction("DeNghiTrichLuc");
                    //return Json(new { success = true, message = filepath, status = 200 });
                    //return RedirectToAction("GetReport?ReportURL=" + filepath);
                }
                else
                    return Json(new { success = false, message = "Không có dữ liệu hiển thị!", status = 500 });
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới !", status = 500 });
        }
        [HttpGet]
        public ActionResult DNPrint(string sophieulist)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/ExportExcel").Count() > 0;
            if (Quyen)
            {
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string reportfile = "\\Report\\CRDeNghiTrichLuc.rpt";
                string fileName = "\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "DonXinTrichLuc.pdf";
                DataTable dt = db.spBenhAn_DeNghiTrichLuc_Print(sophieulist);
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        //string filepath = 
                        return PrintPDF(reportfile, fileName, dt, _config);
                        //return RedirectToAction("DeNghiTrichLuc");
                        //return Json(new { success = true, message = filepath, status = 200 });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Error: " + ex.ToString(), status = 500 });
                    }

                }
                else
                    return Json(new { success = false, message = "Không có dữ liệu hiển thị!", status = 500 });
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới !", status = 500 });
        }


        [HttpGet]
        public ActionResult PrintHSBA(string sophieulist)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/ExportExcel").Count() > 0;
            if (Quyen)
            {
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string reportfile = "\\Report\\CRTrichLucHSBA.rpt";
                string fileName = "\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "TrichLucHSBA.pdf";
                DataTable dt = db.spBenhAn_DeNghiTrichLuc_PrintHSBA(sophieulist);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //string filepath = 
                    DataRow[] contains = dt.Select("LoaiDeNghi = 1");
                    if (contains != null && contains.Count() > 0)
                        return PrintZip(reportfile, fileName, dt, _config);
                    else
                        return PrintPDF(reportfile, fileName, dt, _config);
                    //return RedirectToAction("DeNghiTrichLuc");
                    //return Json(new { success = true, message = filepath, status = 200 });
                }
                else
                    return Json(new { success = false, message = "Không có dữ liệu hiển thị!", status = 500 });
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới !", status = 500 });
        }
        public DataTable GetSalePara(string loai)
        {
            DataTable dr = new DataTable();
            string tenStore = "sp_ThamSoKyBkav";
            string StrConection = ConfigurationManager.ConnectionStrings["Medyx_EMR_BCA_SQLConnectionString"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            using (SqlConnection Conection = new SqlConnection(StrConection))
            {
                Conection.Open();
                using (SqlCommand Command = new SqlCommand(tenStore, Conection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.Add(new SqlParameter("@Loai", loai));
                    SqlDataAdapter dp = new SqlDataAdapter(Command);
                    dp.Fill(dr);
                }
                return dr;
            }
        }

        //Ký file trích lục
        [HttpGet]
        public ActionResult SignHSBA(string sophieulist)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/ExportExcel").Count() > 0;
            if (Quyen)
            {
                string dk = u.Pub_sNguoiSD;
                SignPdfRequest request = new SignPdfRequest();
                DataTable signBkav = GetSalePara("KySoBKAV_" + dk);
                if (signBkav.Rows.Count > 0)
                {
                    request.CerUrl = signBkav.Rows[0]["CerUrl"].ToString();
                    request.CerPath = signBkav.Rows[0]["CerPath"].ToString();
                    request.CerPass = signBkav.Rows[0]["CerPass"].ToString();
                    request.Id = signBkav.Rows[0]["PDFId"].ToString();
                    request.Serial = signBkav.Rows[0]["Serial"].ToString();
                    request.PartnerName = signBkav.Rows[0]["PartnerName"].ToString();
                    request.FileUrl = signBkav.Rows[0]["FileUrl"].ToString();
                    request.SignatureCreator = signBkav.Rows[0]["SignatureCreator"].ToString();
                    request.Reason = signBkav.Rows[0]["Reason"].ToString();
                    request.Location = signBkav.Rows[0]["Location"].ToString();
                }
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string reportfile = "\\Report\\CRTrichLucHSBA.rpt";
                string fileName = "\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "TrichLucHSBA.pdf";

                DataTable dt = db.spBenhAn_DeNghiTrichLuc_PrintHSBA(sophieulist);
                string filePath = "";
                string maba = "";
                string loaiDeNghi = "";
                if (dt != null && dt.Rows.Count > 0)
                {
                    filePath = PrintPDFString(reportfile, fileName, dt, _config);
                    maba = dt.Rows[0]["MaBA"].ToString();
                    loaiDeNghi = dt.Rows[0]["LoaiDeNghi"].ToString();
                }
                string fileold = maba + "_" + loaiDeNghi;
                byte[] pdfData = System.IO.File.ReadAllBytes(filePath);
                var pdfSigner = new PDFSigning(pdfData, request.CerUrl, request.CerPath, request.CerPass)
                {
                    id = request.Id,
                    serial = request.Serial,
                    partnerName = request.PartnerName
                };
                if (!Directory.Exists(request.FileUrl))
                {
                    Directory.CreateDirectory(request.FileUrl);
                }
                string signedFileName = $"signed_{Guid.NewGuid()}.pdf";
                string signedFilePath = Path.Combine(request.FileUrl, signedFileName);
                //string signedFilePath = Path.Combine(Path.GetTempPath(), $"signed_{Guid.NewGuid()}.pdf");

                pdfSigner.Sign(signedFilePath, request.SignatureCreator, request.Reason, request.Location, u.Pub_sTenNguoiSD,filePath);
                spDMTrangThaiKy_Create(fileold, dk, signedFilePath, u.Pub_sTenNguoiSD);


                return Json(new { success = true, message = "", status = 200, duongDanFile = signedFilePath });
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới !", status = 500 });
        }
        [HttpGet]
        public ActionResult PrintPDFHSBA(string sophieulist)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/ExportExcel").Count() > 0;
            if (Quyen)
            {
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string reportfile = "\\Report\\CRTrichLucHSBA.rpt";
                string fileName = "\\" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "TrichLucHSBA.pdf";
                DataTable dt = db.spBenhAn_DeNghiTrichLuc_PrintHSBA(sophieulist);
                if (dt != null && dt.Rows.Count > 0)
                {

                    return PrintPDF(reportfile, fileName, dt, _config);
                }
                else
                    return Json(new { success = false, message = "Không có dữ liệu hiển thị!", status = 500 });
            }
            else
                return Json(new { success = false, message = "Không có quyền thêm mới !", status = 500 });
        }

        #region View file trích lục đã ký
        [HttpGet]
        public ActionResult ShowPdfhis(string sophieulist)
        {
            HL7CoreDataDataContext db = new HL7CoreDataDataContext();
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
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            string dk = u.Pub_sNguoiSD;
            DataTable dt = db.spBenhAn_DeNghiTrichLuc_PrintHSBA(sophieulist);
            string maba = "";
            string loaiDeNghi = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                maba = dt.Rows[0]["MaBA"].ToString();
                loaiDeNghi = dt.Rows[0]["LoaiDeNghi"].ToString();
            }
            string IDBAEMR = maba + "_" + loaiDeNghi;
            string tenStore = "spGetDMTrangThaiKy_FileDaKy";
            string connectionString = ConfigurationManager.ConnectionStrings["SqlDataProvider"].ConnectionString + "; connection timeout=600; pooling=true; Max Pool Size=6000;Timeout=600;MultipleActiveResultSets=True";
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                using (SqlCommand Command = new SqlCommand(tenStore, Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@IDBAEMR", IDBAEMR);
                    Command.Parameters.AddWithValue("@MaBS", dk);
                    Command.Parameters.AddWithValue("@LoaiGiayTo", loaiDeNghi);
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
                            message = "Giấy tờ này đã ký đủ, các bác sĩ đã ký: " + tenBSNames;
                        }
                    }
                    else if (lanky == 0)
                    {
                        message = "Giấy tờ này bạn chưa ký!";
                    }
                    else
                    {
                        message = "Giấy tờ này bạn đã ký bởi bác sĩ:" + tenBSNames + "!";
                    }

                    return Ok(new { DuongDanFile = FilePathGoc, Message = message, IsCheckKy = isCheckKy });

                }
            }

        }


        #endregion
        #endregion

        #region Huỷ ký 
        [HttpPost]
        public ActionResult DeleteTrangThaiKyUrl(string sophieulist)
        {
            string mss = "";
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            HL7CoreDataDataContext db = new HL7CoreDataDataContext();
            string MaMay = this.GetLocalIPAddress();
            DataTable dt = db.spBenhAn_DeNghiTrichLuc_PrintHSBA(sophieulist);
            string maba = "";
            string loaiDeNghi = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                maba = dt.Rows[0]["MaBA"].ToString();
                loaiDeNghi = dt.Rows[0]["LoaiDeNghi"].ToString();
            }
            string IDBAEMR = maba + "_" + loaiDeNghi;
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
                    Command.Parameters.AddWithValue("@ltl", loaiDeNghi);
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


        #endregion



        #region View delete de nghi trich luc

        [HttpPost]
        public ActionResult DeleteDN(string sophieu)
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/trichlucHSBA/DeNghiTrichLuc/Delete").Count() > 0;
            if (Quyen)
            {
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                string MaMay = this.GetLocalIPAddress();
                db.spBenhAn_DeNghiTrichLuc_DELETED(sophieu, MaMay, u.Pub_sNguoiSD);

                return RedirectToAction("DeNghiTrichLuc");
            }
            else
                return Json(new { success = false, message = "Không có quyền phiếu!", status = 500 });
        }

        #endregion 
        [HttpGet]
        public FileResult GetReport(string ReportURL)
        {
            //string ReportURL = "{Your File Path}";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
    }
}
