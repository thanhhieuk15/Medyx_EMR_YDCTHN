/************************************************************************/
/* CheckPluginValid: version plugin >1.0.12                         	*/
/************************************************************************/
var functionCallBackAfterCheckSuccessEnvironmentSign;
// document.addEventListener(EXTENSION_EVENT_NAME.EXTENSION_VALID, function (data) {
//     var result = document.getElementById('hrSignedData').value;
//     console.log(result);
//     alert(result);
// });

function CheckPluginValid(jsCallback) {
    functionCallBackAfterCheckSuccessEnvironmentSign = jsCallback;

    // $.isLoading({ text: (step == undefined ? "" : "Bước " + step + ": ") + "Kiểm tra môi trường cài đặt extension và plugin" });
    setTimeout(CheckPlugin, 1);
    // BkavCAPlugin.Connect(functionCallBackAfterCheckSuccessEnvironmentSign);
}

function CheckPlugin() {
    BkavCAPlugin.Connect(ProcessResultConnect);
}
function ProcessResultConnect(result) {

    if (result == 1)//connect socket ok       
    {

        BkavCAPlugin.Connect(functionCallBackAfterCheckSuccessEnvironmentSign);
    }

    else {
        alert("Vui lòng kiểm tra lại Plugin trên máy");

    }

}

function CheckPlugin_Extension() {
    isCheckedPlugin = false;

    try {
        if (checkBrowser() == 1 || checkBrowser() == 3) {//Chrome
            BkavCAPlugin.ExtensionIsValid(functionCallBackAfterCheckSuccessEnvironmentSign);

            // Bắt event EXTENSION_EVENT_NAME.EXTENSION_VALID để lấy kết quả trả về  
            // Kết quả trả về: 0: Chưa cài Extension; 1: OK; 2: Chưa cài NativeApp
        }
        else {
            initPlugin();  // de su dung duoc plugin thi can goi ham initPlugin();
            // chi can goi ham nay 1 lan trong qua trinh su dung cac ham cua plugin
            var result = BkavPluginSigner.PluginValid();// kết quả trả về: true or false
            if (!result) {
                ShowMessageCheckCert(CODE_ERROR.CHECK_PLUGIN_YET_SETUP_PLUGIN);
                $.isLoading("hide");

            }
            else
                CheckVersionPlugin();


        }
    }
    catch (e) {
        alert(e)
    }
}
function ProcessExtensionIsValid_Sign() {
    if (!isCheckedPlugin) {
        isCheckedPlugin = true;

        var result = document.getElementById('hrSignedData').value;
        // Kết quả trả về: 0: Chưa cài Extension; 1: OK; 2: Chưa cài NativeApp ; 3: Plugin không phù hợp
        if (result == 1) {
            CheckVersionPlugin();
            return;
        }

        if (result == 0)
            ShowMessageCheckCert(CODE_ERROR.CHECK_PLUGIN_YET_SETUP_EXTENSION);

        if (result == 2)
            ShowMessageCheckCert(CODE_ERROR.CHECK_PLUGIN_YET_SETUP_PLUGIN);

        if (result == 3)
            ShowMessageCheckCert(CODE_ERROR.CHECK_PLUGIN_SETUP_PLUGIN_NOT_VALID);

        $.isLoading("hide");
    }
}

//--------Check version plugin---------------//
var isCheckedVersionPlugin;
function CheckVersionPlugin() {
    isCheckedVersionPlugin = false;

    try {
        if (checkBrowser() == 1 || checkBrowser() == 3) {//Chrome
            ListenerGetVersion();
            BkavCAPlugin.GetVersion();
        }
        else {
            initPlugin();  // de su dung duoc plugin thi can goi ham initPlugin();
            var result = BkavPluginSigner.GetVersion();// kết quả trả về: true or false
            if (CheckVersion(result)) {
                CallFunctionCallback();
                //if (certificateSerial == "") CallFunctionCallback();
                //else ValidateCertificate(certificateSerial, ValidCertCallback);
                return;
            }
            else
                ShowMessageCheckCert(CODE_ERROR.CHECK_PLUGIN_SETUP_PLUGIN_OLD);
        }
    }
    catch (e) {
        bkav_alert_error(e)
    }
}

function ListenerGetVersion() {
    document.addEventListener(EXTENSION_EVENT_NAME.GET_VERSION, function (data) {
        if (!isCheckedVersionPlugin) {
            isCheckedVersionPlugin = true;

            var result = document.getElementById('hrSignedData').value;

            if (CheckVersion(result)) {
                isSetUpExtensionAndPlugin = true;

                CallFunctionCallback();
                //if(certificateSerial == "" ) CallFunctionCallback();
                //else ValidateCertificate(certificateSerial, ValidCertCallback);                 
            }
            else {
                ShowMessageCheckCert(CODE_ERROR.CHECK_PLUGIN_SETUP_PLUGIN_OLD);
                $.isLoading("hide");
            }
        }
    });
}

function CheckVersion(version) {
    var arrVersion = version.split(".");
    if (arrVersion.length != 3)
        return false;
    else
        if (arrVersion[0] == 1 && arrVersion[1] == 0 && arrVersion[2] <= 14)
            return false;

    return true;
}

//--------Validate Certificate---------------//

function ValidateCertificate(serial, jsCallback) {
    SetLicenseKey();

    var objectCert = new ObjCertificate()
    objectCert.CertificateSerialNumber = serial;
    objectCert.FunctionCallback = jsCallback;
    var timeCheck = getDateTime();
    objectCert.TimeCheck = timeCheck;
    objectCert.OcspUrl = "";
    try {
        if (checkBrowser() == 1 || checkBrowser() == 3) {//Chrome
            BkavCAPlugin.ValidateCertificate(objectCert);
        }
        else {
            BkavPluginSigner.ValidateCertificate(objectCert);
        }
    } catch (e) {
        alert(e);
    }
}

function getDateTime() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    if (month.toString().length == 1) {
        var month = '0' + month;
    }
    if (day.toString().length == 1) {
        var day = '0' + day;
    }
    if (hour.toString().length == 1) {
        var hour = '0' + hour;
    }
    if (minute.toString().length == 1) {
        var minute = '0' + minute;
    }
    if (second.toString().length == 1) {
        var second = '0' + second;
    }
    var dateTime = year + '/' + month + '/' + day + ' ' + hour + ':' + minute + ':' + second;
    return dateTime;
}

function ValidCertCallback(data) {
    var iRet = parseInt(data);
    if (CheckValidateResult(iRet)) CallFunctionCallback();
    else $.isLoading("hide");
}

function CallFunctionCallback() {
    if (functionCallBackAfterCheckSuccessEnvironmentSign !== undefined && typeof functionCallBackAfterCheckSuccessEnvironmentSign === "function") functionCallBackAfterCheckSuccessEnvironmentSign();
    else $.isLoading("hide");
}


function CheckPluginValidx(jsCallback) {
    try {
        BkavCAPlugin.Connect(jsCallback); // Bắt event EXTENSION_EVENT_NAME.EXTENSION_VALID để lấy kết quả trả về  
    }
    catch (e) {
        alert(e)
    }
}
