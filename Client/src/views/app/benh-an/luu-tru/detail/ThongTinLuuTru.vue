<template>
  <v-container fluid class="mb-4">
    <el-form size="medium" ref="form" :model="form" label-position="top" :rules="rules">
      <v-row>
        <v-col cols="3" class="py-0">
          <el-form-item prop="soLuuTru" style="width: 100%">
            <el-input style="width: 100%" v-model="form.soLuuTru" placeholder="Số lưu trữ"
              :disabled="daLuuTru"></el-input> </el-form-item></v-col>

        <v-col cols="3" class="py-0">
          <el-form-item prop="`nguoiLuuTru`" style="width: 100%">
            <base-select-async v-model="form.nguoiLuuTru" placeholder="Người lưu trữ" keyValue="maNv" label="hoTen"
              :apiFunc="getNhanVien" style="width: 100%" size="medium" :disabled="daLuuTru"></base-select-async>
          </el-form-item></v-col>
        <v-col cols="3" class="py-0">
          <el-form-item prop="ngayLuuTru" style="width: 100%">
            <el-date-picker v-model="form.ngayLuuTru" type="date" placeholder="Ngày lưu trữ" style="width: 100%"
              :format="'dd/MM/yyyy'" :value-format="'yyyy-MM-dd'" :disabled="daLuuTru">
            </el-date-picker> </el-form-item></v-col>
        <v-col cols="12" class="py-0">
          <el-button @click="$emit('regenerate')" :disabled="daLuuTru" type="primary" size="small">Tạo lại bản in
            HSBA</el-button>
          <el-button type="success" :disabled="daLuuTru" @click="submitForm" size="medium">Lưu trữ</el-button>
          <el-button type="success" @click="submitExport" size="medium">Kết xuất</el-button>
          <el-button type="success" @click="onDownloadFile" size="medium">Tải file đã ký</el-button>
          <el-button type="primary" @click="onPreviewFile" size="medium">Xem file đã ký</el-button>
        </v-col>

        <!-- <el-form-item>
            <el-button
            style="margin-top: 30px;"
            :disabled="false"
            type="primary"
            size="small"
            >
           
            Chọn File Ký </el-button
          >
          </el-form-item> -->

        <!-- 
          <el-form-item style="width: 100%">

           

            <el-input
              style="width: 100%"
              placeholder="Đường dẫn tới file cần ký"
              :disabled="true"
            ></el-input> </el-form-item> -->


        <!-- <el-form-item >
              <el-select
                size="small"
                
                placeholder="Loại chứng thư"
              >
                <el-option
                  
                >
                </el-option>
              </el-select>

              <el-button
            
            :disabled="false"
            type="primary"
            size="small"
            >
           
            Tải loại chứng thư </el-button
          >
            </el-form-item> -->


        <!-- <el-form-item style="width: 100%">
            <el-input
              style="width: 100%"
              placeholder="Nhà Cung Cấp"
              :disabled="true"
            ></el-input> </el-form-item> -->

        <!-- <el-form-item style="width: 100%">
            <el-input
              style="width: 100%"
              placeholder="Số serial của chứng thư số"
              :disabled="true"
            ></el-input> </el-form-item> -->

        <!-- 
            <el-form-item style="width: 100%">
            <el-input
              style="width: 100%"
              placeholder="Hạn Sử Dụng"
              :disabled="true"
            ></el-input> </el-form-item> -->
        <!--           
            <el-button
            @click="test"
            :disabled="false"
            type="primary"
            size="medium"
            >
           
            Ký </el-button> -->
        <div>




        </div>
      </v-row>
    </el-form>
  </v-container>
</template>

<script>
import { getNhanVien } from "@/api/phieu-kham-benh-vao-vien";
import { getThongTinLuuTru, luuTru, khoiPhuc } from "@/api/luu-tru";
import { exportHSBA, DownloadFile, PreviewFile } from "@/api/tap-tin-dinh-kem.js"



