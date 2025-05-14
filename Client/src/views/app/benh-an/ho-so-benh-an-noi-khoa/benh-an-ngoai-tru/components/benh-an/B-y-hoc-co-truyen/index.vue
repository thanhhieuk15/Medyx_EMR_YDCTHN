<template>
  <div class="pt-2">
    <div style="font-size: 16px; font-weight: bold" class="mb-3 mt-4">
      B. Y HỌC CỔ TRUYỀN
    </div>
    <el-form>
      <v-row>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="I. Vọng chẩn">
            <el-input v-model="form.moTaVongChan" type="textarea"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols">
          <el-form-item label="II. Văn chấn">
            <el-input v-model="form.moTaVanChan" type="textarea"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols">
          <el-form-item label="III. Vấn chẩn">
            <el-input v-model="form.mtvaanChan" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="IV. Thiết chẩn">
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="1. Xúc chẩn">
            <el-input v-model="form.moTaXucChan" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="2. Mạch chẩn">
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="Mạch tay trái:">
            <el-input v-model="form.machTrai" type="textarea"></el-input>
          </el-form-item>
        </v-col>

        <v-col cols="12" class="padding-cols">
          <el-form-item label="Mạch tay phải :">
            <el-input v-model="form.machPhai" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="V.Tóm tắt tứ chẩn">
            <el-input v-model="form.tomTatTuChan" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="VI.Biện chứng luận trị">
            <el-input v-model="form.bienChungLuanTri" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="VII. CHẨN ĐOÁN">
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="1. Bệnh danh">
            <base-select-async v-model="form.maBenhDanhTheoYhct"
              :label="(item) => `${item.maBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh" :apiFunc="getChanDoanBenhCT"
              placeholder="" style="width: 100%" size="small">
            </base-select-async>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="2. Bát cương:">
            <el-input v-model="form.batCuong" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="3.Nguyên nhân:">
            <el-input v-model="form.moTaNguyenNhan" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="3.Tạng phủ :">
            <el-input v-model="form.moTaTangPhu" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="4.Kinh mạch :">
            <el-input v-model="form.moTaKinhMach" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="6. Định vị bệnh (dinh, vệ, khí, huyết):">
            <el-input v-model="form.moTaDinhViBenhTheo" type="textarea"></el-input>
          </el-form-item>
        </v-col>

        <div style="font-size: 16px; font-weight: bold" class="mb-3 mt-4">
          C. ĐIỀU TRỊ
        </div>
      
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="I. Y HỌC CỔ TRUYỀN">
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="1. Pháp điều trị :">
            <el-input v-model="form.ppdtyhct" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="2. Phương">
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="Phương dược :">
            <el-input v-model="form.phuongDuoc" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="Phương pháp điều trị không dùng thuốc :">
            <el-input v-model="form.ppdtkhongDungThuoc" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="II. Y HỌC HIỆN ĐẠI: Hướng điều trị (Phương pháp điều trị, chế độ dinh dưỡng, chế độ chăm sóc,	):">
            <el-input v-model="form.huongDtyhhd" type="textarea"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" lg="12" md="12" class="padding-cols">
          <el-form-item label="III. Dự hậu (Tiền lượng) :">
            <el-input v-model="form.tienLuong" type="textarea"></el-input>
          </el-form-item>
        </v-col>
      </v-row>
    </el-form>
  </div>
</template>

<script>
import * as apiBenhTatCT from "@/api/benh-tat-yhct.js";
import { getKhamYhct, getKhamYhhd } from "@/api/benh-an.js";

export default {
  data: () => ({
    form: {
      bienChungLuanTri: null,
      maBenhDanhTheoYhct: null,
      batCuong: null,
      moTaNguyenNhan: null,
      moTaTangPhu: null,
      moTaKinhMach: null,
      moTaDinhViBenhTheo: null,
      moTaVongChan: null,
      moTaVanChan: null,
      moTaXucChan: null,
      mtvaanChan: null,
      phuongDuoc: null,
      phuongHuyet: null,
      ppdtkhongDungThuoc: null,
      ppdtyhct: null,
      ppkhac: null,
      tomTatTuChan: null,
      tienLuong: null,
      huongDtyhhd: null,
      machTrai: null,
      machPhai: null,
    },
  }),
  watch: {
    form: {
      handler(val) {
        this.$emit('get-phanB', val)
      },
      deep: true
    },
  },
  mounted() {
    this.getData();
  },
  methods: {
    async getChanDoanBenhCT(params) {
      return await apiBenhTatCT.index(params);
    },
    async getData() {
      const id = window.location.href.split("/").at(-1);
      let data = await getKhamYhct(id);
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key];
        }
      }
      data = await getKhamYhhd(id);
      for (let key in this.form) {
        if (data && data.data && data.data.hasOwnProperty(key)) {
          this.form[key] = data.data[key]
        }
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
