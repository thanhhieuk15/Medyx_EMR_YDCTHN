<template>


  <div class="pl-2">
    <div class="mb-2">
      <div class="d-flex">
        <div class="body-1 font-weight-bold ml-1">Danh sách tờ điều trị</div>





        <el-checkbox class="ml-auto" v-model="checkAll" label="Tất cả"></el-checkbox>
      </div>
      <div class="mt-2 pa-0">
        <v-toolbar dense outlined class="mt-1 boxLag">
          <el-input placeholder="Tìm kiếm ..." prefix-icon="el-icon-search" v-model="searchValue" size="mini"
            class="mr-2">
          </el-input>
          <v-spacer></v-spacer>
          <v-btn icon small @click="handleExports()" :disabled="!permission.find(
          (e) => e.ActionDetailsName == '/HSBA/todieutri/export'
        )
          ">
            <v-icon title="Xuất dữ liệu">mdi-printer</v-icon>
          </v-btn>
          <v-btn icon small @click="onAdd()" class="ml-2" title="Thêm mới" :disabled="!permission.find(
          (e) => e.ActionDetailsName == '/HSBA/todieutri/create'
        )
          ">
            <v-icon>mdi-plus-circle-outline</v-icon>
          </v-btn>
        </v-toolbar>
      </div>
    </div>

    <div class="overflow-auto" style="width: 300px; max-height: calc(100vh - 300px)">
      <div v-for="(item, index) in menus" :key="`menu_${index}`" class="py-2 pl-2 fill-width menu-item" :style="item.active
            ? 'background-color: #ebebeb'
            : 'background-color: #ffffff'
          ">
        <div class="d-flex mr-1">
          <div class="flex-grow-0">
            <el-checkbox class="pa-0" v-model="item.check" hide-details :disabled="item.huy"></el-checkbox>
          </div>
          <div class="d-flex flex-column flex-grow-1 ml-1 mr-1">
            <div class="d-flex ml-1">
              <div class="text-caption font-weight-bold" :style="item.active ? ' color:#6495ED' : 'color:black'"
                @click="handleChoseTab(index, item)" v-if="!item.huy">
                {{ item.stt || null }}. {{ item.khoaDieuTri.tenKhoa || null }}
              </div>
              <div class="text-caption font-weight-bold text-decoration-line-through"
                :style="item.active ? ' color:red' : 'color:red'" @click="handleChoseTab(index, item)" v-else>
                {{ item.stt || null }}. {{ item.khoaDieuTri.tenKhoa || null }}
              </div>

              <div class="ml-auto">
                <div class="d-flex">
                  <el-button @click="onUpdate(item)" style="padding: 1px" title="Cập nhật" v-if="!item.huy" :disabled="!permission.find(
          (e) => e.ActionDetailsName == '/HSBA/todieutri/modify'
        )
          ">
                    <v-icon style="font-size: initial" small>mdi-pencil</v-icon>
                  </el-button>
                  <el-button class="ml-1" @click="onPrint(item)" style="padding: 1px" title="In" v-if="!item.huy"
                    :disabled="!permission.find(
          (e) => e.ActionDetailsName == '/HSBA/todieutri/export'
        )
          ">
                    <i class="el-icon-printer" style="font-size: initial"></i></el-button>



                  <el-button class="ml-1" @click="onPreview(item)" style="padding: 1px" title="Preview" v-if="!item.huy"
                    :disabled="!permission.find(
          (e) => e.ActionDetailsName == '/HSBA/todieutri/export'
        )
          ">
                    <i class="el-icon-view" style="font-size: initial"></i></el-button>
                  <el-button class="ml-1" @click="onPrintDigitalSig(item)" style="padding: 1px" title="In file đã ký"
                    v-if="!item.huy" :disabled="!permission.find(
          (e) => e.ActionDetailsName == '/HSBA/todieutri/export'
        )
          ">
                    <i class="el-icon-printer" style="font-size: initial"></i></el-button>


                  <!-- <el-button
                    class="ml-1"
                    @click="Sign(item)"
                    style="padding: 1px"
                    title="Preview"
                    v-if="!item.huy"
                    :disabled="
                      !permission.find(
                        (e) => e.ActionDetailsName == '/HSBA/todieutri/export'
                      )
                    "
                  >
                    <i class="el-icon-edit" style="font-size: initial"></i
                  ></el-button> -->


                  <el-dialog title="Preview tài liệu" :visible.sync="dialogVisible" width="80%" v-if="document"
                    top="10px">
                    <embed :style="{
          width: '100%',
          height:
            document.type === 'application/pdf'
              ? 'calc(100vh - 150px)'
              : 'unset',
        }" :src="document.link" />
                  </el-dialog>

                </div>
              </div>
              <div class="mb-2" @click="handleChoseTab(index, item)">
                <div class="d-flex">
                  <div class="text-caption black--text ml-1">
                    Thời gian :
                    <span class="font-weight-bold">
                      {{ formatDatetime(item.ngayYLenh) }}
                    </span>
                  </div>
                </div>
                <div class="d-flex">
                  <div class="text-caption black--text ml-1">
                    <span class="font-weight-bold">
                      {{ item.bsdieuTri.hoTen || null }}
                    </span>
                  </div>
                </div>
              </div>
            </div>

          </div>
        </div>
      </div>
    </div>
    <tDTDialog ref="form" @reload="eventDone" :idba="idba" />
    <tDTDialogUpdate ref="formUpDate" @reload="eventDone" :idba="idba" />
  </div>
