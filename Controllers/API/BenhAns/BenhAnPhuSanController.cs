using Medyx_EMR.ApiAssets.Models;
using Medyx_EMR.ApiAssets.QueryParameters;
using Medyx_EMR.ApiAssets.Services;
using Medyx_EMR_BCA.ApiAssets.AttributeCustom;
using Medyx_EMR_BCA.ApiAssets.Enums;
using Medyx_EMR_BCA.ApiAssets.Helpers;
using Medyx_EMR_BCA.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.Models.Session;
using Medyx_EMR_BCA.ApiAssets.QueryParameters;
using Medyx_EMR_BCA.ApiAssets.Repository;
using Medyx_EMR_BCA.ApiAssets.ResponseHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Medyx_EMR.Controllers.API.BenhAns
{

    [Route("api/benh-an-phu-san")]
    [ApiController]
    //[SessionFilter]
    public class BenhAnPhuSanController : ControllerBase
    {
        private readonly BenhAnPhuSanService _benhAnPhuSanService;
        private IRepository<BenhAnPhuSan> repository = null;
        private UploadFileRespository _uploadFileRespository = null;
        public BenhAnPhuSanController(IHttpContextAccessor accessor, BenhAnPhuSanService benhAnPhuSanService, UploadFileRespository uploadFileRespository = null)
        {
            _benhAnPhuSanService = benhAnPhuSanService;
            _uploadFileRespository = uploadFileRespository;
            repository = new GenericRepository<BenhAnPhuSan>(accessor);
        }

        // GET: api/<BenhAnPhuSanController>
        [HttpGet]
        [SetActionContextItem(ActionType.PagedList)]
        public Response<BenhAnPhuSan> Get([FromQuery] BenhAnPhuSanParameters parameters)
        {
            var user = SessionHelper.GetObjectFromJson<UserSession>(HttpContext.Session, "UserProfileSessionData");
            IQueryable<BenhAnPhuSan> query = _benhAnPhuSanService.Get(parameters, user);

            return Res<BenhAnPhuSan>.Get(query, parameters.PageNumber, parameters.PageSize);
        }
        // GET api/<BenhAnPhuSanController>/5
        [HttpGet("{Idba}")]
        public BenhAnPhuSan Detail(string Idba, [FromQuery] BenhAnPhuSanParameters parameters)
        {
            var model = repository.GetById(Convert.ToDecimal(Idba));
            if (Convert.ToBoolean(parameters.getModelNull) && model == null)
            {
                model = new BenhAnPhuSan();
            }
            return model;
        }

        // POST api/<BenhAnKhoaDieuTriController>
        [HttpPost]
        public ActionResult Post([FromBody] BenhAnPhuSan Info)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(Info);
            }
            return Ok();
        }

    }
}
