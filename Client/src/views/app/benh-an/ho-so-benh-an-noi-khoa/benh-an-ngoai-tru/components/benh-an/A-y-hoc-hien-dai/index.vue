<template>
  <div>
    <div style="font-size: 16px; font-weight: bold" class="mb-3">
      A. Y HỌC HIỆN ĐẠI
    </div>
    <el-form :model="form" :rules="rules" ref="form">
      <v-row>
        <v-col cols="6" lg="6" md="6">
          <el-form-item label="I. LÝ DO VÀO VIỆN">
            <el-input v-model="form.lyDoVv" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" lg="6" md="6">
          <el-form-item label="II. BỆNH SỬ">
            <el-input v-model="form.benhSu" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="III. TIỀN SỬ"> </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="1. Bản Thân">
            <el-input v-model="form.tienSuBanThan" type="textarea" rows="3"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item>
            <br />
            <el-checkbox @change="setModelFormCheckbox(tienSuBenhAn)" v-for="(item, index) in tienSuBenhAn" :key="index"
              v-model="item.checkbox">{{ item.ten }}</el-checkbox>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <el-form-item label="2. Gia đình">
            <el-input v-model="form.tienSuGiaDinh" type="textarea" rows="3"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="6">
          <el-form-item label="Đặc điểm liên quan đến bệnh tất">
            <el-input v-model="form.dacDiemLienQuanBenh" type="textarea" rows="3"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12">
          <el-form-item label="Mô tả">
            <el-input v-model="form.moTaTienSu" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="IV. KHÁM BỆNH"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item>
            <div>
              <div style="font-weight: 500">1. Khám toàn thân:</div>
              <div style="font-style: italic; line-height: 8px" class="pb-4">
                (Ý thức, da niêm mạc, hệ thống hạch, tuyến giáp, vị trí, kích
                thước, số lượng, di động...)
              </div>
            </div>
            <el-input v-model="form.toanThan" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12">
          <v-row>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="Mạch (lần/phút):" prop="mach">
                <el-input type="number" v-model="form.mach" size="mini" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="Nhiệt độ (℃):" prop="nhietDo">
                <el-input type="number" size="mini" v-model="form.nhietDo" :step="0.1"
                  controls-position="right" style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="Huyết áp (mmHg):">
                <el-input v-model="form.huyetAp" size="mini" style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="Nhịp thở (lần/phút):" prop="nhipTho">
                <el-input type="number" v-model="form.nhipTho" size="mini" controls-position="right" style="width: 100%">
                </el-input>
              </el-form-item>
            </v-col>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="Cân Nặng (kg):">
                <el-input type="number" size="mini" v-model="form.canNang" :step="0.1"
                  controls-position="right" style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="Chiều cao (cm):">
                <el-input type="number" v-model="form.chieuCao" size="mini" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
            <v-col cols="2" class="padding-cols">
              <el-form-item label="BMI:">
                <el-input type="number" v-model="form.bmi" size="mini" :step="0.1" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
          </v-row>
        </v-col>
        <v-col cols="12" class="padding-cols mt-4">
          <el-form-item label="2. Khám bộ phận"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tuần hoàn:">
            <el-input v-model="form.tuanHoan" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Hô hấp:"></el-form-item>
          <el-input v-model="form.hoHap" type="textarea"></el-input>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tiêu hóa:">
            <el-input v-model="form.tieuHoa" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tiết niệu-sinh dục:">
            <el-input v-model="form.thanTnieuSduc" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Thần kinh:">
            <el-input v-model="form.thanKinh" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Cơ xương khớp:">
            <el-input v-model="form.xuongKhop" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tai mũi họng:">
            <el-input v-model="form.taiMuiHong" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Răng hàm mặt:">
            <el-input v-model="form.rangHamMat" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Mắt:">
            <el-input v-model="form.mat" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Nội tiết, dinh dưỡng vã các bênh lý khác(nếu có):">
            <el-input v-model="form.noiTietDd" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="V. CẬN LÂM SÀNG">
            <el-input v-model="form.canLamSang" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols">
          <el-form-item label="VI. TÓM TẮT BỆNH ÁN">
            <el-input v-model="form.tomTatBenhAn" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="VII. CHẨN ĐOÁN"></el-form-item>
        </v-col>
        <v-col cols="6">
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
        <v-col cols="6">
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
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 2</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv2" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
              @get-item="getTenBenhKem2Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv2" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 3</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv3" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem3Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv3" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 4</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv4" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem4Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv4" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 5</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv5" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem5Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv5" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 6</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv6" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem6Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv6" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 7</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv7" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem7Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv7" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 8</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv8" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem8Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv8" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6">
          <b>Bệnh kèm theo 9</b>
          <el-form-item label="Mã bệnh">
            <base-select-async v-model="form.maBenhKemVv9" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
              @get-item="getTenBenhKem9Vv" @change="onChangeBenhKem">
            </base-select-async>
          </el-form-item>
          <el-form-item label="Tên bệnh">
            <el-input v-model="form.tenBenhKemVv9" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="Bệnh phân biệt:">
            <base-select-async v-model="form.tenBenhPhanBiet" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" size="small">
            </base-select-async>
          </el-form-item>
        </v-col>
      </v-row>
    </el-form>
  </div>
