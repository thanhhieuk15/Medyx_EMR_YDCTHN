<template>
  <div>
    <div style="font-size: 16px; font-weight: bold" class="mb-4">
      III. CHẨN ĐOÁN
    </div>
    <el-form>
      <v-row>
        <v-col cols="12">
          <div style="border: 1px solid #d6dbdf" class="pa-4 pb-8">
            <v-row class="mt-0">
              <v-col cols="12" class="padding-cols mb-2">
                <div style="font-weight: 600" class="pb-1">
                  CHẨN ĐOÁN THEO YHHĐ
                </div>
                <hr />
              </v-col>
              <v-col cols="12" class="padding-cols">
                <b>Nơi chuyển đến</b>
              </v-col>
              <v-col class="padding-cols" cols="12" xs="12" sm="12" md="12" lg="12" xl="12">
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhNoiChuyenDenYhhd"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    placeholder="" style="width: 100%" size="small" @get-item="getTenBenhNoiChuyenDenYhhd" clearable>
                  </base-select-async>
                </el-form-item>
              </v-col>
              <v-col cols="12" xs="12" sm="12" md="12" lg="12" xl="12" class="padding-cols">
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhNoiChuyenDenYhhd" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12" class="mt-3">
                <b>Khoa khám bệnh</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKkbyhhd" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKkbyhhd">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKkbyhhd" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh chính</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhChinhVv" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhChinhVv" :firstEmitGetItem="false">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhChinhVv" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 1</b>
                                  <!-- v-model="form.benhKem1.maBenh" -->
                <el-form-item label="Mã bệnh">
                  <base-select-async 
                  v-model="form.maBenhKemVv1"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    placeholder="" style="width: 100%" clearable size="small" @get-item="getTenBenhKem1Vv"
                    @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <!-- <el-input v-model="form.benhKem1.tenBenh" size="small"></el-input> -->
                  <el-input v-model="form.tenBenhKemVv1" size="small"></el-input>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 2</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv2"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    placeholder="" style="width: 100%" clearable size="small" @get-item="getTenBenhKem2Vv"
                    @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv2" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 3</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv3"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh}`" keyValue="maBenh" :apiFunc="getChanDoanBenh"
                    clearable placeholder="" style="width: 100%" size="small" @get-item="getTenBenhKem3Vv"
                    @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv3" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 4</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv4" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%"
                    size="small" @get-item="getTenBenhKem4Vv" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv4" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 5</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv5" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%"
                    size="small" @get-item="getTenBenhKem5Vv" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv5" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 6</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv6" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%"
                    size="small" @get-item="getTenBenhKem6Vv" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv6" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 7</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv7" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%"
                    size="small" @get-item="getTenBenhKem7Vv" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv7" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 8</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv8" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%"
                    size="small" @get-item="getTenBenhKem8Vv" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv8" size="small"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - Bệnh kèm theo 9</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv9" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" clearable placeholder="" style="width: 100%"
                    size="small" @get-item="getTenBenhKem9Vv" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv9" size="small"></el-input>
                </el-form-item>
              </v-col> -->
              <v-col cols="6">
                <el-form-item label="Thủ thuật">
                  <el-radio-group v-model="form.thuThuatYhhd" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="6">
                <el-form-item label="Phẫu thuật">
                  <el-radio-group v-model="form.phauThuatYhhd" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col> -->
              <v-col cols="12">
                <b>Ra viện - Bệnh chính</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhChinhRv" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhChinhRv" :firstEmitGetItem="false">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhChinhRv" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 1</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv1" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv1" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv1" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 2</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv2" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv2" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv2" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 3 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv3" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv3" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv3" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 4 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv4" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv4" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv4" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 5 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv5" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv5" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv5" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 6 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv6" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv6" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv6" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 7 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv7" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv7" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv7" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 8 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv8" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv8" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv8" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 9 </b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv9" :label="(item) => `${item.maBenh} - ${item.tenBenh}`"
                    keyValue="maBenh" :apiFunc="getChanDoanBenh" placeholder="" style="width: 100%" clearable
                    size="small" @get-item="getTenBenhKemRv9" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv9" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col> -->
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
            </v-row>
          </div>
        </v-col>
        <v-col cols="12">
          <div style="border: 1px solid #d6dbdf" class="pa-4 pb-8">
            <v-row class="mt-0">
              <v-col cols="12" class="padding-cols mb-2">
                <div style="font-weight: 600" class="pb-1">
                  CHẨN ĐOÁN THEO YHCT
                </div>
                <hr />
              </v-col>
              <v-col cols="12" class="padding-cols">
                <b>Nơi chuyển đến</b>
              </v-col>
              <v-col class="padding-cols" cols="12" xs="12" sm="12" md="12" lg="12" xl="12">
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhNoiChuyenDenYhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhNoiChuyenDenYhct">
                  </base-select-async>
                </el-form-item>
              </v-col>
              <v-col class="padding-cols" cols="12" xs="12" sm="12" md="12" lg="12" xl="12">
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhNoiChuyenDenYhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12" class="mt-3">
                <b>Khoa khám bệnh</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKkbyhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKkbyhct">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKkbyhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Khi vào khoa điều trị</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhChinhVvyhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhChinhVvyhct" :firstEmitGetItem="false">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhChinhVvyhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 1</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv1yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv1yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv1yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 2</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv2yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv2yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv2yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 3</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv3yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv3yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv3yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 4</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv4yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv4yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv4yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 5</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv5yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv5yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv5yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 6</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv6yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv6yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv6yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 7</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv7yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv7yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv7yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 8</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv8yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv8yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv8yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Vào khoa điều trị - bệnh kèm 9</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemVv9yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemVv9yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemVv9yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col> -->
              <v-col cols="6">
                <el-form-item label="Thủ thuật">
                  <el-radio-group v-model="form.thuThuatYhct" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="6">
                <el-form-item label="Phẫu thuật">
                  <el-radio-group v-model="form.phauThuatYhct" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col> -->
              <v-col cols="12">
                <b>Ra viện - Bệnh chính</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhChinhRvyhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhChinhRvyhct" :firstEmitGetItem="false">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhChinhRvyhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b> Ra viện - Bệnh kèm theo 1</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv1yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv1yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv1yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <!-- <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 2</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv2yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv2yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv2yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 3</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv3yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv3yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv3yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 4</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv4yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv4yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv4yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 5</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv5yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv5yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv5yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 6</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv6yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv6yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv6yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 7</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv7yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv7yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv7yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 8</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv8yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv8yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv8yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col>
              <v-col cols="12">
                <b>Ra viện - Bệnh kèm theo 9</b>
                <el-form-item label="Mã bệnh">
                  <base-select-async v-model="form.maBenhKemRv9yhct"
                    :label="(item) => `${item.maBenh} - ${item.tenBenh} - ${item.tenBenhBhyt}`" keyValue="maBenh"
                    :apiFunc="getChanDoanBenhCT" placeholder="" style="width: 100%" clearable size="small"
                    @get-item="getTenBenhKemRv9yhct" @change="onChangeBenhKem">
                  </base-select-async>
                </el-form-item>
                <el-form-item label="Tên bệnh">
                  <el-input v-model="form.tenBenhKemRv9yhct" size="small" placeholder="Tên bệnh"></el-input>
                </el-form-item>
              </v-col> -->

              <v-col cols="6" class="padding-cols">
                <el-form-item label="Tai biến">
                  <el-radio-group v-model="form.taiBienYhct" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col>
              <v-col cols="6" class="padding-cols">
                <el-form-item label="Biến chứng">
                  <el-radio-group v-model="form.bienChungYhct" style="width: 100%">
                    <el-radio :label="1">Có</el-radio>
                    <el-radio :label="2">Không</el-radio>
                  </el-radio-group>
                </el-form-item>
              </v-col>
            </v-row>
          </div>
        </v-col>
      </v-row>
    </el-form>
  </div>
</template>

<script>
import * as apiBenhTat from "@/api/benh-tat.js";
import * as apiBenhTatCT from "@/api/benh-tat-yhct.js";
import { getBenhTat } from "@/api/phieu-kham-benh-vao-vien";
import store from '@/store'
export default {
  props: ["benhAn", "benhAnKhoaDieuTri"],
  data: () => ({
    form: {
      maBenhNoiChuyenDenYhct: null,
      maBenhNoiChuyenDenYhhd: null,
      maBenhChinhVv: null,
      tenBenhChinhVv: null,
      maBenhKkbyhct: null,
      maBenhKkbyhhd: null,
      maBenhChinhRv: null,
      maBenhChinhRvyhct: null,
      maBenhKemRv1: null,
      maBenhKemRv1yhct: null,
      maBenhKemRv2: null,
      maBenhKemRv2yhct: null,
      maBenhKemRv3: null,
      maBenhKemRv4: null,
      maBenhKemRv5: null,
      maBenhKemRv6: null,
      maBenhKemRv7: null,
      maBenhKemRv8: null,
      maBenhKemRv9: null,
      maBenhKemRv3yhct: null,
      maBenhKemRv4yhct: null,
      maBenhKemRv5yhct: null,
      maBenhKemRv6yhct: null,
      maBenhKemRv7yhct: null,
      maBenhKemRv8yhct: null,
      maBenhKemRv9yhct: null,
      tenBenhNoiChuyenDenYhct: null,
      tenBenhNoiChuyenDenYhhd: null,
      tenBenhKkbyhct: null,
      tenBenhKkbyhhd: null,
      tenBenhChinhRv: null,
      tenBenhChinhRvyhct: null,
      tenBenhKemRv1: null,
      tenBenhKemRv1yhct: null,
      tenBenhKemRv2: null,
      tenBenhKemRv2yhct: null,
      tenBenhKemRv3: null,
      tenBenhKemRv4: null,
      tenBenhKemRv5: null,
      tenBenhKemRv6: null,
      tenBenhKemRv7: null,
      tenBenhKemRv8: null,
      tenBenhKemRv9: null,
      tenBenhKemRv3yhct: null,
      tenBenhKemRv4yhct: null,
      tenBenhKemRv5yhct: null,
      tenBenhKemRv6yhct: null,
      tenBenhKemRv7yhct: null,
      tenBenhKemRv8yhct: null,
      tenBenhKemRv9yhct: null,
      phauThuatYhct: null,
      phauThuatYhhd: null,
      taiBienYhct: null,
      taiBienYhhd: null,
      thuThuatYhct: null,
      thuThuatYhhd: null,
      bienChungYhct: null,
      bienChungYhhd: null,
      maBenhChinhVvyhct: null,
      maBenhKemVv1yhct: null,
      maBenhKemVv2yhct: null,
      maBenhKemVv3yhct: null,
      maBenhKemVv4yhct: null,
      maBenhKemVv5yhct: null,
      maBenhKemVv6yhct: null,
      maBenhKemVv7yhct: null,
      maBenhKemVv8yhct: null,
      maBenhKemVv9yhct: null,
      tenBenhChinhVvyhct: null,
      tenBenhKemVv1yhct: null,
      tenBenhKemVv2yhct: null,
      tenBenhKemVv3yhct: null,
      tenBenhKemVv4yhct: null,
      tenBenhKemVv5yhct: null,
      tenBenhKemVv6yhct: null,
      tenBenhKemVv7yhct: null,
      tenBenhKemVv8yhct: null,
      tenBenhKemVv9yhct: null,
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
      benhKem4: {
        maBenh: null,
        tenBenh: null,
      },
      benhKem5: {
        maBenh: null,
        tenBenh: null,
      },
      benhKem6: {
        maBenh: null,
        tenBenh: null,
      },
      benhKem7: {
        maBenh: null,
        tenBenh: null,
      },
      benhKem8: {
        maBenh: null,
        tenBenh: null,
      },
      benhKem9: {
        maBenh: null,
        tenBenh: null,
      },
    },
    chanDoan: [
      {
        maChanDoan: null,
        tenChanDoan: null,
      },
    ],
    isClickBenhKem: false
  }),
  watch: {
    benhAn: function (val) {
      for (let key in this.form) {
        if (val && val.hasOwnProperty(key)) {
          this.form[key] = val[key];
        }
      }
    },
    benhAnKhoaDieuTri: function (val) {
      for (let key in this.form) {
        if (val && val.hasOwnProperty(key)) {
          this.form[key] = val[key];
        }
      }
    },
    form: {
      handler(val) {
        this.$emit('get-chanDoan', val)
        store.dispatch("hosobenhan/setChanDoan", val)
      },
      deep: true
    },
    CHANDOAN: {
      handler(val) {
        for (let key in this.form) {
          if (val && val.hasOwnProperty(key)) {
            this.form[key] = val[key]
          }
        }
      },
      deep: true
    },
    TONGKETRAVIEN: {
      handler(val) {
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
    TONGKETRAVIEN() {
      return this.$store.state.hosobenhan.tongKetRaVien
    },
    CHANDOAN() {
      return this.$store.state.hosobenhan.chanDoanYHHD
    },

  },
  mounted() {

  },
  methods: {
    onChangeBenhKem() {
      this.isClickBenhKem = true;
    },
    async getChanDoanBenh(params) {
      return await apiBenhTat.index(params);
    },
    async getChanDoanBenhCT(params) {
      return await apiBenhTatCT.index(params);
    },
    getTenBenhNoiChuyenDenYhhd(item) {
      if (item) {
        this.form.tenBenhNoiChuyenDenYhhd = item.tenBenh;
      }
      else {
        this.form.tenBenhNoiChuyenDenYhhd = null
      }
    },
    getTenBenhKkbyhhd(item) {
      if (item) {
        this.form.tenBenhKkbyhhd = item.tenBenh;
      }
      else {
        this.form.tenBenhKkbyhhd = null
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
    getTenBenhKem1Vv(item) {
      if (item) {
        if (!this.form.tenBenhKemVv1) {
          this.form.tenBenhKemVv1= item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv1= item.tenBenh;
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
    
    getTenBenhKkbyhct(item) {
      if (item) {
        this.form.tenBenhKkbyhct = item.tenBenhBhyt;

      }
      else {
        this.form.tenBenhKkbyhct = null
      }
    },
    getTenBenhNoiChuyenDenYhct(item) {
      if (item) {
        this.form.tenBenhNoiChuyenDenYhct = item.tenBenhBhyt;
      }
      else {
        this.form.tenBenhNoiChuyenDenYhct = null
      }
    },
    getTenBenhChinhRv(item) {
      if (item) {
        this.form.tenBenhChinhRv = item.tenBenh;
      }
      else {
        this.form.tenBenhChinhRv = null
      }
    },
    getTenBenhChinhRvyhct(item) {
      if (item) {
        this.form.tenBenhChinhRvyhct = item.tenBenhBhyt;
      }
      else {
        this.form.tenBenhChinhRvyhct = null
      }
    },
    getTenBenhKemRv1(item) {
      if (item) {
        if (!this.form.tenBenhKemRv1) {
          this.form.tenBenhKemRv1 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv1 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv1 = null;
      }
    },

    getTenBenhKemRv1yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv1yhct) {
          this.form.tenBenhKemRv1yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv1yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv1yhct = null;
      }
    },
    getTenBenhKemRv2(item) {
      if (item) {
        if (!this.form.tenBenhKemRv2) {
          this.form.tenBenhKemRv2 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv2 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv2 = null;
      }
    },
    getTenBenhKemRv2yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv2yhct) {
          this.form.tenBenhKemRv2yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv2yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv2yhct = null;
      }
    },
    getTenBenhKemRv3(item) {
      if (item) {
        if (!this.form.tenBenhKemRv3) {
          this.form.tenBenhKemRv3 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv3 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv3 = null;
      }
    },
    getTenBenhKemRv4(item) {
      if (item) {
        if (!this.form.tenBenhKemRv4) {
          this.form.tenBenhKemRv4 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv4 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv4 = null;
      }
    },
    getTenBenhKemRv5(item) {
      if (item) {
        if (!this.form.tenBenhKemRv5) {
          this.form.tenBenhKemRv5 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv5 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv5 = null;
      }
    },
    getTenBenhKemRv6(item) {
      if (item) {
        if (!this.form.tenBenhKemRv6) {
          this.form.tenBenhKemRv6 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv6 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv6 = null;
      }
    },
    getTenBenhKemRv7(item) {
      if (item) {
        if (!this.form.tenBenhKemRv7) {
          this.form.tenBenhKemRv7 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv7 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv7 = null;
      }
    },
    getTenBenhKemRv8(item) {
      if (item) {
        if (!this.form.tenBenhKemRv8) {
          this.form.tenBenhKemRv8 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv8 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv8 = null;
      }
    },
    getTenBenhKemRv9(item) {
      if (item) {
        if (!this.form.tenBenhKemRv9) {
          this.form.tenBenhKemRv9 = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv9 = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv9 = null;
      }
    },
    getTenBenhKemRv3yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv3yhct) {
          this.form.tenBenhKemRv3yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv3yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv3yhct = null;
      }
    },
    getTenBenhKemRv4yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv4yhct) {
          this.form.tenBenhKemRv4yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv4yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv4yhct = null;
      }
    },
    getTenBenhKemRv5yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv5yhct) {
          this.form.tenBenhKemRv5yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv5yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv5yhct = null;
      }
    },
    getTenBenhKemRv6yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv6yhct) {
          this.form.tenBenhKemRv6yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv6yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv6yhct = null;
      }
    },
    getTenBenhKemRv7yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv7yhct) {
          this.form.tenBenhKemRv7yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv7yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv7yhct = null;
      }
    },
    getTenBenhKemRv8yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv8yhct) {
          this.form.tenBenhKemRv8yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv8yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv8yhct = null;
      }
    },
    getTenBenhKemRv9yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemRv9yhct) {
          this.form.tenBenhKemRv9yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemRv9yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemRv9yhct = null;
      }
    },
   
    getTenBenhChinhVvyhct(item) {
      if (item) {
        this.form.tenBenhChinhVvyhct = item.tenBenhBhyt;
      }
      else {
        this.form.tenBenhChinhVvyhct = null
      }
    },
    getTenBenhKemVv1yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv1yhct) {
          this.form.tenBenhKemVv1yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv1yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv1yhct = null;
      }
    },
    getTenBenhKemVv2yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv2yhct) {
          this.form.tenBenhKemVv2yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv2yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv2yhct = null;
      }
    },
    getTenBenhKemVv3yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv3yhct) {
          this.form.tenBenhKemVv3yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv3yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv3yhct = null;
      }
    },
    getTenBenhKemVv4yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv4yhct) {
          this.form.tenBenhKemVv4yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv4yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv4yhct = null;
      }
    },
    getTenBenhKemVv5yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv5yhct) {
          this.form.tenBenhKemVv5yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv5yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv5yhct = null;
      }
    },
    getTenBenhKemVv6yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv6yhct) {
          this.form.tenBenhKemVv6yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv6yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv6yhct = null;
      }
    },
    getTenBenhKemVv7yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv7yhct) {
          this.form.tenBenhKemVv7yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv7yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv7yhct = null;
      }
    },
    getTenBenhKemVv8yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv8yhct) {
          this.form.tenBenhKemVv8yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv8yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv8yhct = null;
      }
    },
    getTenBenhKemVv9yhct(item) {
      if (item) {
        if (!this.form.tenBenhKemVv9yhct) {
          this.form.tenBenhKemVv9yhct = item.tenBenh;
        } else if (this.isClickBenhKem) {
          this.form.tenBenhKemVv9yhct = item.tenBenh;
        }
      } else {
        this.form.tenBenhKemVv9yhct = null;
      }
    },
   

  },
};
</script>

<style scoped>
.padding-cols {
  padding-bottom: 0px !important;
  padding-top: 0px !important;
}
</style>
