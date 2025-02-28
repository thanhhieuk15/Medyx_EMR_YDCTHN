using System.Collections.Generic;
namespace Medyx_EMR_BCA.ApiAssets.Models
{
    public partial class DmbenhTatYhct
    {
        public string Sttchuong { get; set; }
        public string MaChuong { get; set; }
        public string TenChuong { get; set; }
        public int? Stt { get; set; }
        public string MaBenh { get; set; }
        public string TenBenh { get; set; }
        public string MaBenhIcd { get; set; }
        public string TenBenhIcd { get; set; }
        public string TenBenhBhyt { get; set; }
        public virtual ICollection<BenhAn> BenhNoiChuyenDenYHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKKBYHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhChinhVVYHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV1YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV2YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV3YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV4YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV5YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV6YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV7YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV8YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV9YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV10YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV11YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemVV12YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhChinhRVYHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV1YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV2YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV3YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV4YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV5YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV6YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV7YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV8YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV9YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV10YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV11YHCTs { get; set; }
        public virtual ICollection<BenhAn> BenhKemRV12YHCTs { get; set; }
        public virtual ICollection<BenhAnKhamYhct> BenhAnKhamYhcts { get; set; }
    }
}
