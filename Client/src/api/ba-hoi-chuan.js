import request from "@/utils/request";
import axios from "axios";
export function index(idba, params) {
  return request({
    url: "/benh-an-hoi-chuan",
    method: "get",
    params: {
      idba,
      ...params,
    },
  });
}
export function detail(id) {
  return request({
    url: `/benh-an-hoi-chuan/${id}`,
    method: "get",
  });
}
export function update(idba,stt,data) {
  return request({
    url: `benh-an-hoi-chuan/${idba}/chi-tiet/${stt}`,
    method: "put",
    data
  });
}
export function destroy(id, stt) {
  return request({
    url: `/benh-an-hoi-chuan/${id}/chi-tiet/${stt}`,
    method: "delete",
  });
}
export function create(idba, data) {
  return request({
    url: `/benh-an-hoi-chuan/${idba}`,
    method: "post",
    data,
  });
}

export async function print(idba, stt) {
  window.open(
    `${window.origin}/api/benh-an-hoi-chuan/${idba}/print-ba-file/${stt}/Phieuxetnghiem_stt_${stt}.pdf`,
    "_blank"
  );
  return;
}
export function printDigitalSig(idba, stt,sttKhoa) {
  const  paramsd= `${window.origin}/api/benh-an-hoi-chuan/${idba}/${stt}/${sttKhoa}///print-ba-file/${stt}/Phieuxetnghiem.pdf`;
  return axios.get(`/DMTrangThaiKy/ShowFileDaKY?base64URL=${paramsd}`);
}
// export async function printDigitalSig(idba, stt) {

//   const  paramsd= `${window.origin}/api/benh-an-hoi-chuan/${idba}/print-ba-file/${stt}/${idba}.pdf`;
//   return axios.get(`/DMTrangThaiKy/ShowPdfhis?base64URL=${paramsd}`);

// }