</template>

<script>
import * as apiBenhTat from "@/api/benh-tat.js";
import store from '@/store'
import { getSelectBox } from "@/api/danh-muc.js";
import { getChiTietPhieuKhamVaoVien } from "@/api/phieu-kham-benh-vao-vien";
import {
  chiTietToBenhAn,
  getTienSuBenh,
  getKhamYhhd,
  getTongKetBenhAn,
  getKhoaDieuTri,
  getKhamYhct,
} from "@/api/benh-an.js";
export default {
  data() {
    var checkMach = (rule, value, callback) => {
      // if (!value) {
      //   return callback(new Error('Mục bắt buộc'));
      // }
      setTimeout(() => {
        if (value && (value > 226 || value < 0)) {
          callback(new Error('Giá trị nhập lớn hơn 0 và nhỏ hơn 226'));
        } else {
          callback();
        }
      }, 1000);
    };
    var checknhipTho = (rule, value, callback) => {
      // if (!value) {
      //   return callback(new Error('Mục bắt buộc'));
      // }
      setTimeout(() => {
        if (value && (value < 0)) {
          callback(new Error('Giá trị nhập lớn hơn 0'));
        } else {
          callback();
        }
      }, 1000);
    };
    var checknhietDo = (rule, value, callback) => {
      // if (!value) {
      //   return callback(new Error('Mục bắt buộc'));
      // }
      setTimeout(() => {
        if (value && (value < 34 || value > 42)) {
          callback(new Error('Giá trị nhập lớn hơn 34 hoặc nhỏ hơn 42'));
        } else {
          callback();
        }
      }, 1000);
    };
    return {
      form: {
        benhSu: null,
        bmi: null,
        bskham: null,
        canLamSang: null,
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
        xuongKhop: null,
        maTienSu: null,
        moTaTienSu: null,
        tienSuBanThan: null,
        tienSuGiaDinh: null,
        dacDiemLienQuanBenh: null,
        maBenhChinhVv: null,
        tenBenhChinhVv: null,
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
        tenBenhKemVv1: null,
        maBenhKemVv1: null,
        tenBenhKemVv2: null,
        maBenhKemVv2: null,
        tenBenhKemVv3: null,
        maBenhKemVv3: null,
        maBenhKemVv4: null,
        tenBenhKemVv4: null,
        maBenhKemVv5: null,
        tenBenhKemVv5: null,
        maBenhKemVv6: null,
        tenBenhKemVv6: null,
        maBenhKemVv7: null,
        tenBenhKemVv7: null,
        maBenhKemVv8: null,
        tenBenhKemVv8: null,
        maBenhKemVv9: null,
        tenBenhKemVv9: null,
        isClickBenhKem: false
      },
      benh_khac: [
        {
          ten_benh: null,
          ma_benh: null,
        },
      ],
      tienSuBenhAn: [
        { ten: "Dị ứng", checkbox: false },
        { ten: "Rượu", checkbox: false },
        { ten: "Ma Túy", checkbox: false },
        { ten: "Thuốc lá", checkbox: false },
      ],
      rules: {
        mach:
        {
          validator: checkMach,
          trigger: "blur",
        },
        nhipTho:
        {
          validator: checknhipTho,
          trigger: "blur",
        },
        nhietDo:{
          validator: checknhietDo,
          trigger: "blur",
        }
      }
    }
  },
  mounted() {
    this.getData();
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
    onChangeBenhKem() {
      this.isClickBenhKem = true;
    },
    async getChanDoanBenh(params) {
      return await apiBenhTat.index(params);
    },
    async getData() {
      const id = window.location.href.split("/").at(-1);
      let data = await getKhamYhhd(id);
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key];
        }
      }
      data = await getTienSuBenh(id, { getModelNull: true });
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key];
        }
      }
      data = await getSelectBox({ MaParent: "017" });
      this.tienSuBenhAn = this.setCheckbox(this.form.maTienSu, data.data);
      if (this.form.canNang && this.form.chieuCao) {
        this.form.bmi = (this.form.canNang / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
      }
      data = await getChiTietPhieuKhamVaoVien(id)
      // console.log(data)
      if (this.form.lyDoVv == null && data.data) {
        this.form.lyDoVv = data.data.lyDoVv
      }
      if (this.form.canLamSang == null && data.data) {
        this.form.canLamSang = data.data.tomTatKqcls
      }
    },
    setCheckbox(model, checkBoxArray) {
      if (model) {
        let arrModel = model.split(",");
        arrModel = arrModel.map((item) => (item = item.trim()));
        return checkBoxArray.map((item) => {
          item.checkbox = false;
          if (arrModel.includes(item.ma)) {
            item.checkbox = true;
          }
          return item;
        });
      }
      return checkBoxArray;
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
    getTenBenhChinhVv(item) {
      if (item) {
        this.form.benhChinh.tenBenh = item.tenBenh;
      }
      else {
        this.form.benhChinh.tenBenh = null
      }
    },
    getTenBenhChinhVv(item) {
      if (item) {
        this.form.benhChinh.tenBenh = item.tenBenh;
      }
      else {
        this.form.benhChinh.tenBenh = null
      }
    },
    getTenBenhKem1Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv1) {
          this.form.tenBenhKemVv1 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv1 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv1 = null;
      }
    },
    getTenBenhKem2Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv2) {
          this.form.tenBenhKemVv2 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv2 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv2 = null;
      }
    },
    getTenBenhKem3Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv3) {
          this.form.tenBenhKemVv3 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv3 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv3 = null;
      }
    },
    getTenBenhKem4Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv4) {
          this.form.tenBenhKemVv4 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv4 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv4 = null;
      }
    },
    getTenBenhKem5Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv5) {
          this.form.tenBenhKemVv5 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv5 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv5 = null;
      }
    },
    getTenBenhKem6Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv6) {
          this.form.tenBenhKemVv6 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv6 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv6 = null;
      }
    },
    getTenBenhKem7Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv7) {
          this.form.tenBenhKemVv7 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv7 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv7 = null;
      }
    },
    getTenBenhKem8Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv8) {
          this.form.tenBenhKemVv8 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv8 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv8 = null;
      }
    },
    getTenBenhKem9Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv9) {
          this.form.tenBenhKemVv9 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv9 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv9 = null;
      }
    },
  },
};
</script>

<style>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}
</style>
