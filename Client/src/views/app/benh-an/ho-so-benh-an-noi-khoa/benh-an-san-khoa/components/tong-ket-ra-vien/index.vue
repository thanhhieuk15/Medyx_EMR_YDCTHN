<template>
  <div>
    <div style="width: 100%; font-size: 18px; font-weight: bold" class="d-flex justify-center">
      TỔNG KẾT RA VIỆN
    </div>
    <el-form>
      <v-row>
        <!-- <v-col cols="12" class="padding-cols">
          <el-form-item label="Lý do vào viện">
            <el-input placeholder="Lý do vào viện" size="small" v-model="form.lyDoVv"></el-input>
          </el-form-item>
        </v-col> -->
        <!-- <v-col cols="12" class="padding-cols">
          <el-form-item label="Quá trình bệnh lý và diễn biến lâm sàng">
            <el-input type="textarea" :rows="2" size="small" v-model="form.quaTrinhBenhLy"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Tóm tắt kết quả xét nghiệm cận lâm sàng có giá trị chẩn đoán:">
            <el-input size="small" v-model="form.tomTatKetQuaCls"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Vào Viện"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <div style="border: 1px solid #d6dbdf" class="pa-4 pb-8">
            <v-row>
              <v-col cols="12">
                <b>Bệnh chính</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form2.maBenhChinhVv" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhChinhVv" :firstEmitGetItem="false">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form2.tenBenhChinhVv" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Bệnh kèm theo 1</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form2.benhKem1.maBenh"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    placeholder="" style="width: 100%" clearable size="small" @get-item="getTenBenhKem1Vv"
                    @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form2.tenBenhKemVv1" size="small"></el-input>
                </el-form-item>
              </v-col>
          
              <v-col cols="12" class="padding-cols">
                <el-form-item label="Phương pháp điều trị">
                  <el-input v-model="form.ppdttheoYhhd" size="small"></el-input>
                </el-form-item>
              </v-col>
            </v-row>
          </div>
        </v-col>
        
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Ra viện"></el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols mb-3">
          <div style="border: 1px solid #d6dbdf" class="pa-4 pb-8">
            <v-row> 
              <v-col cols="12">
                <b>Bệnh chính</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form2.maBenhChinhRv" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhChinhRv" :firstEmitGetItem="false">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form2.tenBenhChinhRv" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Bệnh kèm 1</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form2.maBenhKemRv1" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv1" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form2.tenBenhKemRv1" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
            </v-row>
          </div>
        </v-col>
        <v-col cols="3" class="padding-cols mt-3">
          <el-form-item label="Kết quả điều trị">
            <el-select size="small" style="width: 100%" v-model="form.kqdt">
              <el-option v-for="item in ketQuaDieuTri" :key="item.ma" :label="item.ten" :value="item.ma">
              </el-option>
            </el-select>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Quá Trình bệnh lý và diễn biến lâm sàng">
            <el-input size="small" v-model="form.quaTrinhBenhLy"></el-input>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Tóm tắt kết quả xét nghiệm cận lâm sàng có giá trị chẩn đoán:">
            <el-input size="small" v-model="form.tomTatKetQuaCls"></el-input>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="3. Phương pháp điều trị">
            <el-input size="small" v-model="form.phuongPhapDT"></el-input>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="4. Tình trạng người bệnh ra viện">
            <el-input size="small" v-model="form.tinhTrangBnrv"></el-input>
          </el-form-item>
        </v-col> -->
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="5. Hướng điều trị và các chế độ tiếp theo">
            <el-input v-model="form.huongDt" size="small"></el-input>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ X-Quang">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToXquang">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ CT Scanner">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToCt">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ MRI">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToMri">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ siêu âm">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToSa">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ xét nghiệm">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToXn">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Khác">
            <el-input size="small" v-model="form.khac"></el-input>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ khác">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToKhac">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Số tờ toàn bộ hồ sơ">
            <el-input-number style="width: 100%" size="small" controls-position="right" v-model="form.soToToanBoHs">
            </el-input-number>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Người giao hồ sơ">
            <base-select-async v-model="form.nguoiGiaoHs" placeholder="Tìm kiếm theo tên"
              :label="(item) => `${item.hoTen} - ${item.maNv} - ${item.khoa.tenKhoa}`" keyValue="maNv"
              :apiFunc="getNhanVien" style="width: 100%" size="small"></base-select-async>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Người nhận hồ sơ">
            <base-select-async v-model="form.nguoiNhanHs" placeholder="Tìm kiếm theo tên"
              :label="(item) => `${item.hoTen} - ${item.maNv} - ${item.khoa.tenKhoa}`" keyValue="maNv"
              :apiFunc="getNhanVien" style="width: 100%" size="small"></base-select-async>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Ngày tháng năm ký">
            <el-date-picker type="datetime" style="width: 100%" size="small" v-model="form.ngayKy"
              format="dd/mm/yyyy HH:mm:ss" value-format="yyyy-MM-ddTHH:mm:ss"
              placeholder="dd/mm/yyyy HH:mm"></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col xl="3" lg="3" md="4" cols="3" class="padding-cols">
          <el-form-item label="Bác sĩ khám, chữa bệnh">
            <base-select-async v-model="form.bsdieuTri" placeholder="Tìm kiếm theo tên"
              :label="(item) => `${item.hoTen} - ${item.maNv} - ${item.khoa.tenKhoa}`" keyValue="maNv"
              :apiFunc="getNhanVien" style="width: 100%" size="small"></base-select-async>
          </el-form-item>
        </v-col>
      </v-row>
    </el-form>
  </div>
