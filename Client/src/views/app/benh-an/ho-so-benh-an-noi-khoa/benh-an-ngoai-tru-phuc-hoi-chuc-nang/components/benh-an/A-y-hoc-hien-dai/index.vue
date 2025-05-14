<template>
  <div>
    <el-form :model="form" :rules="rules" ref="form">
      <v-row>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="I. LÝ DO VÀO VIỆN">
            <el-input v-model="form.lyDoVv" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="II. Hỏi bệnh">
            <div>1. Quá trình bệnh lý:(khởi phát, diễn biến, chẩn đoán, điều trị của tuyến dưới v.v ).</div>
            <el-input v-model="form.benhSu" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="2.Tiền sử bệnh"></el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Dị ứng">
            <el-input v-model="form.moTaTienSu" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Bản Thân(chú ý các yếu tố liên quan đến khuyết tật nếu có): ">
            <el-input v-model="form.tienSuBanThan" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Gia đình">
            <el-input v-model="form.tienSuGiaDinh" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="IV. KHÁM BỆNH"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item>
            <div>
              <div style="font-weight: 500">1.Toàn thân:</div>
            </div>
            <el-input v-model="form.toanThan" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12">
          <v-row>
            <v-col cols="3" class="padding-cols">
              <el-form-item label="Mạch (lần/phút):" prop="mach">
                <el-input type="number" v-model="form.mach" size="mini" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="3" class="padding-cols">
              <el-form-item label="Nhiệt độ (℃):">
                <el-input type="number" v-model="form.nhietDo" size="mini" :step="0.01" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="3" class="padding-cols">
              <el-form-item label="Huyết áp (mmHg):">
                <el-input v-model="form.huyetAp" size="mini" style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="3" class="padding-cols">
              <el-form-item label="Nhịp thở (lần/phút):">
                <el-input type="number" v-model="form.nhipTho" size="mini" controls-position="right"
                  style="width: 100%">
                </el-input>
              </el-form-item>
            </v-col>
            <v-col cols="3" class="padding-cols">
              <el-form-item label="Cân Nặng (kg):">
                <el-input type="number" v-model="form.canNang" size="mini" :step="0.01" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="12" class="padding-cols">
              <el-form-item label="2.Các bộ phận khác">
                <el-input v-model="form.spO2" type="textarea"></el-input>
              </el-form-item>
            </v-col>
          </v-row>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="3. Tóm tắt kết quả cận lâm sàng">
            <el-input v-model="form.canLamSang" type="textarea" rows="6"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="4. Chuẩn đoán ban đầu"></el-form-item>
        </v-col>
        <v-col cols="12">
          <b>Bệnh chính</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhChinhVv" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
              @get-item="getTenBenhChinhVv" :firstEmitGetItem="false">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhChinhVv" size="small"></el-input>
          </el-form-item>
        </v-col>
        <!-- <v-col cols="6">
          <b>Bệnh kèm theo 1</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv1" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
              @get-item="getTenBenhKem1Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv1" size="small"></el-input>
          </el-form-item>
        </v-col> -->
        <v-col cols="12" class="padding-cols">
          <el-form-item label="6. Đã xử lý(thuốc, chăm sóc)">
            <el-input v-model="form.tuanHoan" type="textarea" rows="6"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols mb-2 pt-3">
          <div style="font-weight: 600" class="pb-1">6. Chuẩn đoán khi ra viện</div>
          <hr />
        </v-col>
        <v-col cols="12">
          <b>Bệnh chính</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhChinhRv" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
              @get-item="getTenBenhChinhRv" :firstEmitGetItem="false">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhChinhRv" size="small" placeholder="Tên bệnh"></el-input>
          </el-form-item>
        </v-col>
        <!-- <v-col cols="6">
          <b>Bệnh kèm 1</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form2.maBenhKemRv1" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
              @get-item="getTenBenhKemRv1" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form2.tenBenhKemRv1" size="small" placeholder="Tên bệnh"></el-input>
          </el-form-item>
        </v-col> -->
        <v-col cols="6" class="padding-cols">
          <el-form-item label="7.Điều trị ngoại trú từ ngày ">
            <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm"
              v-model="form.ngayHenKhamLai"  size="small" style="width: 100%"></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Đến ngày ">
            <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm"
              v-model="form.ngayHenXNLai" size="small" style="width: 100%"></el-date-picker>
          </el-form-item>
        </v-col>
      </v-row>
    </el-form>
  </div>
