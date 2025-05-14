<template>
  <div>
    <el-form>
      <v-row :gutters="20">
        <v-col cols="4" class="padding-cols">
          <el-form-item label="Ngày giờ vào viện">
            <el-date-picker v-model="form.ngayVv" type="datetime" size="small" placeholder="dd/mm/yyyy hh:mm"
              style="width: 100%" format="dd/MM/yyyy HH:mm" value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="Trực tiếp vào">
            <el-select v-model="form.trucTiepVao" filterable clearable placeholder="Trực tiếp vào" style="width: 100%" size="small">
              <el-option v-for="item in trucTiepVaos" :key="item.ma" :label="item.ten" :value="item.ma">
              </el-option>
            </el-select>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="Thời gian ra viện">
            <el-date-picker v-model="form.ngayRv" type="datetime" placeholder="dd/mm/yyyy hh:mm" size="small"
              style="width: 100%" format="dd/MM/yyyy HH:mm" value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
          </el-form-item>
        </v-col>
      
      </v-row>
    </el-form>
  </div>
</template>

<script>
import { getKhoa } from "@/api/phieu-kham-benh-vao-vien";
import { getListBenhVien } from "@/api/danh-muc";
export default {
  props: ["benhAn", "benhAnKhoaDieuTri"],
  data: () => ({
    form: {
      tongSoNgayDtsauPt:null,
      ngayVv: null,
      trucTiepVao: null,
      noiGt: null,
      vvlan: null,
      maKhoaVv: null,
      ngayVaoKhoa: null,
      soNgayDt: null,
      maKhoa: null,
      chuyenVien: null,
      tenBv: null,
      ngayRv: null,
      htraVien: null,
      maBv: null,
      tongSoNgayDt: null, 
      tongsolanpt:null,
      maBvChuyenDen:null
    },

    pickerOptions: {
      disabledDate(time) {
        return time.getTime() > Date.now();
      },
    },
    noiGioiThieus: [
      {
        ma: "1",
        ten: "Cơ quan y tế",
      },
      {
        ma: "2",
        ten: "Tự đến",
      },
      {
        ma: "3",
        ten: "Khác",
      },
    ],
    trucTiepVaos: [
      {
        ma: "1",
        ten: "Y tế",
      },
      {
        ma: "2",
        ten: "Tự đến",
      },
    ],
    chuyenVien: [
      {
        ma: "1",
        ten: "Tuyến trên",
      },
      {
        ma: "2",
        ten: "Tuyến dưới",
      },
      {
        ma: "3",
        ten: "CK",
      },
    ],
    hinhThucRaVien: [
      {
        ma: "1",
        ten: "Ra viện",
      },
      {
        ma: "2",
        ten: "Chuyển viện",
      },
      {
        ma: "3",
        ten: "Trốn viện",
      },
      {
        ma: "4",
        ten: "Xin ra viện",
      },
    ],
    khoaDieuTris: [],
  }),
  computed: {
    tongSoNgayDt: function () {
      // if (
      //   this.benhAnKhoaDieuTri &&
      //   this.benhAnKhoaDieuTri[0] &&
      //   this.benhAnKhoaDieuTri[0].ngayVaoKhoa &&
      //   this.form.ngayRv
      // ) {
      //   var t2 = new Date(this.benhAnKhoaDieuTri[0].ngayVaoKhoa).getTime();
      //   var t1 = new Date(this.form.ngayRv).getTime();
      //   var tong = parseInt((t1 - t2) / (24 * 3600 * 1000) + 1)
      //   this.form.tongSoNgayDt = tong
      //   return tong;
      // }
      // return 0;
      return this.form.tongSoNgayDt;
    },
  },
  watch: {
    benhAn: function (val) {
      for (let key in this.form) {
        if (val && val.hasOwnProperty(key)) {
          this.form[key] = val[key];
        }
      }
    },
    benhAnKhoaDieuTri: function (val) {
      if (val) {
        val.forEach((item, index) => {
          let a = {};
          a.khoa = item.khoa;
          a.buong = item.buong;
          a.giuong = item.giuong;
          a.ngayVaoKhoa = item.ngayVaoKhoa;
          if (index < val.length - 1) {
            a.soNgayDt = this.gettongSoNgayDieuTri(
              val[index + 1].ngayVaoKhoa,
              item.ngayVaoKhoa
            );
          } else if (index == val.length - 1) {
            if (this.form.ngayRv) {
              a.soNgayDt = this.gettongSoNgayDieuTri(
                this.form.ngayRv,
                item.ngayVaoKhoa
              );
            }
          }
          if(index!=0){
            this.khoaDieuTris.push(a);
          }
        });
      }
    },
    form: {
      handler(val) {
        this.$emit('get-quanLyNguoibenh', val)
      },
      deep: true
    }
  },
  mounted() { },
  methods: {
    async getSelectKhoa(params) {
      return await getKhoa({ ...params, loai: 1 });
    },
    gettongSoNgayDieuTri(d1, d2) {
      var t2 = new Date(d2).getTime();
      var t1 = new Date(d1).getTime();
      return parseInt((t1 - t2) / (24 * 3600 * 1000) + 1);
    },
    async getBenhVien(params) {
      return await getListBenhVien(params);
    },
  },
};
</script>

<style>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}

label {
  margin-bottom: -6px !important;
}

.el-form-item {
  margin-bottom: 0px !important;
}

.el-form-item__label {
  font-size: 13px;
}

.el-form-item__content {
  line-height: 35px;
}
</style>
