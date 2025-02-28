import request from "@/utils/request";

export function index(params) {
  return request({
    url: `/benh-an-loai-tai-lieu`,
    method: "get",
    params: {
      ...params,
    },
  });
}
export function indexDV(params) {
  return request({
    url: `/benh-an-file-phi-cau-truc`,
    method: "get",
    params: {
      ...params,
    },
  });
}
export function dmDv(params) {
  return request({
    url: `/dm-dich-vu`,
    method: "get",
    params: {
      ...params,
    },
  });
}
export function upLoadFile(data) {
  return request({
    url: `/benh-an-file-phi-cau-truc`,
    method: "post",
    data
  });
}
export function dmDvLoaiTl(params) {
  return request({
    url: `/benh-an-loai-tai-lieu/dich-vu`,
    method: "get",
    params: {
      ...params,
    },
  });
}
export function detroyFile(idba, stt) {
  return request({
    url: `/benh-an-file-phi-cau-truc/${idba}/chi-tiet/${stt}`,
    method: "delete",
  });
}
export function exportHSBA(id) {
  window.open(
    `${window.origin}/api/benh-an/xuat-file/${id}`,
    "_blank"
  );
  return;
}
export function DownloadFile(id) {
  const host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '')
  return fetch(host + `/api/benh-an/DownloadFileZip/${id}`, {
    method: "get",
    headers: {
      'Accept': 'application/zip', // Adjust to receive JSON response
    }
  });
}
// export function PreviewFile(id) {
//   window.open(
//     `${window.origin}/api/benh-an/xuat-file-Da-Ky/${id}`,
//     "_blank"
//   );
//   return;
// }
// export function ShowFileDinhKem(params) {
//   return request({
//     url: `/BaoCao/showFileDinhKem`,
//     method: "get",
//     params: params,

//   });
// }
export function PreviewFile(id) {
  return request({
    url: `/benh-an/xuat-file-Da-Ky/` + id,
    method: "Get",
  });
}
// export function DownloadFile(id) {
//   return request({
//     url: `/benh-an/DownloadFileZip/${id}`,
//     method: "get",
//     headers: {
//       'Accept': 'application/zip',
//     },
//   });
// }