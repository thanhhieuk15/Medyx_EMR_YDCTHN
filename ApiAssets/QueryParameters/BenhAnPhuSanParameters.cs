using Medyx_EMR_BCA.ApiAssets.QueryParameters;
using System;

namespace Medyx_EMR.ApiAssets.QueryParameters
{
    public class BenhAnPhuSanParameters : QueryStringParameters
    {
        public BenhAnPhuSanParameters()
        {
        }
        public decimal? Idba { get; set; }
        public bool? getModelNull { get; set; }

    }
}
