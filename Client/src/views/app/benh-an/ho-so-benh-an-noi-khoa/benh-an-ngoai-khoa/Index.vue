<template>
  <app-wrapper :showThongTinChungBenhAn="false">
    <div class="d-flex flex-column align-center" style="background: #f8f9f9; position: relative">
      <div class="menu-top">
        <div class="d-flex justify-space-around align-center" style="
            max-width: 1080px;
            width: 100%;
            border-bottom: 2px solid #497caf;
            height: 100%;">
          <div style="flex: 1">
            <div style="font-size: 20px; font-weight: bold">Tờ bệnh án</div>
            <div class="mt-1" style="font-style: italic">
              Thông tin chi tiết tờ bệnh án
            </div>
          </div>

          <div class="d-flex">

            <v-btn color="primary" @click="handlePrint1" class="mr-4" small de depressed
              :disabled="!disableActions.export" :loading="isLoadingPrint1">
              <v-icon small left> mdi-printer </v-icon>In phần 1
            </v-btn>

            <v-btn color="primary" @click="handlePrint2" class="mr-4" small de depressed
              :disabled="!disableActions.export" :loading="isLoadingPrint2">
              <v-icon small left> mdi-printer </v-icon>In phần 2
            </v-btn>

            <v-btn color="primary" @click="handlePrint3" class="mr-4" small de depressed
              :disabled="!disableActions.export" :loading="isLoadingPrint3">
              <v-icon small left> mdi-printer </v-icon>In phần 3
            </v-btn>

            <v-btn color="primary" @click="updateData" small depressed :disabled="!disableActions.export"
              :loading="isLoading">
              <v-icon small left> mdi-pencil </v-icon>Cập nhật
            </v-btn>
            <v-btn small @click="exit" class="ml-2" color="primary"><v-icon>mdi-exit-to-app</v-icon> Thoát(F10)</v-btn>
          </div>
        </div>
      </div>

      <div class="menu-top ">
        <div class="d-flex justify-space-around align-center" style="
            max-width: 1080px;
            width: 100%;
            border-bottom: 2px solid #497caf;
            height: 100%;
          ">
          <div class="d-flex">
            <v-btn color="primary" @click="handleSign1" class="mr-4" small de depressed
              :disabled="!disableActions.export" :loading="isLoading1">
              <v-icon small left> mdi-printer </v-icon>Ký phần 1
            </v-btn>
            <v-btn color="primary" @click="handleSign2" class="mr-4" small de depressed
              :disabled="!disableActions.export" :loading="isLoading2">
              <v-icon small left> mdi-printer </v-icon>Ký phần 2
            </v-btn>
            <v-btn color="primary" @click="handleSign3" class="mr-4" small de depressed
              :disabled="!disableActions.export" :loading="isLoading3">
              <v-icon small left> mdi-printer </v-icon>Ký phần 3
            </v-btn>
            <!-- <v-btn color="primary" small de depressed class="mr-4" @click="onDownloadFile" size="medium"> <v-icon small left> mdi-printer </v-icon>Tải file đã ký</v-btn> -->
            <v-btn color="primary" small de depressed class="mr-4" @click="onPreviewFile" size="medium"> <v-icon small
                left> mdi-printer </v-icon>Xem file đã ký</v-btn>
          </div>
        </div>
      </div>

      <div class="side-content pb-4 mt-4" style="flex: 1">
        <div class="pa-4" style="flex: 1">
          <div class="mb-6">
            <TieuDe :thongTinChung="thongTinChung" />
          </div>
          <div class="mb-6" id="phan-chung">
            <PhanChung :thongTinChung="thongTinChung" v-intersect="scrollPhanChung"
              @get-ThongTinBenhAn="getThongTinBenhAn" />
          </div>
          <div class="mb-6 mt-6 pt-6" id="benh-an" v-intersect="scrollBenhAn">
            <BanhAn @get-benhAn="getBenhAn" />
          </div>
          <div class="mt-6 pt-6" id="tong-ket" v-intersect="scrollTongKet">
            <TongKetRaVien @get-tongKetRaVien="getTongKetRaVien" />
          </div>
          <div class="mt-6 pt-6" id="file-dinh-kem" v-intersect="scrollTapTin">
            <TapTinDinhKem @get-total-files="getTotalFile" />
          </div>
        </div>
      </div>
      <div class="d-flex justify-center bottom-side-benh-an">
        <div class="title-tab d-flex mt-2 px-4" v-for="item in tabs" :key="item.index" @click="scrollToElement(item)"
          :style="{
            'border-bottom': item.index == tabView ? '4px solid #2980b9' : '',
          }">
          <div class="d-flex align-center" style="height: 100%"
            :style="{ color: item.index == tabView ? '#2980B9' : '' }">
            <v-badge color="red" :content="tongFile" v-if="item.bage" small
              :dot="tongFile && tongFile > 0 ? false : true">
              <div class="">{{ item.name }}</div>
            </v-badge>
            <div v-else>{{ item.name }}</div>
          </div>
        </div>
      </div>
    </div>

  </app-wrapper>
