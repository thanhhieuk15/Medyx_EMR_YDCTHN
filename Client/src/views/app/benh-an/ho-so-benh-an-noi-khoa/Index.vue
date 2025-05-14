<template>
  <div>
    <BenhAnNoiTru v-if="loaiBenhAn == 1" :disableActions="disableActions" />
    <BenhAnNgoaiTru v-if="loaiBenhAn == 2" :disableActions="disableActions" />
    <BenhAnManTinh v-if="loaiBenhAn == 3" :disableActions="disableActions" />
    <BenhAnNoiTru v-if="loaiBenhAn == 4" :disableActions="disableActions" />
    <BenhAnNoiTru v-if="loaiBenhAn == 5" :disableActions="disableActions" />
    <BenhAnNgoaiKhoa v-if="loaiBenhAn == 6" :disableActions="disableActions" />
    <BenhAnNhi v-if="loaiBenhAn == 7" :disableActions="disableActions" />
    <BenhAnNhiKhoa v-if="loaiBenhAn == 8" :disableActions="disableActions" />
    <BenhAnTruyenNhiem v-if="loaiBenhAn == 9" :disableActions="disableActions" />
    <BenhAnSanKhoa v-if="loaiBenhAn == 10" :disableActions="disableActions" />
    <BenhAnPhuKhoa v-if="loaiBenhAn == 11" :disableActions="disableActions" />
    <BenhAnDaLieu v-if="loaiBenhAn == 12" :disableActions="disableActions" />
    <BenhAnRangHamMat v-if="loaiBenhAn == 13" :disableActions="disableActions" />
    <BenhAnTaiMuiHong v-if="loaiBenhAn == 14" :disableActions="disableActions" />
    <BenhAnNgoaiTruRangHamMat v-if="loaiBenhAn == 16" :disableActions="disableActions" />
    <BenhAnNgoaiTruPHCN v-if="loaiBenhAn == 17" :disableActions="disableActions" />
  </div>
</template>

<script>
import BenhAnNoiTru from "./benh-an-noi-tru/Index.vue";
import BenhAnNgoaiTru from "./benh-an-ngoai-tru/Index.vue";
import BenhAnManTinh from "./benh-an-man-tinh/Index.vue";
import BenhAnNgoaiKhoa from "./benh-an-ngoai-khoa/Index.vue";
import BenhAnNhi from "./benh-an-nhi/index.vue";
import BenhAnNhiKhoa from "./benh-an-nhi-khoa/index.vue";
import BenhAnTruyenNhiem from "./benh-an-truyen-nhiem/index.vue";
import BenhAnSanKhoa from "./benh-an-san-khoa/index.vue"
import BenhAnPhuKhoa from "./benh-an-phu-khoa/Index.vue"
import BenhAnDaLieu from "./benh-an-da-lieu/Index.vue"
import BenhAnRangHamMat from "./benh-an-rang-ham-mat/Index.vue"
import BenhAnTaiMuiHong from "./benh-an-tai-mui-hong/Index.vue"
import BenhAnNgoaiTruRangHamMat from "./benh-an-chuyen-khoa-rang-ham-mat/Index.vue"
import BenhAnNgoaiTruPHCN from "./benh-an-ngoai-tru-phuc-hoi-chuc-nang/Index.vue"
import { getLoai } from "@/api/benh-an.js";
export default {
  props: {
    id: {
      type: [Number, String],
      required: true,
    },
    permission: {
      type: Array,
      default: () => [],
    },
  },
  components: {
    BenhAnNgoaiTruRangHamMat,
    BenhAnSanKhoa,
    BenhAnPhuKhoa,
    BenhAnNoiTru,
    BenhAnNgoaiTru,
    BenhAnManTinh,
    BenhAnNgoaiKhoa,
    BenhAnNhi,
    BenhAnNhiKhoa,
    BenhAnTruyenNhiem,
    BenhAnDaLieu,
    BenhAnRangHamMat,
    BenhAnTaiMuiHong,
    BenhAnNgoaiTruPHCN
},
  data: (vm) => ({
    // loaiBenhAn: 3,
    loaiBenhAn: null,
    disableActions: {
      modify: vm.permission.find(
          (e) => e.ActionDetailsName == "/HSBA/tobenhan/modify"
        ),
      export: vm.permission.find(
          (e) => e.ActionDetailsName == "/HSBA/tobenhan/export"
        ),
    },
  }),
  mounted() {
    this.getLoaiBenhAn();
  },
  methods: {
    async getLoaiBenhAn() {
      let data = await getLoai(this.id);
      this.loaiBenhAn = data.data.ma
    },
  },
};
</script>

<style></style>
