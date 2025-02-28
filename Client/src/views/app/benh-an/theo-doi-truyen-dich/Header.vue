<template>
  <v-row>
    <v-col cols="12">
      <div class="d-flex mr-auto">
        <div class="mr-1 align-center">
          <v-avatar color="primary" size="30" tile>
            <v-icon small dark>mdi-table</v-icon>
          </v-avatar>
        </div>
        <div class="mt-1 ml-2" style="font-size: 19px; font-weight: bold">
          {{ title }}
        </div>
        <v-spacer></v-spacer>
        <v-btn small class="ml-2" color="primary" dark @click="print">
          <v-icon small class="mr-1">mdi-printer</v-icon>
          In phiếu
        </v-btn>


        <v-btn small class="ml-2" color="primary" dark @click="kySo">
          <v-icon small class="mr-1">mdi-printer</v-icon>
          Ký Số
        </v-btn>
        <v-btn
          small
          @click="$emit('update:isSearching', !isSearching)"
          class="ml-2"
          color="primary"
        >
          <v-icon small class="mr-1"
            >mdi-{{ isSearching ? "filter-remove" : "filter" }}</v-icon
          >
          Filter
        </v-btn>
      </div>
    </v-col>
  </v-row>
</template>
<script>
import headerMixin from "@/mixins/crud/header";
import { print } from "@/api/thong-tin-truyen-dich";
export default {
  components: {},
  mixins: [headerMixin],
  data: () => ({}),
  methods: {
    async print() {
      const idba = window.location.href.split("/").at(-1);
      await print(idba);
    },

    kySo() {
         const idba = window.location.href.split("/").at(-1);
      const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port: '')
          const fileSignUrl =  `${window.origin}/api/benh-an-theo-gioi-truyen-dich/${idba}/print-ba-file/PhieuTruyenDich_idba${idba}.pdf`
          window.open(host + "/client/sample/Demo.htm?fileSignUrl=" + fileSignUrl)
    },
    showFormCopy() {},
  },
};
</script>