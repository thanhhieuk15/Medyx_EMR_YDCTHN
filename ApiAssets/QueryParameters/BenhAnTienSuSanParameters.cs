using Medyx_EMR_BCA.ApiAssets.QueryParameters;

namespace Medyx_EMR.ApiAssets.QueryParameters
{
    public class BenhAnTienSuSanParameters : QueryStringParameters
    {
        public BenhAnTienSuSanParameters()
        {
        }
        public decimal? Idba { get; set; }
        public bool? getModelNull { get; set; }
    }
}