</template>
<script>
import TieuDe from "./components/tieuDe.vue";
import PhanChung from "./components/phan-chung/index.vue";
import BanhAn from "./components/benh-an/index.vue";
import TongKetRaVien from "./components/tong-ket-ra-vien/index.vue";
import TapTinDinhKem from "./components/tap-tin/Index.vue";
import { update, printBa, printBa1, printBa2, printBa3 } from "@/api/benh-an.js";

export default {
  props: ['disableActions'],
  components: {
    TieuDe,
    PhanChung,
    BanhAn,
    TongKetRaVien,
    TapTinDinhKem,
  },
  data() {
    return {
      isLoading: false,
      isLoading1: false,
      isLoading2: false,
      isLoading3: false,
      isLoadingPrint1: false,
      isLoadingPrint2: false,
      isLoadingPrint3: false,
      form: {
        benhAn: null,
        benhAnKhamYhct: null,
        benhAnKhamYhhd: null,
        BenhAnKhoaDieuTri: null,
        benhantongketbenhan: null,
        benhAnPhauThuatPhieuPttt: null,
      },
      tabView: 1,
      tongFile: 0,
      tabs: [
        {
          name: "Phần I. Phần chung",
          index: 1,
          icon: "mdi-book-open",
          id: "phan-chung",
          bage: false,
        },
        {
          name: "Phần II. Bệnh Án",
          index: 2,
          icon: "mdi-medical-bag",
          id: "benh-an",
          bage: false,
        },
        {
          name: "Tổng kết ra viện",
          index: 3,
          icon: "mdi-lead-pencil",
          id: "tong-ket",
          bage: false,
        },
        {
          name: "File đính kèm",
          index: 4,
          icon: "mdi-lead-pencil",
          id: "file-dinh-kem",
          bage: true,
        },
      ],
      thongTinChung: {},
      idba: window.location.href.split("/").at(-1),
    };
  },
  mounted() {
    setTimeout(() => {
      this.dragElement(document.getElementById("button-action"));
    }, 800);
  },
  created() {
    window.addEventListener("keydown", this.handlerKeyPress, false);
  },

  methods: {
    handlerKeyPress(e) {
      if (e.key == 'F10') {
        location.href = `${window.origin}/HSBADS/Index`;
        e.preventDefault();
      }
    },
    exit() {
      location.href = `${window.origin}/HSBADS/Index`;
    },
    scrollToElement(item) {
      this.tabView = item.index;
      const element = document.getElementById(item.id);
      element.scrollIntoView({ behavior: "smooth" });
    },
    scrollBenhAn(entries, observer) {
      if (entries[0].isIntersecting) {
        this.tabView = 2;
      }
    },
    scrollTongKet(entries, observer) {
      if (entries[0].isIntersecting) {
        this.tabView = 3;
      }
    },
    scrollPhanChung(entries, observer) {
      if (entries[0].isIntersecting) {
        this.tabView = 1;
      }
    },
    scrollTapTin(entries, observer) {
      if (entries[0].isIntersecting) {
        this.tabView = 4;
      }
    },
    getTotalFile(tongFile) {
      this.tongFile = tongFile;
    },
    dragElement(elmnt) {
      var pos1 = 0,
        pos2 = 0,
        pos3 = 0,
        pos4 = 0;
      if (document.getElementById("button-action")) {
        /* if present, the header is where you move the DIV from:*/
        document.getElementById("button-action").onmousedown = dragMouseDown;
      } else {
        /* otherwise, move the DIV from anywhere inside the DIV:*/
        elmnt.onmousedown = dragMouseDown;
      }

      function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();
        // get the mouse cursor position at startup:
        pos3 = e.clientX;
        pos4 = e.clientY;
        document.onmouseup = closeDragElement;
        // call a function whenever the cursor moves:
        document.onmousemove = elementDrag;
      }

      function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        // calculate the new cursor position:
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        // set the element's new position:
        elmnt.style.top = elmnt.offsetTop - pos2 + "px";
        elmnt.style.left = elmnt.offsetLeft - pos1 + "px";
      }

      function closeDragElement() {
        /* stop moving when mouse button is released:*/
        document.onmouseup = null;
        document.onmousemove = null;
      }
    },
    getThongTinBenhAn(benhAn, benhAnkdt, benhAnPhauThuatPhieuPttt) {
      this.form.benhAn = benhAn;
      this.form.BenhAnKhoaDieuTri = benhAnkdt;
      this.form.benhAnPhauThuatPhieuPttt = benhAnPhauThuatPhieuPttt;
    },
    getBenhAn(benhAnTienSuBenh, benhAnYhhd, benhAnYhct) {
      this.form.benhAnKhamYhct = benhAnYhct;
      this.form.benhAnKhamYhhd = benhAnYhhd;
      this.form.benhAnTienSuBenh = benhAnTienSuBenh;
    },
    getTongKetRaVien(data) {
      console.log(data);
      this.form.benhantongketbenhan = data;
      this.form.benhAn = data;
     
      
    },
    async updateData() {
      const id = window.location.href.split("/").at(-1);
      try {
        this.isLoading = true
        await update(id, this.form);
        this.$message({
          message: "Cập nhật thành công.",
          type: "success",
        });
      } catch (error) {
        console.log(error);
        this.$message.error("Thất bại");
      } finally {
        this.isLoading = false
      }
    },
    handlePrint() {
      return printBa(this.idba);
    },

    handlePrint1() {
      return printBa1(this.idba) + "_1";
    },
    async handlePrint1() {
      const host = `${window.location.protocol}//${window.location.hostname}${window.location.port ? ":" + window.location.port : ""
        }`;
      try {
        window.open(
          `${host}/api/benh-an/${this.idba}/print-ba-file1/${this.idba}_1.pdf`,
          "_blank"
        );
      } catch (error) {
        console.log(error);
      }
      // return printBa1(this.idba) + "_1";
    },

    async handlePrint2() {
      const host = `${window.location.protocol}//${window.location.hostname}${window.location.port ? ":" + window.location.port : ""
        }`;
      try {
        window.open(
          `${host}/api/benh-an/${this.idba}/print-ba-file2/${this.idba}_2.pdf`,
          "_blank"
        );
      } catch (error) {
        console.log(error);
      }
      // return printBa1(this.idba) + "_1";
    },
    async handlePrint3() {
      const host = `${window.location.protocol}//${window.location.hostname}${window.location.port ? ":" + window.location.port : ""
        }`;
      try {
        window.open(
          `${host}/api/benh-an/${this.idba}/print-ba-file3/${this.idba}_3.pdf`,
          "_blank"
        );
      } catch (error) {
        console.log(error);
      }
      // return printBa1(this.idba) + "_1";
    },

    async handleSign1() {
      const host = `${window.location.protocol}//${window.location.hostname}${window.location.port ? ":" + window.location.port : ""
        }`;

      try {
        this.isLoading1 = true
        const fileSignUrl = `${host}/api/benh-an/${this.idba}/print-ba-file1/${this.idba}_1.pdf`;
        window.open(`${host}/client/sample/Demo.htm?fileSignUrl=${fileSignUrl}`);
        window.open(fileSignUrl);
        this.isLoading1 = false
      } catch (error) {
        console.log(error);
        this.isLoading1 = false
      }
    },
    async handleSign2() {
      const host = `${window.location.protocol}//${window.location.hostname}${window.location.port ? ":" + window.location.port : ""
        }`;

      try {
        this.isLoading2 = true
        const fileSignUrl = `${host}/api/benh-an/${this.idba}/print-ba-file2/${this.idba}_2.pdf`;
        window.open(`${host}/client/sample/Demo.htm?fileSignUrl=${fileSignUrl}`);
        window.open(fileSignUrl);
        this.isLoading2 = false
      } catch (error) {
        console.log(error);
        this.isLoading2 = false
      }
    },
    async handleSign3() {
      const host = `${window.location.protocol}//${window.location.hostname}${window.location.port ? ":" + window.location.port : ""
        }`;

      try {
        this.isLoading3 = true
        const fileSignUrl = `${host}/api/benh-an/${this.idba}/print-ba-file3/${this.idba}_3.pdf`;
        window.open(`${host}/client/sample/Demo.htm?fileSignUrl=${fileSignUrl}`);
        window.open(fileSignUrl);
        this.isLoading3 = false
      } catch (error) {
        console.log(error);
        this.isLoading3 = false
      }
    },

    // handleSign1() {
    //       const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
    //       const fileSignUrl =  `${window.origin}/api/benh-an/${this.idba}/print-ba-file1/${this.idba}_1.pdf`
    //       window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    // },
    // handleSign2() {
    //   const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
    //       const fileSignUrl =  `${window.origin}/api/benh-an/${this.idba}/print-ba-file2/${this.idba}.pdf`
    //       window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    // },

    // handleSign3() {
    //   const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
    //       const fileSignUrl =  `${window.origin}/api/benh-an/${this.idba}/print-ba-file3/${this.idba}.pdf`
    //       window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    // },

    // handleSign1() {
    //       const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
    //       const fileSignUrl =  `${window.origin}/api/benh-an/${this.idba}/print-ba-file1/${this.idba}.pdf`
    //       window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    // },
    // handleSign2() {
    //   const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
    //       const fileSignUrl =  `${window.origin}/api/benh-an/${this.idba}/print-ba-file2/${this.idba}.pdf`
    //       window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    // },

    // handleSign3() {
    //   const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
    //       const fileSignUrl =  `${window.origin}/api/benh-an/${this.idba}/print-ba-file3/${this.idba}.pdf`
    //       window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    // },
    // handlePrint2() {
    //   return printBa2(this.idba);
    // },
    // handlePrint3() {
    //   return printBa3(this.idba);
    // },
  },
};
</script>
<style scoped>
.bottom-side-benh-an {
  background: #f1f7fb;
  height: 40px;
  z-index: 20;
  position: sticky;
  bottom: 0px;
  width: 100%;
  border-top: 1px solid #d7dbdd;
}

.side-content {
  max-width: 1080px;
  border: 1px solid #e5e7e9;
  background: white;
}

.title-tab {
  cursor: pointer;
  font-weight: 500;
  color: black;
  font-size: 13px;
}

.title-tab:hover {
  background: #d6eaf8;
}

.menu-top {
  background: white;
  height: 80px;
  width: 100%;
  top: 0;
  z-index: 20;
  position: sticky;
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  align-items: center;
}
</style>
