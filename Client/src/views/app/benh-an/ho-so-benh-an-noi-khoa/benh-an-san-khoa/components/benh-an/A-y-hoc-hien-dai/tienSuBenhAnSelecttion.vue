<template>
    <Fragment>
        <!-- <portal :to="`${crudId}-actions-creating`">
            <v-btn @click="createData" color="success" icon x-small>
                <v-icon dark>mdi-check</v-icon></v-btn>
            <v-btn @click="$emit('form-reset')" color="primary" icon x-small>
                <v-icon dark>mdi-close</v-icon>
            </v-btn>
        </portal> -->
        <!-- <el-form-item prop="stt" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-stt`
                : `${crudId}-creating-stt`
                ">
                <el-input v-model="form.stt" placeholder="STT" disabled size="small"></el-input>
            </portal>
        </el-form-item> -->

        <el-form-item prop="soLanCoThai" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-soLanCoThai`
                : `${crudId}-creating-soLanCoThai`
                ">
                <el-input v-model="form.soLanCoThai" placeholder="Số lần có thai" size="small"></el-input>
            </portal>
        </el-form-item>

        <el-form-item prop="nam" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-nam`
                : `${crudId}-creating-nam`
                ">
                <el-input v-model="form.nam" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="deDuThang" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-deDuThang`
                : `${crudId}-creating-deDuThang`
                ">
                <el-input v-model="form.deDuThang" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="deThieuThang" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-deThieuThang`
                : `${crudId}-creating-deThieuThang`
                ">
                <el-input v-model="form.deThieuThang" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="say" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-say`
                : `${crudId}-creating-say`
                ">
                <el-input v-model="form.say" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="hut" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-hut`
                : `${crudId}-creating-hut`
                ">
                <el-input v-model="form.hut" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="nao" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-nao`
                : `${crudId}-creating-nao`
                ">
                <el-input v-model="form.nao" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="coVac" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-coVac`
                : `${crudId}-creating-coVac`
                ">
                <el-input v-model="form.coVac" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="chuaNgoaiTC" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-chuaNgoaiTC`
                : `${crudId}-creating-chuaNgoaiTC`
                ">
                <el-input v-model="form.chuaNgoaiTC" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="chuaTrung" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-chuaTrung`
                : `${crudId}-creating-chuaTrung`
                ">
                <el-input v-model="form.chuaTrung" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="thaiChetLuu" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-thaiChetLuu`
                : `${crudId}-creating-thaiChetLuu`
                ">
                <el-input v-model="form.thaiChetLuu" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="conSong" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-conSong`
                : `${crudId}-creating-conSong`
                ">
                <el-input v-model="form.conSong" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="canNang" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-canNang`
                : `${crudId}-creating-canNang`
                ">
                <el-input v-model="form.canNang" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="phuongPhapDe" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-phuongPhapDe`
                : `${crudId}-creating-phuongPhapDe`
                ">
                <el-input v-model="form.phuongPhapDe" size="small"></el-input>
            </portal>
        </el-form-item>
        <el-form-item prop="taiBien" :style="{ 'margin-bottom': 0 }">
            <portal :to="isEditForm
                ? `${crudId}-editing-${this.currentEditingRowId}-taiBien`
                : `${crudId}-creating-taiBien`
                ">
                <el-input v-model="form.taiBien" size="small"></el-input>
            </portal>
        </el-form-item>
    </Fragment>
</template>

<script>
import { getBenhAnTienSuSan,storeTienSuSan } from "@/api/benh-an.js";
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
        
    },
};
</script>