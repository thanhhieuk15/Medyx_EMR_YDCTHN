<template>
  <el-dialog :visible.sync="dialogVisible" width="1000px" top="1vh">
    <template slot="title">
      <div class="px-3">
        <div style="font-size: 16px; font-weight: bold" class="mb-3">
          {{
            edit
              ? "Cập nhật phiếu thông tin thăm dò chức năng"
              : "Thêm mới phiếu thông tin thăm dò chức năng"
          }}
        </div>
        <v-progress-linear
          color="primary"
          rounded
          value="100"
          height="2"
        ></v-progress-linear>
      </div>
    </template>
    <el-form :model="form" :rules="rules" ref="form" class="px-3">
      <v-row>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="1. Khoa" prop="khoa">
            <el-input size="small" v-model="form.khoa"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="2. Tên dịch vụ khám">
            <el-input size="small" v-model="form.dichVuKham"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="3. Ngày chỉ định">
            <el-date-picker
              format="dd/MM/yyyy"
              value-format="yyyy-MM-ddTHH:mm:ss"
              v-model="form.ngayChiDinh"
              placeholder="VD: 09/08/2022"
              size="small"
              style="width: 100%"
            ></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="4. Bác sĩ chỉ định">
            <base-select-async
              placeholder="Tìm kiếm theo tên"
              v-model="form.bacSiChiDinh"
              :label="(item) => `${item.hoTen}`"
              keyValue="maNv"
              :apiFunc="getNhanVien"
              style="width: 100%"
              size="small"
            ></base-select-async>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="5. Khoa phòng thực hiện" prop="maKhoa">
            <base-select-async
              placeholder="Tìm kiếm theo mã khoa"
              v-model="form.maKhoa"
              :label="(item) => `${item.maKhoa} - ${item.tenKhoa}`"
              keyValue="maKhoa"
              :apiFunc="getKhoa"
              style="width: 100%"
              size="small"
            ></base-select-async>
          </el-form-item>
        </v-col>
        <v-col cols="4" class="padding-cols">
          <el-form-item label="8. Ngày tiếp nhận">
            <el-date-picker
              format="dd/MM/yyyy"
              value-format="yyyy-MM-ddTHH:mm:ss"
              v-model="form.ngayTiepNhan"
              placeholder="VD: 09/08/2022"
              size="small"
              style="width: 100%"
            ></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="6. Kỹ thuật">
            <el-input
              type="textarea"
              rows="2"
              size="small"
              v-model="form.kyThuat"
            ></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="6. Mô tả">
            <el-input
              type="textarea"
              rows="2"
              size="small"
              v-model="form.moTa"
            ></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12" class="padding-cols">
          <el-form-item label="6. Kết luận" prop="ketLuan">
            <el-input
              type="textarea"
              rows="2"
              size="small"
              v-model="form.ketLuan"
            ></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="3" class="padding-cols">
          <el-form-item label="7. Bác sĩ chuyên khoa">
            <base-select-async
              placeholder="Tìm kiếm theo tên"
              v-model="form.bacSiChuyenKhoa"
              :label="(item) => `${item.hoTen}`"
              keyValue="maNv"
              :apiFunc="getNhanVien"
              style="width: 100%"
              size="small"
            ></base-select-async>
          </el-form-item>
        </v-col>
        <v-col cols="3" class="padding-cols">
          <el-form-item label="8. Ngày thực hiện">
            <el-date-picker
              format="dd/MM/yyyy"
              value-format="yyyy-MM-ddTHH:mm:ss"
              v-model="form.ngayThucHien"
              placeholder="VD: 09/08/2022"
              size="small"
              style="width: 100%"
            ></el-date-picker>
          </el-form-item>
        </v-col>
        <v-col cols="6" class="padding-cols">
          <el-form-item label="6. Đường link xem kết quả từ hệ thống PACS">
            <el-input size="small" v-model="form.link"></el-input>
          </el-form-item>
        </v-col>
        <v-col cols="12">
          <div style="height: 200px">
            <v-progress-linear
              color="primary"
              rounded
              value="100"
              height="1"
            ></v-progress-linear>
            <div style="font-size: 14px; font-weight: bold" class="mt-4">
              <v-icon>mdi-attachment</v-icon>
              Tệp đính kèm
            </div>
            <div
              style="
                display: flex;
                justify-content: space-between;
                align-items: center;
              "
            >
              <div class="ma-5">
                <input
                  name="file"
                  ref="upload-image"
                  style="display: none"
                  type="file"
                  multiple
                  @change="handleUpload($event)"
                />
                <v-tooltip
                  bottom
                  v-if="tienTrinhUpload == 0 || tienTrinhUpload == 100"
                >
                  <template v-slot:activator="{ on, attrs }">
                    <div
                      class="box-file d-flex align-center justify-center flex-column"
                      v-bind="attrs"
                      v-on="on"
                      @click="clickUpload"
                      v-if="!loading"
                    >
                      <v-icon large color="#8c939d"> mdi-upload </v-icon>
                      <div style="font-size: 12px" class="mt-1">
                        Tải lên tập tin
                      </div>
                    </div>
                    <div
                      class="box-file d-flex align-center justify-center mr-6 ml-2"
                      v-bind="attrs"
                      v-on="on"
                      v-else
                    >
                      <v-progress-circular
                        :rotate="-90"
                        :size="50"
                        :width="15"
                        :value="tienTrinhUpload"
                        color="primary"
                      >
                        {{ tienTrinhUpload }}
                      </v-progress-circular>
                    </div>
                  </template>
                  <span>{{
                    !loading ? "Thêm tập tin" : "Đang tải lên..."
                  }}</span>
                </v-tooltip>
              </div>
              <div style="width: 720px">
                <div class="mb-2 pl-2">Danh sách file phi cấu trúc:</div>
                <div v-if="!files" style="height: 160px; overflow-y: scroll">
                  <div v-for="(item, index) in files" :key="index" class="pl-3">
                    <div class="files">
                      <div class="d-flex align-center">
                        <div style="width: 20px">{{ index + 1 }}</div>
                        <v-icon class="mr-3 ml-3">mdi-file</v-icon>
                        {{ item.name }}
                      </div>
                      <div class="d-flex">
                        <v-tooltip bottom class="mr-4">
                          <template v-slot:activator="{ on, attrs }">
                            <v-icon
                              color="rgba(0, 0, 0, 0.54)"
                              dark
                              v-bind="attrs"
                              v-on="on"
                              @click="
                                getDownloadFile(item.idba, item.stt, item.name)
                              "
                            >
                              mdi-download
                            </v-icon>
                          </template>
                          <span>Tải xuống</span>
                        </v-tooltip>
                        <v-tooltip bottom class="mr-3 ml-3">
                          <template v-slot:activator="{ on, attrs }">
                            <v-icon
                              color="rgba(0, 0, 0, 0.54)"
                              dark
                              v-bind="attrs"
                              v-on="on"
                              class="ml-3"
                            >
                              mdi-delete
                            </v-icon>
                          </template>
                          <span>Xóa</span>
                        </v-tooltip>
                      </div>
                    </div>
                    <div
                      style="
                        border-bottom: 0.5px solid #d9d9d9;
                        width: 100%;
                        height: 0px;
                      "
                    ></div>
                  </div>
                </div>
                <div
                  v-else
                  style="height: 160px"
                  class="d-flex align-center justify-center"
                >
                  <div>
                    <v-icon
                      large
                      color="#8c939d"
                      class="d-flex align-center justify-center mb-2"
                      >mdi-file-find</v-icon
                    >
                    <div>Không có tệp đính kèm nào</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </v-col>
      </v-row>
    </el-form>
    <span slot="footer" class="pr-3 pb-4">
      <el-button size="small" @click="dialogVisible = false">Hủy</el-button>
      <el-button
        size="small"
        icon="el-icon-plus"
        type="primary"
        @click="submit('form')"
        >{{ edit ? "Cập nhật" : "Thêm mới" }}</el-button
      >
    </span>
  </el-dialog>
