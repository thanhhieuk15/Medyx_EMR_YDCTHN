using System.Linq;
using Medyx_EMR_BCA.Models.DanhMuc;
using Excel;
using MongoDB.Driver;
using Medyx_EMR_BCA.Models;
using Newtonsoft.Json;
using Medyx_EMR_BCA.Models.DBConText;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;

namespace Medyx_EMR.Controllers.KiSo
{
    public class KiSoController : BCAController
    {
        #region Khởi tạo
        // GET: Client/DMBA_DM_Tinh
        private IMemoryCache cache;
        //public readonly ISession session;
        public KiSoController(IMemoryCache cache)
        {
            this.cache = cache;
            //this.session = HttpContext.Session;

        }
        public ActionResult Index()
        {
            return Khoitao(cache);
        }
        #endregion Khởi tạo
    }
}
