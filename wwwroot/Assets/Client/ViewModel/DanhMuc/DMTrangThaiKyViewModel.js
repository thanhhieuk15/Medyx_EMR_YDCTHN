﻿/// <reference path="../libs/knockoutJs/knockout-3.4.0.js" />
/// <reference path="../../../Extention/Extention.js" />
/// <reference path="../../libs/knockoutJs/knockout.mapping-latest.js" />
/// <reference path="../../../Extention/GeneralCategory.js" />
/// <reference path="../../../Extention/Extention.js" />
/// <reference path="../../admin/libs/jquery/dist/jquery.min.js" />
var DMTrangThaiKyViewModel = function () {
    //khởi tạo tham số
    //biến trung gian để lưu thông tin của bản ghi được chọn trước khi thay đổi thông tin 
    var temp;
    var tempTenBS;
    var tempHuy;
    var self = this;
    //biến lưu từ khóa để load dữ liệu
    //self.keyWord = ko.observable('');
    //biến để kiểm tra trạng thái load đầu tiên của trang, nếu trang load lần đầu thì sẽ thực hiện phân trang, nếu không thì không phân trang lại
    self.LoadLanDau = ko.observable(true);
    //biến lưu trang hiện tại
    self.pageIndex = ko.observable(1);
    //biến lưu số lượng bản ghi trong một trang
    self.pageSize = ko.observable(20);
    //biến lưu tổng số bản ghi 
    self.tongSoBanGhi = ko.observable(0);
    self.tongSoTrang = ko.observable(0);
    //mảng lưu danh mục
    self.DMTrangThaiKys = ko.observableArray();
    //biến loading để hiển thị div loading khi trang đang load
    self.loading = ko.observable(false);
    //biến lưu trạng thái có thêm mới hay không? Sử dụng để hiển thị bản ghi mới thêm lên dòng đầu tiên của bảng 
    self.AddNew = ko.observable(false);
    //biến lưu bản ghi đang được chọn
    self.selectedItem = ko.observable();
    self.checkAdd = ko.observable(false);
    //biến lưu trạng thái search
    self.filter = ko.observable(false);
    //#region Knockout Observables
    // Observable array that represents each column in the table
    self.columns = ko.observableArray([
        { property: "ThaoTac", header: "", type: "", state: ko.observable(""), width: '4%', visible: ko.observable(true), field: '', search: '' },
        { property: "STT", header: "STT", type: "int", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'int' },
        { property: "IDBA", header: "Mã Bệnh Án", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "MaBN", header: "Mã Bệnh Nhân", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "MaBS", header: "Mã Bác Sĩ", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "TenBS", header: "Tên Bác Sĩ", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "TrangThaiKy", header: "Trạng Thái Ký", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "LanKy", header: "Lần Ký ", type: "int", state: ko.observable(""), width: '3%', visible: ko.observable(true), field: '', search: 'number' },
        { property: "DuongDanFile", header: "Đường Dẫn File", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "MaMay", header: "Mã máy", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "NgayLap", header: "Ngày Lập", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'date' },
        { property: "NgaySD", header: "Ngày SĐ", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'date' },
        { property: "NguoiSD", header: "Người SĐ", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "NgayHuy", header: "Ngày Hủy", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'date' },
        { property: "NguoiHuy", header: "Người Hủy", type: "string", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: '', search: 'string' },
        { property: "Huy", header: "Hủy", type: "", state: ko.observable(""), width: '5%', visible: ko.observable(true), field: 'checkbox', search: '' }
    ]);
    //Khởi tạo mảng lưu các cột được export
    var cot = "";
    $(self.columns()).each(function (index, value) {
        if (value.visible) {
            if (value.property == 'ThaoTac' || value.property == 'STT') 
            { }
            else
            {
                cot += ' ' + value.property + ',';
            }
        }
    });
    cot = cot.substring(0, cot.length - 1);
    self.colExport = ko.observable(cot);

    self.sortClick = function (column) {
        try {
            // Call this method to clear the state of any columns OTHER than the target
            // so we can keep track of the ascending/descending
            clearColumnStates(column);

            // Get the state of the sort type
            if (column.state() === "" || column.state() === descending) {
                column.state(ascending);
            }
            else {
                column.state(descending);
            }

            switch (column.type) {
                case "":
                    break;
                case "number":
                    self.numberSort(column);
                    break;
                case "date":
                    self.dateSort(column);
                    break;
                case "object":
                    self.objectSort(column);
                    break;
                case "string":
                default:
                    self.stringSort(column);
                    break;
            }
        }
        catch (err) {
            alert(err);
        }
    };
    // Generic sort method for numbers and strings
    self.stringSort = function (column) { // Pass in the column object
        self.DMTrangThaiKys(self.DMTrangThaiKys().sort(function (a, b) {
            // Set strings to lowercase to sort properly
            var playerA = a[column.property].toLowerCase(),
                playerB = b[column.property].toLowerCase();
            if (column.state() === ascending) {
                return playerA.localeCompare(playerB);
            }
            else {
                return playerB.localeCompare(playerA);
            }
        }));
    };
    // Sort by number
    self.numberSort = function (column) {
        self.DMTrangThaiKys(self.DMTrangThaiKys().sort(function (a, b) {
            var playerA = a[column.property], playerB = b[column.property];
            if (column.state() === ascending) {
                return playerA - playerB;
            }
            else {
                return playerB - playerA;
            }
        }));

    };
    // Sort by date
    self.dateSort = function (column) {
        self.DMTrangThaiKys(self.DMTrangThaiKys().sort(function (a, b) {
            if (column.state() === ascending) {
                return new Date(a[column.property]) - new Date(b[column.property]);
            }
            else {
                return new Date(b[column.property]) - new Date(a[column.property]);
            }
        }));
    };
    // Using a deep get method to find nested object properties
    self.objectSort = function (column) {
        self.DMTrangThaiKys(self.DMTrangThaiKys().sort(function (a, b) {
            var playerA = deepGet(a, column.property),
                playerB = deepGet(b, column.property);
            if (playerA < playerB) {
                return (column.state() === ascending) ? -1 : 1;
            }
            else if (playerA > playerB) {
                return (column.state() === ascending) ? 1 : -1;
            }
            else {
                return 0
            }
        }));
    };
    //#endregion
    //#region Utility Methods
    clearColumnStates = function (selectedColumn) {
        var otherColumns = self.columns().filter(function (col) {
            return col != selectedColumn;
        });
        for (var i = 0; i < otherColumns.length; i++) {
            otherColumns[i].state("");
        }
    };
    //hàm để xác định trạng thái sửa/xem của dòng đang chọn 
    self.displayMode = function (DMTrangThaiKy) {
        return DMTrangThaiKy.Edit() ? 'edit-template' : 'read-template';
    }
    //sự kiện nhấn phím khi đang sửa thông tin bản ghi
    self.updateEnterInput = function (DMTrangThaiKy, event) {
        var keyCode = (event.which ? event.which : event.keyCode);
        if (keyCode === 13) {
            DMTrangThaiKy.TenBS = $('#txtTenBS_Edit').val();
            self.done(DMTrangThaiKy);
            return false;
        }
        return true;
    }
    //sự kiện nhấn phím trong checkbox hủy cho phép lấy lại bản ghi bị xóa
    self.updateEnterHuy = function (DMTrangThaiKy, event) {
        var keyCode = (event.which ? event.which : event.keyCode);
        if (keyCode === 32) {
            if ($('#chkHuy_edit').is(":checked")) {
                DMTrangThaiKy.Huy = !DMTrangThaiKy.Huy;
                $('#chkHuy_edit').prop('checked', !$('#chkHuy_edit').is(":checked"));
            }
            return false;
        }
        else if (keyCode === 13) {
            DMTrangThaiKy.TenBS = $('#txtTenBS_Edit').val();
            self.done(DMTrangThaiKy);
            return false;
        }
        return true;
    }
    //sửa thông tin
    self.edit = function (DMTrangThaiKy) {
        if ($('#txtQuyenXem').val()=='true')
        {
            if (temp != undefined) {
                self.cancel(temp);
            }
            temp = DMTrangThaiKy;
            tempTenBS = DMTrangThaiKy.TenBS;
            tempHuy = DMTrangThaiKy.Huy;
            self.AddNew(false);
            DMTrangThaiKy.Edit(true);
            //$('#' + self.selectedItem().MaBN).focus();
            $('#txtTenBS_Edit').focus();
            //$("tbody").css("overflow-y", "hidden");
            //$("body").css("overflow-y", "hidden");
            //Extention.autoResizeTable();
        }
    }
    //xác nhận thông tin đã thay dổi
    self.done = function (DMTrangThaiKy) {
        if (DMTrangThaiKy.TenBS.trim() == "") {
            DMTrangThaiKy.Edit(false);
            alert('Update không thành công!');
        }
        else {
            self.checkAdd(false);
            DMTrangThaiKy.Edit(false);
            if (!self.AddNew()) {
                self.updateDMTrangThaiKy(DMTrangThaiKy);
            }
            temp = null;
            tempTenBS = "";
            tempHuy = false;
        }
        //$("tbody").css("overflow-y", "auto");
        //Extention.autoResizeTable();
    }
    //hủy thông tin đã thay đổi
    self.subCancel = function (DMTrangThaiKy) {
        for (var i = 0; i < self.DMTrangThaiKys().length; i++) {
            if (self.DMTrangThaiKys()[i].IDBA == DMTrangThaiKy.IDBA) {
                self.DMTrangThaiKys()[i].TenBS = tempTenBS;
                self.DMTrangThaiKys()[i].Huy   = tempHuy;
            }
        }
        DMTrangThaiKy.Edit(false);
        //$("tbody").css("overflow-y", "auto");
        //Extention.autoResizeTable();
    }

    self.cancel = function (DMTrangThaiKy) {
        self.subCancel(DMTrangThaiKy);
        temp = null;
        tempTenBS = "";
        tempHuy = false;
        var item = self.DMTrangThaiKys()[0];
        if (self.AddNew()) {
            self.DMTrangThaiKys.remove(DMTrangThaiKy);
            self.AddNew(false)
        }
    }
    //xóa bản ghi
    self.delete = function (DMTrangThaiKy) {
        if ($('#txtQuyenXem').val() == 'true')
        {
            if (confirm('Bạn chắc chắn muốn xóa trạng thái ký  "' + DMTrangThaiKy.IDBA + ' - ' + DMTrangThaiKy.TenBS + '" ?')) {
                DMTrangThaiKy.Huy = true;
                self.deleteChucDanh(DMTrangThaiKy);
            }
        }
    }
    //hàm xác định bản ghi đang được chọn
    self.selectItem = function (DMTrangThaiKy) {
        self.selectedItem(DMTrangThaiKy);
    };
    //searchResults = dmTinhs --- selectResult = selectItem --- selectedResult = selectedItem
    //move row up and down in table
    self.selectPrevious = function () {
        //var index = self.DMTrangThaiKys().indexOf(self.selectedItem()) - 1;
        var index = 0;
        if (self.selectedItem() != undefined) {
            if (self.selectedItem().STT > 1 && self.selectedItem().STT % self.pageSize() == 0) {
                index = self.pageSize() - 2;
            }
            else
                index = self.DMTrangThaiKys().indexOf(self.selectedItem()) - 1;
        }
        if (index < 0) index = 0;
        self.selectedItem(self.DMTrangThaiKys()[index]);
        Extention.selectHeaderRow($('#' + self.selectedItem().IDBA), 31);
        $('#' + self.selectedItem().IDBA).focus();
    };

    self.selectNext = function () {
        //var index = self.DMTrangThaiKys().indexOf(self.selectedItem()) + 1;
        var index = 0;
        if (self.selectedItem() != undefined)
        { index = self.DMTrangThaiKys().indexOf(self.selectedItem()) + 1; }
        if (index >= self.DMTrangThaiKys().length) index = self.DMTrangThaiKys().length - 1;
        self.selectedItem(self.DMTrangThaiKys()[index]);
        Extention.selectHeaderRow($('#' + self.selectedItem().IDBA), 31);
        $('#' + self.selectedItem().IDBA).focus();
    };
    //tìm kiếm
    //self.search = function () {
    //    self.keyWord($('#txtsearch').val());
    //    self.pageIndex(1);
    //    self.LoadLanDau(true);
    //    self.loadData();
    //}
    //sự kiện nhấn phím trong bảng
    self.upAndDown = function (data, e) {
        //e.preventDefault();
        //nhấn up, down arrow
        if (e.keyCode == 38) {
            if (self.selectedItem().Edit() == false) {
                self.selectPrevious();
            }
            else {
                $('#' + self.selectedItem().IDBA).focus();
            }
            event.stopPropagation();
            return false;
        }
        else if (e.keyCode == 40) {
            if (self.selectedItem().Edit() == false) {
                self.selectNext();
            }
            else
                $('#' + self.selectedItem().IDBA).focus();
            event.stopPropagation();
            return false;
        }
        else if (event.keyCode == 13) {
            if (self.selectedItem() != undefined) {
                if (self.selectedItem().Edit() == false) {
                    self.edit(self.selectedItem());
                }
                return false;
            }
            else return true;
        }
        else if (e.keyCode == 27) {
            self.subCancel(self.selectedItem());
            self.selectNext();
            $('#' + self.selectedItem().IDBA).focus();
            return false;
        }
        else if (e.keyCode == 9) {
            if (self.selectedItem() == undefined) {
                self.selectNext();
                $('#' + self.selectedItem().IDBA).focus();
            }
            if (this.IDBA != self.selectedItem().IDBA) {
                var index = self.DMTrangThaiKys().indexOf(self.selectedItem()) + 1;
                if (index >= self.DMTrangThaiKys().length) $('#table').focusout();
                else $('#' + self.selectedItem().IDBA).focus();
            }
            return false;
        }
        else return true;
    }

    $(document).ready(function () {
        //thay đổi số lượng bản ghi hiển thị trên một trang
        $("#txtPageSize").change(function () {
            if (self.pageSize() == "")
                self.pageSize(20);
            self.LoadLanDau(true);
            self.loadData();
        });
        //sự kiện nhấn phím trong ô search để search các bản ghi
        //$('#txtsearch').keypress(function (e) {
        //    var code;
        //    if (document.all) {
        //        e = window.event;
        //        code = e.keyCode;
        //    }
        //    if (e.which) {
        //        code = e.which;
        //    }
        //    //nhấn enter để tìm kiếm trong sql
        //    if (code == 13) {// Enter
        //        self.search();
        //    }
        //});
        $('#txtPageSize').keypress(function (e) {
            var code;
            if (document.all) {
                e = window.event;
                code = e.keyCode;
            }
            if (e.which) {
                code = e.which;
            }
            if (code == 13) {// Enter
                self.pageSize($('#txtPageSize').val());
                self.LoadLanDau(true);
                self.loadData();
            }
        });
        //bắt sự kiện nhấn enter ở nút nhập trạng thái ký 
        $('#txtiTenBS').keypress(function (e) {
            var code;
            if (document.all) {
                e = window.event;
                code = e.keyCode;
            }
            if (e.which) {
                code = e.which;
            }
            if (code == 13) {// Enter
                self.insert();
            }
        });
        //set focus vào ô nhập mới  trạng thái ký  khi trang được load
        if ($('#txtQuyenXem').val() == 'true') {
            $('#txtiTenBS').focus();
        }
        else{
            $('#txtiTenBS').attr('disabled','disabled');
        }
        //$(":checkbox").focus(function () {
        //    $(this).css('outline-color', 'red');
        //});
        $('.btnFilter').click(function () {
            self.filter(!self.filter());
            if (self.filter() == true) {
                $('#txtsSTT').focus();
            }
            else {
                clearFilter();
                self.loadData();
            }
        });
        $('#SearchTemplate :input').keypress(function (e) {
            var code;
            if (document.all) {
                e = window.event;
                code = e.keyCode;
            }
            if (e.which) {
                code = e.which;
            }
            //nhấn enter để tìm kiếm trong sql
            if (code == 13) {// Enter
                self.pageIndex(1);
                self.LoadLanDau(true);
                self.loadData();
            }
        });
    });
    //load dữ liệu
    self.loadData = function () {
       // var STT = 0;
        var add = false;
        if (self.checkAdd() == true) {
            add = true;
        }
        var ngay = $('#txtsNgaySD').val();
        if (ngay == '') {
            ngay = undefined;
        }
        else if (ngay != undefined) {
            ngay = Extention.convertToMMDDYYYY(ngay);
        }
        var data = {
            stt: $('#txtsSTT').val(),
            idBA: $('#txtsIDBA').val(),
            maBN: $('#txtsMaBN').val(),
            maBS: $('#txtsMaBS').val(),
            tenBS: $('#txtsTenBS').val(),
            trangthaiKy: $('#txtsTrangThaiKy').val(),
            lanKy: $('#txtsLanKy').val(),
            duongdanFile: $('#txtsDuongDanFile').val(),
            maMay: $('#txtsMaMay').val(),
            ngayLap: ngay,
            ngaySD: ngay,
            nguoiSD: $('#txtsNguoiSD').val(),
            ngayHuy: ngay,
            nguoiHuy: $('#txtsNguoiHuy').val(),
            pageIndex: self.pageIndex,
            pageSize: self.pageSize,
            add: add
        };
        STT = self.pageSize() * (self.pageIndex() - 1);
        $.getJSON('/DMTrangThaiKy/LoadData', data, function (data) {
            self.tongSoBanGhi(data.totalCounts);
            self.tongSoTrang = data.totalPages;
            self.DMTrangThaiKys(ko.utils.arrayMap(data.items, function (DMTrangThaiKy) {
                //STT += 1;
                var objDMTrangThaiKy = {
                    //STT: STT,
                    Edit: ko.observable(false),
                    STT: DMTrangThaiKy.stt, 
                    IDBA: DMTrangThaiKy.idba,
                    MaBN: DMTrangThaiKy.maBN,
                    MaBS: DMTrangThaiKy.maBS,
                    TenBS: DMTrangThaiKy.tenBS,
                    TrangThaiKy: DMTrangThaiKy.trangThaiKy,
                    LanKy: DMTrangThaiKy.lanKy,
                    DuongDanFile: DMTrangThaiKy.duongDanFile,
                    MaMay: (DMTrangThaiKy.maMay == undefined) ? "" : DMTrangThaiKy.maMay,
                    NgayLap: Extention.ConvertSQLDateTimeToStringFormat(DMTrangThaiKy.ngayLap),
                    NgaySD: Extention.ConvertSQLDateTimeToStringFormat(DMTrangThaiKy.ngaySD),
                    NguoiSD: (DMTrangThaiKy.nguoiSD == undefined) ? "" : DMTrangThaiKy.nguoiSD,
                    NgayHuy: Extention.ConvertSQLDateTimeToStringFormat(DMTrangThaiKy.ngayHuy),
                    NguoiHuy: (DMTrangThaiKy.nguoiSD == undefined) ? "" : DMTrangThaiKy.nguoiHuy,
                    Huy: DMTrangThaiKy.huy
                }
                return objDMTrangThaiKy;

                if (DMTrangThaiKy.Huy == false) {
                    $('#chkHuy_read').checked(true);
                }
            }));
            if (self.DMTrangThaiKys().length != 0) {
            if (self.LoadLanDau())
                self.paging();
            if (self.selectedItem() != undefined) {
                self.selectedItem().Edit(false);
                self.selectItem(self.selectedItem());
                $('#' + self.selectedItem().IDBA).focus().addClass('danger');
            }
            }
            else {
                alert('Không có dữ liệu để hiển thị!');
            }
        });
    }
    
    //phân trang
    self.paging = function () {
        $('.pagination-custom').empty().removeData("twbs-pagination").unbind("page").twbsPagination({
            totalPages: parseInt(self.tongSoTrang), //Giá trị này sẽ được lấy từ hàm LoadData 
            visiblePages: 3,
            first: 'Đầu Trang',
            last: 'Cuối Trang',
            next: 'Trang tiếp',
            prev: 'Quay lại',
            initiateStartPageClick: true,
            onPageClick: function (event, page) {
                self.pageIndex(page); // Set pageIndex
                if (page > 1)
                    self.LoadLanDau(false); // first Trùng tên từ khóa
                if (self.LoadLanDau() == false) {
                    self.loadData();
                    return;
                }
            }

        });
    }
    //khởi tạo
    self.init = function () {
        Extention.backToHome();
        self.loadData();
        //self.regestEvent();
    }
    self.init();
    //thêm mới bản ghi
    self.insert = function (DMTrangThaiKy) {
        if ($('#txtQuyenXem').val() == 'true') {
            self.checkAdd(true);
            if ($('#txtiTenBS').val().trim() != '') {
                $.ajax({
                    url: '/DMTrangThaiKy/CreateTrangThaiKy',
                    type: 'POST',
                    data: { TenBS: $('#txtiTenBS').val().trim() },
                    success: function (response) {
                        if (response != null && response.success == false)
                            alert(response.message);
                        else {
                            alert('Thêm mới thành công!');
                            self.cancelInsert();
                            self.loadData();
                        }
                    },
                    error: function (error) {
                        alert('Thêm mới không thành công! Kiểm tra lại trường nhập Trạng Thái Ký ');
                    }
                });
            }
            else {
                alert('Thêm mới không thành công! Kiểm tra lại trường nhập  trạng thái ký ');
                Extention.errorBorder('txtiTenBS');
                self.cancelInsert();
                $('#txtiTenBS').focus();
            }
        }
    }
    //xóa bản ghi
    self.deleteTrangThaiKy = function (DMTrangThaiKy) {
        $.ajax({
            url: '/DMTrangThaiKy/DeleteTrangThaiKy',
            type: 'POST',
            data: { idBA: DMTrangThaiKy.IDBA },
            success: function (response) {
                if (response != null && response.success == false)
                    alert(response.message);
                else {
                    if (response != null && response.success == false)
                        alert(response.message);
                    else {
                        alert("Xóa Trạng Thái  thành công!");
                        self.loadData();
                    }
                }
            },
            error: function (error) {
                alert("Xóa Trạng Thái không thành công!");
            }
        })
    }
    //Import dữ liệu
    self.Import = function () {
        $('#importPopUp').modal("show");
    }
    self.submitImport = function () {
        //var myFile = $('#importFile').prop('files');
        var fileUpload = $("#importFile").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        fileData.append('insert', $('#importNhapMoi').is(":checked"));
        if ($('#txtQuyenXem').val() == 'true') {
            //self.checkAdd(true);
            if ($('#importFile').val().trim() != '') {
                $.ajax({
                    url: '/DMTrangThaiKy/ImportDanhMuc',
                    type: 'POST',
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (response) {
                        if (response != null && response.success == false)
                            alert(response.message);
                        else {
                            alert('Import dữ liệu thành công!');
                            self.loadData();
                        }
                    },
                    error: function (error) {
                        alert('Import dữ liệu không thành công! Kiểm tra lại file import');
                    }
                });
            }
        }
        $('#importPopUp').modal("hide");
    }
    //Export dữ liệu
    self.Export = function () {
        if (self.tongSoBanGhi() <= 1048570) {
            if (self.colExport().length > 0) {
                var dk = generateDK().replace('%', '%25');
                var obj = {
                    key: dk,
                    columns: self.colExport()
                }
                window.open('/DMTrangThaiKy/ExportExcel?obj=' + JSON.stringify(obj));
                //$.ajax({
                //    url: '/DMTrangThaiKy/ExportDMTrangThaiKy',
                //    type: 'POST',
                //    data: { key: generateDK(), columns: self.colExport() },
                //    success: function (response) {
                //        alert('Export dữ liệu thành công!');
                //    },
                //    error: function (error) {
                //        alert("Export dữ liệu không thành công!");
                //    }
                //})
            }
            else {
                alert("Không thể export dữ liệu! Vui lòng chọn lại cột hiển thị");
            }
        }
        else {
            alert("Số lượng bản ghi quá lớn! Không thể export dữ liệu! Vui lòng hạn chế số lượng bản ghi <= 1,048,500 bản ghi!");
        }
    }
    //cập nhật thông tin của bản ghi
    self.updateDMTrangThaiKy = function (DMTrangThaiKy) {
        $.ajax({
            url: '/DMTrangThaiKy/Modify',
            type: 'POST',
            data: { trangthaiKy : DMTrangThaiKy },
            success: function (response) {
                if (response != null && response.success == false)
                    alert(response.message);
                else {
                    self.loadData();
                    alert('Update thành công!');
                }
            },
            error: function (error) {
                alert('Update không thành công!');
            }
        });
    }
    //reset ô nhập thông tin thêm mới
    self.cancelInsert = function () {
        $('#txtiTenBS').val('');

    }
    
    //sự kiện hiển thị các cột trong bảng
    self.CreateVisibleColumn = function () {
        var cotExport = '';
        //event.preventDefault();
        var mang = [];
        $('#cboColumn input:checked').each(function (index, value) {
            if (value.id == "All") { }
            else
            {
                mang.push(value.id);
                if (value.id == "ThaoTac") { }//|| value.id == "STT"
                else {
                    cotExport += ' ' + value.id + ',';
                }
            }
        });
        var i = 0;
        $(self.columns()).each(function (index, value) {
            if (value.property == mang[i]) {
                value.visible(true);
                i++;
            }
            else value.visible(false);
        });
        cotExport = cotExport.substring(0, cotExport.length - 1);
        self.colExport(cotExport);
    }
    //Load combo columns
    self.LoadDMColumn = function () {
        GeneralCategory.LoadDMColumn('cboColumn', self.columns());
        $('#cboColumn .dropdown-menu a').click(function (event) {
            self.CreateVisibleColumn();
            //Căn chỉnh lại kích thước cột
            Extention.autoResizeTable();
        });
        $('#cboColumn input[type=checkbox]').change(function () {
            if ($(this).attr('id') == "All") {
                if ($('#All').is(":checked") == true) {
                    $('#All').prop('checked', true);
                    $('#cboColumn input[type=checkbox]').each(function (index, value) {
                        $(value).prop('checked', true);
                    });
                }
                else {
                    $('#All').prop('checked', false);
                    $('#cboColumn input[type=checkbox]').each(function (index, value) {
                        $(value).prop('checked', false);
                    });
                }
            }
            else {
                if ($('#All').is(":checked") == true) {
                    $('#All').prop('checked', false);
                }
            }
            self.CreateVisibleColumn();
            //Căn chỉnh lại kích thước cột
            Extention.autoResizeTable();
        });
    }
    self.LoadDMColumn();
    //Create insert Template
    self.CreateSearchTemplate = function () {
        GeneralCategory.CreateSearchTemplate('thead', self.columns());
    }
    self.CreateSearchTemplate();
    //Create read Template
    self.CreateReadTemplate = function () {
        if ($('#txtQuyenXem').val() == 'true') {
            GeneralCategory.CreateReadTemplate('boxBody', self.columns(), '');
        }
        else {
            GeneralCategory.CreateReadTemplate('boxBody', self.columns(), 'disabled');
        }
    }
    self.CreateReadTemplate();
    //Create insert Template
    self.CreateInsertTemplate = function () {
        if ($('#txtQuyenXem').val() == 'true') {
            GeneralCategory.CreateInsertTemplate('thead', self.columns(), '');
        }
        else {
            GeneralCategory.CreateInsertTemplate('thead', self.columns(), 'disabled');
        }
    }
    self.CreateInsertTemplate();
    //Create edit Template
    self.CreateEditTemplate = function () {
        if ($('#txtQuyenXem').val() == 'true') {
            GeneralCategory.CreateEditTemplate('thead', self.columns(), '');
        }
        else {
            GeneralCategory.CreateEditTemplate('thead', self.columns(), 'disabled');
        }
    }
    self.CreateEditTemplate();
}
//custom binding ngăn không cho click vào checkbox
ko.bindingHandlers.preventBubble = {
    init: function (element, valueAccessor) {
        var eventName = ko.utils.unwrapObservable(valueAccessor());
        ko.utils.registerEventHandler(element, eventName, function (event) {
            event.cancelBubble = true;
            if (event.stopPropagation) {
                event.stopPropagation();
            }
        });
    }
};
ko.applyBindings(new DMTrangThaiKyViewModel());