</template>
<script>
import { formatDatetime } from "@/utils/formatters";
import { destroy, exportTDT,printDigitalSig } from "@/api/to-dieu-tri";
import debounce from "@/utils/debounce";
import tDTDialog from "./dialog/tDTDialog.vue";
import tDTDialogUpdate from "./dialog/tDTDialogUpdate.vue";
import ThongTinLuuTru from "../luu-tru/detail/ThongTinLuuTru.vue";
import moment from 'moment';
export default {
  components: { tDTDialog, tDTDialogUpdate },
  data: (vm) => ({
    tab: 0,
    dialogVisible: false,
    document: null,
    searchValue: null,
    formatDatetime,
    search: false,
    checkAll: false,
    menuShow: [],
  }),
  watch: {
    searchValue: debounce(function (newVal) {
      this.$emit("on:Search", newVal);
    }, 1000),
    checkAll(val) {
      this.menus.map((e) => {
        if (!e.huy) e.check = val;
      });
    },
  },
  computed: {
    menuProcessed() {
      if (!searchValue) {
        return this.menus;
      } else {
      }
    },
    sortedItems() {
      const sortedArrr = [...this.item]
      sortedArrr.sort((a, b) => new Date(b.ngayYLenh) - new Date(a.ngayYLenh))
      return sortedArrr
    }
  },
  props: {
    menus: {
      type: Array,
      default: () => [],
    },
    idba: {
      type: String,
    },
    permission: { type: Array },
  },
  methods: {
    onAdd() {
      this.add = !this.add;
      this.$refs.form.open();
    },
    handleChoseTab(index, item) {
      this.$emit("on:choseTab", item);
      this.menus.forEach((element, i) => {
        element.active = index == i ? true : false;
      });
    },
    async handleExports() {
      let arr = this.menus.filter((e) => e.check);
      try {
        if (!arr.length) {
          this.$message.info("Chưa chọn tờ bệnh án nào !");
          return;
        }
        var listStt = arr.map((x) => x.stt);
        const res = await this.$confirm(
          "Xác nhận tải xuống danh sách tờ điều trị có số thứ tự là : " +
          listStt.join(),
          {
            title: "Xác nhận tải xuống",
            buttonTrueText: "Đồng ý",
            buttonFalseText: "Hủy",
          }
        );
        if (res) {
          let tempt = listStt;
          const fileName = "TodieuTri";
          var params = "";
          for (let index = 0; index < listStt.length; index++) {
            if (index != listStt.length - 1) {
              params += `Stt[]=${listStt[index]}&`;
            } else {
              params += `Stt[]=${listStt[index]}`;
            }
          }
          if (this.resetCheck()) {
            await exportTDT(this.idba, fileName, params);
          }
          return;
        }
      } catch (error) {
        console.log(error);
      }
    },
    async onPrint(item) {
      try {
        if (!item.stt || !item.idba) {
          return;
        }
        const res = await this.$confirm(
          "Xác nhận In tờ điều trị có số thứ tự là : " + item.stt,
          {
            title: "Xác nhận In",
            buttonTrueText: "Đồng ý",
            buttonFalseText: "Hủy",
          }
        );
        if (res) {
          if (this.resetCheck()) {
            await exportTDT(item.idba, item.idba, `Stt[]=${item.stt}`);
          }
          return;
        }
      } catch (error) {
        console.log(error);
      }
    },
    async onPrintDigitalSig(item) {
      try {
        if (!item.stt || !item.idba) {
          return;
        }
        const res = await this.$confirm(
          "Xác nhận In phiếu đã ký số có số thứ tự là : " + item.stt,
          {
            title: "Xác nhận In",
            buttonTrueText: "Đồng ý",
            buttonFalseText: "Hủy",
          }
        );
        if (res) {
          if (this.resetCheck()) {
            const formattedDateTime = moment(item.ngayYlenh).subtract(1, 'days').format('DD-MM-YYYY HH:mm');
            const response = await printDigitalSig(item.idba, item.stt, item.sttkhoa, formattedDateTime);
            if (response && response.data.data) {
              window.open(response.data.data, "_blank");
            } else {
              alert("Không có file đã ký nào!");
              console.log("Không có dữ liệu:", response);
            }
          }
          return;
        }
      } catch (error) {
        console.log(error);
      }
    },
    async onPreview(item) {
      try {
        if (!item.stt || !item.idba) {
          return;
        }
        this.document = {
          link: `${window.origin}/api/benh-an-to-dieu-tri/${item.idba}/print-ba-file/ToDieuTri.pdf?Stt[]=${item.stt}`,
          type: 'application/pdf'
        }
        this.dialogVisible = true
      } catch (error) {
        console.log(error);
      }
    },

    async Sign(item) {
      try {
        if (!item.stt || !item.idba) {
          return;
        }
        const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
        const fileSignUrl = `http://localhost:5000/BaoCao/KyHSBABC`
        window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
        this.dialogVisible = true
      } catch (error) {
        console.log(error);
      }
    },
    example(item) {


      const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
      const fileSignUrl = ``
      const nampe = "http://192.1.0.25:8000/UI/QLHSBA/CKSPDFEMR.aspx?fileSignUrl="
      window.open(nampe + fileSignUrl)

    },
    nextPage() {
      const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
      window.open(host + "/client/Signbase64.html?&filePath=6_" + this.idba)
    },

    resetCheck() {
      this.menus.map((e) => {
        e.check = false;
      });
      return true;
    },
    eventDone() {
      this.$emit("reset");
    },
    onUpdate(item) {
      this.$refs.formUpDate.open(item);
    },
  },
};
</script>

<style scoped>
.menu-item {
  cursor: pointer;
  border-bottom: 1px solid beige;
  border-radius: 5px;
}

.menu-item:hover {
  background-color: aliceblue;
}

.isActive {
  background-color: aquamarine;
}

.v-checkbox {
  margin: 0px !important;
}

.v-toolbar {
  box-shadow: none !important;
  padding: 0px 0px !important;
}

.boxLag {
  position: relative;
}

.poti {
  position: absolute;
  top: 13px;
  left: 3px;
}
</style>
