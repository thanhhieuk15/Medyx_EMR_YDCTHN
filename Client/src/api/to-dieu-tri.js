import request from "@/utils/request";
import axios from "axios";
export function index(idba, params) {
  return request({
    url: `/benh-an-to-dieu-tri`,
    method: "get",
    params: {
      idba: idba,
      ...params,
    },
  });
}
export function show(idba, stt) {
  return request({
    url: `/benh-an-to-dieu-tri/${idba}/chi-tiet/${stt}`,
    method: "get",
  });
}
export function update(idba, stt, data) {
  return request({
    url: `/benh-an-to-dieu-tri/${idba}/chi-tiet/${stt}`,
    method: "put",
    data,
  });
}
export function store(data) {
  return request({
    url: `/benh-an-to-dieu-tri`,
    method: "post",
    data,
  });
}
export function destroy(idba, stt) {
  return request({
    url: `/benh-an-to-dieu-tri/${idba}/chi-tiet/${stt}`,
    method: "delete",
  });
}
export async function exportTDT(idba, fileName, params) {
  window.open(
    `${window.origin}/api/benh-an-to-dieu-tri/${idba}/print-ba-file/ToDieuTri.pdf?${params}`,
    "_blank"
  );
  return;
}

export async function KyTDT(idba, fileName, params) {
  window.open(
    `${window.origin}/api/benh-an-to-dieu-tri/KyHSBA`,
    "_blank"
  );
  return;
}

export function getNhanVien(params) {
  return request({
    url: "/dm-nhan-vien",
    method: "get",
    params,
  });
  
}
export function getNhanVienBSPT(params) {
  return request({
    url: "/dm-nhan-vien",
    method: "get",
    params,
  });
}
export function getBenhAnKhoaDieuTri(params) {
  return request({
    url: "/benh-an-khoa-dieu-tri",
    method: "get",
    params,
  });
}
export function saoToDieuTri(data) {
  return request({
    url: "/benh-an-to-dieu-tri/sao-chep",
    method: "post",
    data,
  });
}
export function printDigitalSig(idba, stt,sttKhoa,ngayYLenh) {
  const  paramsd= `${window.origin}/api/benh-an-to-dieu-tri/${idba}/${stt}/${sttKhoa}/${ngayYLenh}//print-ba-file/${stt}/ToDieuTri.pdf`;
  return axios.get(`/DMTrangThaiKy/ShowFileDaKY?base64URL=${paramsd}`);
}