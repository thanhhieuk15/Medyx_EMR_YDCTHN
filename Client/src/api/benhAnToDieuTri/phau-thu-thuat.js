import request from "@/utils/request";
import axios from "axios";
export function index( params) {
  return request({
    url: `/benh-an-phau-thuat`,
    method: "get",
    params: {
      ...params,
    },
  });
}
export function store(data) {
  return request({
    url: `/benh-an-phau-thuat`,
    method: "post",
    data,
  });
}
export function update(idba, stt, data) {
  return request({
    url: `/benh-an-phau-thuat/${idba}/chi-tiet/${stt}`,
    method: "put",
    data,
  });
}
export function destroy(idba,stt) {
  return request({
    url: `/benh-an-phau-thuat/${idba}/chi-tiet/${stt}`,
    method: "delete",
  });
}
export function dsPhauThuThats( params) {
  return request({
    url: `/dm-phau-thuat`,
    method: "get",
    params: {
      ...params,
    },
  });
}
export async function printDigitalSig(idba, stt) {
  const  paramsd= `${window.origin}/benh-an-cls/${idba}/print-ba-file/${stt}/${idba}.pdf`;
  return axios.get(`/DMTrangThaiKy/ShowPdfhis?base64URL=${paramsd}`);

}