import request from "@/utils/request";

export function index(params) {
  return request({
    url: "/benh-an",
    method: "get",
    params,
  });
}
export function viewPdf(data) {
  return request({
    url: "/benh-an/print-test",
    method: "get",
    data,
  });
}

export function createBa(data) {
  return request({
    url: "/benh-an/them-moi-thong-tin-benh-an",
    method: "post",
    data,
  });
}

export function updateThongTinChung(id, data) {
  return request({
    url: `/benh-an/${id}/cap-nhap-thong-tin-benh-an/`,
    method: "post",
    data
  });
}
export function updateDongThongTinChung(id, data) {
  return request({
    url: `/benh-an/${id}/dong-thong-tin-benh-an/`,
    method: "post",
    data
  });
}
export function update(id, data) {
  return request({
    url: `/benh-an/${id}/cap-nhap-to-benh-an`,
    method: "post",
    data
  });
}
export function destroy(id) {
  return request({
    url: `/benh-an/${id}`,
    method: "delete",
  });
}

export function getLoaiBenhAn() {
  return request({
    url: "/loai-benh-an",
    method: "get",
  });
}
export function getKhoa() {
  return request({
    url: "/khoa",
    method: "get",
    cache: true,
  });
}
export function GetByAccount() {
  return request({
    url: "/khoa/GetByAccount",
    method: "get",
    //cache: true,
  });
}
export function getDetailBenhAn(id) {
  return request({
    url: "/benh-an/" + id,
    method: "get",
  });
}

export function chiTietToBenhAn(id) {
  return request({
    url: "/benh-an/" + id + "/detail",
    method: "get",
  });
}

export function getTienSuBenh(id, params) {
  return request({
    url: "/benh-an-tien-su-benh/" + id,
    method: "get",
    params
  });
}
export function getKhamYhhd(id) {
  return request({
    url: "/benh-an-kham-yhhd/" + id,
    method: "get",
  });
}

export function getKhamYhct(id) {
  return request({
    url: "/benh-an-kham-yhct/" + id,
    method: "get",
  });
}

export function getTongKetBenhAn(id) {
  return request({
    url: "/benh-an-tong-ket-benh-an/" + id,
    method: "get",
  });
}

export function getKhoaDieuTri(params) {
  return request({
    url: "/benh-an-khoa-dieu-tri/",
    method: "get",
    params
  });
}
export function getBenhAnTienSuSan(id) {
  return request({
    url: "/benh-an-tien-su-san/" + id,
    method: "get",
  });
}
export function getDeatilPhieuPhauThuat(id) {
  return request({
    url: "/benh-an-phau-thuat-phieu-pttt/" + id,
    method: "get",
  });
}
export function addPhieuPhauThuatPttt(data) {
  return request({
    url: `/benh-an-phau-thuat-phieu-pttt/PostPhauThuatPhieuPttt`,
    method: "post",
    data
  });
}
export function storeTienSuSan(data) {
  return request({
    url: `/benh-an-tien-su-san`,
    method: "post",
    data,
  });
}
export function updateTienSuSan(id, stt, data) {
  return request({
    url: `/benh-an-tien-su-san/${id}/cap-nhat-tien-su-san/${stt}`,
    method: "put",
    data,
  });
}
export function deleteTienSuSan(id, stt) {
  return request({
    url: `/benh-an-tien-su-san/${id}/xoa-tien-su-san/${stt}`,
    method: "delete",
  });
}
export function getBenhAnPhuSan(id) {
  return request({
    url: "/benh-an-phu-san/" + id,
    method: "get",
  });
}
export function getDetailBenhAnPttt(id) {
  return request({
    url: `/benh-an-phau-thuat-phieu-pttt/${id}/chi-tiet-maxId`,
    method: "get",
  });
}
export function getLoai(id) {
  return request({
    url: "/benh-an/" + id + "/loai-benh-an",
    method: "get",
  });
}

export function printBa(idba) {
  window.open(
    `${window.origin}/api/benh-an/${idba}/print-ba-file/${idba}.pdf`,
    "_blank"
  );
  return;
}

export function printBa1(idba) {
  window.open(
    `${window.origin}/api/benh-an/${idba}/print-ba-file1/${idba}_1.pdf`,
    "_blank"
  );
  return;
}
export function printViewBa1(id) {
  return request({
    url: `/benh-an/${id}/PrintViewKy1/${id}.pdf`,
    method: "get",
  });
}

export function printBa2(idba) {
  window.open(
    `${window.origin}/api/benh-an/${idba}/print-ba-file2/${idba}.pdf`,
    "_blank"
  );
  return;
}


export function printBa3(idba) {
  window.open(
    `${window.origin}/api/benh-an/${idba}/print-ba-file3/${idba}.pdf`,
    "_blank"
  );
  return;
}