/// <reference path="../../pdf/base64.js" />
/// <reference path="../../pdf/bkavcaplugin.js" />
/// <reference path="../../pdf/bkav-extension-signer-v2.0.1.js" />
/// <reference path="../../pdf/bootstrap.js" />
/// <reference path="../../pdf/certutils.js" />
/// <reference path="../../pdf/checkbrowser.js" />
/// <reference path="../../pdf/checkresult.js" />
/// <reference path="../../pdf/jquery.xml2json.js" />
/// <reference path="../../pdf/jquery-1.10.2.js" />
/// <reference path="../../pdf/jquery-1.10.2.min.js" />
/// <reference path="../../pdf/modernizr-2.6.2.js" />
/// <reference path="../../pdf/pdf.js" />
/// <reference path="../../pdf/utils.js" />
/// <reference path="../../pdf/respond.js" />
/// <reference path="../../pdf/verify.js" />
/// <reference path="../../pdf/xml2json.js" />
/// <reference path="../../pdf/xml2json.min.js" />
/// <reference path="../../../Extention/Extention.js" />

$(document).ready(function () {
    $("#btnSelectFile").click(function () {
        //checkpluginvalid(CheckPluginCallbackFileBrowser);
        nampey.testEvent();
    });
});

function XmlFileBrowserCallback(data) {
    if (iCheck == 1) {
        document.getElementById('xmlFilePath').value = base64.decode(data);
        document.getElementById('xmlFilePath').innerHTML = base64.decode(data);
    }
    else {
        document.getElementById('xmlFilePath').value = data;
        document.getElementById('xmlFilePath').innerHTML = data;
    }
}
function XmlFileBrowser() {
    FileBrowser(OPEN_FILE_FILTER.PDF, XmlFileBrowserCallback);
}
function SignPdfFileCallback(data) {
    var iRet = parseInt(data);
    //alert(data);
    var dataRet;
    switch (iRet) {
        case 0:
            dataRet = "Không có quyền sử dụng chức năng này";
            alert("Không có quyền sử dụng chức năng này");
            break;
        case 1:
            dataRet = "Dữ liệu được lưu tại: " + xmlFileOut;
            alert("Ký thành công");
            break;
        case 2:
            dataRet = "Không thể ký dữ liệu do dữ liệu đã bị mã hóa";
            alert("Không thể ký dữ liệu do dữ liệu đã bị mã hóa");
            break;
        case 3:
            dataRet = "Không tìm thấy chứng thư số";
            alert("Không tìm thấy chứng thư số");
            break;
        case 4:
            dataRet = "Dữ liệu đầu vào không đúng định dạng";
            alert("Dữ liệu đầu vào không đúng định dạng");
            break;
        case 5:
            dataRet = "Có lỗi trong quá trình ký";
            alert("Có lỗi trong quá trình ký");
            break;
        case 6:
            dataRet = "Có lỗi trong quá trình lưu chữ ký";
            alert("Có lỗi trong quá trình lưu chữ ký");
            break;
        case 13:
            dataRet = "Người dùng hủy bỏ";
            alert("Người dùng hủy bỏ");
            break;
        default:
            dataRet = "Lỗi không xác định";
            alert("Lỗi không xác định");
            break;
    }
    document.getElementById('SigningResult').value = dataRet;
    document.getElementById('SigningResult').innerHTML = dataRet;
}

var xmlFileOut;
function SignXMLFile() {
    var xmlFileIn = document.getElementById('xmlFilePath').value;
    var serial = document.getElementById('selectCertSerial').value.substring("Số serial: ".length, document.getElementById('selectCertSerial').value.length);

    var fileName = xmlFileIn.split('.').pop();
    xmlFileOut = xmlFileIn.substring(0, xmlFileIn.length - fileName.length - 1) + '_signed.pdf';
    var signername1 = document.getElementById('selectCert').value;
    console.log("signerName1 = " + signername1);
    var urlIcon = "";
    var offsetX = "0"
    var offsetY = "600"

    SignPDF(xmlFileIn, xmlFileOut, serial, signername1, urlIcon, offsetX, offsetX, SignPdfFileCallback);
}
function VerifyXmlFileBrowserCallback(data) {
    if (iCheck == 1) {
        document.getElementById('xmlVerifyFilePath').value = base64.decode(data);
        document.getElementById('xmlVerifyFilePath').innerHTML = base64.decode(data);
    }
    else {
        document.getElementById('xmlVerifyFilePath').value = data;
        document.getElementById('xmlVerifyFilePath').innerHTML = data;
    }
}
function VerifyXmlFileBrowser() {
    FileBrowser(OPEN_FILE_FILTER.PDF, VerifyXmlFileBrowserCallback);
}
function VerifyXMlFileCallback(data) {
    var iRet = parseInt(data);
    CheckVerifyResult(iRet);
}
function VerifyXMLFile() {
    var xmlFileIn = document.getElementById('xmlVerifyFilePath').value;
    VerifyPDF(xmlFileIn, VerifyXMlFileCallback);
}


function GetVersionplugin() {
    BkavCAPlugin.GetVersion(GetVersionCallback);
}


function GetVersionCallback(data) {
    alert(data);
}

// verify
function CheckPluginCallbackFileBrowserVerify(data) {
    switch (data) {
        case "0":
            alert("Extension chưa được cài đặt!");
            break;
        case "1":
            VerifyXmlFileBrowser();
            break;
        case "2":
            alert("Plugin chưa được cài đặt!");
            break;
        case "3":
            alert("Có lỗi trong việc xác thực Plugin!");
            break;
    }
}
function CheckPluginCallbackVerify(data) {
    switch (data) {
        case "0":
            alert("Extension chưa được cài đặt!");
            break;
        case "1":
            VerifyXMLFile();
            break;
        case "2":
            alert("Plugin chưa được cài đặt!");
            break;
        case "3":
            alert("Có lỗi trong việc xác thực Plugin!");
            break;
    }
}
// sign
function CheckPluginCallbackFileBrowser(data) {
    switch (data) {
        case "0":
            alert("Extension chưa được cài đặt!");
            break;
        case "1":
            XmlFileBrowser();
            break;
        case "2":
            alert("Plugin chưa được cài đặt!");
            break;
        case "3":
            alert("Có lỗi trong việc xác thực Plugin!");
            break;
    }
}

function CheckPluginCallbackGetCert(data) {
    switch (data) {
        case "0":
            alert("Extension chưa được cài đặt!");
            break;
        case "1":
            GetCertListByFilter(GetCertResult);
            break;
        case "2":
            alert("Plugin chưa được cài đặt!");
            break;
        case "3":
            alert("Có lỗi trong việc xác thực Plugin!");
            break;
    }
}
function CheckPluginCallbackSignXML(data) {
    switch (data) {
        case "0":
            alert("Extension chưa được cài đặt!");
            break;
        case "1":
            SignXMLFile();
            break;
        case "2":
            alert("Plugin chưa được cài đặt!");
            break;
        case "3":
            alert("Có lỗi trong việc xác thực Plugin!");
            break;
    }
}