</template>

<script>
import * as apiBenhTat from "@/api/benh-tat.js";
import store from '@/store'
import { getSelectBox } from '@/api/danh-muc.js';
import { chiTietToBenhAn, getTienSuBenh, getKhamYhhd, getTongKetBenhAn, getKhoaDieuTri, getKhamYhct } from "@/api/benh-an.js";
import { getChiTietPhieuKhamVaoVien } from "@/api/phieu-kham-benh-vao-vien";
import {
  uploadFile,
  getListFile,
  downloadFile,
  deleteFile,
} from "@/api/file-phi-cau-truc.js";
import { saveAs } from "file-saver";
export default {
  data() {
    var checkMach = (rule, value, callback) => {
      if (!value) {
        return callback(new Error("Phải nhập mạch"));
      }
      setTimeout(() => {
        if (value > 226 || value < 0) {
          callback(new Error("Giá trị nhập lớn hơn 0 và nhỏ hơn 226"));
        } else {
          callback();
        }
      }, 1000);
    };
    return {
      tienTrinhUpload: 0,
      loading: false,
      files: null,
      form: {
        benhSu: null,
        ngayHenKhamLai: null,
        ngayHenXNLai: null,
        vaoNgayThu: null,
        spO2: null,
        bmi: null,
        canLamSang: null,
        trieuChung: null,
        canNang: null,
        cdchamSoc: null,
        cddinhDuong: null,
        chieuCao: null,
        dtketHopYhhd: null,
        hoHap: null,
        huyetAp: null,
        idba: null,
        kqcdha: null,
        kqdt: null,
        kqxnmau: null,
        kqxnnuocTieu: null,
        lyDoVv: null,
        mach: null,
        mat: null,
        nhietDo: null,
        nhipTho: null,
        noiTietDd: null,
        rangHamMat: null,
        taiMuiHong: null,
        tenBenhPhanBiet: null,
        thanKinh: null,
        thanTnieuSduc: null,
        tieuHoa: null,
        toanThan: null,
        tomTatBenhAn: null,
        tuanHoan: null,
        benhNgoaikhoa: null,
        xuongKhop: null,
        maTienSu: null,
        moTaTienSu: null,
        tienSuBanThan: null,
        tienSuGiaDinh: null,
        dacDiemLienQuanBenh: null,
        maBenhChinhVv: null,
        tenBenhChinhVv:null,
        maBenhKemVv1: null,
        maBenhChinhRv:null,
        tenBenhChinhRv:null,
        benhChinh: {
          maBenh: null,
          tenBenh: null,
        },
        benhKem1: {
          maBenh: null,
          tenBenh: null,
        },
        benhKem2: {
          maBenh: null,
          tenBenh: null,
        },
        benhKem3: {
          maBenh: null,
          tenBenh: null,
        },
      },
      benh_khac: [
        {
          ten_benh: null,
          ma_benh: null,
        },
      ],
      tienSuBenhAn: [
        { ten: "Dị ứng", checkbox: false },
        { ten: "Ma Túy", checkbox: false },
        { ten: "Rượu Bia", checkbox: false },
        { ten: "Thuốc lá", checkbox: false },
        { ten: "Thuốc Lào", checkbox: false },
      ],
      tienSuBenhAnNew: [
        { ten: "Dị ứng", checkbox: false },
        { ten: "Ma Túy", checkbox: false },
        { ten: "Rượu Bia", checkbox: false },
        { ten: "Thuốc lá", checkbox: false },
        { ten: "Thuốc Lào", checkbox: false },
      ],
      rules: {
        mach: [
          {
            validator: checkMach,
            trigger: "blur",
          },
        ],
      }
    }
  },
  mounted() {
    this.getData();
    this.getDsFile();
  },
  watch: {
    form: {
      handler(val) {
        this.$emit('get-phanA', val)
        store.dispatch("hosobenhan/setChanDoanYHHD", val)
      },
      deep: true
    },
    "form.canNang": function (val) {
      if (val && this.form.chieuCao) {
        this.form.bmi = (val / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
      }
    },
    "form.chieuCao": function (val) {
      if (this.form.canNang && val) {
        this.form.bmi = (this.form.canNang / ((val / 100) * (val / 100))).toFixed(2)
      }
    },
    CHANDOAN: {
      handler(val) {
        // console.log(22222, val)
        for (let key in this.form) {
          if (val && val.hasOwnProperty(key)) {
            this.form[key] = val[key]
          }
        }
      },
      deep: true
    }
  },
  computed: {
    CHANDOAN() {
      return this.$store.state.hosobenhan.dataChanDoan
    },
  },
  methods: {
    async getChanDoanBenh(params) {
      return await apiBenhTat.index(params);
    },
    getTenBenhChinhRv(item) {
      if (item) {
        this.form.tenBenhChinhRv = item.tenBenh;
      }
      else {
        this.form.tenBenhChinhRv = null
      }
    },
    getTenBenhChinhVv(item) {
      if (item) {
        this.form.tenBenhChinhVv = item.tenBenh;
      }
      else {
        this.form.tenBenhChinhVv = null
      }
    },
    async getData() {
      const id = window.location.href.split("/").at(-1);
      let data = await getKhamYhhd(id);
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key]
        }
      }
      data = await chiTietToBenhAn(id);
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key]
        }
      }
      data = await getTienSuBenh(id, { getModelNull: true })
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key]
        }
      }
      data = await getSelectBox({ MaParent: '017' });
      this.tienSuBenhAnNew = this.setCheckbox(this.form.maTienSu, data.data)
      if (this.form.canNang && this.form.chieuCao) {
        this.form.bmi = (this.form.canNang / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
      }
      data = await getChiTietPhieuKhamVaoVien(id)
      if (this.form.lyDoVv == null) {
        this.form.lyDoVv = data.data.lyDoVv
      }
      // if (this.form.canLamSang == null) {
      //   this.form.canLamSang = data.data.tomTatKqcls
      // }
    },
    setCheckbox(model, checkBoxArray) {
      if (model) {
        let arrModel = model.split(",")
        arrModel = arrModel.map(item => item = item.trim())
        return checkBoxArray.map(item => {
          item.checkbox = false;
          if (arrModel.includes(item.ma)) {
            item.checkbox = true;
          }
          return item
        })
      }
      return checkBoxArray
    },
    setModelFormCheckbox(arrCheckbox) {
      let ma = "";
      arrCheckbox.forEach(item => {
        if (item.checkbox) {
          ma = ma + item.ma + ","
        }
      })
      this.form.maTienSu = ma;
    },
    clickUpload() {
      this.$refs["upload-image"].click();
    },
    async getDsFile() {
      const id = window.location.href.split("/").at(-1);
      let data = await getListFile({
        idba: id,
        loaiTaiLieu: 40,
      });
      this.files = data.data;
      this.files.forEach((item) => {
        item.name = item.link.split("\\").at(-1);
      });
      this.$emit("get-total-files", this.files ? this.files.length : 0);
    },
    async handleUpload(e) {
      this.loading = true;
      const idba = window.location.href.split("/").at(-1);
      this.tienTrinhUpload = 0;
      let file = e.target.files;
      var isValidate = true;
      let data = new FormData();
      if (this.file) {
        return;
      }
      data.append("file", file[0]);
      data.append("idba", idba);
      data.append("loaiTaiLieu", 40);

      if (!isValidate) return;

      try {
        await uploadFile(data, (e) => {
          this.tienTrinhUpload = e;
          this.loading = false;
        });
        this.$message({
          message: "Tải lên thành công",
          type: "success",
        });
        this.getDsFile();
      } catch (error) {
        console.log(error);
        this.$message({
          message: "Tải lên không thành công",
          type: "error",
        });
      }
      this.$refs["upload-image"].value = null;
    },
    async getDownloadFile(idba, stt, name) {
      try {
        let data = await downloadFile(idba, stt)
        var blob = new Blob([data]);
        saveAs.saveAs(blob, `${name}`);
      } catch (error) {
        console.log(error);
      }
    },
    async removeFile(item) {
      var loaiTaiLieu = 1;
      await deleteFile(item.idba, item.stt, loaiTaiLieu);
      this.getDsFile();
    },
  },
};
</script>
<style scoped>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}

.files {
  padding: 10px;
  width: 100%;
  color: rgb(88, 88, 88);
  display: flex;
  align-items: center;
  justify-content: space-between;
  cursor: pointer;
}

.files:hover {
  background: rgba(231, 231, 231, 0.616);
  font-weight: 500;
}

.box-file {
  cursor: pointer;
  border: 2px dashed #d9d9d9;
  width: 100px;
  height: 100px;
}

.box-file:hover {
  border: 2px dashed #2874a6;
  width: 100px;
  height: 100px;
}
</style>
