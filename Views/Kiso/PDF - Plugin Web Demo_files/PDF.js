// SignPDF
var dllName = "BkavCA,BkavCAv2S";
var licenseKey = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+PExpY2Vuc2UgaWQ9IkwwMDEiPjxUZW5QaGFuTWVtPkJrYXZDQUxpY2Vuc2VUZXN0PC9UZW5QaGFuTWVtPjxOZ3VvaUNhcD5Ca2F2IENvcnBvcmF0aW9uPC9OZ3VvaUNhcD48RG9uVmlEdW9jQ2FwPkJrYXZDQUxpY2Vuc2VUZXN0PC9Eb25WaUR1b2NDYXA+PEhhblN1RHVuZz48TmdheUNhcD4yMC0wOS0yMDE4PC9OZ2F5Q2FwPjxOZ2F5SGV0SGFuPjIwLTEyLTIwMTg8L05nYXlIZXRIYW4+PC9IYW5TdUR1bmc+PFF1eWVuU3VEdW5nPjxVbmdEdW5nPio8L1VuZ0R1bmc+PE1vZHVsZVhNTD4xPC9Nb2R1bGVYTUw+PE1vZHVsZVBERj4xPC9Nb2R1bGVQREY+PE1vZHVsZUNNUz4xPC9Nb2R1bGVDTVM+PE1vZHVsZU9PWE1MPjE8L01vZHVsZU9PWE1MPjxNb2R1bGVDZXJ0aWZpY2F0ZVV0aWxzPjE8L01vZHVsZUNlcnRpZmljYXRlVXRpbHM+PE1vZHVsZUltcG9ydENlcnQ+MTwvTW9kdWxlSW1wb3J0Q2VydD48L1F1eWVuU3VEdW5nPjxTaWduYXR1cmUgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxTaWduZWRJbmZvPjxDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMjAwMS9SRUMteG1sLWMxNG4tMjAwMTAzMTUjV2l0aENvbW1lbnRzIi8+PFNpZ25hdHVyZU1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNyc2Etc2hhMSIvPjxSZWZlcmVuY2UgVVJJPSIiPjxUcmFuc2Zvcm1zPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSIvPjwvVHJhbnNmb3Jtcz48RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI3NoYTEiLz48RGlnZXN0VmFsdWU+amxNM05HK2pyeDNnbFpVdUJ1cUIzZGVnYTlrPTwvRGlnZXN0VmFsdWU+PC9SZWZlcmVuY2U+PC9TaWduZWRJbmZvPjxTaWduYXR1cmVWYWx1ZT54VnQ3QnpTUzdMK1I2SUZFUms4MFlxZHRwT3cyeFdmMDRCeGN4OHY0dFFNTUFUQXMrazJHYUtPeW1QRUFNaU5uVnhCbTRVYzlhQjNGDQpmbGt4ZmFsTDN1WitSYUNwa2tZVkdVM0JVV2FrMTNuWVlXYWZ3Ymx2dHVjOTVVNHJRR25VbWg1RnBnTTFjc01CVm9rQzJtanhLVVZTDQpCcE5ROS9mYzd2ZHBVMHRmNUVBSXpqd1d5VW83LytnbnlxQjlLNnVpUmJYVHVWTUtWb0g2R2gwbWNaSmdJY0taYXdGZ25CbG1jN0VqDQpkNTRjajVVSkQ1RDBxZm1qc1grd1R5OU85Nms5WFBUNXo4STNrdEVTQlcweEF0aUtxMStIb00yZDRxU3NaZEFZSVRNcXMwVXFmSER1DQpuTEUyT0VsTXZMNjU2WWc4SmFMSnM4OUNUblBSMjB1blN3MGlnQT09PC9TaWduYXR1cmVWYWx1ZT48S2V5SW5mbz48WDUwOURhdGE+PFg1MDlTdWJqZWN0TmFtZT5DPVZOLFNUPUjDoCBO4buZaSxMPUPhuqd1IEdp4bqleSxPPUPDtG5nIHR5IEPhu5UgcGjhuqduIEJrYXYsT1U9QmFuIEFOTSxDTj1Ca2F2Q0EgTGljZW5zZSxVSUQ9TVNUOjAxMDEzNjA2OTctQmthdkNBTGljZW5zZTwvWDUwOVN1YmplY3ROYW1lPjxYNTA5Q2VydGlmaWNhdGU+TUlJRTFqQ0NBNzZnQXdJQkFnSVFWQU5nZldkZ1gwQVZHSHVZSk01RXJqQU5CZ2txaGtpRzl3MEJBUVVGQURCSk1Rc3dDUVlEVlFRRw0KRXdKV1RqRU9NQXdHQTFVRUJ4TUZTR0Z1YjJreEdUQVhCZ05WQkFvVEVFSnJZWFlnUTI5eWNHOXlZWFJwYjI0eER6QU5CZ05WQkFNVA0KQmtKcllYWkRRVEFlRncweE9EQTRNRGt3T0RRd01qSmFGdzB5TURBMU1UVXdNek0xTkRoYU1JRzFNU3d3S2dZS0NaSW1pWlB5TEdRQg0KQVF3Y1RWTlVPakF4TURFek5qQTJPVGN0UW10aGRrTkJUR2xqWlc1elpURVhNQlVHQTFVRUF3d09RbXRoZGtOQklFeHBZMlZ1YzJVeA0KRURBT0JnTlZCQXNNQjBKaGJpQkJUazB4SWpBZ0JnTlZCQW9NR1VQRHRHNW5JSFI1SUVQaHU1VWdjR2podXFkdUlFSnJZWFl4RlRBVA0KQmdOVkJBY01ERVBodXFkMUlFZHA0YnFsZVRFU01CQUdBMVVFQ0F3SlNNT2dJRTdodTVscE1Rc3dDUVlEVlFRR0V3SldUakNDQVNJdw0KRFFZSktvWklodmNOQVFFQkJRQURnZ0VQQURDQ0FRb0NnZ0VCQVBMZFEyY1RnWTlSYnkzTzVHRVl1N2NoMXlSV0FTQldkY2RxWGphWg0KTGJMYnFFV1lMMFQrQXZBNHFBa3FDQWR3czkwbmVrR1Y2TVo3c1R1KzBLK3IxQS9sUnNBMDhEbUY3My9CQzVtQ0dPZW94bmJDYUFaKw0KSENreTQ0bElReUFoME9NdVFJZUhPT2VvMFJIWDhiVlV6M2VCdGRiaGZmU1QvM2FiMndPZGlaRWFaeklORmhQSmRqSE10Ym1qWFJQLw0KNUc1emQxRnpFRmJFckw1RVB6N1NXNHpLSzJid3pEcko4OVBrMEY2bm1TTE5OZE0zSWxkRTZocGZRWDd4eXBXVlpNeXIzQ3V0RkpLTw0KOTRxcFZSWjJhdW01K0NHZUtXOEh6OTB0N1RPRzVRMThJUHVKdWo5eFRkRlB4Ymhaa00zY09rYW5lTTBxREhsdGhLN1o1aEgzeCtrQw0KQXdFQUFhT0NBVXN3Z2dGSE1ERUdDQ3NHQVFVRkJ3RUJCQ1V3SXpBaEJnZ3JCZ0VGQlFjd0FZWVZhSFIwY0RvdkwyOWpjM0F1WW10aA0KZG1OaExuWnVNQjBHQTFVZERnUVdCQlRtR2VVa0dZeU5iQWNiUEV2Ti9MSmYrYlJ4bGpBTUJnTlZIUk1CQWY4RUFqQUFNQjhHQTFVZA0KSXdRWU1CYUFGQjZ3RDBpWDM5RERaNmRHaER0WU80Z05VNVNHTUg4R0ExVWRId1I0TUhZd2RLQWpvQ0dHSDJoMGRIQTZMeTlqY213dQ0KWW10aGRtTmhMblp1TDBKcllYWkRRUzVqY215aVRhUkxNRWt4RHpBTkJnTlZCQU1NQmtKcllYWkRRVEVaTUJjR0ExVUVDZ3dRUW10aA0KZGlCRGIzSndiM0poZEdsdmJqRU9NQXdHQTFVRUJ3d0ZTR0Z1YjJreEN6QUpCZ05WQkFZVEFsWk9NQTRHQTFVZER3RUIvd1FFQXdJRQ0KOERBekJnTlZIU1VFTERBcUJnZ3JCZ0VGQlFjREFRWUlLd1lCQlFVSEF3SUdDQ3NHQVFVRkJ3TUVCZ29yQmdFRUFZSTNDZ01NTUEwRw0KQ1NxR1NJYjNEUUVCQlFVQUE0SUJBUUNTQk53TlVuU2kyejFLSytQNUN1ZGdTaWJLSXpJb3ZsZjAyYTFISCtTMWZxVFZnczU0cFdHdw0KRUZ3QVhwam9hQXVOZDdoakdXTHJFRkRxSEpnNmRjU3Bhc1ZpQ3VhRjE2K2RJblNoYXpWNkNWTng0WjF2bHl1SG8wRFByRXpEb1VHcw0KR2dGb3N0dDRyM2Q1Z25Ybkloa0hncmQ3bG1sTzZxMmFaekkzNE1HNmh5WTVXeHpuZ2ZvTTYwbk83Z1Z4cC90Zm9lRTBoRVE0N0FuUA0KcTQwb3dJNEF5VS91cjREM2tqQ2xLbVhyMnB1dUs5OGVMVU1mYVVDZTEyazJaN3dETlp3OHhWQ21YUWdPcmhLeTBNZFdUWEs5RXBTNw0KOFRCZFJjZXI3Nm5Wb0QxZzBuaXNnZi9BZGx6NVZqeGZVSXZnMVpkc3ZBSjV3Z3JkaFNMVTNmWHZGVzRMPC9YNTA5Q2VydGlmaWNhdGU+PFg1MDlDZXJ0aWZpY2F0ZT5NSUlFTnpDQ0F4K2dBd0lCQWdJS1lRcnhWQUFBQUFBQUR6QU5CZ2txaGtpRzl3MEJBUVVGQURCK01Rc3dDUVlEVlFRR0V3SldUakV6DQpNREVHQTFVRUNoTXFUV2x1YVhOMGNua2diMllnU1c1bWIzSnRZWFJwYjI0Z1lXNWtJRU52YlcxMWJtbGpZWFJwYjI1ek1Sc3dHUVlEDQpWUVFMRXhKT1lYUnBiMjVoYkNCRFFTQkRaVzUwWlhJeEhUQWJCZ05WQkFNVEZFMUpReUJPWVhScGIyNWhiQ0JTYjI5MElFTkJNQjRYDQpEVEUxTURVeE5UQXpNalUwT0ZvWERUSXdNRFV4TlRBek16VTBPRm93U1RFTE1Ba0dBMVVFQmhNQ1ZrNHhEakFNQmdOVkJBY1RCVWhoDQpibTlwTVJrd0Z3WURWUVFLRXhCQ2EyRjJJRU52Y25CdmNtRjBhVzl1TVE4d0RRWURWUVFERXdaQ2EyRjJRMEV3Z2dFaU1BMEdDU3FHDQpTSWIzRFFFQkFRVUFBNElCRHdBd2dnRUtBb0lCQVFERGpZeTJCem81cjMzdmx3WVRwN3F4V3g0ZHBmcGl6YWY2ZVE2eHpFRFBlUlFODQpqbW1XNi9SRmczZDF0djhrY1NXV3g2S2h1bUlMenpaZHZmRVJYTWtRcFRHdWVxcTM1ekc3ZDlHVWxrbUlWRHljUTRWd3ZveHE5TVRXDQpObnJRc1luL0FScWl4MXVFMFpPc1lueWMzY2NTaTByS1prZ09yeUJHWFVka211ekhPMVhNazhJR04yQUxoZzBJcjBsWStEZENtNHRlDQplYXNiMHNZZGNiVUR3SEpQdGcxa0VKZTFUMm1YU3dZQ05IQnY3TGc3aW5DK0FSZnhvQzBBbGFIYVpVUHpISEJtV3R5SlIyV0h3dVlwDQpETUU0Um04Tkp1MG9mRzdCK05uWmdxMXMyYUdLWG00Y3g0RTk1eFBKbnZLM2U2d25qeGFBNS8zWENaYS9HV1FsQlJJMUFnTUJBQUdqDQpnZXN3Z2Vnd0VnWURWUjBUQVFIL0JBZ3dCZ0VCL3dJQkFEQUxCZ05WSFE4RUJBTUNBWVl3SFFZRFZSME9CQllFRkI2d0QwaVgzOUREDQpaNmRHaER0WU80Z05VNVNHTUI4R0ExVWRJd1FZTUJhQUZNMWljZVJodmY0OTdMSkFZTk9CZGQwNnJHdkdNRHdHQTFVZEh3UTFNRE13DQpNYUF2b0MyR0syaDBkSEE2THk5d2RXSnNhV011Y205dmRHTmhMbWR2ZGk1MmJpOWpjbXd2YldsamJuSmpZUzVqY213d1J3WUlLd1lCDQpCUVVIQVFFRU96QTVNRGNHQ0NzR0FRVUZCekFDaGl0b2RIUndPaTh2Y0hWaWJHbGpMbkp2YjNSallTNW5iM1l1ZG00dlkzSjBMMjFwDQpZMjV5WTJFdVkzSjBNQTBHQ1NxR1NJYjNEUUVCQlFVQUE0SUJBUUJVZVVtdXIra2I4cFpVb3ZpcFNickhUVE4xWGl6UUlYYWtsNG9YDQpaVkZPcHphVE5uU1FXTk5BbzZNY1VORjJOTDFxNHhHZUZjcWJud2MxZFlHamFqcngwU2ZhS28yRmtiaDY1NldieEdUMW1xRS91d2orDQp4cyt6OWRnY3JMK3pTU1QraEdrYXVjdGFBSktMWlRZQWJTSC80VjFlZGRDN2UwYlBJVm81aW5OWVJpdlB5MU1JdXc0TkZHaDlzOG1sDQpuc1A2YmlXMGh0OGVybk14NVlIblFmd2RNK0srYXJ6ZGlKREx6ck5QUmZvN2dTeTUwYzNrSmpmVUZZeGJTZGdUVVhDRERXN240eFk1DQpvdFBOOEJOQ0EyVERiYkhzbWJKWHYzUmE1QjJyZTczNGIwRlBtenoxQmFuWU9hbTJOQW83K3lINzVjSmhZaWVKUjZOc3dzaGRrU3pDPC9YNTA5Q2VydGlmaWNhdGU+PFg1MDlDZXJ0aWZpY2F0ZT5NSUlEMXpDQ0FyK2dBd0lCQWdJUUcrUnppaDgrd0k5SG42YlBOY1dZSWpBTkJna3Foa2lHOXcwQkFRVUZBREIrTVFzd0NRWURWUVFHDQpFd0pXVGpFek1ERUdBMVVFQ2hNcVRXbHVhWE4wY25rZ2IyWWdTVzVtYjNKdFlYUnBiMjRnWVc1a0lFTnZiVzExYm1sallYUnBiMjV6DQpNUnN3R1FZRFZRUUxFeEpPWVhScGIyNWhiQ0JEUVNCRFpXNTBaWEl4SFRBYkJnTlZCQU1URkUxSlF5Qk9ZWFJwYjI1aGJDQlNiMjkwDQpJRU5CTUI0WERUQTRNRFV4TmpBeE1USTBPVm9YRFRRd01EVXhOakF4TWpBek1sb3dmakVMTUFrR0ExVUVCaE1DVms0eE16QXhCZ05WDQpCQW9US2sxcGJtbHpkSEo1SUc5bUlFbHVabTl5YldGMGFXOXVJR0Z1WkNCRGIyMXRkVzVwWTJGMGFXOXVjekViTUJrR0ExVUVDeE1TDQpUbUYwYVc5dVlXd2dRMEVnUTJWdWRHVnlNUjB3R3dZRFZRUURFeFJOU1VNZ1RtRjBhVzl1WVd3Z1VtOXZkQ0JEUVRDQ0FTSXdEUVlKDQpLb1pJaHZjTkFRRUJCUUFEZ2dFUEFEQ0NBUW9DZ2dFQkFLRS9XVkVPL2pEL1lkdVdlQlNMMjBNOE5yNWhyOXkxUDJBZTB3MEJRYTM0DQp5WXBDanNqdE1vWkh4ZjYxOStyV1JEY1FFc05JQ0ZGUXV1Vlg2YzQxeVk0Y2N3bUZNMHpodXppc2pxMjNFd1F1Wm9GWExjejdHdjB1DQpuSXY5Q1VEd1lCZWJjVVZ0ZmVQYkt0SzdtdDNyekY3a0FOL1ZiRENGbTcxWGZ5M1VKTk9BKytBb1ViNncxbUVIek9XZ1IrZVJiUytIDQpXT2kwcmNHeFJyUGNXaDA0Q2RuN3RTZVlubDc4OGZSSS8raWhPLzlRTTlrbXE3S1pZcDNNZThoU1RaNWNRb3R2ZEg3OGxCUGVDdEx3DQp0V3I0bGt4UW5PWWhqc0hsbHdGT3paK3dRQmw4RzFsdlhEZ1ptamZhMFlFNUZqTHZnYTJ3SVdzUmw4TEJDTDF2STF3RUQ5TUNBd0VBDQpBYU5STUU4d0N3WURWUjBQQkFRREFnR0dNQThHQTFVZEV3RUIvd1FGTUFNQkFmOHdIUVlEVlIwT0JCWUVGTTFpY2VSaHZmNDk3TEpBDQpZTk9CZGQwNnJHdkdNQkFHQ1NzR0FRUUJnamNWQVFRREFnRUFNQTBHQ1NxR1NJYjNEUUVCQlFVQUE0SUJBUUJNbmMxK0l5Q0FIQ2pQDQo4UEhKM3hIS3NtbFRvL0pmRExObG5DOVU0UnhRS3VCVkY4UVh2cWlUVVVhcWh1MGtaQzlQRTQ2d3RCU2NmRU8rTFU1alVtemIxbkFYDQpXVWRib2xxeng1WjZ0ZzMxTFEzWlpEcXYwRlE2MFJOb3R2bzREZ1hyNFB3dzkweWJYK0x1WjN2NFl1cDByM0pVVE5UNlhvdnM2N2duDQpnU3lZanZmS29GR1djOFlYaWZuMFU1Yy9WOFBiVlNoSmMwOUtOeXBuaE1VVHZzYko3Z2xIWXIrb3N1cDg1VjhrMnp1NGREV3c0WVdQDQppcGRJanVkNFo0bkw1YVFDN0Z0WG9ibkhscmZCNmVWZGpwbW1weVdhSGJETzFqdHJNL0srU2VFdDFvZUJ1WGF1cC96TnM4WjJNcTlODQpVRkpzTFEyeXZkZFE1ZE4xWTU5ZHpRcVo8L1g1MDlDZXJ0aWZpY2F0ZT48L1g1MDlEYXRhPjwvS2V5SW5mbz48L1NpZ25hdHVyZT48L0xpY2Vuc2U+";

