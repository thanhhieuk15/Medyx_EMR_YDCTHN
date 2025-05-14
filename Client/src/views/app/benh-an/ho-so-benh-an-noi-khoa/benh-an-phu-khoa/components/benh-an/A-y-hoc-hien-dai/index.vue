<template>
  <div>
    <div style="font-size: 16px; font-weight: bold" class="mb-3">
      A. BỆNH ÁN
    </div>
    <el-form :model="form" :rules="rules" ref="form">
      <v-row>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="I. LÝ DO VÀO VIỆN">
            <el-input v-model="form.lyDoVV" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="II. Hỏi bệnh">
            <div>1. Quá trình bệnh lý:</div>
            <el-input v-model="form.benhSu" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="2. Tiền sử bệnh"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item class="item-form"
            label="Bản thân: (Phát triển thể lực từ nhỏ đến lớn, những bệnh đã mắc, phương pháp điều trị, tiêm phòng, ăn uống, sinh hoạt v.v ..)">
            <el-input v-model="form.tienSuBanThan" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Gia đình">
            <el-input v-model="form.tienSuGiaDinh" type="textarea" rows="2"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="3. Tiền sử sản phụ khoa"></el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Bắt đầu thấy kinh năm:">
            <el-input v-model="form.chiSoHamax" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Tuổi:">
            <el-input v-model="form.tuoiCoKinh" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Tính chất kinh nguyệt:">
            <el-input v-model="form.thoiDiemPhatHienTangHa" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Chu kỳ (ngày)">
            <el-input v-model="form.dtdmax" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Số ngày thấy kinh: ">
            <el-input v-model="form.nuoiDuong" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Lượng kinh: ">
            <el-input v-model="form.chamSoc" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Kinh lần cuối ngày: ">
            <el-input v-model="form.tinhTrangSinh" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item class="item-form" label="Đau bụng">
            <el-radio-group v-model="form.diTatBamSinh" style="width: 100%">
              <div class="gender">
                <el-radio :label="true">Có</el-radio>
                <el-radio :label="false">Không</el-radio>
              </div>
            </el-radio-group>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item class="item-form" label="Thời gian">
            <el-radio-group v-model="form.deKho" style="width: 100%">
              <div class="gender">
                <el-radio :label="1">Trước</el-radio>
                <el-radio :label="2">Trong</el-radio>
                <el-radio :label="3">Sau</el-radio>
              </div>
            </el-radio-group>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Lấy chồng năm: ">
            <el-input v-model="form.anDuoi1Tuoi" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Tuổi: ">
            <el-input v-model="form.anTren1Tuoi" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Hết kinh năm: ">
            <el-input v-model="form.thangCaiSua" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Tuổi: ">
            <el-input v-model="form.thangNoi" type="text"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Những bệnh phụ khoa đã điều trị: ">
            <el-input v-model="form.benhLyKhac" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="4.Tiền sử sản khoa:"></el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item>
            <div class="title-group-checkbox mr-4">Tiền thai (Para)</div>
            <el-checkbox @change="setModelFormCheckboxPara(Para)" v-for="(item, index) in Para" :key="index"
              v-model="item.checkbox">{{ item.ten }}
            </el-checkbox>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="III. KHÁM BỆNH"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item>
            <div>
              <div style="font-weight: 500">1. Toàn thân:</div>
              <div style="font-style:italic; line-height: 8px;" class="pb-4">
                da niêm mạc
              </div>
            </div>
            <el-input v-model="form.toanThan" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Hạch">
            <el-input v-model="form.hach" type="textarea" rows="4"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Vú">
            <el-input v-model="form.vu" type="textarea" rows="4"></el-input>
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
                <el-input type="number" v-model="form.canNangps" size="mini" :step="0.01" controls-position="right"
                  style="width: 100%"></el-input>
              </el-form-item>
            </v-col>
          </v-row>
        </v-col>
        <v-col cols="12" class="padding-cols mt-4">
          <el-form-item label="2.Các cơ quan:"></el-form-item>
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
          <el-form-item label="Thận - Tiết niệu:">
            <el-input v-model="form.thanTietNieu" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Thần kinh:">
            <el-input v-model="form.thanKinh" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Cơ - xương - khớp:">
            <el-input v-model="form.xuongKhop" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Khác:">
            <el-input v-model="form.khac" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols mt-4">
          <el-form-item label="2. Khám chuyên khoa:"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols mt-4">
          <el-form-item label="a. Khám ngoài:"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Các dấu hiệu sinh dục thứ phát">
            <el-input v-model="form.dHdDthuPhat" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Môi lớn">
            <el-input v-model="form.moiLon" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Môi bé">
            <el-input v-model="form.moiBe" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Âm vật">
            <el-input v-model="form.amVat" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Âm hộ">
            <el-input v-model="form.amHo" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Màng trinh">
            <el-input v-model="form.mangTrinh" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tầng sinh môn">
            <el-input v-model="form.tanSinhMon" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols mt-4">
          <el-form-item label="a. Khám trong:"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Âm đạo">
            <el-input v-model="form.amDao" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Cổ tử cung">
            <el-input v-model="form.coTuCung" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Thân tử cung">
            <el-input v-model="form.thanTuCung" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Phần phụ">
            <el-input v-model="form.phanPhu" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Các túi cùng">
            <el-input v-model="form.cacTuiCung" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="4. Các xét nghiệm cận lâm sàng cần làm">
            <el-input v-model="form.canLamSang" type="textarea" rows="5"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="5 Tóm tắt bệnh án">
            <el-input v-model="form.tomTatBenhAn" type="textarea" rows="6"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="IV. Chẩn đoán khi vào khoa điều trị"></el-form-item>
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
        <v-col cols="4" class="padding-cols">
          <el-form-item label="Phân biệt:">
            <base-select-async v-model="form.maCDPhanBiet" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
              keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" size="small">
            </base-select-async>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tiên Lượng">
            <el-input v-model="form.tienLuong" type="textarea"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols">
          <el-form-item label="Hướng Dẫn điều trị">
            <el-input v-model="form.huongDT" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Ngày ký">
            <el-date-picker v-model="form.ngayKyDT" type="datetime" size="small" placeholder="dd/mm/yyyy hh:mm"
              style="width: 100%" format="dd/MM/yyyy HH:mm" value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="Bác sỹ làm bệnh án">
            <el-input v-model="form.bSDieuTri" type="textarea"></el-input>
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
import { getBenhAnPhuSan, getTienSuBenh,getKhamYhhd } from "@/api/benh-an.js";
import { getChiTietPhieuKhamVaoVien } from "@/api/phieu-kham-benh-vao-vien";

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
      form: {
        idba: null,
        kqdt: null,
        lyDoVV: null,
        benhSu: null,
        tienSuBanThan: null,
        tienSuGiaDinh: null,
        chiSoHamax: null,
        tuoiCoKinh: null,
        thoiDiemPhatHienTangHa: null,
        dtdmax: null,
        nuoiDuong: null,
        chamSoc: null,
        tinhTrangSinh: null,
        diTatBamSinh: null,
        deKho: null,
        anDuoi1Tuoi: null,
        anTren1Tuoi: null,
        thangCaiSua: null,
        thangNoi: null,
        benhLyKhac: null,
        para: null,
        toanThan: null,
        hach: null,
        vu: null,
        mach: null,
        nhietDo: null,
        huyetAp: null,
        nhipTho: null,
        canNangps: null,
        tuanHoan: null,
        hoHap: null,
        tieuHoa: null,
        thanTietNieu: null,
        thanKinh: null,
        xuongKhop: null,
        khac: null,
        dHdDthuPhat: null,
        moiLon: null,
        moiBe: null,
        amVat: null,
        amHo: null,
        mangTrinh: null,
        tanSinhMon: null,
        amDao: null,
        coTuCung: null,
        thanTuCung: null,
        phanPhu: null,
        cacTuiCung: null,
        canLamSang: null,
        tomTatBenhAn: null,
        maCDPhanBiet: null,
        tienLuong: null,
        huongDT: null,
        ngayKyDT: null,
        bSDieuTri: null,
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
        isClickBenhKem: false,
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
        { ten: "Rượu bia", checkbox: false },
        { ten: "Thuốc lá", checkbox: false },
        { ten: "Thuốc lào", checkbox: false },
        { ten: "Khác", checkbox: false }
      ],
      Para: [
        { name: "Sinh (đủ tháng)", checkbox: false },
        { name: "Sớm (đẻ non)", checkbox: false },
        { name: "Sẩy (nạo, hút)", checkbox: false },
        { name: "Sống", checkbox: false }
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
  },
  watch: {
    form: {
      handler(val) {
        this.$emit('get-phanA', val)
        store.dispatch("hosobenhan/setChanDoanYHHD", val)
      },
      deep: true
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

    async getData() {
      const id = window.location.href.split("/").at(-1);
      let data = await getKhamYhhd(id);
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
      data = await getSelectBox({ MaParent: '217' });
      this.Para = this.setCheckbox(this.form.para, data.data)
      data = await getBenhAnPhuSan(id)
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key]
        }
      }
      this.form.canNangps = data.data.canNang;
      data = await getChiTietPhieuKhamVaoVien(id)

    },
    getTenBenhChinhVv(item) {
      if (item) {
        this.form.benhChinh.tenBenh = item.tenBenh;
      }
      else {
        this.form.benhChinh.tenBenh = null
      }
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
    setModelFormCheckboxPara(arrCheckbox) {
      let ma = "";
      arrCheckbox.forEach(item => {
        if (item.checkbox) {
          ma = ma + item.ma + ","
        }
      })
      this.form.para = ma;
    },
  },
};
</script>

<style>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}

.el-form-item__label {
  text-align: left !important;
  line-height: 22px !important;
  margin: 10px 0px !important;

}
</style>