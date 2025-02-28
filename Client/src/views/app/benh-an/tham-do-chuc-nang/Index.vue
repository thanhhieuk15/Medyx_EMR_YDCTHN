<template>
  <Crud :fields="fields" :apiCrudFunctions="apiCrudFunctions" :apiCategoryFunctions="apiCategoryFunctions"
    :headerComponent="headerComponent" :disableActions="disableActions" :actions="actions" primaryKey="stt"
    label-width="100px" title="Thông tin thăm dò chức năng" tableHeight="calc(100vh - 400px)" />
</template>

<script>
import Crud from "@/components/crud/Index.vue";
import Header from "./Header.vue";
import { index, print, destroy, printDigitalSig } from "@/api/benh-an-can-lam-san.js";
import { formatDate, formatDatetime } from "@/utils/formatters";
export default {
  props: {
    id: {
      type: Number,
    },
    permission: {
      type: Array,
      default: () => [],
    },
  },
  components: {
    Crud,
  },
  data(vm) {
    return {
      headerComponent: Header,
      fields: [
        {
          text: "Số thứ tự",
          // sortable: true,
          showable: true,
          type: "text",
          value: "stt",
          width: 150,
        },
        {
          text: "Khoa",
          value: "khoaDieuTri.khoa.tenKhoa",
          sortable: true,
          showable: true,
          filterable: true,
          type: "text",
          width: 120,
        },
        {
          text: "Ngày TH",
          sortable: true,
          showable: true,
          value: "ngayYlenh",
          formatter: function (_, __, value) {
            return formatDatetime(value);
          },
          width: 150,
        },
        {
          text: "Tên dịch vụ",
          sortable: true,
          showable: true,
          value: "dichVu.tenDv",
          width: 400,
        },
        {
          text: "Bác sĩ chỉ định",
          sortable: true,
          showable: true,
          value: "bsChiDinh.hoTen",
          width: 150,
        },
        {
          text: "kết luận",
          sortable: true,
          showable: true,
          value: "benhAnClsKq.ketLuan",
          type: "text",
          width: 150,
        },
        {
          text: "BS TH 1",
          sortable: true,
          showable: true,
          value: "benhAnClsKq.bschuyenKhoa.hoTen",
          type: "text",
          width: 170,
        },
        {
          text: "Ngày giờ trả kết quả",
          sortable: true,
          showable: true,
          value: "benhAnClsKq.ngayKq",
          formatter: function (_, __, value) {
            return formatDate(value);
          },
          width: 170,
        },
        {
          text: "Hủy",
          sortable: false,
          showable: true,
          value: "huy",
          type: "boolean",
          align: "center",
          width: 70,
        },
        {
          text: "Ngày lập",
          sortable: true,
          showable: true,
          formatter: function (_, __, value) {
            return formatDate(value);
          },
          value: "ngayLap",
          width: 150,
        },
        {
          text: "Người lập",
          showable: true,
          value: "nguoiLap.hoTen",
          type: "text",
          width: 150,
        },
        {
          text: "Ngày sử dụng",
          showable: true,
          value: "ngaySD",
          formatter: function (_, __, value) {
            return formatDate(value);
          },
          type: "text",
          width: 150,
        },
        {
          text: "Người sử dụng",
          showable: true,
          value: "nguoiSD.hoTen",
          type: "text",
          width: 150,
        },
        {
          text: "Ngày Hủy",
          showable: true,
          value: "ngayHuy",
          formatter: function (_, __, value) {
            return formatDate(value);
          },
          type: "text",
          width: 150,
        },
        {
          text: "Người Hủy",
          showable: true,
          value: "nguoiHuy.hoTen",
          type: "text",
          width: 150,
        },
      ],

      apiCategoryFunctions: {},

      actions: [
        "delete",
        "print",
        "detail:/HSBADS/ChiTietThamDoChucNang/:idba/chi-tiet/:stt",
        "sign",
        "printDigitalSig"
      ],
      disableActions: {
        edit: (item) => (item.huy && !vm.currentUser.is_admin) || item.idhis,
        delete: (item) =>
          item.huy ||
          !vm.permission.find(
            (e) => e.ActionDetailsName == "/HSBA/ThongTinThamDoChucNang/delete"
          ) || item.idhis,
        print: (item) =>
          item.huy ||
          !vm.permission.find(
            (e) => e.ActionDetailsName == "/HSBA/ThongTinThamDoChucNang/export"
          ),
        sign: (item) =>
          item.huy ||
          !vm.permission.find(
            (e) => e.ActionDetailsName == "/HSBA/ThongTinThamDoChucNang/export"
          ),
        printDigitalSig: (item) => item.huy,
      },
    };
  },
  methods: {
    printDigitalSig(...item) {
      const loaiTaiLieu = 11;
      // const response = printDigitalSig(this.id, ...item, loaiTaiLieu)
      const response =  printDigitalSig(this.id, item[1].stt,item[1].sttkhoa,loaiTaiLieu);
      if (response && response.data.data) {
        window.open(response.data.data, "_blank");
      } else {
        alert("Không có file đã ký nào!");
        console.log("Không có dữ liệu:", response);
      }
    }
  },
  computed: {
    apiCrudFunctions() {
      const maChungLoai = 4;
      const loaiTaiLieu = 11;
      return {
        index: (params) =>
          index({ ...params, idba: this.id, MaChungLoai: maChungLoai }),
        print: (...item) => print(this.id, ...item, maChungLoai, loaiTaiLieu),
        destroy: (...item) => destroy(this.id, ...item, maChungLoai),
        sign: item => {
          const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
          const fileSignUrl = `${window.origin}/api/benh-an-cls/${this.id}/print-ba-file/${item.stt}/BACLS_stt_${item.stt}.pdf`
          window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
        },
        printDigitalSig: (...item) => this.printDigitalSig(...item,loaiTaiLieu),
      };
    },
  },
};
</script>
<style></style>
