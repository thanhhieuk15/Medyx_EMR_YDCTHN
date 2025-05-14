<template>
    <Crud :fields="fields" :apiCrudFunctions="apiCrudFunctions" :apiCategoryFunctions="apiCategoryFunctions"
        :actions="actions" :disableActions="disableActions" primaryKey="stt" label-width="100px" tableHeight="400px"
        :wrapper="false" onlyTable :hasExit="false" ref="table" :customFormFields="customFormFields">
    </Crud>
</template>

<script>

import { getBenhAnTienSuSan, storeTienSuSan, updateTienSuSan, deleteTienSuSan } from "@/api/benh-an.js";
import Crud from "@/components/crud/Index.vue";
import tienSuBenhAnSelecttion from "../A-y-hoc-hien-dai/tienSuBenhAnSelecttion.vue";
export default {
    components: {
        Crud,
    },
    props: {
        title: {
            type: String,
            default: "",
        },
        params: {
            type: Object,
            default: () => { },
        },
        dataDetail: {
            type: Object,
            default: () => { },
        },
        permission: {
            type: Array,
            default: () => [],
        },
    },
    mounted() {

    },
    watch: {
        params: {
            handler: function (neVal) {
                this.$refs.table.reset();
            },
            deep: true,
        },
    },
    data: (vm) => ({
        customFormFields: tienSuBenhAnSelecttion,
        currentUser: JSON.parse(localStorage.getItem("currentUser")),
        fields: [
            {
                text: "STT",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "stt",
                searchValue: "stt",
                width: 70,
                align: "center",

            },
            {
                text: "Số lần có thai",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "soLanCoThai",
                searchValue: "soLanCoThai",
                width: 250,
                align: "center",
                form: {
                    value: "soLanCoThai",
                    fromValue: "soLanCoThai",
                    default: 1,
                    type: "text",
                    creatable: true,
                    editable: true,
                    rules: [
                        {
                            validator: (v) => v !== null && v !== undefined,
                            message: "Trường này bắt buộc nhập",
                        },
                        {
                            validator: (v) => v > 0,
                            message: "Trường này bắt buộc lớn hơn 0",
                        },
                        {
                            validator: (v) => v < 127,
                            message: "Trường này bắt buộc nhập nhỏ hơn hoặc bằng 127",
                        },
                    ],
                },
            },
            {
                text: "Năm",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "nam",
                searchValue: "nam",
                width: 120,
                align: "center",
                form: {
                    value: "nam",
                    fromValue: "nam",
                    default: 1,
                    type: "text",
                    creatable: true,
                    editable: true,
                    rules: [
                        {
                            validator: (v) => v !== null && v !== undefined,
                            message: "Trường này bắt buộc nhập",
                        },

                    ],
                },
            },
            {
                text: "Đẻ đủ tháng",
                value: "deDuThang",
                searchValue: "deDuThang",
                showable: true,
                filterable: false,
                filterValue: "deDuThang",
                type: "text",
                width: 200,
                form: {
                    value: "deDuThang",
                    fromValue: "deDuThang",
                    default: 1,
                    type: "text",
                    creatable: true,
                    editable: true,
                    rules: [
                        {
                            validator: (v) => v !== null && v !== undefined,
                            message: "Trường này bắt buộc nhập",
                        },

                    ],
                },

            },
            {
                text: "Đẻ thiếu tháng",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "deThieuThang",
                searchValue: "deThieuThang",
                width: 200,
                form: {
                    value: "deThieuThang",
                    fromValue: "deThieuThang",
                    default: 1,
                    type: "text",
                    creatable: true,
                    editable: true,
                    rules: [
                        {
                            validator: (v) => v !== null && v !== undefined,
                            message: "Trường này bắt buộc nhập",
                        },

                    ],
                },

            },
            {
                text: "Sẩy",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "say",
                searchValue: "say",
                width: 200,
                form: {
                    value: "say",
                    fromValue: "say",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },

            },

            {
                text: "Hút",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "hut",
                searchValue: "hut",
                width: 120,
                form: {
                    value: "hut",
                    fromValue: "hut",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Nạo",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "nao",
                searchValue: "nao",
                width: 200,
                form: {
                    value: "nao",
                    fromValue: "nao",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Co-vac",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "coVac",
                searchValue: "coVac",
                width: 300,
                form: {
                    value: "coVac",
                    fromValue: "coVac",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Chửa ngoài tử cung",
                type: "text",
                showable: true,
                filterable: false,
                value: "chuaNgoaiTC",
                searchValue: "chuaNgoaiTC",
                width: 230,
                form: {
                    value: "chuaNgoaiTC",
                    fromValue: "chuaNgoaiTC",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },

            {
                text: "Chửa trứng",
                type: "text",
                showable: true,
                filterable: false,
                value: "chuaTrung",
                searchValue: "chuaTrung",
                filterValue: "chuaTrung",
                width: 170,
                form: {
                    value: "chuaTrung",
                    fromValue: "chuaTrung",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Thai chết lưu",
                type: "text",
                showable: true,
                filterable: false,
                value: "thaiChetLuu",
                searchValue: "thaiChetLuu",
                filterValue: "thaiChetLuu",
                width: 230,
                sortable: true,
                form: {
                    value: "thaiChetLuu",
                    fromValue: "thaiChetLuu",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Con sống",
                type: "text",
                showable: true,
                value: "conSong",
                searchValue: "conSong",
                filterable: false,
                width: 170,
                sortable: true,
                form: {
                    value: "conSong",
                    fromValue: "conSong",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Cân nặng",
                type: "text",
                showable: true,
                filterable: false,
                value: "canNang",
                searchValue: "canNang",
                width: 100,
                align: "center",
                form: {
                    value: "canNang",
                    fromValue: "canNang",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Phương pháp đẻ",
                type: "text",
                showable: true,
                filterable: false,
                value: "phuongPhapDe",
                searchValue: "phuongPhapDe",
                width: 170,
                form: {
                    value: "phuongPhapDe",
                    fromValue: "phuongPhapDe",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
            {
                text: "Tai biến",
                type: "text",
                showable: true,
                filterable: false,
                value: "taiBien",
                searchValue: "taiBien",
                width: 170,
                form: {
                    value: "taiBien",
                    fromValue: "taiBien",
                    default: null,
                    type: "text",
                    creatable: true,
                    editable: true,
                },
            },
        ],
        apiCategoryFunctions: {
        },
        actions: ["edit", "delete", "create"],
        disableActions: {
            edit: (item) => item.huy,
            delete: (item) => item.huy,
        },
    }),
    methods: {},
    created() {
    },
    computed: {
        apiCrudFunctions() {
            const vm = this;
            const id = window.location.href.split("/").at(-1);
            return {
                index: (params) => {
                    if (vm.currentUser.is_admin) {
                        return getBenhAnTienSuSan(id);
                    } else {
                        return getBenhAnTienSuSan(id);
                    }
                },
                store: (data) =>
                    storeTienSuSan({
                        idba: id,
                        ...data,
                    }),
                update: (...data) => {
                    data[1].stt = data[0];
                    updateTienSuSan(id,data[0], data[1])
                },
                destroy: (...data) => deleteTienSuSan(id, ...data[0]),
            };
        },
    },
};
</script>