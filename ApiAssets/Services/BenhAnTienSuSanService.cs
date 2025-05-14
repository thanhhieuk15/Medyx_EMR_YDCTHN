using Medyx.ApiAssets.Models.Configure;
using Medyx_EMR.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.Helpers;
using Medyx_EMR_BCA.ApiAssets.Models;
using Medyx_EMR_BCA.ApiAssets.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Medyx_EMR.ApiAssets.Services
{
    public class BenhAnTienSuSanService
    {
        private IRepository<BenhAnTienSuSan> _benhAnTienSuSanRepository = null;

        private PrintSetting PrintSetting { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;
        public BenhAnTienSuSanService(IHttpContextAccessor accessor, IOptions<PrintSetting> options = null, IHostingEnvironment hostingEnvironment = null)
        {
            _benhAnTienSuSanRepository = new GenericRepository<BenhAnTienSuSan>(accessor);
            PrintSetting = options != null ? options.Value : new PrintSetting();
            _hostingEnvironment = hostingEnvironment;
        }
        public void Destroy(decimal idba, int stt)
        {
            PermissionThrowHelper.GetBenhAnAndCheckPermission(idba);
            _benhAnTienSuSanRepository.Delete(idba, stt);
        }
    }
}
