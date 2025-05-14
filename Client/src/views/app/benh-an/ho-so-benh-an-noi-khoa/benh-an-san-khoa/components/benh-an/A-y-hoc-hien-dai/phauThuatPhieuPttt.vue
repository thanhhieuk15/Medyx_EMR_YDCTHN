<template>
    <Crud :fields="fields" :apiCrudFunctions="apiCrudFunctions" :apiCategoryFunctions="apiCategoryFunctions"
        :actions="actions" :disableActions="disableActions" primaryKey="sttpt" label-width="100px" tableHeight="400px"
        :wrapper="false" onlyTable :hasExit="false" ref="table" :customFormFields="customFormFields">
    </Crud>
</template>

<script>
import { getDeatilPhieuPhauThuat, addPhieuPhauThuatPttt } from "@/api/benh-an.js";
import { updatePhieuPhauThuatPttt, detroyPhieuPhauThuat } from "@/api/phau-thuat-thu-thuat.js";
import Crud from "@/components/crud/Index.vue";
import { getNhanVien, getNhanVienBSPT } from "@/api/to-dieu-tri";
import { formatDate } from "@/utils/formatters";
import phauThuatPhieuPtttSelection from "../A-y-hoc-hien-dai/phauThuatPhieuPtttSelection.vue";
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
        customFormFields: phauThuatPhieuPtttSelection,
        currentUser: JSON.parse(localStorage.getItem("currentUser")),
        fields: [
            {
                text: "STT",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "sttpt",
                searchValue: "sttpt",
                width: 70,
                align: "center",

            },
            {
                text: "Giờ, ngày",
                type: "datetime",
                sortable: true,
                showable: true,
                filterable: false,
                value: "ngayPt",
                searchValue: "ngayPt",
                width: 150,
                align: "center",
                formatter: function (_, __, value) {
                    return formatDate(value);
                },
                form: {
                    value: "ngayPt",
                    fromValue: "ngayPt",
                    default: 1,
                    type: "datetime",
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
                text: "Phương pháp phẫu thuật/ vô cảm",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "phuongPhapVoCam",
                searchValue: "phuongPhapVoCam",
                width: 250,
                form: {
                    value: "phuongPhapVoCam",
                    fromValue: "phuongPhapVoCam",
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
                text: "Phẫu thuật viên",
                value: "bspt.hoTen",
                searchValue: "bspt.hoTen",
                showable: true,
                filterable: false,
                filterValue: "bspt.hoTen",
                type: "text",
                width: 200,
                form: {
                    value: "bspt",
                    fromValue: "bspt.maNv",
                    default: null,
                    creatable: true,
                    editable: true,
                    type: "select-async",
                    label: (item) => `${item.maNv}- ${item.hoTen} `,
                    keyValue: "maNv",
                    apiFunc: getNhanVienBSPT,
                },

            },
            {
                text: "Bác sỹ gây mê",
                type: "text",
                sortable: true,
                showable: true,
                filterable: false,
                value: "bsgayMe.hoTen",
                searchValue: "bsgayMe.hoTen",
                width: 200,
                form: {
                    value: "bsgayMe",
                    fromValue: "bsgayMe.maNv",
                    default: null,
                    creatable: true,
                    editable: true,
                    type: "select-async",
                    label: (item) => `${item.maNv}- ${item.hoTen} `,
                    keyValue: "maNv",
                    apiFunc: getNhanVien,
                },

            },
        ],
        apiCategoryFunctions: {
            getNhanVien: {
                func: getNhanVien,
                textField: "hoTen",
                valueField: "maNv",
            },
            getNhanVienBSPT: {
                func: getNhanVienBSPT,
                textField: "hoTen",
                valueField: "maNv",
            },
        },
        actions: ["edit", "delete", "create"],
        disableActions: {
            edit: (item) => item.huy,
            delete: (item) => item.huy,
        },

    }),
    computed: {
        apiCrudFunctions() {
            const id = window.location.pathname.split("/").filter(Boolean).at(-1);
            return {
                index: async (params) => {
                    try {
                        const response = await getDeatilPhieuPhauThuat(id);
                        return response;
                    } catch (error) {
                        console.error("Lỗi khi tải dữ liệu:", error);
                        return null;
                    }
                },
                store: async (data) => {
                    try {
                        await addPhieuPhauThuatPttt({
                            idba: id,
                            ...data,
                        });
                        this.$refs.table.refresh();
                    } catch (error) {
                        console.error("Lỗi khi thêm dữ liệu:", error);
                    }
                },
                update: async (...data) => {
                    try {
                        await updatePhieuPhauThuatPttt(id, data[0], data[1]);
                        this.$refs.table.load(response.data);
                    } catch (error) {
                        console.error("Lỗi khi cập nhật:", error);
                    }
                },
                destroy: async (...data) => {
                    try {
                        await detroyPhieuPhauThuat(id, ...data[0]);
                    } catch (error) {
                        console.error("Lỗi khi xóa:", error);
                    }
                },
            };
        },
    },


};
</script>