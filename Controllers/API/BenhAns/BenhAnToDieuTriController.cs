using Medyx.ApiAssets.Models.Configure;
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
using Medyx_EMR_BCA.Models.DBConText;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Web.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using Spire.Pdf.Exporting.XPS.Schema;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Medyx_EMR_BCA.Controllers.API.BenhAnKhoaDieuTris
{
    [Route("api/benh-an-to-dieu-tri")]
    [ApiController]
    //[SessionFilter]
    public class BenhAnToDieuTriController : ControllerBase
    {
        private readonly BenhAnToDieuTriService _benhAnToDieuTriService;
        private UploadFileRespository uploadFileRespository = null;
        public BenhAnToDieuTriController(BenhAnToDieuTriService benhAnToDieuTriService)
        {
            _benhAnToDieuTriService = benhAnToDieuTriService;
            uploadFileRespository = new UploadFileRespository();
        }

		// GET: api/<BenhAnKhoaDieuTriController>
		[HttpGet]
		[SetActionContextItem(ActionType.PagedList)]
		public Response<BenhAnToDieuTriDto> Get([FromQuery] BenhAnToDieuTriParameters parameters)
		{
			var user = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserSession>(HttpContext.Session, "UserProfileSessionData");
			IQueryable<BenhAnToDieuTriDto> query = _benhAnToDieuTriService.Get(parameters, user);

			return Res<BenhAnToDieuTriDto>.Get(query, parameters.PageNumber, parameters.PageSize);
		}

		// GET api/<BenhAnKhoaDieuTriController>/5
		[HttpGet("{idba}/chi-tiet/{stt}")]
		public BenhAnToDieuTriDetailDto Detail(string idba, string stt)
		{
			return _benhAnToDieuTriService.Detail(idba, stt);
		}

        // POST api/<BenhAnKhoaDieuTriController>
        [HttpPost]
        [SetActionContextItem(ActionType.Create)]
        [SessionMiddlewareFilter("HSBA/todieutri/create")]
		public ActionResult Post([FromBody] BenhAnToDieuTriCreateVM benhAnToDieuTri)
        {
			if (ModelState.IsValid)
			{
				_benhAnToDieuTriService.Store(benhAnToDieuTri);
			}
			return Ok();
		}

		[HttpPost("sao-chep")]
		[SetActionContextItem(ActionType.Create)]
        [SessionMiddlewareFilter("HSBA/todieutri/create")]
		public ActionResult MakeCopy([FromBody] BenhAnToDieuTriSaoChepVM benhAnToDieuTri)
        {
			if (ModelState.IsValid)
			{
				_benhAnToDieuTriService.MakeCopy(benhAnToDieuTri);
			}
			return Ok();
		}

		// PUT api/<BenhAnKhoaDieuTriController>/5
		[HttpPut("{idba}/chi-tiet/{stt}")]
		[SetActionContextItem(ActionType.Update)]
        [SessionMiddlewareFilter("HSBA/todieutri/modify")]
		public ActionResult Put(decimal idba, int stt, [FromBody] BenhAnToDieuTriVM benhAnToDieuTri)
        {
			if (ModelState.IsValid)
			{
				_benhAnToDieuTriService.Update(idba, stt, benhAnToDieuTri);
			}
			return Ok();
		}

		// DELETE api/<BenhAnKhoaDieuTriController>/5
		[HttpDelete("{idba}/chi-tiet/{stt}")]
		[SetActionContextItem(ActionType.Delete)]
        [SessionMiddlewareFilter("HSBA/todieutri/delete")]
		public ActionResult Delete(decimal idba, int stt)
        {
			if (ModelState.IsValid)
			{
				_benhAnToDieuTriService.Destroy(idba, stt);
			}
			return Ok();
		}
		[HttpGet("{idba}/print-ba-file/{maba}.pdf")]
       // [SessionMiddlewareFilter("HSBA/todieutri/export")]
		public ActionResult Print(decimal idba, [FromQuery] ToDieuTriPrintVM info, [FromQuery] PrintParameters parameters)
		{
            if (ModelState.IsValid)
            {
				string path = _benhAnToDieuTriService.Print(idba, info);
                //HL7CoreDataDataContext db = new HL7CoreDataDataContext();
                //db.spFilePath_KyHSBA_Set(path, idba.ToString(), "Tờ điều trị");
                if (parameters.ShouldReturnPath)
                	return new JsonResult(new { path });
				DownloadFileResult downloadFileResult = uploadFileRespository.Download(path, true, true);
				return File(downloadFileResult.FileBytes, downloadFileResult.contentType);
            }
			return Ok();
        }
        //[HttpGet("KyHSBA")]
        ////public ActionResult KyHSBA()
        ////{
            
        ////    HL7CoreDataDataContext db = new HL7CoreDataDataContext();
        ////    //string ip = db.GetIp();
        ////    var ff = db.spFilePath_KyHSBA_Get();
        ////    if (ff.Length>0)
        ////    {

        ////        //var fileName = ff.Replace("/","\\\\");
        ////        string fileName = "D:\\EMR\\Storage\\Print\\202311301059333595.pdf";
        ////        //FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
        ////        //return File(fs, "application/pdf");
        ////        byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);

        ////        //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        ////        DownloadFileResult downloadFileResult = uploadFileRespository.Download(fileName, true, true);
        ////        return File(fileBytes, "application/pdf");
        ////    }
        ////    else return Ok();
        ////}
        
        //public void spFilePath_KyHSBA_Set(string Path, string ip, string idba, string loaiBA)
        //{
        //    DataTable dr = new DataTable();
        //    var conn = System.Configuration.ConfigurationManager.ConnectionStrings["Medyx_EMR_BCA_SQLConnectionString"].ConnectionString;
        //    string tenStore = "spFilePath_KyHSBA_Set";
        //    using (SqlConnection Conection = new SqlConnection(conn))
        //    {
        //        Conection.Open();
        //        using (SqlCommand Command = new SqlCommand(tenStore, Conection))
        //        {
        //            Command.CommandType = CommandType.StoredProcedure;
        //            Command.Parameters.Add(new SqlParameter("@Path", Path));
        //            Command.Parameters.Add(new SqlParameter("@ip", ip));
        //            Command.Parameters.Add(new SqlParameter("@idba", idba));
        //            Command.Parameters.Add(new SqlParameter("@loaiBA", loaiBA));
        //            SqlDataAdapter dp = new SqlDataAdapter(Command);
        //            dp.Fill(dr);
        //        }
        //        //if (dr.Rows.Count > 0)

        //        //return dr;
        //        //else
        //        //    return "";
        //    }
        //}
        //public string spFilePath_KyHSBA_Get(string ip)
        //{
        //    DataTable dr = new DataTable();
        //    var conn = System.Configuration.ConfigurationManager.ConnectionStrings["Medyx_EMR_BCA_SQLConnectionString"].ConnectionString;
        //    string tenStore = "spFilePath_KyHSBA_Get";
        //    using (SqlConnection Conection = new SqlConnection(conn))
        //    {
        //        Conection.Open();
        //        using (SqlCommand Command = new SqlCommand(tenStore, Conection))
        //        {
        //            Command.CommandType = CommandType.StoredProcedure;
        //            Command.Parameters.Add(new SqlParameter("@ip", ip));
        //            SqlDataAdapter dp = new SqlDataAdapter(Command);
        //            dp.Fill(dr);
        //        }
        //        if (dr.Rows.Count > 0)
        //            return dr.Rows[0][0].ToString();
        //        //return dr;
        //        else
        //            return "";
        //    }
        //}
    }
}