export default {
  props: ["id", "danhSachLoaiTaiLieu"],
  data() {
    return {

      getNhanVien,
      loading: null,
      daLuuTru: false,
      date: null,
      rules: {
        soLuuTru: [
          {
            required: true,
            message: "Trường này bắt buộc nhập",
            trigger: "blur",
          },
        ],

        nguoiLuuTru: [
          {
            required: true,
            message: "Trường này bắt buộc nhập",
            trigger: "blur",
          },
        ],
        ngayLuuTru: [
          {
            required: true,
            message: "Trường này bắt buộc nhập",
            trigger: "blur",
          },
        ],
      },
      form: {
        soLuuTru: "",
        xacNhanLuuTru: 1,
        nguoiLuuTru: null,
        ngayLuuTru: new Date().toLocaleDateString("en-CA"),
      },
    };
  },
  computed: {},
  methods: {
    async getThongTinLuuTru() {
      try {
        const { data } = await getThongTinLuuTru(this.id);
        if (!data) {
          this.daLuuTru = false;
          return;
        }
        for (const field in this.form) {
          this.form[field] = data[field];
        }
        this.daLuuTru = true;
      } catch (error) {
        console.log(error)
      }
    },

    //thư viện bkav


    validate() {
      return new Promise((resolve) => {
        this.$refs["form"].validate((valid) => {
          resolve(valid);
        });
      });
    },
    async submitForm() {
      const valid = await this.validate();
      if (!valid) return;
      this.loading = this.$loading({
        lock: true,
        text: "Đang lưu trữ bệnh án",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      await luuTru(this.id, this.form);
      window.location = "/HSBAPH/Detail/" + this.id;
    },
    async submitExport() {
      try {
        console.log("Kết xuất:", this.id);
        await exportHSBA(this.id);
      } catch (error) {

      }

    },
    async onPreviewFile() {
      try {
        const response = await PreviewFile(this.id);
        if (response == undefined) {
          console.error('Không có file đã ký nào');
          alert('Không có file đã ký nào');
        } else {
          window.open(
            `${window.origin}/api/benh-an/xuat-file-Da-Ky/${this.id}`,
            "_blank"
          );
        }
        console.log(response);
        // if (response) {
        //   const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
        //   window.open(
        //     `${host}/Storage/DownloadFolder/`+response.data,
        //     "_blank"
        //   );
        // }
      } catch (error) {
        console.log(error)
      }
    },
    async forceFileDownload(blob, title) {
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', title);
      document.body.appendChild(link);
      link.click();
      window.URL.revokeObjectURL(url);
    },

    async onDownloadFile() {
      try {
        const response = await DownloadFile(this.id);

        if (!response.ok) {
          throw new Error(`Failed to download file: ${response.statusText}`);
        }

        const blob = await response.blob();
        const contentDisposition = response.headers.get('Content-Disposition');
        const now = new Date();
        const formattedDate = now.getFullYear() +
          '-' + String(now.getMonth() + 1).padStart(2, '0') +
          '-' + String(now.getDate()).padStart(2, '0') +
          '_' + String(now.getHours()).padStart(2, '0') +
          '-' + String(now.getMinutes()).padStart(2, '0') +
          '-' + String(now.getSeconds()).padStart(2, '0');
        let fileName = 'HSBA_' + formattedDate;

        if (contentDisposition) {
          const matches = contentDisposition.match();
          //const matches = contentDisposition.match(/filename="?([^"]+)"?/);
          if (matches && matches[1]) {
            fileName = matches[1];
          }
        }
        this.forceFileDownload(blob, fileName);
      } catch (error) {
        console.error('Error downloading file:', error);
      }
    },
    async restore() {
      this.loading = this.$loading({
        lock: true,
        text: "Đang khôi phục bệnh án",
        spinner: "el-icon-loading",
        background: "rgba(0, 0, 0, 0.7)",
      });
      await khoiPhuc(this.id);
      window.location.reload();
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },

    nextPage() {
      const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
      window.open(host + "/client/sample/Demo.htm")
    },
    BkavSignature() {
      const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
      window.open(host + "/client/Signbase64.html")
    }
  },
  created() {
    // console.log("CheckPlugin", BkavCAPlugin,CheckPluginValid,value)


  },
};
</script>
