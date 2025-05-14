import request from "@/utils/request";

export function getDetailPhuSan(id) {
  return request({
    url: "/benh-an-phu-san/" + id,
    method: "get",
  });
}

