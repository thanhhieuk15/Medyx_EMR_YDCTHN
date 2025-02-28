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
                <!-- <v-col cols="4" class="padding-cols">
            <el-form-item label="Vào viện do bệnh này lần thứ">
              <el-input v-model="form.vvlan" dense outlined type="number" :min="0" size="small"></el-input>
            </el-form-item>
          </v-col> -->
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="II. Bệnh Sử">

                        <el-input v-model="form.benhSu" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <div> 1. Tình hình lúc đẻ</div>
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">1.1 Con thứ:</div>
                        <el-radio-group v-model="form.conThu">
                            <el-radio :label="1">1</el-radio>
                            <el-radio :label="2">2</el-radio>
                            <el-radio :label="3">3</el-radio>
                            <el-radio :label="4">4</el-radio>
                            <el-radio :label="5">5</el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <!-- <el-form-item label="1.1 Con thứ:">

                        <el-checkbox @change="setModelFormCheckbox(conThu)" v-for="(item, index) in conThu" :key="index"
                            v-model="item.checkbox">{{ item.ten }}
                        </el-checkbox>
                    </el-form-item> -->
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">1.2 Tình trạng thai phụ</div>
                        <el-radio-group v-model="form.deDuThang">
                            <el-radio :label="1">Đẻ đủ</el-radio>
                            <el-radio :label="2">Thiếu tháng</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">1.3 Tình trạng sinh</div>
                        <el-radio-group v-model="form.deKho">
                            <el-radio :label="1">Đẻ dễ</el-radio>
                            <el-radio :label="2">Đẻ khó</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">1.4 Cân nặng</div>
                        <el-radio-group v-model="form.canNangNhi">
                            <el-radio :label="1"> ≤ 2,5 kg </el-radio>
                            <el-radio :label="2"> > 2,5kg </el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">1.5 Lúc đẻ ngạt thở</div>
                        <el-radio-group v-model="form.deNgatTho">
                            <el-radio :label="1"> Có </el-radio>
                            <el-radio :label="2"> Không </el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">1.6 Thời gian rụng rốn</div>
                        <el-radio-group v-model="form.rungRon">
                            <el-radio :label="1"> Bình thường (5-7 ngày) </el-radio>
                            <el-radio :label="2"> Bất thường (>7 ngày) </el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <div>2. Chế độ ăn</div>
                    <v-row>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form" label="Dưới 1 tuổi">
                                <el-input v-model="form.anDuoi1Tuoi" type="textarea" rows="2"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form" label="Tháng cai sữa">
                                <el-input v-model="form.thangCaiSua" type="textarea" rows="2"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form" label="Lớn hơn 1 tuổi">
                                <el-input v-model="form.anTren1Tuoi" type="textarea" rows="2"></el-input>
                            </el-form-item>
                        </v-col>

                    </v-row>

                </v-col>
                <br />
                <v-col cols="12">
                    <div>3.Phát dục</div>
                    <v-row>

                        <v-col cols="6">
                            <el-form-item class="item-form">
                                <div class="title-group-checkbox mr-4">Tháng biết lẫy</div>
                                <el-radio-group v-model="form.thangLay">
                                    <el-radio :label="1"> Bình thường (3-4 tháng) </el-radio>
                                    <el-radio :label="2"> Chậm > 4 tháng</el-radio>
                                </el-radio-group>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form">
                                <div class="title-group-checkbox mr-4">Tháng biết bò</div>
                                <el-radio-group v-model="form.thangBo">
                                    <el-radio :label="1"> Bình thường (6-7 tháng), </el-radio>
                                    <el-radio :label="2"> Chậm > 7 tháng</el-radio>
                                </el-radio-group>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form">
                                <div class="title-group-checkbox mr-4">Tháng biết đi</div>
                                <el-radio-group v-model="form.thangDi">
                                    <el-radio :label="1"> Bình thường (9-12 tháng), </el-radio>
                                    <el-radio :label="2"> Chậm > 12 tháng</el-radio>
                                </el-radio-group>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form">
                                <div class="title-group-checkbox mr-4">Tháng biết mọc răng</div>
                                <el-radio-group v-model="form.thangMocRang">
                                    <el-radio :label="1"> Bình thường (> 6 tháng), </el-radio>
                                    <el-radio :label="2"> Chậm > 12 tháng</el-radio>
                                </el-radio-group>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form" label="Tháng biết nói">
                                <el-input v-model="form.thangNoi" type="textarea" rows="2"></el-input>
                            </el-form-item>
                        </v-col>
                        <v-col cols="6" class="padding-cols">
                            <el-form-item class="item-form" label="Tuổi bắt đầu có kinh nguyệt">
                                <el-input v-model="form.tuoiCoKinh" type="textarea" rows="2"></el-input>
                            </el-form-item>
                        </v-col>
                    </v-row>

                </v-col>
                <br />
                <v-col cols="12">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">4. Đã tiêm chủng</div>
                        <el-radio-group v-model="form.daTiemChung">
                            <el-radio :label="1"> Có </el-radio>
                            <el-radio :label="2"> Không </el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <br />
                <v-col cols="12">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">5. Bệnh đã mắc</div>
                        <el-radio-group v-model="form.benhDaMac">
                            <el-radio :label="1"> Có </el-radio>
                            <el-radio :label="2"> Không </el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <br />
                    <el-form-item class="item-form" label="Mô tả">
                        <el-input v-model="form.moTaTienSu" type="textarea" rows="2"></el-input>
                    </el-form-item>
                    <br />
                    <el-form-item class="item-form" label="Cá nhân">
                        <el-input v-model="form.tienSuBanThan" type="textarea" rows="2"></el-input>
                    </el-form-item>
                    <br />
                    <el-form-item class="item-form" label="Gia đình">
                        <el-input v-model="form.tienSuGiaDinh" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12">
                    <el-form-item class="item-form" label="6. Đặc điểm sinh hoạt (Vật chất, tinh thần, nghề nghiệp)">
                        <el-input v-model="form.dacDienSH" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12">
                    <el-form-item class="item-form" label="7. Tiền sử đặc biệt khác (nếu có)">
                        <el-input v-model="form.dacDiemLienQuanBenh" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>

                <v-col cols="12" class="padding-cols">
                    <el-form-item label="III. KHÁM BỆNH"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item>
                        <div>
                            <div style="font-weight: 500">1. Khám toàn thân:</div>
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
                            <el-form-item label="BMI:">
                                <el-input type="number" v-model="form.bmi" size="mini" :step="0.001"
                                    controls-position="right" style="width: 100%"></el-input>
                            </el-form-item>
                        </v-col>
                    </v-row>
                </v-col>
                <v-col cols="12" class="padding-cols mt-4">
                    <el-form-item label="Các cơ quan:"></el-form-item>
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
                    <el-form-item label="Thận - Tiết niệu-sinh dục:">
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
                    <el-form-item label="Tai - mũi - họng:">
                        <el-input v-model="form.taiMuiHong" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Răng -  hàm - mặt:">
                        <el-input v-model="form.rangHamMat" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Mắt:">
                        <el-input v-model="form.mat" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Nội tiết, dinh dưỡng vã các bênh lý khác:">
                        <el-input v-model="form.noiTietDd" type="textarea"></el-input>
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
                        <base-select-async v-model="form.maBenhChinhVv"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
                            @get-item="getTenBenhChinhVv" :firstEmitGetItem="false">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhChinhVv" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 1</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv1"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
                            @get-item="getTenBenhKem1Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv1" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 2</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv2"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
                            @get-item="getTenBenhKem2Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv2" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 3</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv3"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem3Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv3" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 4</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv4"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem4Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv4" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 5</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv5"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem5Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv5" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 6</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv6"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem6Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv6" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 7</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv7"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem7Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv7" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 8</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv8"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem8Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv8" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6">
                    <b>Bệnh kèm theo 9</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhKemVv9"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%" size="small"
                            @get-item="getTenBenhKem9Vv" @change="onChangeBenhKem">
                        </base-select-async>
                    </el-form-item>
                    <el-form-item label="Tên bệnh">
                        <el-input v-model="form.tenBenhKemVv9" size="small"></el-input>
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
                        <el-input v-model="form.huongDanDieuTri" type="textarea"></el-input>
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
                daTiemChung: null,
                benhDaMac: null,
                dacDienSH: null,
                cdchamSoc: null,
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
                isClickBenhKem: false
            },
            benh_khac: [
                {
                    ten_benh: null,
                    ma_benh: null,
                },
            ],
            // conThu: [
            //     { ten: "1", checkbox: false },
            //     { ten: "2", checkbox: false },
            //     { ten: "3", checkbox: false },
            //     { ten: "4", checkbox: false },
            //     { ten: "5", checkbox: false },
            //     // { ten: "Khác", checkbox: false }
            // ],
            //       tienSuBenhAn :[
            // {name:"Dị Ứng", checkbox:false},
            // {name:"Ma Túy", checkbox:false},
            // {name:"Rượu bia", checkbox:false},
            // {name:"Thuốc Lá", checkbox:false},
            // {name:"Thuốc lào", checkbox:false},
            // {name:"Khác", checkbox:false}
            //       ],
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

            for (let key in this.form) {
                if (data && data.data && data.data.hasOwnProperty(key)) {
                    this.form[key] = data.data[key]

                }
            }
            // data = await getSelectBox({ MaParent: '017' });
            // this.conThu = this.setCheckbox(this.form.conThu, data.data.conThu)
            if (this.form.canNang && this.form.chieuCao) {
                this.form.bmi = (this.form.canNang / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
            }
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
            this.form.maTienSu = ma;
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