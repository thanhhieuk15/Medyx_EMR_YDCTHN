using CrystalDecisions.Shared;
using Medyx_EMR.ApiAssets.Models;
using Medyx_EMR.ApiAssets.QueryParameters;
using Medyx_EMR.ApiAssets.Services;
using Medyx_EMR_BCA.ApiAssets.AttributeCustom;
using Medyx_EMR_BCA.ApiAssets.Enums;
using Medyx_EMR_BCA.ApiAssets.Helpers;
using Medyx_EMR_BCA.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.QueryParameters;
using Medyx_EMR_BCA.ApiAssets.Repository;
using Medyx_EMR_BCA.ApiAssets.ResponseHandler;
using Medyx_EMR_BCA.ApiAssets.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.WebRequestMethods;

namespace Medyx_EMR.Controllers.API.BenhAns
{
    [Route("api/benh-an-tien-su-san")]
    [ApiController]
    [SessionFilter]
    public class BenhAnTienSuSanController : ControllerBase
    {
        private IRepository<BenhAnTienSuSan> repository = null;
        private BenhAnTienSuSanService _benhAnTienSuSanService;
        public BenhAnTienSuSanController(IHttpContextAccessor accessor, BenhAnTienSuSanService benhAnTienSuSanService)
        {
            repository = new GenericRepository<BenhAnTienSuSan>(accessor);
            _benhAnTienSuSanService = benhAnTienSuSanService;
        }
        // GET: api/<BenhAnTienSuSanController>
        [HttpGet]
        [SetActionContextItem(ActionType.PagedList)]
        public Response<BenhAnTienSuSan> Get([FromQuery] BenhAnTienSuSanParameters parameters)
        {
            var query = repository.Table.AsQueryable();
            if (parameters.Idba.HasValue)
            {
                query = query.Where(x => x.Idba == parameters.Idba);
            }
            query = SortHelper.ApplySort(query, parameters.SortBy);

            return Res<BenhAnTienSuSan>.Get(query, parameters.PageNumber, parameters.PageSize);
        }
        // GET api/<BenhAnKhoaDieuTriController>/5
        [HttpGet("{Idba}")]
        public List<BenhAnTienSuSan> Detail(string Idba, [FromQuery] BenhAnTienSuSanParameters parameters)
        {
            List<BenhAnTienSuSan> model = repository.Table.Where(x => x.Idba == Convert.ToDecimal(Idba)).ToList();
            if (Convert.ToBoolean(parameters.getModelNull) && model == null)
            {
                model = new List<BenhAnTienSuSan>();
            }
            return model;
        }

        // POST api/<BenhAnKhoaDieuTriController>
        [HttpPost]
        [SetActionContextItem(ActionType.Create)]
        public ActionResult Post([FromBody] BenhAnTienSuSan Info)
        {
            if (ModelState.IsValid)
            {
                var list = repository.Table.Where(x => x.Idba == Convert.ToDecimal(Info.Idba)).ToList();
                var maxSTT = list.Any() ? list.Max(x => x.STT) : 0;
                int newSTT = maxSTT + 1;
                Info.STT = newSTT;
                repository.Insert(Info);
            }
            return Ok();
        }

        // PUT api/<BenhAnKhoaDieuTriController>/5
        [HttpPut("{id}/cap-nhat-tien-su-san/{stt}")]
        [SetActionContextItem(ActionType.Update)]
        public ActionResult Put(decimal id, int stt, [FromBody] BenhAnTienSuSan Info)
        {
            if (ModelState.IsValid)
            {
                repository.Update(Info, id, stt );
            }
            return Ok();
        }

        // DELETE api/<BenhAnKhoaDieuTriController>/5
        [HttpDelete("{id}/xoa-tien-su-san/{stt}")]
        [SetActionContextItem(ActionType.Delete)]
        public ActionResult Delete(decimal id, int stt)
        {

            if (ModelState.IsValid)
            {
                _benhAnTienSuSanService.Destroy(id, stt);
            }
            return Ok();

        }


    }
}
