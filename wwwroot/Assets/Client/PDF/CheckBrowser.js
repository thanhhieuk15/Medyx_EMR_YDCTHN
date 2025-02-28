function checkBrowser() {
    if (navigator.userAgent.indexOf("Chrome") != -1) {
        return 1;
    }
    else if (navigator.userAgent.indexOf("Opera") != -1) {
        return 1;
    }
    else if (navigator.userAgent.indexOf("Firefox") != -1) {
        return 1;
    }
    else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) //IF IE > 10
    {
        return 1;
    }
    else {
        return 1;
    }
    //return 1;
}