document.addEventListener(EXTENSION_EVENT_NAME.SIGN_PDF_FILE, function (data) {
    var result = document.getElementById('hrSignedData').value;
    document.getElementById('bkavca-out-stream').value = result;
    alert("No Callback");
    var iRet = parseInt(result);
    switch (iRet) {
        case 0:
            alert("Không có quyền sử dụng chức năng này");
            break;
        case 1:
            alert("Ký thành công");
            break;
        case 2:
            alert("Không thể ký dữ liệu do dữ liệu đã bị mã hóa");
            break;
        case 3:
            alert("Không tìm thấy chứng thư số");
            break;
        case 4:
            alert("Dữ liệu đầu vào không đúng định dạng");
            break;
        case 5:
            alert("Có lỗi trong quá trình ký");
            break;
        case 6:
            alert("Có lỗi trong quá trình lưu chữ ký");
            break;
        case 13:
            alert("Người dùng hủy bỏ");
            break;
        default:
            alert("Lỗi không xác định");
            break;
    }
});

//function SignPDFSignPDF(pdfFileIn, pdfFileOut, serial,nameSigner,urlIcon, offsetX, offsetY, jsCallback) 
//{
//    try {
//        var iCheck = checkBrowser();
//        var objPDF = new ObjPdfSigner();
//        if (pdfFileIn == "" || undefined == pdfFileIn) {
//            alert("Bạn chưa chọn file");
//            return;
//        }
//        objPDF.PathFileInput  = pdfFileIn;
//        objPDF.PathFileOutput = pdfFileOut;
//        objPDF.Signer = nameSigner
//      //  objPDF.SigningTime = "2020/10/25 10:00:00"
//        objPDF.CertificateSerialNumber = serial;
//        //objPDF.SigningType = PDF_SIGNING_TYPE.SIGN_PDF_BASE64;
//      //objPDF.Signer ="Phongnq";
//        // objPDF.urlIcon = "C:\Program Files (x86)\Bkav Corporation\BkavCA Signer Plugin 2.0\stamppdf.jpg";
//		//console.log("offX:" + );
//		//console.log("offY: " +offsetX);
		
