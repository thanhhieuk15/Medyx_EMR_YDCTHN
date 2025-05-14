using Medyx_EMR_BCA.ApiAssets.Dto;
using Medyx_EMR_BCA.ApiAssets.Helpers;
using Medyx_EMR_BCA.ApiAssets.Models.Session;
using Medyx_EMR_BCA.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.QueryParameters;
using Medyx_EMR_BCA.ApiAssets.Repository;
using System.Linq;
using System;
using Medyx.ApiAssets.Models.Configure;
using Medyx_EMR_BCA.ApiAssets.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Medyx_EMR.ApiAssets.Models;
using Medyx_EMR.ApiAssets.QueryParameters;

namespace Medyx_EMR.ApiAssets.Services
{
    public class BenhAnPhuSanService
    {
        private IRepository<BenhAnPhuSan> repository = null;
        private IHttpContextAccessor _accessor { get; set; }
        public BenhAnPhuSanService(IHostingEnvironment hostingEnvironment = null, IOptions<PrintSetting> options = null, IHttpContextAccessor accessor = null, UploadFileRespository uploadFileRespository = null)
        {
            repository = new GenericRepository<BenhAnPhuSan>(accessor);
            _accessor = accessor;
           
        }
        private IQueryable<BenhAnPhuSan> QueryFilter(IQueryable<BenhAnPhuSan> query, BenhAnPhuSanParameters parameters)
        {
            var querys = query.ToList();
            if (!string.IsNullOrEmpty(parameters.Search))
            {
            }
          
            if (parameters.Idba.HasValue)
            {
                query = query.Where(x => x.Idba == parameters.Idba);
            }
           
            query = SortHelper.ApplySort(query, parameters.SortBy);
            return query;
        }
        public IQueryable<BenhAnPhuSan> Get(BenhAnPhuSanParameters parameters, UserSession user = null)
        {
            var query = repository.Table.Where(x => x.Idba == parameters.Idba);

            if (user == null || !Convert.ToBoolean(user.Pub_bQadmin))
            {
                query = query.Where(x => !Convert.ToBoolean(x.Huy));
            }

            query = QueryFilter(query, parameters);
            return query;
        }
    }
}
