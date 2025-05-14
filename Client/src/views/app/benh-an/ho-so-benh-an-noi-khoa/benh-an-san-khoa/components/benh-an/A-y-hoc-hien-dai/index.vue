<template>
    <div>
        <div style="font-size: 16px; font-weight: bold" class="mb-3">
            A. BỆNH ÁN
        </div>
        <el-form :model="form" :rules="rules" ref="form">
            <v-row>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="I. LÝ DO VÀO VIỆN">
                        <el-input v-model="form.lyDoVV" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Vào viện ngày thứ">
                        <el-input v-model="form.vaoNgayThuCuaBenh" dense outlined type="number" :min="0"
                            size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="II. Hỏi bệnh"></el-form-item>
                    <el-form-item label="1. Quá trình kỳ thai này:"></el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Kinh cuối cùng từ ngày">
                        <el-date-picker v-model="form.kinhTuNgay" type="datetime" size="small"
                            placeholder="dd/mm/yyyy hh:mm" style="width: 100%" format="dd/MM/yyyy HH:mm"
                            value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Đến ngày">
                        <el-date-picker v-model="form.kinhDenNgay" type="datetime" size="small"
                            placeholder="dd/mm/yyyy hh:mm" style="width: 100%" format="dd/MM/yyyy HH:mm"
                            value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Tuổi thai(tuần)">
                        <el-input v-model="form.benhTangHa" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Khám thai tại:">
                        <el-input v-model="form.noiDieuTriTangHa" type="textarea"></el-input>
                    </el-form-item>
                </v-col>

                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Tiêm phòng uốn ván">
                        <el-radio-group v-model="form.thucHienCdanDtd" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="1">Có</el-radio>
                                <el-radio :label="2">Không</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Được tiêm (lần)">
                        <el-input v-model="form.benhDtd" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Ngày bắt đầu chuyển dạ từ">
                        <el-date-picker v-model="form.thoiDiemPhatHienDtd" type="datetime" size="small"
                            placeholder="dd/mm/yyyy hh:mm" style="width: 100%" format="dd/MM/yyyy HH:mm"
                            value-format="yyyy-MM-ddTHH:mm:ss"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item class="item-form" label="Dấu hiệu lúc đầu">
                        <el-input v-model="form.dieuTriTangHathuongXuyen" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item class="item-form" label="Biến chuyển">
                        <el-input v-model="form.donDaTriLieuHa" type="textarea"></el-input>
                    </el-form-item>
                </v-col>

                <v-col cols="12" class="padding-cols">
                    <el-form-item label="2. Tiền sử bệnh"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item class="item-form"
                        label="Bản thân:(những bệnh đã mắc, dị ứng, thói quen ăn uống, sinh hoạt, khác	)">
                        <el-input v-model="form.tienSuBanThan" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Gia đình">
                        <el-input v-model="form.tienSuGiaDinh" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="3. Tiền sử phụ khoa">
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Bắt đầu thấy kinh năm:">
                        <el-input v-model="form.chiSoHamax" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Tuổi">
                        <el-input v-model="form.tuoiCoKinh" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Tính chất kinh nguyệt">
                        <el-input v-model="form.thoiDiemPhatHienTangHa" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Chu kỳ(ngày)">
                        <el-input v-model="form.dtdmax" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Lượng kinh">
                        <el-input v-model="form.chamSoc" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Lấy chồng năm">
                        <el-input v-model="form.anDuoi1Tuoi" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Tuổi">
                        <el-input v-model="form.anTren1Tuoi" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Những bệnh phụ khoa đã điều trị">
                        <el-input v-model="form.benhLyKhac" type="textarea" rows="2"></el-input>
                    </el-form-item>
                </v-col>

                <!-- /**
                 Tiền sử sản khoa - lấy từ bảng bệnh án tiền sử sản
                 */ -->
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="4. Tiền sử sản khoa"></el-form-item>
                </v-col>
                <TienSuBenhAn />


                <v-col cols="12" class="padding-cols">
                    <el-form-item label="III. KHÁM BỆNH"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="1.Toàn thân"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item>
                        <div>
                            <div style="font-weight: 500">Toàn trạng</div>
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
                    </v-row>
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
                    <el-form-item label="Tiết niệu:">
                        <el-input v-model="form.thanTietNieu" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Các bộ phận khác:">
                        <el-input v-model="form.khac" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">Phù</div>
                        <el-radio-group v-model="form.phu">
                            <el-radio :label="true">Có</el-radio>
                            <el-radio :label="false">Không</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="2. Khám ngoài">
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item class="item-form">
                        <div class="title-group-checkbox mr-4">Bụng có sẻo phẫu thuật cũ</div>
                        <el-radio-group v-model="form.seoPTCu">
                            <el-radio :label="true">Có</el-radio>
                            <el-radio :label="false">Không</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item label="Hình dạng TC">
                        <el-input v-model="form.hinhDangTC" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item label="Tư thế">
                        <el-input v-model="form.tuThe" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item label="Chiều cao TC (cm)">
                        <el-input v-model="form.chieuCaoTC" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item label="Vòng bụng (cm)">
                        <el-input v-model="form.vongBung" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="4" class="padding-cols">
                    <el-form-item label="Tim thai (lần/phút)">
                        <el-input v-model="form.timThai" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Cơn co TC">
                        <el-input v-model="form.conCoTC" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Vú">
                        <el-input v-model="form.vu" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="2. Khám trong">
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Chỉ số Bishop (điểm)">
                        <el-input v-model="form.csBishop" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Âm hộ">
                        <el-input v-model="form.amHo" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Âm đạo">
                        <el-input v-model="form.amDao" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Cổ tử cung">
                        <el-input v-model="form.coTuCung" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Phần phụ">
                        <el-input v-model="form.phanPhu" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Ối vỡ lúc">
                        <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm"
                            v-model="form.oiVoLuc" size="small" style="width:100%"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Màu sắc nước ối">
                        <el-input v-model="form.mauSacNuocOi" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Ngôi">
                        <el-input v-model="form.ngoi" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Thế">
                        <el-input v-model="form.the" type="text" size="mini"></el-input>
                    </el-form-item>
                </v-col>
                <br />
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="">
                        <br />
                        <div class="title-group-checkbox">Độ lót</div>
                        <el-checkbox @change="setModelFormCheckboxDoLot(DoLot)" v-for="(item, index) in DoLot"
                            :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="">
                        <div class="title-group-checkbox">Tình trạng ối</div>
                        <el-checkbox @change="setModelFormCheckboxTinhTrangOi(TinhTrangOi)"
                            v-for="(item, index) in TinhTrangOi" :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item>
                        <div class="title-group-checkbox">Ối vỡ</div>
                        <el-checkbox @change="setModelFormCheckboxOiVo(OiVo)" v-for="(item, index) in OiVo" :key="index"
                            v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Tầng sinh môn">
                        <el-input v-model="form.tanSinhMon" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Nước ối nhiều hay ít">
                        <el-input v-model="form.nuoiOiNhieu" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Kiểu thể">
                        <el-input v-model="form.kieuThe" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Đường kính nhô hạ thể">
                        <el-input v-model="form.duongKinhNhoHaVe" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Các xét nghiệm cận lâm sàng cần làm:">
                        <el-input v-model="form.canLamSang" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="5. Chuẩn đoán:"></el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <b>Khi vào khoa</b>
                </v-col>
                <v-col cols="6">
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.maBenhChinhVv"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable size="small"
                            @get-item="getTenBenhChinhVv" :firstEmitGetItem="false">
                        </base-select-async>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Phân biệt:">
                        <base-select-async v-model="form.maCDPhanBiet"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" size="small">
                        </base-select-async>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="6.Tiên Lượng">
                        <el-input v-model="form.tienLuong" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="7.Hướng điều trị">
                        <el-input v-model="form.huongDT" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Phương pháp chính">
                        <el-input v-model="form.ppdtChinh" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="IV. Theo dõi tại buồng đẻ:">
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Vào buồng đẻ">
                        <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm"
                            v-model="form.vaoBuongDe" size="small" style="width:100%"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Tên người theo dõi">
                        <el-input v-model="form.nguoiTheoDoi" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Chức danh">
                        <el-input v-model="form.maChucDanh" type="textarea"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="1. Đặc điểm trẻ sơ sinh:">
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Đẻ lúc">
                        <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm"
                            v-model="form.ngayDe" size="small" style="width:100%"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Apgar 1 phút (điểm)">
                        <el-input v-model="form.apgar1Phut" type="text"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Apgar 5 phút (điểm)">
                        <el-input v-model="form.apgar5Phut" type="text"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Apgar 10 phút (điểm)">
                        <el-input v-model="form.apgar10Phut" type="text"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Cân nặng (gram)">
                        <el-input v-model="form.canNangCon" type="text"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Chiều cao(cm)">
                        <el-input v-model="form.chieuCaoCon" type="text"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="3" class="padding-cols">
                    <el-form-item label="Vòng đầu (cm)">
                        <el-input v-model="form.vongDauCon" type="text"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <div class="title-group-checkbox">Con</div>
                    <el-form-item >
                        <el-radio-group v-model="form.donThai" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="1">Đơn thai </el-radio>
                                <el-radio :label="2">Đa thai</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                    <!-- <el-form-item label="Đơn thai">
                        <el-checkbox @change="setModelFormCheckboxDonThai(DonThai)" v-for="(item, index) in DonThai"
                            :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item> -->
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Giới tính">
                        <el-radio-group v-model="form.gTCon" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="1">Trai </el-radio>
                                <el-radio :label="2">Gái</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                    <!-- <el-form-item label="Đa thai">
                        <el-checkbox @change="setModelFormCheckboxDaThai(DaThai)" v-for="(item, index) in DaThai"
                            :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item> -->
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Tật bẩm sinh">
                        <el-radio-group v-model="form.tatBamSinh" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="true">Có </el-radio>
                                <el-radio :label="false">Không</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Có hậu môn">
                        <el-radio-group v-model="form.coHauMon" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="true">Có</el-radio>
                                <el-radio :label="false">Không</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Cụ thể tật bẩm sinh">
                        <el-input v-model="form.cuTheTatBamSinh" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Tình trạng trẻ sơ sinh sau khi đẻ">
                        <el-input v-model="form.ttConSauDe" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Xử lý và kết quả">
                        <el-input v-model="form.xuLyCon" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="2. Đặc điểm sổ rau:">
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Rau">
                        <el-radio-group v-model="form.rau" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="1">Bóc </el-radio>
                                <el-radio :label="2">Sổ</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>

                    <!-- <el-form-item label="Rau">
                        <el-checkbox @change="setModelFormCheckboxRau(SoRau)" v-for="(item, index) in SoRau"
                            :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item> -->
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Rau sổ lúc ">
                        <el-date-picker type="datetime" value-format="yyyy-MM-ddTHH:mm:ss" format="dd/MM/yyyy HH:mm"
                            v-model="form.ngaySoRau" size="small" style="width:100%"></el-date-picker>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Cách sổ rau ">
                        <el-input v-model="form.cachSoRau" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Mặt màng ">
                        <el-input v-model="form.matMang" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Mặt múi ">
                        <el-input v-model="form.matMui" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Bánh rau ">
                        <el-input v-model="form.banhRau" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Cân nặng">
                        <el-input v-model="form.canNangRau" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Rau cuốn cổ">
                        <el-radio-group v-model="form.rauCuonCo" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="true">Có</el-radio>
                                <el-radio :label="false">Không</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Cuống rau dài (cm)">
                        <el-input v-model="form.cuongRauDai" type="number" :min="0" size="small"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Có chảy máu sau mổ">
                        <el-radio-group v-model="form.chayMauCo" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="true">Có</el-radio>
                                <el-radio :label="false">Không</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Nếu có lượng máu mất (ml)">
                        <el-input v-model="form.luongMauMat" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Kiểm soát cổ tử cung">
                        <el-radio-group v-model="form.kiemSoatTC" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="true">Có</el-radio>
                                <el-radio :label="false">Không</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Xử lý và kết quả">
                        <el-input v-model="form.xuLyRau" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="3. Tình trạng sản phủ sau đẻ">
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Da, niêm mạc">
                        <el-input v-model="form.daMeSauDe" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Phương pháp đẻ">
                        <el-checkbox @change="setModelFormCheckboxPPD(PhuongPhapDe)"
                            v-for="(item, index) in PhuongPhapDe" :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Lý do can thiệp">
                        <el-input v-model="form.lyDoCanThiep" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Đơn thai">
                        <el-radio-group v-model="form.tangSinhMonSauDe" style="width: 100%">
                            <div class="gender">
                                <el-radio :label="1">Không rách </el-radio>
                                <el-radio :label="2">Rách</el-radio>
                                <el-radio :label="3">Cắt</el-radio>
                            </div>
                        </el-radio-group>
                    </el-form-item>
                    <!-- <el-form-item label="Tầng sinh môn">
                        <el-checkbox @change="setModelFormCheckboxTangSM(TangSinhMon)"
                            v-for="(item, index) in TangSinhMon" :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item> -->
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="Nếu có, phương pháp khâu và loại chỉ">
                        <el-input v-model="form.phuongPhapKhau" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Số mũi khâu">
                        <el-input v-model="form.soMuiKhau" type="textarea" row="2"></el-input>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Cổ tử cung">
                        <el-radio-group v-model="form.rachCoTCSauDe" style="width: 100%">
                            <el-radio :label="1">Không rách</el-radio>
                            <el-radio :label="2">Rách</el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <!-- <el-form-item label="Cổ tử cung">
                        <el-checkbox @change="setModelFormCheckboxCTC(CoTuCung)" v-for="(item, index) in CoTuCung"
                            :key="index" v-model="item.checkbox">{{ item.ten
                            }}
                        </el-checkbox>
                    </el-form-item> -->
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item label="3. Tình hình phẫu thuật">
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="mt-3">
                    <b>Tình hình phẫu thuật</b>
                    <el-form-item >
                        <el-radio-group v-model="form.cachThucPt" style="width: 100%">
                            <el-radio :label="1">Cấp Cứu</el-radio>
                            <el-radio :label="2">Chủ động</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="mt-3">
                    <b>Chẩn đoán trước phẫu thuật</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.chanDoanTruocPt"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" firstEmitGetItem="false" placeholder="" style="width: 100%"
                            size="small">
                        </base-select-async>


                    </el-form-item>
                </v-col>
                <v-col cols="6" class="mt-3">
                    <b>Chẩn đoán sau phẫu thuật</b>
                    <el-form-item label="Mã bệnh">
                        <base-select-async v-model="form.chanDoanSauPt"
                            :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh"
                            :apiFunc="getChanDoanBenh" firstEmitGetItem="false" placeholder="" style="width: 100%"
                            size="small">
                        </base-select-async>
                    </el-form-item>
                </v-col>
                <!-- Phẫu thuật phiếu pttt -->
                <v-col cols="12" class="padding-cols">

                    <PhauThuatPhieuPttt />

                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Tai biến">
                        <el-radio-group v-model="form.taiBienYhhd" style="width: 100%">
                            <el-radio :label="1">Có</el-radio>
                            <el-radio :label="2">Không</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="6" class="padding-cols">
                    <el-form-item label="Biến chứng">
                        <el-radio-group v-model="form.bienChungYhhd" style="width: 100%">
                            <el-radio :label="1">Có</el-radio>
                            <el-radio :label="2">Không</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </v-col>
                <v-col cols="12" class="padding-cols">
                    <el-form-item>
                        <el-radio-group v-model="form.taiBienYhct" style="width: 100%">
                            <el-radio :label="1">Do phẫu thuật</el-radio>
                            <el-radio :label="2">Do gây mê</el-radio>
                            <el-radio :label="3">Do truyền nhiễm</el-radio>
                            <el-radio :label="4">Khác</el-radio>
                        </el-radio-group>
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
import { getDetailBenhAnPttt, getTienSuBenh, getBenhAnTienSuSan, getBenhAnPhuSan, chiTietToBenhAn } from "@/api/benh-an.js";
import TienSuBenhAn from "../A-y-hoc-hien-dai/tienSuBenhAn.vue";
import PhauThuatPhieuPttt from "../A-y-hoc-hien-dai/phauThuatPhieuPttt.vue";
export default {
    components: {
        TienSuBenhAn,
        PhauThuatPhieuPttt
    },
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
                taiBienYhhd:null,
                taiBienYhct: null,
                vaoNgayThuCuaBenh: null,
                maCDPhanBiet: null,
                benhSu: null,
                bmi: null,
                canLamSang: null,
                canNang: null,
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
                lyDoVV: null,
                mach: null,
                mat: null,
                nhietDo: null,
                nhipTho: null,
                noiTietDd: null,
                rangHamMat: null,
                taiMuiHong: null,
                tenBenhPhanBiet: null,
                thanKinh: null,
                thanTietNieu: null,
                tieuHoa: null,
                toanThan: null,
                tomTatBenhAn: null,
                tuanHoan: null,
                xuongKhop: null,
                maTienSu: null,
                moTaTienSu: null,
                tienSuBanThan: null,
                tienSuGiaDinh: null,
                dacDiemLienQuanBenh: null,
                tienLuong: null,
                huongDanDieuTri: null,
                maBenhChinhVv: null,
                tenBenhChinhVv: null,
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
                isClickBenhKem: false,
                noiDieuTriTangHa: null,
                thucHienCdanDtd: null,
                benhTangHa: null,
                kinhDenNgay: null,
                kinhTuNgay: null,
                benhDtd: null,
                thoiDiemPhatHienDtd: null,
                dieuTriTangHathuongXuyen: null,
                donDaTriLieuHa: null,
                tienSuBanThan: null,
                tienSuGiaDinh: null,
                chiSoHamax: null,
                tuoiCoKinh: null,
                thoiDiemPhatHienTangHa: null,
                dtdmax: null,
                chamSoc: null,
                anDuoi1Tuoi: null,
                anTren1Tuoi: null,
                benhLyKhac: null,
                khac: null,
                phu: null,
                seoPTCu: null,
                hinhDangTC: null,
                tuThe: null,
                chieuCaoTC: null,
                vongBung: null,
                timThai: null,
                conCoTC: null,
                vu: null,
                csBishop: null,
                amHo: null,
                amDao: null,
                coTuCung: null,
                phanPhu: null,
                oiVoLuc: null,
                mauSacNuocOi: null,
                ngoi: null,
                the: null,
                doLot: null,
                tinhTrangOi: null,
                oiVo: null,
                tanSinhMon: null,
                nuoiOiNhieu: null,
                kieuThe: null,
                duongKinhNhoHaVe: null,
                tienLuong: null,
                huongDT: null,
                ppdtChinh: null,
                vaoBuongDe: null,
                nguoiTheoDoi: null,
                maChucDanh: null,
                ngayDe: null,
                apgar1Phut: null,
                apgar5Phut: null,
                apgar10Phut: null,
                canNangCon: null,
                chieuCaoCon: null,
                vongDauCon: null,
                donThai: null,
                gTCon: null,
                tatBamSinh: null,
                coHauMon: null,
                cuTheTatBamSinh: null,
                ttConSauDe: null,
                xuLyCon: null,
                rau: null,
                ngaySoRau: null,
                cachSoRau: null,
                matMang: null,
                matMui: null,
                banhRau: null,
                canNangRau: null,
                rauCuonCo: null,
                cuongRauDai: null,
                chayMauCo: null,
                luongMauMat: null,
                kiemSoatTC: null,
                xuLyRau: null,
                daMeSauDe: null,
                ppDe: null,
                lyDoCanThiep: null,
                tangSinhMonSauDe: null,
                phuongPhapKhau: null,
                soMuiKhau: null,
                rachCoTCSauDe: null,
                chanDoanTruocPt: null,
                chanDoanSauPt: null,
                cachThucPt:null,
                tenChuanDoanTruocPt: null,
                tenChuanDoanSauPt: null,
                tinhTrangSinh: null,
                bienChungYhhd:null
            },
            BienChungSanKhoa: [
                { ten: "Do phẫu thuật", checkbox: false },
                { ten: "Do gây mê", checkbox: false },
                { ten: "Do nhiễm khuẩn", checkbox: false },
                { ten: "Khác", checkbox: false },
            ],
            DoLot: [
                { ten: "Cao", checkbox: false },
                { ten: "Chúc", checkbox: false },
                { ten: "Chặt", checkbox: false },
                { ten: "Lọt", checkbox: false },
            ],
            TinhTrangOi: [
                { ten: "Ối phồng", checkbox: false },
                { ten: "Ối dẹt", checkbox: false },
                { ten: "Ối quả lê", checkbox: false },
            ],
            OiVo: [
                { ten: "Tự nhiên", checkbox: false },
                { ten: "Bấm ối", checkbox: false },
            ],
            DonThai: [
                { ten: "Trai", checkbox: false },
                { ten: "Gái", checkbox: false },
            ],
            DaThai: [
                { ten: "Trai", checkbox: false },
                { ten: "Gái", checkbox: false },
            ],
            SoRau: [
                { ten: "Bóc", checkbox: false },
                { ten: "Sổ", checkbox: false },
            ],
            PhuongPhapDe: [
                { ten: "Đẻ thường", checkbox: false },
                { ten: "Forceps", checkbox: false },
                { ten: "Giác hút", checkbox: false },
                { ten: "PT", checkbox: false },
                { ten: "Đẻ chỉ huy", checkbox: false },
                { ten: "Khác", checkbox: false },
            ],
            TangSinhMon: [
                { ten: "Không rách", checkbox: false },
                { ten: "Rách", checkbox: false },
                { ten: "Cắt", checkbox: false },
            ],
            CoTuCung: [
                { ten: "Không rách", checkbox: false },
                { ten: "Rách", checkbox: false },

            ],
            benh_khac: [
                {
                    ten_benh: null,
                    ma_benh: null,
                },
            ],
            tienSuBenhAn: [
                { ten: "Dị ứng", checkbox: false },
                { ten: "Ma Túy", checkbox: false },
                { ten: "Rượu bia", checkbox: false },
                { ten: "Thuốc lá", checkbox: false },
                { ten: "Thuốc lào", checkbox: false },
                { ten: "Khác", checkbox: false }
            ],
            rules: {
                mach: [
                    {
                        validator: checkMach,
                        trigger: "blur",
                    },
                ],
            },
        }
    },

    mounted() {
        this.getData();
    },
    watch: {
        params: {
            handler: function (neVal) {
                this.$refs.table.reset();
            },
            deep: true,
        },
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
        getTenBenhChinhVv(item) {
            if (item) {
                this.form.tenBenhChinhVv = item.tenBenh;
            }
            else {
                this.form.tenBenhChinhVv = null
            }
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
        async getData() {
            try {
                const id = window.location.href.split("/").at(-1);
                let data = await chiTietToBenhAn(id);
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
                if (this.form.tinhTrangSinh != null) {
                    var thoiGianParts = data.data.tinhTrangSinh.split("|");
                    this.form.kinhTuNgay = thoiGianParts[0] || null;
                    this.form.kinhDenNgay = thoiGianParts.length > 1 ? thoiGianParts[1] || null : null;
                }
                if (this.form.canNang && this.form.chieuCao) {
                    this.form.bmi = (this.form.canNang / ((this.form.chieuCao / 100) * (this.form.chieuCao / 100))).toFixed(2)
                }
                data = await getDetailBenhAnPttt(id)
                for (let key in this.form) {
                    if (data && data.data && data.data.hasOwnProperty(key)) {
                        this.form[key] = data.data[key]

                    }
                }
                data = await getBenhAnPhuSan(id)
                for (let key in this.form) {
                    if (data && data.data && data.data.hasOwnProperty(key)) {
                        this.form[key] = data.data[key]
                    }
                }
                data = await getSelectBox({ MaParent: '215' });
                this.TinhTrangOi = this.setCheckbox(this.form.tinhTrangOi, data.data)
                data = await getSelectBox({ MaParent: '216' });
                this.OiVo = this.setCheckbox(this.form.oiVo, data.data)
                data = await getSelectBox({ MaParent: '214' });
                this.DoLot = this.setCheckbox(this.form.doLot, data.data)
                data = await getSelectBox({ MaParent: '211' });
                this.PhuongPhapDe = this.setCheckbox(this.form.ppDe, data.data)
            } catch (error) {
                console.log("Error:" + error)
            }
        },
        getTenBenhChinhVv(item) {
            if (item) {
                this.form.benhChinh.tenBenh = item.tenBenh;
            }
            else {
                this.form.benhChinh.tenBenh = null
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
        setModelFormCheckboxDoLot(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.doLot = ma;
        },
        setModelFormCheckboxOiVo(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.oiVo = ma;
        },
        setModelFormCheckboxPPD(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.ppDe = ma;
        },
        setModelFormCheckboxTinhTrangOi(arrCheckbox) {
            let ma = "";
            arrCheckbox.forEach(item => {
                if (item.checkbox) {
                    ma = ma + item.ma + ","
                }
            })
            this.form.tinhTrangOi = ma;
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