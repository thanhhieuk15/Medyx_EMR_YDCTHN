<template>
  <div>
    <div style="font-size: 16px; font-weight: bold" class="mb-4">
      III. CHẨN ĐOÁN
    </div>
    <el-form>
      <v-row>
        <v-col cols="12">
          <div style="border: 1px solid #d6dbdf" class="pa-4 pb-8">
            <v-row class="mt-0">
              <v-col cols="12" class="padding-cols mb-2">
                <div style="font-weight: 600" class="pb-1">
                  CHẨN ĐOÁN
                </div>
                <hr />
              </v-col>
              <v-col cols="12" class="padding-cols">
                <b>Nơi chuyển đến</b>
              </v-col>
              <v-col class="padding-cols" cols="12" xs="12" sm="12" md="12" lg="12" xl="12">
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhNoiChuyenDenYhhd"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    placeholder="" style="width: 100%" size="small" @get-item="getTenBenhNoiChuyenDenYhhd" clearable>
                  </base-select-async>
                </el-form-item>
              </v-col>
              <v-col cols="12" xs="12" sm="12" md="12" lg="12" xl="12" class="padding-cols">
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhNoiChuyenDenYhhd" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12" class="mt-3">
                <b>Chẩn đoán KKB/cấp cứu</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKkbyhhd" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKkbyhhd">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKkbyhhd" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>

              <!-- <v-col cols="6" class="padding-cols">
                <el-form-item label="Ngày đẻ(mổ đẻ)">
                  <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm" v-model="form.ngayDe"
                  size="small" style="width:100%"></el-date-picker>
                </el-form-item>
              </v-col> 
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Cách thức đẻ">
                  <el-input v-model="form.pPDe" type="textarea"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12" class="padding-cols">
                <el-form-item label="Kiểm soát tử cung">
                  <el-input v-model="form.cachKiemSoatTC" type="textarea"></el-input>
                </el-form-item>
              </v-col> -->
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Tai biến">
                  <el-radio-group v-model="form.taiBienYhhd" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Biến chứng">
                  <el-radio-group v-model="form.bienChungYhhd" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="12" class="padding-cols">
                <el-form-item>
                  <br />
                  <el-checkbox @change="setModelFormCheckbox(BienChungSanKhoa)"
                    v-for="(item, index) in BienChungSanKhoa" :key="index" v-model="item.checkbox">{{ item.ten }}
                  </el-checkbox>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="mt-3">
                <b>Chẩn đoán trước phẫu thuật</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.chuanDoanTruocPt"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    placeholder="" style="width: 100%" clearable size="small" @get-item="getTenChuanDoanTruocPt">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenChuanDoanTruocPt" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="mt-3">
                <b>Chẩn đoán sau phẫu thuật</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.chuanDoanSauPt" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenChuanDoanSauPt">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenChuanDoanSauPt" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12" class="padding-cols">
                <el-form-item label="Phương pháp phẫu thuật">
                  <el-input v-model="form.phuongPhap" type="textarea"></el-input>
                </el-form-item>
              </v-col> -->
              <!-- <v-col cols="6" class="padding-cols">
                <el-form-item label="Trẻ sơ sinh" prop="donThai">
                  <el-radio-group v-model="form.donThai" style="width: 100%">
                    <div class="gender">
                      <el-radio :label="1">Đơn thai</el-radio>
                      <el-radio :label="2">Đa thai</el-radio>
                    </div>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Giới tính con" prop="gTCon">
                  <el-radio-group v-model="form.gTCon" style="width: 100%">
                    <div class="gender">
                      <el-radio :label="1">Trai</el-radio>
                      <el-radio :label="2">Gái</el-radio>
                    </div>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Dị tật">
                  <el-input v-model="form.cuTheTatBamSinh" type="textarea"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Cân nặng">
                  <el-input v-model="form.canNang" type="textarea"></el-input>
                </el-form-item>
              </v-col> -->
            </v-row>
          </div>
        </v-col>
      </v-row>
    </el-form>
  </div>
