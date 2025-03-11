<template>
    <div>
        <div style="font-size: 16px; font-weight: bold" class="mb-3">
            A. Y HỌC HIỆN ĐẠI
        </div>
        <el-form :model="form" :rules="rules" ref="form">
            <v-row>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="I. LÝ DO VÀO VIỆN">
                        <el-input v-model="form.lyDoVv" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>

                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Vào ngày thứ">
                        <el-input v-model="form.vaoNgayThu" dense outlined type="number" :min="0"
                            size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="II. Hỏi bệnh">
                        <div>1. Quá trình bệnh lý:(khởi phát,diễn biến, chẩn đoán, điều trị tuyến dưới v.v...)</div>
                        <el-input v-model="form.benhSu" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="2. Tiền sử bệnh"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item class="item-form"
                        label="Bản thân: (Phát triển thể lực từ nhỏ đến lớn, những bệnh đã mắc, phương pháp điều trị, tiêm phòng, ăn uống, sinh hoạt v.v ..)">
                        <el-input v-model="form.tienSuBanThan" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item
                        label="2. Gia đình:((Những người trong gia đình: bệnh đã mắc, đời sống, tinh thần, vật chất v.v.)">
                        <el-input v-model="form.tienSuGiaDinh" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="3. Quá trình sinh trưởng"></el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Con thứ mấy ">
                        <el-input v-model="form.conThu" type="number" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Tiền thai (Para) ">
                        <el-input v-model="form.para" type="text" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item>
                        <div class="title-group-checkbox mr-4">Tình trạng khi sinh</div>
                        <el-checkbox @change="setModelFormCheckbox(tinhTrangSinh)"
                            v-for="(item, index) in tinhTrangSinh" :key="index" v-model="item.checkbox">{{ item.ten }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Cân nặng lúc sinh">
                        <el-input v-model="form.canNangSinh" type="number" size="mini" :step="0.01"
                            controls-position="right" style="width: 100%"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">Dị tật bấm sinh</div>
                        <el-radio-group v-model="form.diTatBamSinh">
                            <el-radio :label="1">Có</el-radio>
                            <el-radio :label="2">Không</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Cụ thể dị tật bẩm sinh:">
                        <el-input v-model="form.cuTheDiTat" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item class="item-form" label="Phát triển về tinh thần:">
                        <el-input v-model="form.phatTrienTinhThan" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item class="item-form" label="Phát triển về vận động:">
                        <el-input v-model="form.phatTrienVanDong" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item class="item-form" label="Các bệnh lý khác:">
                        <el-input v-model="form.benhLyKhac" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item>
                        <div class="title-group-checkbox mr-4">Nuôi dưỡng</div>
                        <el-checkbox @change="setModelFormCheckboxNuoiDuong(nuoiDuong)"
                            v-for="(item, index) in nuoiDuong" :key="index" v-model="item.checkbox">{{ item.ten }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Cai sữa tháng thứ:">
                        <el-input v-model="form.thangCaiSua" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item>
                        <div class="title-group-checkbox mr-4">Chăm sóc:</div>
                        <el-checkbox @change="setModelFormCheckboxChamSoc(chamSoc)"
                            v-for="(item, index) in chamSoc" :key="index" v-model="item.checkbox">{{ item.ten }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item>
                        <div class="title-group-checkbox mr-4">Đã tiêm chủng:</div>
                        <el-checkbox @change="setModelFormCheckboxDTC(daTiemChung)"
                            v-for="(item, index) in daTiemChung" :key="index" v-model="item.checkbox">{{ item.ten }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Cụ thể những bệnh khác được tiêm chủng:">
                        <el-input v-model="form.tiemChungBenhKhac" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>

                <v-col cols="12" class="padding-cols">
                    <el-form-item label="III. KHÁM BỆNH"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item>
                        <div>
                            <div style="font-weight: 500">1.Toàn thân:</div>
                            <div style="font-style:italic; line-height: 8px;" class="pb-4">
                                (Ý thức, da niêm mạc, hệ thống hạch, tuyến giáp, vị trí, kích
                                thước, số lượng, di động...)
                            </div>
                        </div>
                        <el-input v-model="form.toanThan" type="textarea" rows="4"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12">
                    <v-row>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Mạch (lần/phút):" prop="mach">
                                <el-input type="number" v-model="form.mach" size="mini" controls-position="right"
                                    style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Nhiệt độ (℃):">
                                <el-input type="number" v-model="form.nhietDo" size="mini" :step="0.01"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Huyết áp (mmHg):">
                                <el-input v-model="form.huyetAp" size="mini" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Nhịp thở (lần/phút):">
                                <el-input type="number" v-model="form.nhipTho" size="mini" controls-position="right"
                                    style="width: 100%">
                                </el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Cân Nặng (kg):">
                                <el-input type="number" v-model="form.canNang" size="mini" :step="0.01"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Chiều cao (cm):">
                                <el-input type="number" v-model="form.chieuCao" size="mini" :step="0.01"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Vòng ngực (cm):">
                                <el-input type="number" v-model="form.vongNguc" size="mini" :step="0.01"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="Vòng đầu (cm):">
                                <el-input type="number" v-model="form.vongDau" size="mini" :step="0.01"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="3" class="padding-cols">
                            <el-form-item label="BMI:">
                                <el-input type="number" v-model="form.bmi" size="mini" :step="0.001"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                    </v-row>
                </v-col>
                <v-col cols="12" class="padding-cols mt-4">
                    <el-form-item label="2.Các cơ quan:"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Tuần hoàn:">
                        <el-input v-model="form.tuanHoan" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Hô hấp:"></el-form-item>
                    <el-input v-model="form.hoHap" type="textarea"></el-input>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Tiêu hóa:">
                        <el-input v-model="form.tieuHoa" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Thận-Tiết niệu-Sinh dục:">
                        <el-input v-model="form.thanTnieuSduc" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Thần kinh:">
                        <el-input v-model="form.thanKinh" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Cơ - xương - khớp:">
                        <el-input v-model="form.xuongKhop" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Tai- Mũi- Họng, Răng-Hàm-Mặt, Mắt, Dinh dưỡng và các bệnh lý khác:">
                        <el-input v-model="form.cacBenhLyKhac" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="3. Các xét nghiệm cận lâm sàng cần làm">
                        <el-input v-model="form.canLamSang" type="textarea" rows="5"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="4 Tóm tắt bệnh án">
                        <el-input v-model="form.tomTatBenhAn" type="textarea" rows="6"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="IV. Chẩn đoán khi vào khoa điều trị"></el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh chính</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.benhChinh.maBenh"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
                            @get-item="getTenBenhChinhVv" :firstEmitGetItem="false">
                        </base-select-async>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv1"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
                            @get-item="getTenBenhKem1Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item label="Phân biệt:">
                        <base-select-async v-model="form.tenBenhPhanBiet"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" size="small">
                        </base-select-async>
                    </el-form-item>
                </v-col>

                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Tiên Lượng">
                        <el-input v-model="form.tienLuong" type="textarea"></el-input>
                    </el-form-item>
                </v-col>

                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Hướng Dẫn điều trị">
                        <el-input v-model="form.huongDT" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
            </v-row>
        </el-form>
    </div>
</template>

<script>
import * as apiBenhTat from "@/api/benh-tat.js";
import store from '@/store'
import { getSelectBox } from '@/api/danh-muc.js';
import { chiTietToBenhAn, getTienSuBenh, getKhamYhhd, getTongKetBenhAn, getKhoaDieuTri, getKhamYhct } from "@/api/benh-an.js";
import { getChiTietPhieuKhamVaoVien } from "@/api/phieu-kham-benh-vao-vien";

export default {
    data() {
        var checkMach = (rule, value, callback) => {
            if (!value) {
                return callback(new Error("Phải nhập mạch"));
            }
            setTimeout(() => {
                if (value > 226 || value < 0) {
                    callback(new Error("Giá trị nhập lớn hơn 0 và nhỏ hơn 226"));
                } else {
                    callback();
                }
            }, 1000);
        };
        return {
            form: {
                benhSu: null,
                bmi: null,
                canLamSang: null,
                canNang: null,
                canNangNhi: null,
                deDuThang: null,
                deKho: null,
                deNgatTho: null,
                rungRon: null,
                anDuoi1Tuoi: null,
                anTren1Tuoi: null,
                thangCaiSua: null,
                thangLay: null,
                thangBo: null,
                thangDi: null,
                thangNoi: null,
                thangMocRang: null,
                tuoiCoKinh: null,
                chiSoHamax: null,
                daTiemChung:null,
                benhDaMac: null,
                dacDienSH: null,
                cdchamSoc: null,
                vaoNgayThu: null,
                benhSu: null,
                vongDau: null,
                vongNguc: null,
                cddinhDuong: null,
                chieuCao: null,
                dtketHopYhhd: null,
                hoHap: null,
                huyetAp: null,
                idba: null,
                kqcdha: null,
                kqdt: null,
                kqxnmau: null,
                kqxnnuocTieu: null,
                lyDoVv: null,
                mach: null,
                mat: null,
                nhietDo: null,
                nhipTho: null,
                noiTietDd: null,
                rangHamMat: null,
                taiMuiHong: null,
                tenBenhPhanBiet: null,
                thanKinh: null,
                thanTnieuSduc: null,
                tieuHoa: null,
                toanThan: null,
                tomTatBenhAn: null,
                tuanHoan: null,
                xuongKhop: null,
                conThu: null,
                moTaTienSu: null,
                tienSuBanThan: null,
                tienSuGiaDinh: null,
                dacDiemLienQuanBenh: null,
                tienLuong: null,
                huongDanDieuTri: null,
                tinhTrangSinh: null,
                diTatBamSinh: null,
                cuTheDiTat: null,
                para: null,
                phatTrienTinhThan: null,
                phatTrienVanDong: null,
                benhLyKhac: null,
                nuoiDuong: null,
                chamSoc: null,
                tiemChungBenhKhac: null,               
                benhChinh: {
                    maBenh: null,
                    tenBenh: null,
                },
                benhKem1: {
                    maBenh: null,
                    tenBenh: null,
                },
                benhKem2: {
                    maBenh: null,
                    tenBenh: null,
                },
                benhKem3: {
                    maBenh: null,
                    tenBenh: null,
                },
                tenBenhKemVv1: null,
                maBenhKemVv1: null,
                tenBenhKemVv2: null,
                maBenhKemVv2: null,
                tenBenhKemVv3: null,
                maBenhKemVv3: null,
                maBenhKemVv4: null,
                tenBenhKemVv4: null,
                maBenhKemVv5: null,
                tenBenhKemVv5: null,
                maBenhKemVv6: null,
                tenBenhKemVv6: null,
                maBenhKemVv7: null,
                tenBenhKemVv7: null,
                maBenhKemVv8: null,
                tenBenhKemVv8: null,
                maBenhKemVv9: null,
                tenBenhKemVv9: null,
                isClickBenhKem: false,
                canNangSinh: null
            },
            
            benh_khac: [
                {
                    ten_benh: null,
                    ma_benh: null,
                },
            ],
            nuoiDuong: [
                { ten: "Sữa mẹ", checkbox: false },
                { ten: "Nuôi nhân tạo", checkbox: false },
                { ten: "Hỗn hợp", checkbox: false },
            ],
            tinhTrangSinh: [
                { name: "Đẻ thường", checkbox: false },
                { name: "Forceps", checkbox: false },
                { name: "Giác hút", checkbox: false },
                { name: "Đẻ phẫu thuật", checkbox: false },
                { name: "Đẻ chỉ huy", checkbox: false },
                { name: "Khác", checkbox: false }
            ],
            chamSoc: [
                { ten: "Tại vườn trẻ", checkbox: false },
                { ten: "Tại nhà", checkbox: false },
               
            ],
            daTiemChung: [
                { name: "Lao", checkbox: false },
                { name: "Bại Liệt", checkbox: false },
                { name: "Sởi", checkbox: false },
                { name: "Ho gà", checkbox: false },
                { name: "Uốn ván", checkbox: false },
                { name: "Bạch Hầu", checkbox: false },
                { name: "Tiêm chủng khác", checkbox: false }
            ],
            rules: {
                mach: [
                    {
                        validator: checkMach,
                        trigger: "blur",
                    },
                ],
            }
        }
    },
    mounted() {
        this.getData();
    },
    watch: {
        form: {
            handler(val) {
                this.$emit('get-phanA', val)
                store.dispatch("hosobenhan/setChanDoanYHHD", val)
            },
            deep: true
        },
        "form.canNang": function (val) {
            if (val && this.form.chieuCao) {
                this.form.bmi = (val / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
            }
        },
        "form.chieuCao": function (val) {
            if (this.form.canNang && val) {
                this.form.bmi = (this.form.canNang / ((val / 100) * (val / 100))).toFixed(2)
            }
        },
        CHANDOAN: {
            handler(val) {
                // console.log(22222, val)
                for (let key in this.form) {
                    if (val && val.hasOwnProperty(key)) {
                        this.form[key] = val[key]
                    }
                }
            },
            deep: true
        }
    },
    computed: {
        CHANDOAN() {
            return this.$store.state.hosobenhan.dataChanDoan
        },
    },
    methods: {
        onChangeBenhKem() {
            this.isClickBenhKem = true;
        },
        async getChanDoanBenh(params) {
            return await apiBenhTat.index(params);
        },
        async getData() {
            const id = window.location.href.split("/").at(-1);
            let data = await getKhamYhhd(id);
            for (let key in this.form) {
                if (data && data.data && data.data.hasOwnProperty(key)) {
                    this.form[key] = data.data[key]
                }
            }
            data = await getTienSuBenh(id, { getModelNull: true })
            console.log(data.data)
            for (let key in this.form) {
                if (data && data.data && data.data.hasOwnProperty(key)) {
                    this.form[key] = data.data[key]

                }
            }
            this.form.canNangSinh = data.data.CanNang;
           
            // data = await getSelectBox({ MaParent: '017' });
            // this.conThu = this.setCheckbox(this.form.conThu, data.data.conThu)
            if (this.form.canNang && this.form.chieuCao) {
                this.form.bmi = (this.form.canNang / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
            }
            data = await getSelectBox({ MaParent: '207' });
            this.daTiemChung = this.setCheckbox(this.form.chiSoHamax, data.data)
            data = await getSelectBox({ MaParent: '204' });
            this.tinhTrangSinh = this.setCheckbox(this.form.tinhTrangSinh, data.data)
            data = await getSelectBox({ MaParent: '205' });
            this.nuoiDuong = this.setCheckbox(this.form.nuoiDuong, data.data)
            data = await getSelectBox({ MaParent: '206' });
            this.chamSoc = this.setCheckbox(this.form.chamSoc, data.data)
            console.log(this.chiSoHamax)
            data = await getChiTietPhieuKhamVaoVien(id)
            if (this.form.lyDoVv == null) {
                this.form.lyDoVv = data.data.lyDoVv
            }
            if (this.form.canLamSang == null) {
                this.form.canLamSang = data.data.tomTatKqcls
            }
        },
        setCheckbox(model, checkBoxArray) {
            if (model) {
                let arrModel = model.split(",")
                arrModel = arrModel.map(item => item = item.trim())
                return checkBoxArray.map(item => {
                    item.checkbox = false;
                    if (arrModel.includes(item.ma)) {
                        item.checkbox = true;
                    }
                    console.log(item)
                    return item
                })
            }
            return checkBoxArray
        },
        setModelFormCheckbox(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.tinhTrangSinh = ma;
        },
        setModelFormCheckboxNuoiDuong(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.nuoiDuong = ma;
        },
        setModelFormCheckboxChamSoc(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.chamSoc = ma;
        },
        setModelFormCheckboxDTC(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.chiSoHamax = ma;
        },
        getTenBenhKem1Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv1) {
                    this.form.tenBenhKemVv1 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv1 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv1 = null;
            }
        },
        getTenBenhKem2Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv2) {
                    this.form.tenBenhKemVv2 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv2 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv2 = null;
            }
        },
        getTenBenhKem3Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv3) {
                    this.form.tenBenhKemVv3 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv3 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv3 = null;
            }
        },
        getTenBenhKem4Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv4) {
                    this.form.tenBenhKemVv4 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv4 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv4 = null;
            }
        },
        getTenBenhKem5Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv5) {
                    this.form.tenBenhKemVv5 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv5 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv5 = null;
            }
        },
        getTenBenhKem6Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv6) {
                    this.form.tenBenhKemVv6 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv6 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv6 = null;
            }
        },
        getTenBenhKem7Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv7) {
                    this.form.tenBenhKemVv7 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv7 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv7 = null;
            }
        },
        getTenBenhKem8Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv8) {
                    this.form.tenBenhKemVv8 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv8 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv8 = null;
            }
        },
        getTenBenhKem9Vv(item) {
            if (item) {
                if (!this.form.tenBenhKemVv9) {
                    this.form.tenBenhKemVv9 = item.tenBenh;
                } else if (this.isClickBenhKem) {
                    this.form.tenBenhKemVv9 = item.tenBenh;
                }
            } else {
                this.form.tenBenhKemVv9 = null;
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

.el-form-item__label {
    text-align: left !important;
    line-height: 22px !important;
    margin: 10px 0px !important;

}
</style>