</template>
<script>
import * as apiNhanVien from "@/api/nhan-vien.js";
import * as apiKhoa from "@/api/khoa.js";

import {
  uploadFile,
  getListFile,
  downloadFile,
} from "@/api/file-phi-cau-truc.js";
import { saveAs } from "file-saver";

export default {
  data() {
    return {
      tienTrinhUpload: 0,
      loading: false,
      files: null,
      dialogVisible: false,
      edit: true,
      form: {
        khoa: null,
        dichVuKham: null,
        ngayChiDInh: null,
        bacSiChiDinh: null,
        maKhoa: null,
        ngayTiepNhan: null,
        kyThuat: null,
        moTa: null,
        ketLuan: null,
        bacSiChuyenKhoa: null,
        ngayThucHien: null,
        link: null,
      },
      rules: {
        khoa: [
          { required: true, message: "Phải chọn khoa", trigger: "change" },
        ],
        maKhoa: [
          { required: true, message: "Phải chọn khoa", trigger: "change" },
        ],
        ketLuan: [
          { required: true, message: "Phải chọn khoa", trigger: "change" },
        ],
      },
    };
  },
  mounted() {
    this.getDsFile();
  },
  methods: {
    clickUpload() {
      this.$refs["upload-image"].click();
    },
    showFormAdd() {
      this.form = {
        khoa: null,
        dichVuKham: null,
        ngayChiDInh: null,
        bacSiChiDinh: null,
        maKhoa: null,
        ngayTiepNhan: null,
        kyThuat: null,
        moTa: null,
        ketLuan: null,
        bacSiChuyenKhoa: null,
        ngayThucHien: null,
        link: null,
      };
      this.dialogVisible = true;
      this.edit = false;
    },
    async getNhanVien(params) {
      return await apiNhanVien.index(params);
    },
    async getKhoa(params) {
      return await apiKhoa.index(params);
    },
    submit(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          alert("submit!");
        } else {
          console.log("error submit!!");
          return false;
        }
      });
      this.dialogVisible = false;
    },
    async getDsFile() {
      const id = window.location.href.split("/").at(-1);
      let data = await getListFile({
        idba: id,
        loaiTaiLieu: 11,
      });
      this.files = data.data;
      this.files.forEach((item) => {
        item.name = item.link.split("\\").at(-1);
      });
      this.$emit("get-total-files", this.files ? this.files.length : 0);
    },
    async handleUpload(e) {
      this.loading = true;
      const idba = window.location.href.split("/").at(-1);
      this.tienTrinhUpload = 0;
      let file = e.target.files;
      var isValidate = true;
      let data = new FormData();
      if (this.file) {
        return;
      }
      data.append("file", file[0]);
      data.append("idba", idba);
      data.append("loaiTaiLieu", 11);

      if (!isValidate) return;

      try {
        await uploadFile(data, (e) => {
          this.tienTrinhUpload = e;
          this.loading = false;
        });
        this.$message({
          message: "Tải lên thành công",
          type: "success",
        });
        this.getDsFile();
      } catch (error) {
        console.log(error);
        this.$message({
          message: "Tải lên không thành công",
          type: "error",
        });
      }
      this.$refs["upload-image"].value = null;
    },
    async getDownloadFile(idba, stt, name) {
      try {
        let data = await downloadFile(idba, stt);
        var blob = new Blob([data]);
        saveAs.saveAs(blob, `${name}`);
      } catch (error) {
        console.log(error);
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

label {
  margin-bottom: -3px !important;
}

.el-form-item {
  margin-bottom: 0px !important;
}

.el-form-item__label {
  font-size: 13px;
}

.el-form-item__content {
  line-height: 35px;
}
.files {
  padding: 10px;
  width: 100%;
  color: rgb(88, 88, 88);
  display: flex;
  align-items: center;
  justify-content: space-between;
  cursor: pointer;
}
.files:hover {
  background: rgba(231, 231, 231, 0.616);
  font-weight: 500;
}
.box-file {
  cursor: pointer;
  border: 2px dashed #d9d9d9;
  width: 150px;
  height: 150px;
}
.box-file:hover {
  border: 2px dashed #2874a6;
  width: 150px;
  height: 150px;
}
</style>