</template>

<script>
import * as apiBenhTat from "@/api/benh-tat.js";
import * as apiBenhTatCT from "@/api/benh-tat-yhct.js";
import store from '@/store'
export default {
  props: ["benhAn", "benhAnKhoaDieuTri", "benhAnPhuSan"],
  data: () => ({
    form: {
      maBenhNoiChuyenDenYhct: null,
      maBenhNoiChuyenDenYhhd: null,
      maBenhKkbyhhd: null,
      tenBenhNoiChuyenDenYhhd: null,
      tenBenhKkbyhhd: null,
      taiBienYhct: null,
      taiBienYhhd: null,
      bienChungYhhd: null,
      chuanDoanTruocPt: null,
      chuanDoanSauPt: null,
      tenChuanDoanTruocPt: null,
      tenChuanDoanSauPt: null,
      phuongPhap: null,
      donThai: null,
      gTCon: null,
      cuTheTatBamSinh: null,
      canNang: null,
      vaoBuongDe: null,
      ngayDe: null,
      ngoi: null,
      pPDe: null,
      cachKiemSoatTC: null,

    },
    chanDoan: [
      {
        maChanDoan: null,
        tenChanDoan: null,
      },
    ],
    BienChungSanKhoa: [
      { ten: "Do phẫu thuật", checkbox: false },
      { ten: "Do gây mê", checkbox: false },
      { ten: "Do nhiễm khuẩn", checkbox: false },
      { ten: "Khác", checkbox: false },
    ],
    isClickBenhKem: false
  }),
  watch: {
    benhAn: async function (val) {
      for (let key in this.form) {
        if (val && val.hasOwnProperty(key)) {
          this.form[key] = val[key];
        }
      }
      // const data = await getSelectBox({ MaParent: '208' });
      // this.BienChungSanKhoa = this.setCheckbox(this.form.taiBienYhct, data.data);
    },
    benhAnPhuSan: async function (val) {
      for (let key in this.form) {
        if (val && val.hasOwnProperty(key)) {
          this.form[key] = val[key];
        }
      }
    },
    benhAnKhoaDieuTri: async function (val) {
      for (let key in this.form) {
        if (val && val.hasOwnProperty(key)) {
          this.form[key] = val[key];
        }
      }
      // const data = await getSelectBox({ MaParent: '058' });
      // this.BienChungSanKhoa = this.setCheckbox(this.form.taiBienYhct, data.data);
    },
    form: {
      handler(val) {
        this.$emit('get-chanDoan', val)
        store.dispatch("hosobenhan/setChanDoan", val)
      },
      deep: true
    },
    CHANDOAN: {
      handler(val) {
        for (let key in this.form) {
          if (val && val.hasOwnProperty(key)) {
            this.form[key] = val[key]
          }
        }
      },
      deep: true
    },
    TONGKETRAVIEN: {
      handler(val) {
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
    TONGKETRAVIEN() {
      return this.$store.state.hosobenhan.tongKetRaVien
    },
    CHANDOAN() {
      return this.$store.state.hosobenhan.chanDoanYHHD
    },

  },
  mounted() {

  },
  methods: {

    onChangeBenhKem() {
      this.isClickBenhKem = true;
    },
    async getChanDoanBenh(params) {
      return await apiBenhTat.index(params);
    },
    async getChanDoanBenhCT(params) {
      return await apiBenhTatCT.index(params);
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

    getTenBenhNoiChuyenDenYhhd(item) {
      if (item) {
        this.form.tenBenhNoiChuyenDenYhhd = item.tenBenh;
      }
      else {
        this.form.tenBenhNoiChuyenDenYhhd = null
      }
    },
    getTenBenhKkbyhhd(item) {
      if (item) {
        this.form.tenBenhKkbyhhd = item.tenBenh;
      }
      else {
        this.form.tenBenhKkbyhhd = null
      }
    },

  },
};
</script>

<style scoped>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}
</style>
