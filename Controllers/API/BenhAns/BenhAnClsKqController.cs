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
using System.Web.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medyx_EMR_BCA.Controllers.API.BenhAnKhoaDieuTris
{
	[Route("api/benh-an-chuan-doan-hinh-anh-ket-qua")]
	[ApiController]
	[SessionFilter]
	public class BenhAnClsKqController : ControllerBase
	{
		private IRepository<BenhAnClsKq> repository = null;
		private IRepository<BenhanClsKqcs> repositoryCS = null;
		public BenhAnClsKqController(IHttpContextAccessor accessor)
		{
			repository = new GenericRepository<BenhAnClsKq>(accessor);
            repositoryCS = new GenericRepository<BenhanClsKqcs>(accessor);
		}

		// GET: api/<BenhAnKhoaDieuTriController>
		[HttpGet]
		[SetActionContextItem(ActionType.PagedList)]
		public Response<BenhAnClsKq> Get([FromQuery] BenhanClsKqParameters parameters)
		{
			var query = repository.Table.AsQueryable();
			query = SortHelper.ApplySort(query, parameters.SortBy);
			if (parameters.Idba.HasValue)
			{
				query = query.Where(x => x.Idba == parameters.Idba);
			}
			if (parameters.Sttdv.HasValue)
			{
				query = query.Where(x => x.Sttdv == parameters.Sttdv);
			}
            query = SortHelper.ApplySort(query, parameters.SortBy);

			return Res<BenhAnClsKq>.Get(query, parameters.PageNumber, parameters.PageSize);
		}

		// GET api/<BenhAnKhoaDieuTriController>/5
		[HttpGet("{id}/chi-tiet/{stt}")]
		public BenhAnClsKq Detail(string id, string stt)
		{
			var model = repository.GetById(Convert.ToInt32(stt), Convert.ToDecimal(id));
			return model;
		}

		// POST api/<BenhAnKhoaDieuTriController>
		[HttpPost]
		public ActionResult Post([FromBody] BenhAnClsKq value)
		{
			// if (ModelState.IsValid)
            // {
            //     repository.Insert(value);
            // }
            return Ok();
		}

		// PUT api/<BenhAnKhoaDieuTriController>/5
		[HttpPut("{id}/chi-tiet/{stt}")]
		public ActionResult Put(int stt, decimal idba, [FromBody] BenhAnClsKq value)
		{
			// if (ModelState.IsValid)
            // {
            //     repository.Update(value, idba, stt);
            // }
            return Ok();
		}

		// DELETE api/<BenhAnKhoaDieuTriController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

        [HttpGet("{id}/xem-file-kq/{idhis}")]
        public ActionResult ViewFileKQ(decimal id, string Idhis)
        {
			try
			{
                var query = repository.Table.AsQueryable();
                var model = query.Where(x => x.Idba == id && x.Idhis == Idhis).Select(x => x.LinkPacsLis).FirstOrDefault();

                return Ok(new { success = true, data = model, status = 200 });
            }
			catch (Exception ex)
			{

				throw ex;
			}
			
        }

    }
}