//        objPDF.offsetX = "0";
//        objPDF.offsetY = "0";
//        objPDF.SigningType = PDF_SIGNING_TYPE.SIGN_PDF_FILE;
//		objPDF.pagePositionOfDS = "1";
//        objPDF.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 
//        BkavCAPlugin.SetLicenseKey(licenseKey);
//        BkavCAPlugin.SetDLLName(dllName);
//        BkavCAPlugin.SetHashAlgorithm(HASH_ALGORITHM.SHA1);
            
//        BkavCAPlugin.SignPDF(objPDF); // doi voi Extension 

//    } catch (e) {
//        alert(e);
//    }
//} 



function SignPDF(pdfFileIn, pdfFileOut, serial, nameSigner, urlIcon, offsetX, offsetY, pagePositionOfDS, jsCallback) {
    try {
        var iCheck = checkBrowser();
        var objPDF = new ObjPdfSigner();
        if (pdfFileIn == "" || undefined == pdfFileIn) {
            alert("Bạn chưa chọn file");
            return;
        }

        objPDF.Base64String = pdfFileIn;
        objPDF.CertificateSerialNumber = serial;
        objPDF.SigningType = PDF_SIGNING_TYPE.SIGN_PDF_BASE64;
        objPDF.Signer = nameSigner;
        objPDF.offsetX = offsetX;
        objPDF.offsetY = offsetY;
        objPDF.pagePositionOfDS = "none";// specify the page on which the signature widget will be shown
        objPDF.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 


        BkavCAPlugin.SetLicenseKey(licenseKey);
        //BkavCAPlugin.SetAESKey("TEST");
        BkavCAPlugin.SetUsePKCS11(SET_USE_PKCS11.NO);
        BkavCAPlugin.SetDLLName(dllName);
        BkavCAPlugin.SetHashAlgorithm(HASH_ALGORITHM.SHA256);

        BkavCAPlugin.SignPDF(objPDF); //

    } catch (e) {
        alert(e);
    }
}

// Verify
document.addEventListener(EXTENSION_EVENT_NAME.VERIFY_PDF, function (data) {
    var result = document.getElementById('hrSignedData').value;
    alert("No Callback");
    var iRet = parseInt(result);
    CheckVerifyResult(iRet);
});
function VerifyPDF(pdfIN, jsCallback) {
    var timeCheck = "2015/09/17 14:11:11";
    timeCheck = getDateTime();
    var iRet;
    if (pdfIN == "" || undefined == pdfIN) {
        alert("Bạn chưa chọn file");
        return;
    }
    try {
        var iCheck = checkBrowser();
        var objVerifyPDF = new ObjVerifier();
        objVerifyPDF.PathFileInput = pdfIN;
        objVerifyPDF.TimeCheck = timeCheck;
        objVerifyPDF.VerifyType = VERIFY_TYPE.VERYFY_FILE;
        objVerifyPDF.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 

        BkavCAPlugin.VerifyPDF(objVerifyPDF);
       
    } catch (e) {
        alert(e);
    }
}