</template>

<script>
import * as apiNhanVien from "@/api/nhan-vien.js";
import * as apiBenhTat from "@/api/benh-tat.js";
import * as apiBenhTatCT from "@/api/benh-tat-yhct.js";
import { getSelectBox } from '@/api/danh-muc.js';
import store from '@/store'

import {
  chiTietToBenhAn,
  getTienSuBenh,
  getKhamYhhd,
  getKhoaDieuTri,
  getTongKetBenhAn,
  getKhamYhct,
} from "@/api/benh-an.js";
export default {
  data: () => ({
    ketQuaDieuTri: [
      {
        ma: "130001",
        ten: "Khỏi",
      },
      {
        ma: "130002",
        ten: "Đỡ",
      },
      {
        ma: "130003",
        ten: "Không thay đổi",
      },
      {
        ma: "130004",
        ten: "Nặng hơn"
      },
      {
        ma: "130005",
        ten: "Tử vong"
      }
    ],
    form: {
      ppdttheoYhct: null,
      ppdttheoYhhd: null,
      lyDoVv: null,
      quaTrinhBenhLy: null,
      tomTatKetQuaCls: null,
      tinhTrangBnrv: null,
      soToXquang: null,
      soToCt: null,
      soToMri: null,
      soToSa: null,
      soToXn: null,
      soToKhac: null,
      khac: null,
      soToToanBoHs: null,
      nguoiGiaoHs: null,
      nguoiNhanHs: null,
      ngayKy: null,
      bsdieuTri: null,
      kqdt: null,
      huongDt: null,
      phuongPhapDT: null,

    },
    form2: {
      maBenhChinhVv: null,
      tenBenhChinhVv: null,
      maBenhChinhRv: null,
      maBenhChinhRvyhct: null,
      maBenhKemRv1: null,
      maBenhKemRv1yhct: null,
      maBenhKemRv2: null,
      maBenhKemRv2yhct: null,
      maBenhKemRv3: null,
      maBenhKemRv4: null,
      maBenhKemRv5: null,
      maBenhKemRv6: null,
      maBenhKemRv7: null,
      maBenhKemRv8: null,
      maBenhKemRv9: null,
      maBenhKemRv3yhct: null,
      maBenhKemRv4yhct: null,
      maBenhKemRv5yhct: null,
      maBenhKemRv6yhct: null,
      maBenhKemRv7yhct: null,
      maBenhKemRv8yhct: null,
      maBenhKemRv9yhct: null,
      maBenhChinhVvyhct: null,
      maBenhKemVv1yhct: null,
      maBenhKemVv2yhct: null,
      maBenhKemVv3yhct: null,
      maBenhKemVv4yhct: null,
      maBenhKemVv5yhct: null,
      maBenhKemVv6yhct: null,
      maBenhKemVv7yhct: null,
      maBenhKemVv8yhct: null,
      maBenhKemVv9yhct: null,
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
      tenBenhChinhRv: null,
      tenBenhChinhRvyhct: null,
      tenBenhKemRv1: null,
      tenBenhKemRv1yhct: null,
      tenBenhKemRv2: null,
      tenBenhKemRv2yhct: null,
      tenBenhKemRv3: null,
      tenBenhKemRv4: null,
      tenBenhKemRv5: null,
      tenBenhKemRv6: null,
      tenBenhKemRv7: null,
      tenBenhKemRv8: null,
      tenBenhKemRv9: null,
      tenBenhKemRv3yhct: null,
      tenBenhKemRv4yhct: null,
      tenBenhKemRv5yhct: null,
      tenBenhKemRv6yhct: null,
      tenBenhKemRv7yhct: null,
      tenBenhKemRv8yhct: null,
      tenBenhKemRv9yhct: null,
      tenBenhChinhVvyhct: null,
      tenBenhKemVv1yhct: null,
      tenBenhKemVv2yhct: null,
      tenBenhKemVv3yhct: null,
      tenBenhKemVv4yhct: null,
      tenBenhKemVv5yhct: null,
      tenBenhKemVv6yhct: null,
      tenBenhKemVv7yhct: null,
      tenBenhKemVv8yhct: null,
      tenBenhKemVv9yhct: null,
      tenBenhKemVv1: null,
      maBenhKemVv1: null,
      tenBenhKemVv2: null,
      maBenhKemVv2: null,
      tenBenhKemVv3: null,
      maBenhKemVv3: null,
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
      isClickBenhKem: false

    }
  }),
  mounted() {
    this.getData();
    this.getKetQuaDieuTri();
    console.log(this.form2)
  },
  watch: {
    form: {
      handler(val) {
        this.form.soToToanBoHs = this.form.soToXquang + this.form.soToCt + this.form.soToKhac + this.form.soToMri + this.form.soToSa + this.form.soToXn
        this.$emit('get-tongKetRaVien', val)
      },
      deep: true
    },
    form2: {
      handler(val) {
        store.dispatch("hosobenhan/setTongKetRaVien", val)
      },
      deep: true
    },
    CHANDOAN: {
      handler(val) {
        for (let key in this.form2) {
          if (val && val.hasOwnProperty(key)) {
            this.form2[key] = val[key]
          }
        }
      },
      deep: true
    },
  },
  computed: {
    CHANDOAN() {
      return this.$store.state.hosobenhan.dataChanDoan
    }
  },
  methods: {
    onChangeBenhKem() {
      this.isClickBenhKem = true;
    },
    async getData() {
      const id = window.location.href.split("/").at(-1);
      let data = await getTongKetBenhAn(id)
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key]
        }
      }
      data = await chiTietToBenhAn(id);
      for (let key in this.form2) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form2[key] = data.data[key]
        }
      }
      data = await getKhoaDieuTri({ idba: id, forSelect: true });
      data = data && data.data ? data.data[0] : null;
      for (let key in this.form2) {
        if (data && data.hasOwnProperty(key)) {
          this.form2[key] = data[key]
        }
      }
    },
    async getNhanVien(params) {
      return await apiNhanVien.index(params);
    },
    async getChanDoanBenh(params) {
      return await apiBenhTat.index(params);
    },
    async getChanDoanBenhCT(params) {
      return await apiBenhTatCT.index(params);
    },
    async getKetQuaDieuTri() {
      let data = await getSelectBox({ MaParent: '130' });
      this.ketQuaDieuTri = data.data
    },
    getTenBenhChinhVv(item) {
      if (item) {
        this.form2.benhChinh.tenBenh = item.tenBenh;
      }
      else {
        this.form2.benhChinh.tenBenh = null
      }
    },
    getTenBenhKem1Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv1) {
          this.form2.tenBenhKemVv1 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv1 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv1 = null;
      }
    },
    getTenBenhKem2Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv2) {
          this.form2.tenBenhKemVv2 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv2 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv2 = null;
      }
    },
    getTenBenhKem3Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv3) {
          this.form2.tenBenhKemVv3 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv3 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv3 = null;
      }
    },
    getTenBenhKem4Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv4) {
          this.form2.tenBenhKemVv4 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv4 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv4 = null;
      }
    },
    getTenBenhKem5Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv5) {
          this.form2.tenBenhKemVv5 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv5 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv5 = null;
      }
    },
    getTenBenhKem6Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv6) {
          this.form2.tenBenhKemVv6 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv6 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv6 = null;
      }
    },
    getTenBenhKem7Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv7) {
          this.form2.tenBenhKemVv7 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv7 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv7 = null;
      }
    },
    getTenBenhKem8Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv8) {
          this.form2.tenBenhKemVv8 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv8 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv8 = null;
      }
    },
    getTenBenhKem9Vv(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv9) {
          this.form2.tenBenhKemVv9 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv9 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemVv9 = null;
      }
    },
    getTenBenhChinhRv(item) {
      if (item) {
        this.form2.tenBenhChinhRv = item.tenBenh;
      }
      else {
        this.form2.tenBenhChinhRv = null
      }
    },
    getTenBenhChinhRvyhct(item) {
      if (item) {
        this.form2.tenBenhChinhRvyhct = item.tenBenhBhyt;
      }
      else {
        this.form2.tenBenhChinhRvyhct = null
      }
    },
    getTenBenhKemRv1(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv1) {
          this.form2.tenBenhKemRv1 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv1 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv1 = null;
      }
    },

    getTenBenhKemRv1yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv1yhct) {
          this.form2.tenBenhKemRv1yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv1yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv1yhct = null;
      }
    },
    getTenBenhKemRv2(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv2) {
          this.form2.tenBenhKemRv2 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv2 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv2 = null;
      }
    },
    getTenBenhKemRv2yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv2yhct) {
          this.form2.tenBenhKemRv2yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv2yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv2yhct = null;
      }
    },
    getTenBenhKemRv3(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv3) {
          this.form2.tenBenhKemRv3 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv3 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv3 = null;
      }
    },
    getTenBenhKemRv4(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv4) {
          this.form2.tenBenhKemRv4 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv4 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv4 = null;
      }
    },
    getTenBenhKemRv5(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv5) {
          this.form2.tenBenhKemRv5 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv5 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv5 = null;
      }
    },
    getTenBenhKemRv6(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv6) {
          this.form2.tenBenhKemRv6 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv6 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv6 = null;
      }
    },
    getTenBenhKemRv7(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv7) {
          this.form2.tenBenhKemRv7 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv7 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv7 = null;
      }
    },
    getTenBenhKemRv8(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv8) {
          this.form2.tenBenhKemRv8 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv8 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv8 = null;
      }
    },
    getTenBenhKemRv9(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv9) {
          this.form2.tenBenhKemRv9 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv9 = item.tenBenh;
        }
      } else {
        this.form2.tenBenhKemRv9 = null;
      }
    },
    getTenBenhKemRv3yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv3yhct) {
          this.form2.tenBenhKemRv3yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv3yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv3yhct = null;
      }
    },
    getTenBenhKemRv4yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv4yhct) {
          this.form2.tenBenhKemRv4yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv4yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv4yhct = null;
      }
    },
    getTenBenhKemRv5yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv5yhct) {
          this.form2.tenBenhKemRv5yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv5yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv5yhct = null;
      }
    },
    getTenBenhKemRv6yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv6yhct) {
          this.form2.tenBenhKemRv6yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv6yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv6yhct = null;
      }
    },
    getTenBenhKemRv7yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv7yhct) {
          this.form2.tenBenhKemRv7yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv7yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv7yhct = null;
      }
    },
    getTenBenhKemRv8yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv8yhct) {
          this.form2.tenBenhKemRv8yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv8yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv8yhct = null;
      }
    },
    getTenBenhKemRv9yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemRv9yhct) {
          this.form2.tenBenhKemRv9yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemRv9yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemRv9yhct = null;
      }
    },

    getTenBenhChinhVvyhct(item) {
      if (item) {
        this.form2.tenBenhChinhVvyhct = item.tenBenhBhyt;
      }
      else {
        this.form2.tenBenhChinhVvyhct = null
      }
    },
    getTenBenhKemVv1yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv1yhct) {
          this.form2.tenBenhKemVv1yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv1yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv1yhct = null;
      }
    },
    getTenBenhKemVv2yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv2yhct) {
          this.form2.tenBenhKemVv2yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv2yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv2yhct = null;
      }
    },
    getTenBenhKemVv3yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv3yhct) {
          this.form2.tenBenhKemVv3yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv3yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv3yhct = null;
      }
    },
    getTenBenhKemVv4yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv4yhct) {
          this.form2.tenBenhKemVv4yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv4yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv4yhct = null;
      }
    },
    getTenBenhKemVv5yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv5yhct) {
          this.form2.tenBenhKemVv5yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv5yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv5yhct = null;
      }
    },
    getTenBenhKemVv6yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv6yhct) {
          this.form2.tenBenhKemVv6yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv6yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv6yhct = null;
      }
    },
    getTenBenhKemVv7yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv7yhct) {
          this.form2.tenBenhKemVv7yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv7yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv7yhct = null;
      }
    },
    getTenBenhKemVv8yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv8yhct) {
          this.form2.tenBenhKemVv8yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv8yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv8yhct = null;
      }
    },
    getTenBenhKemVv9yhct(item) {
      if (item) {
        if (!this.form2.tenBenhKemVv9yhct) {
          this.form2.tenBenhKemVv9yhct = item.tenBenhBhyt;
        } else if (this.isClickBenhKem) {
          this.form2.tenBenhKemVv9yhct = item.tenBenhBhyt;
        }
      } else {
        this.form2.tenBenhKemVv9yhct = null;
      }
    },
  }
};
</script>

<style>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}
</style>
