<template>
    <Fragment>
      
        <el-form-item prop="ngayPT" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-ngayPT`
                : `${crudId}-creating-ngayPT`
                ">
                <el-date-picker v-model="form.ngayPT" type="datetime" size="small" placeholder="dd/mm/yyyy hh:mm"
                    style="width: 100%" format="dd/MM/yyyy HH:mm" value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
            </portal>
        </el-form-item>

        <el-form-item prop="phuongPhapVoCam" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-phuongPhapVoCam`
                : `${crudId}-creating-phuongPhapVoCam`
                ">
                <el-input v-model="form.phuongPhapVoCam" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="bspt" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `editing-${this.currentEditingRowId}-bspt`
                : 'creating-bspt'
                ">
                <base-select-async v-model="form.bSpT" :label="(item) =>
                    `${item.maNv}-${item.hoTen}-${item.khoa ? item.khoa.tenKhoa : ''}`
                    " keyValue="maNv" :apiFunc="getDsPhuaThuatVien" >
                </base-select-async>
            </portal>
        </el-form-item>
        <el-form-item prop="bsgayMe" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `editing-${this.currentEditingRowId}-bsgayMe`
                : 'creating-bsgayMe'
                ">
                <base-select-async v-model="form.bSGayMe" :label="(item) =>
                    `${item.maNv}-${item.hoTen}-${item.khoa ? item.khoa.tenKhoa : ''}`
                    " keyValue="maNv" :apiFunc="getDsBSGayMe" >
                </base-select-async>
            </portal>
        </el-form-item>
    </Fragment>
</template>

<script>
import { getNhanVien } from "@/api/phieu-kham-benh-vao-vien.js";
export default {
    props: {
        form: Object,
        crudId: String,
        currentEditingRowId: String,
        isEditForm: {
            type: Boolean,
            default: false,
        },
    },
    data() {
        return {
        };
    },
    watch: {
        currentEditingRowId() {

        },
    },
    created() {

    },
    methods: {
        async getDsPhuaThuatVien(params) {
            return await getNhanVien(params);
        },
        async getDsBSGayMe(params) {
            return await getNhanVien(params);
        },
      
    },
};
</script>