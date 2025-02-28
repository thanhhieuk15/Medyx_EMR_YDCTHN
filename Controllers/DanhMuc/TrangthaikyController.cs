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

namespace HTC.WEB.NIOEH.Areas.Client.DanhMuc
{
    public class TrangThaiKyController : BCAController
    {   
        #region Khởi tạo
        // GET: Client/DMBA_TrangThaiKy
        private IMemoryCache cache;
        //public readonly ISession session;
        public TrangThaiKyController(IMemoryCache cache)
        {
            this.cache = cache;
            //this.session = HttpContext.Session;

        }
        public ActionResult Index()
        {
            return Khoitao(cache);
        }
        #endregion Khởi tạo
        #region Load danh sách chức vụ paging
        [HttpGet]
        //public ActionResult LoadData(string MaBN, string MaBS, string TenBS, string TrangThaiKy,int LanKy,string DuongDanFile, string MaMay, string NgayLap, string NgaySD, int NguoiSD, string NgayHuy, string NguoiHuy, string Huy)
        //{
        //    Pagination db = new Pagination();
        //    if (string.IsNullOrEmpty(MaTinh))
        //        MaTinh = "";
        //    if (string.IsNullOrEmpty(MaQU))
        //        MaQU = "";
        //    if (string.IsNullOrEmpty(MaVungLT))
        //        MaVungLT = "";
        //    if (string.IsNullOrEmpty(TenTinh))
        //        TenTinh = "";
        //    if (string.IsNullOrEmpty(MaBHYT))
        //        MaBHYT = "";
        //    if (string.IsNullOrEmpty(MaMay))
        //        MaMay = "";
        //    if (string.IsNullOrEmpty(Huy))
        //        Huy = "";
        //    if (string.IsNullOrEmpty(ngaySD))
        //        ngaySD = "";
        //    if (string.IsNullOrEmpty(nguoiSD))
        //        nguoiSD = "";
        //    if (string.IsNullOrEmpty(Matat))
        //        Matat = "";
        //    //var response = db.TrangThaiKyGetListPaging(MaTinh, MaQU,MaMay, MaVungLT,TenTinh,MaBHYT,Huy, ngaySD, nguoiSD, index, pageSize, add,  Matat);

        //    //return CreateJsonJsonResult(() =>
        //    //{
        //    //    //return Json(response);
        //    //});
        //}
        #endregion

        #region View thực hiện update Tỉnh

        [HttpPost]
        public ActionResult Modify()
        {
            var u = SessionHelper.GetObjectFromJson<UserProfileSessionData>(HttpContext.Session, "UserProfileSessionData");
            var Quyen = u.ListRoleSession.Where(x => x.ActionDetailsName == "/TrangThaiKy/Index/Modify").Count() > 0;
            if (Quyen)
            {
                string MaVungLT = this.GetLocalIPAddress();
                HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                #region ghi log
                string constr = u.MongoDBConnectionString;
                var client = new MongoClient(constr);
                IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
                IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
                TraceLogMongo emp = new TraceLogMongo();
                emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                emp.TenBang = "DMTinh";
                emp.KieuTacDong = "Update";
                emp.NguoiSD = u.Pub_sNguoiSD;
                //emp.NoiDungSD = JsonConvert.SerializeObject(TrangThaiKy);
                collection.InsertOne(emp);
                #endregion
                string MaMay = this.GetLocalIPAddress();
                //db.spTrangThaiKy_UPDATE(TrangThaiKy.MaTinh, TrangThaiKy.MaQU, TrangThaiKy.MaVungLT, TrangThaiKy.TenTinh, "", MaMay, u.Pub_sNguoiSD, TrangThaiKy.Huy);
                return RedirectToAction("Index");
            }
            else
                return Json(new { success = false, message = "Không có quyền sửa chức vụ!", status = 500 });

        }

        #endregion 
    }
}