import request from "@/utils/request";
import axios from "axios";
export function index(params) {
  return request({
    url: "/benh-an-theo-gioi-truyen-dich",
    method: "get",
    params,
  });
}
export function store(data) {
  return request({
    url: `/benh-an-theo-gioi-truyen-dich`,
    method: "post",
    data
  });
}
export function detail(idba, stt) {
  return request({
    url: `/benh-an-theo-gioi-truyen-dich/${idba}/chi-tiet/${stt}`,
    method: "get",
  });
}
export function update(idba, stt, data) {
  return request({
    url: `/benh-an-theo-gioi-truyen-dich/${idba}/chi-tiet/${stt}`,
    method: "put",
    data,
  });
}
export function destroy(idba, stt) {
  return request({
    url: `/benh-an-theo-gioi-truyen-dich/${idba}/chi-tiet/${stt}`,
    method: "delete",
  });
}
export function print(idba) {
  window.open(
    `${window.origin}/api/benh-an-theo-gioi-truyen-dich/${idba}/print-ba-file/PhieuTruyenDich_idba${idba}.pdf`,
    "_blank"
  );
  return;
}

export async function printDigitalSig(idba, stt, sttkhoa) {
  const  paramsd= `${window.origin}/api/benh-an-theo-gioi-truyen-dich/${idba}/${stt}/${sttkhoa}///print-ba-file/${stt}/PhieuTruyenDich.pdf`;
  return axios.get(`/DMTrangThaiKy/ShowFileDaKY?base64URL=${paramsd}`);
 
}