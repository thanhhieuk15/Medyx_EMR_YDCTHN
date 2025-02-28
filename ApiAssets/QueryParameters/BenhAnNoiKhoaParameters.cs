using System;

namespace Medyx_EMR_BCA.ApiAssets.QueryParameters
{
    public class BenhAnNoiKhoaParameters : QueryStringParameters
    {
        public BenhAnNoiKhoaParameters()
        {
            SortBy = "Stt";
        }
        public DateTime? NgayGioVaoKhoa { get; set; }
        public decimal? Idba { get; set; }
        public DateTime? NgayYlenh { get; set;}
    }
}
