# -*- coding: utf-8 -*-
"""
Sinh phần NHÓM 6 (mục 26-30) của 02_DuLieuMau.sql: hóa đơn, chi tiết, thanh toán, giao hàng,
tích điểm tháng 9/2026. Phần này được ghi đè giữa 2 dòng đánh dấu
"-- <<< BẮT ĐẦU NHÓM 6" và "-- <<< KẾT THÚC NHÓM 6 >>>".

Nguồn dữ liệu là các mục 1-25 của chính 02_DuLieuMau.sql (giá, công thức, combo, khách hàng,
nhân viên, kế hoạch bán, phiếu nhập, tồn kho). Script đồng thời cập nhật lại trong file:
  - tiền hệ thống / tiền đếm của 270 ca làm (= tiền đầu ca + tiền mặt thu trong ca)
  - số lượng từng đợt nhập (tổng tháng không đổi) để tồn kho không âm ngày nào
Chạy: python gen_banhang.py   (cùng seed → cùng kết quả)
"""
import datetime as dt
import math
import random
import re
import sys
from collections import Counter, defaultdict
from pathlib import Path

sys.stdout.reconfigure(encoding="utf-8")
DB = Path(__file__).resolve().parent
SEED = DB / "02_DuLieuMau.sql"
SQL = SEED.read_text(encoding="utf-8")
MOC_DAU = "-- <<< BẮT ĐẦU NHÓM 6: BÁN HÀNG - sinh bởi gen_banhang.py, không sửa tay >>>"
MOC_CUOI = "-- <<< KẾT THÚC NHÓM 6 >>>"
rng = random.Random(2026)

CN = ["CN01", "CN02", "CN03"]
NGAY = [dt.date(2026, 9, d) for d in range(1, 31)]


def block(start, end):
    i = SQL.index(start)
    return SQL[i:SQL.index(end, i)]


# ---------------------------------------------------------------- đọc mục 1-25
GIA = {m: int(g) for m, g in re.findall(r"\(N'((?:BM|DU|CB)\d\d)',\s*(\d{5,6})\)",
                                         block("13. GIÁ BÁN", "14. NGUYÊN LIỆU"))}
RECIPE = defaultdict(list)
for sku, t, q in re.findall(r"\(N'((?:BM|DU)\d\d)', N'([^']+)', ([\d.]+)\)", block("15. CÔNG THỨC", "16. TỒN KHO")):
    RECIPE[sku].append((t, float(q)))
COMBO = defaultdict(dict)
for cb, sp, sl in re.findall(r"\(N'(CB\d\d)', N'((?:BM|DU)\d\d)', (\d)\)", block("17. COMBO", "18. KHUYẾN MÃI")):
    COMBO[cb][sp] = int(sl)

PLAN = {cn: {} for cn in CN}
cur = pre = None
for line in block("Kế hoạch bán tháng 9", "Hao hụt tháng 9").splitlines():
    m = re.match(r"\s*(CN0\d)?\s*(BM|DU)?\s+((?:\d\d:\d+\s*)+)$", line)
    if not m:
        continue
    cur = m.group(1) or cur
    pre = m.group(2) or pre
    for k, v in re.findall(r"(\d\d):(\d+)", m.group(3)):
        PLAN[cur][pre + k] = int(v)

KH = []
for ten, sdt, hang, diem, email, cn, active in re.findall(
        r"\(N'([^']+)',\s+'(09\d{8})',\s+N'(Vàng|Bạc|Đồng)',\s+(\d+),\s+(NULL|N'[^']*'),\s+N'(CN0\d)[^']*',\s+([01])\)",
        block("20. KHÁCH HÀNG", "IF (SELECT COUNT(*) FROM dbo.KhachHang)")):
    KH.append(dict(ten=ten, sdt=sdt, diem=int(diem), cn=cn, active=active == "1"))
TAG = {sdt: tag for sdt, tag in re.findall(r"'(09\d{8})'.*?N'CN0\d;?\s*([^']*)'", block("20. KHÁCH HÀNG", "IF (SELECT"))}

NV = {}
for ma, ten, sdt, chuc, cn in re.findall(r"\(N'(NV\d{3})', N'([^']+)',\s+'(\d+)', N'([^']+)',\s+N'(CN0\d)'\)",
                                         block("5. NHÂN VIÊN", "6. TÀI KHOẢN")):
    NV[ma] = dict(ten=ten, sdt=sdt, chuc=chuc, cn=cn)

NHAP_RE = re.compile(r"(\(N'([^']+)',\s+N'([^']+)',\s+([\d.]+), N'(CN0\d)',)(\s+(?:NULL|\d+),\s+(?:NULL|\d+),\s+(?:NULL|\d+),\s+(?:NULL|\d+))\)")
NHAP = {}
for m in NHAP_RE.finditer(block("22. CHI TIẾT PHIẾU NHẬP", "WHERE d.SoLuong > 0")):
    dots = [None if x.strip() == "NULL" else int(x) for x in m.group(6).split(",")]
    NHAP[(m.group(2), m.group(5))] = dict(ncc=m.group(3), dots=dots)
TON = {}
for t, a, b, c in re.findall(r"\(N'([^']+)',\s+(\d+),\s+(\d+),\s+(\d+)\)", block("16. TỒN KHO", "IF (SELECT COUNT(*) FROM dbo.TonKho")):
    TON[(t, "CN01")], TON[(t, "CN02")], TON[(t, "CN03")] = int(a), int(b), int(c)

assert len(GIA) == 46 and len(RECIPE) == 40 and len(COMBO) == 6, (len(GIA), len(RECIPE), len(COMBO))
assert all(len(PLAN[c]) == 40 for c in CN), [len(PLAN[c]) for c in CN]
assert len(KH) == 39 and len(NHAP) == 180 and len(TON) == 180, (len(KH), len(NHAP), len(TON))

# ---------------------------------------------------------------- quy tắc
TRUONG_CA = {"CN01": ["NV002", "NV003", "NV004"], "CN02": ["NV010", "NV011", "NV012"], "CN03": ["NV018", "NV019", "NV020"]}
NHAN_VIEN = {cn: [m for m, v in NV.items() if v["cn"] == cn and v["chuc"] == "Nhân viên"] for cn in CN}
CA = [(6, 11), (11, 16), (16, 21)]

HANG = [(300, 5), (100, 3), (0, 0)]            # (điểm tối thiểu, % giảm)
DOI_DIEM = {"CN01": "0933110001", "CN02": "0933120001", "CN03": "0933130001"}   # đổi 50 điểm = 50.000đ
SO_DIEM_DOI = 50

KM = [  # mã, loại, giá trị, giảm tối đa, đơn tối thiểu, chi nhánh, từ ngày, đến ngày, tỉ lệ khách dùng
    ("KM-KHAITRUONG", "PhanTram", 20, 30000, 50000, None, 1, 7, 1.0),
    ("KM-GIAM10K", "SoTien", 10000, None, 100000, None, 8, 30, 0.7),
    ("KM-THUDUC15", "PhanTram", 15, 25000, 80000, "CN02", 15, 30, 0.6),
    ("KM-TRUNGTHU", "PhanTram", 10, 20000, 60000, None, 20, 27, 0.6),
]
PHI_SHIP_NOIBO = 15000
CUA_KHAU = {  # quận giao hàng quanh chi nhánh
    "CN01": ["Quận 1, TP.HCM", "Quận 1, TP.HCM", "Quận 3, TP.HCM", "Quận 4, TP.HCM", "Quận Bình Thạnh, TP.HCM", "Quận Phú Nhuận, TP.HCM"],
    "CN02": ["TP. Thủ Đức, TP.HCM", "TP. Thủ Đức, TP.HCM", "TP. Thủ Đức, TP.HCM", "TP. Dĩ An, Bình Dương", "Quận Bình Thạnh, TP.HCM"],
    "CN03": ["Quận Gò Vấp, TP.HCM", "Quận Gò Vấp, TP.HCM", "Quận 12, TP.HCM", "Quận Bình Thạnh, TP.HCM", "Quận Tân Bình, TP.HCM", "Quận Phú Nhuận, TP.HCM"],
}
TAI_XE = ["Nguyễn Văn Tài", "Trần Minh Luân", "Lê Văn Hậu", "Phạm Quốc Toàn", "Huỳnh Văn Sang", "Võ Thanh Bình",
          "Đặng Văn Lực", "Bùi Minh Khang", "Đỗ Hữu Nghĩa", "Hồ Văn Tín", "Ngô Thành Công", "Dương Văn Phước",
          "Lý Minh Hoàng", "Trương Văn Hòa", "Phan Thanh Tùng", "Mai Văn Đức", "Châu Minh Trí", "Tạ Văn Kiên"]
TAI_XE_SDT = {ten: "09" + "".join(str(rng.randrange(10)) for _ in range(8)) for ten in TAI_XE}
GHI_CHU_BM = ["Không hành", "Ít cay", "Không rau", "Thêm ớt", "Không đồ chua", "Nướng giòn"]
GHI_CHU_DU = ["Ít đá", "Ít ngọt", "Không đá", "Nhiều đá"]

# ghép cặp combo khi lập đơn (bánh → nước, xác suất khách chọn cặp này)
CAP = {"BM01": ("DU01", 0.45), "BM03": ("DU03", 0.35), "BM12": ("DU02", 0.35), "BM18": ("DU04", 0.35), "BM22": ("DU05", 0.35)}
CHUYEN_COMBO = [("CB06", 0.85), ("CB01", 0.75), ("CB02", 0.7), ("CB03", 0.7), ("CB04", 0.7), ("CB05", 0.7)]

BUOC = {
    "Vỏ bánh mì": 1, "Vỏ bánh mì que": 1, "Trứng cá tầm": 10, "Xíu mại": 10, "Xúc xích Đức": 10,
    "Phô mai Mozzarella": 500, "Đường cát trắng": 1000, "Tương ớt": 1000, "Tương cà": 1000, "Sốt mayonnaise": 1000,
    "Wasabi": 250, "Sốt tiêu đen": 1000, "Sốt cà chua (xíu mại)": 1000, "Nước sốt bánh mì": 1000, "Mỡ hành": 500,
    "Trà đen": 500, "Trà lài": 500, "Đào ngâm": 820, "Siro đào": 750, "Đường phèn": 1000, "Thạch nha đam": 1000,
    "Đá viên": 10000, "Túi giấy kraft đựng bánh mì": 100, "Hộp giấy kraft (cao cấp)": 50, "Ly nhựa PET 500ml": 50,
    "Nắp ly cầu": 50, "Ống hút giấy": 100, "Khăn giấy ăn": 100,
}
for t in ("Bánh mì matcha đậu đỏ (nhập Nhật)", "Bánh mì socola hạnh nhân (nhập Nhật)", "Bánh mì kem dừa nướng (nhập Nhật)"):
    BUOC[t] = 5
for t in ("Coca-Cola (lon 320ml)", "Pepsi (lon 320ml)", "Sting dâu (chai 330ml)", "Red Bull (lon 250ml)",
          "Trà Ô Long Tea+ Plus (chai 450ml)", "Aquafina (chai 500ml)"):
    BUOC[t] = 24
NCC_NUA_THANG = {"Cá tầm Việt Sa Pa", "Tạp hóa sỉ Minh Châu", "NPP nước giải khát Tân Bình", "Bao bì Xanh"}
NGAY_NHAN_TUAN = [0, 8, 15, 22]      # 0 = 31/08
NGAY_NHAN_NUA = [0, 15]


def he_so_ngay(d):
    if d.day in (1, 2):
        return 1.3
    return 1.2 if d.weekday() >= 5 else 0.92


def chia_theo_ngay(n):
    w = [he_so_ngay(d) for d in NGAY]
    s = sum(w)
    exp = [n * x / s for x in w]
    out = [math.floor(e) for e in exp]
    du = n - sum(out)
    thu_tu = sorted(range(30), key=lambda i: (exp[i] - out[i]) + rng.random() * 0.3, reverse=True)
    for i in thu_tu[:du]:
        out[i] += 1
    return out


GIO = [(6, 8), (7, 14), (8, 10), (9, 5), (10, 5), (11, 10), (12, 10), (13, 5), (14, 3), (15, 4),
       (16, 5), (17, 8), (18, 8), (19, 4), (20, 1)]
MAU_DON = [((1, 0), 27), ((1, 1), 34), ((0, 1), 10), ((2, 2), 10), ((2, 1), 6), ((2, 0), 5),
           ((1, 2), 3), ((3, 2), 3), ((4, 4), 2)]


def chon(pairs):
    tong = sum(w for _, w in pairs)
    r = rng.random() * tong
    for v, w in pairs:
        r -= w
        if r <= 0:
            return v
    return pairs[-1][0]


# ---------------------------------------------------------------- lập hóa đơn
class HD:
    __slots__ = ("cn", "ngay", "gio", "ma", "lines", "nguon", "loai", "thu_ngan", "ship_nv", "ca", "km", "km_tien",
                 "kh", "giam_hang", "diem_tru", "tien_hang", "tien_giam", "phi_ship", "tong", "diem_cong",
                 "trang_thai", "tt", "thanh_toan", "giao", "ngay_tt")

    def __init__(self, **kw):
        for k in self.__slots__:
            setattr(self, k, kw.get(k))


def lap_don_ngay(cn, di, banh, nuoc):
    rng.shuffle(banh)
    rng.shuffle(nuoc)
    gio_hang = []
    while banh or nuoc:
        b, k = chon(MAU_DON)
        if not banh:
            b, k = 0, max(k, 1)
        if not nuoc:
            b, k = max(b, 1), 0
        lay_b = [banh.pop() for _ in range(min(b, len(banh)))]
        lay_n = []
        for sp in lay_b:
            if sp in CAP and len(lay_n) < k and rng.random() < CAP[sp][1] and CAP[sp][0] in nuoc:
                nuoc.remove(CAP[sp][0])
                lay_n.append(CAP[sp][0])
        while len(lay_n) < k and nuoc:
            lay_n.append(nuoc.pop())
        gio_hang.append(Counter(lay_b + lay_n))

    ds = []
    for gh in gio_hang:
        lines = []
        for cb, p in CHUYEN_COMBO:
            while all(gh[sp] >= sl for sp, sl in COMBO[cb].items()) and rng.random() < p:
                for sp, sl in COMBO[cb].items():
                    gh[sp] -= sl
                lines.append([cb, 1])
        for sp in sorted(gh):
            if gh[sp] > 0:
                lines.append([sp, gh[sp]])
        gop = {}
        for sp, sl in lines:
            gop[sp] = gop.get(sp, 0) + sl
        lines = [[sp, sl, GIA[sp], None] for sp, sl in gop.items()]
        for ln in lines:
            if rng.random() < 0.06:
                ln[3] = rng.choice(GHI_CHU_DU if ln[0].startswith("DU") else GHI_CHU_BM)
        so_mon = sum(sl for _, sl, _, _ in lines)
        co_chao = any(sp == "BM17" for sp, _, _, _ in lines)
        r = rng.random()
        p_app = 0.26 if so_mon >= 2 else 0.14
        if co_chao:
            nguon, loai = "POS", "TaiQuay"
        elif r < p_app * 0.6:
            nguon, loai = "Grab", "GiaoHang"
        elif r < p_app:
            nguon, loai = "ShopeeFood", "GiaoHang"
        elif r < p_app + 0.04:
            nguon, loai = "POS", "GiaoHang"
        elif r < p_app + 0.04 + 0.48:
            nguon, loai = "POS", "MangDi"
        else:
            nguon, loai = "POS", "TaiQuay"
        ds.append(HD(cn=cn, ngay=di, lines=lines, nguon=nguon, loai=loai, trang_thai="HoanThanh", tt="DaTT"))

    gios = []
    for _ in ds:
        h = chon(GIO)
        gios.append(dt.time(h, rng.randrange(60), rng.randrange(60)))
    gios.sort()
    rng.shuffle(ds)
    for hd, g in zip(ds, gios):
        hd.gio = g
    return ds


def xep_nhan_su(hd):
    ca = next(i for i, (a, b) in enumerate(CA) if a <= hd.gio.hour < b)
    hd.ca = ca
    di = hd.ngay
    nv = NHAN_VIEN[hd.cn]
    xoay = nv[di % 4:] + nv[:di % 4]
    dung_ca = [[xoay[0]], [xoay[1]], [xoay[2], xoay[3]]][ca]
    truong = TRUONG_CA[hd.cn][ca]
    if hd.nguon == "POS" and hd.loai == "GiaoHang":
        hd.thu_ngan = truong
        hd.ship_nv = rng.choice(dung_ca)
    else:
        hd.thu_ngan = truong if rng.random() < 0.5 else rng.choice(dung_ca)


def tien_hang(hd):
    return sum(sl * g for _, sl, g, _ in hd.lines)


def nen_khong_combo(hd):
    return sum(sl * g for sp, sl, g, _ in hd.lines if not sp.startswith("CB"))


def ap_khuyen_mai(hd):
    hd.km, hd.km_tien = None, 0
    if hd.nguon != "POS" or hd.trang_thai != "HoanThanh":
        return
    nen = nen_khong_combo(hd)
    ngay = hd.ngay + 1
    tot = (None, 0)
    for ma, loai, gt, toi_da, don_min, cn, tu, den, ty_le in KM:
        if cn not in (None, hd.cn) or not (tu <= ngay <= den) or nen < don_min or rng.random() >= ty_le:
            continue
        tien = gt if loai == "SoTien" else min(nen * gt // 100 // 1000 * 1000, toi_da)
        if tien > tot[1]:
            tot = (ma, tien)
    hd.km, hd.km_tien = tot


def pct_hang(diem):
    return next(p for m, p in HANG if diem >= m)


def phai_tra_truoc_diem(hd, pct):
    nen = nen_khong_combo(hd) - hd.km_tien
    giam = nen * pct // 100 // 1000 * 1000
    return tien_hang(hd) - hd.km_tien - giam, giam


def diem_cua(hd, pct, doi):
    con, _ = phai_tra_truoc_diem(hd, pct)
    return (con - doi * 1000) // 10000


# ---------------------------------------------------------------- sinh
HOA_DON = []
for cn in CN:
    theo_ngay = defaultdict(lambda: ([], []))
    for sp, n in PLAN[cn].items():
        for di, sl in enumerate(chia_theo_ngay(n)):
            (theo_ngay[di][0] if sp.startswith("BM") else theo_ngay[di][1]).extend([sp] * sl)
    for di in range(30):
        banh, nuoc = theo_ngay[di]
        HOA_DON.extend(lap_don_ngay(cn, di, list(banh), list(nuoc)))

# đơn hủy: khách đổi ý trước khi thanh toán, không trừ kho
for cn in CN:
    for di, sps in ((rng.randrange(3, 10), ["BM03", "DU02"]), (rng.randrange(12, 20), ["BM08"]), (rng.randrange(21, 29), ["BM12", "DU01"])):
        hd = HD(cn=cn, ngay=di, lines=[[sp, 1, GIA[sp], None] for sp in sps], nguon="POS", loai="TaiQuay",
                trang_thai="DaHuy", tt="ChuaTT", gio=dt.time(rng.randrange(7, 20), rng.randrange(60), rng.randrange(60)))
        HOA_DON.append(hd)

for hd in HOA_DON:
    xep_nhan_su(hd)
    ap_khuyen_mai(hd)
    hd.kh, hd.giam_hang, hd.diem_tru = None, 0, 0

HOA_DON.sort(key=lambda h: (h.cn, h.ngay, h.gio))
dem = Counter()
for hd in HOA_DON:
    dem[(hd.cn, hd.ngay)] += 1
    hd.ma = f"{hd.cn}-2609{hd.ngay + 1:02d}-{dem[(hd.cn, hd.ngay)]:03d}"


# ---------------------------------------------------------------- khách thân thiết
def uu_tien(kh, hd):
    tag = TAG.get(kh["sdt"], "")
    sps = {sp for sp, _, _, _ in hd.lines}
    if "combo" in tag:
        return 4.0 if "CB01" in sps else 0.5
    if "cá hồi" in tag:
        return 4.0 if sps & {"BM18", "BM19", "BM20", "BM21"} else 0.5
    if "gia đình" in tag:
        return 3.0 if sum(sl for _, sl, _, _ in hd.lines) >= 3 else 0.6
    return 1.0


da_dung = set()
for cn in CN:
    ung_vien = [h for h in HOA_DON if h.cn == cn and h.nguon == "POS" and h.trang_thai == "HoanThanh"]
    for kh in sorted([k for k in KH if k["cn"] == cn and k["diem"] > 0], key=lambda k: -k["diem"]):
        doi = SO_DIEM_DOI if DOI_DIEM[cn] == kh["sdt"] else 0
        E = kh["diem"] + doi
        den_ngay = 10 if not kh["active"] else 30
        pool = [h for h in ung_vien if id(h) not in da_dung and h.ngay < den_ngay]
        for lan in range(400):
            p = min(0.95, max(1, E / 5.5) * (1 + 0.05 * lan) / len(pool))
            tich, doi_roi, chon_ds = 0, False, []
            for h in pool:
                hien_tai = tich - (SO_DIEM_DOI if doi_roi else 0)
                pct = pct_hang(hien_tai)
                d_doi = 0
                if doi and not doi_roi and hien_tai >= 150 and pct_hang(hien_tai - doi) == pct \
                        and phai_tra_truoc_diem(h, pct)[0] >= 100000:
                    d_doi = doi
                pts = diem_cua(h, pct, d_doi)
                con = E - tich
                if pts < 1 or pts > con or (pts == con and doi and not (doi_roi or d_doi)):
                    continue
                if pts == con and con <= 8:
                    chon_ds.append((h, pct, d_doi))
                    tich += pts
                    break
                if rng.random() < p * uu_tien(kh, h):
                    chon_ds.append((h, pct, d_doi))
                    tich += pts
                    doi_roi = doi_roi or bool(d_doi)
                    if tich == E:
                        break
            if tich == E and (not doi or any(d for _, _, d in chon_ds)):
                break
        else:
            raise SystemExit(f"Không xếp được hóa đơn cho khách {kh['ten']}")
        for h, pct, d_doi in chon_ds:
            da_dung.add(id(h))
            h.kh = kh["sdt"]
            h.giam_hang = phai_tra_truoc_diem(h, pct)[1]
            h.diem_tru = d_doi

# ---------------------------------------------------------------- tiền, thanh toán, giao hàng
TICH_DIEM = []
diem_kh = Counter()
for hd in sorted(HOA_DON, key=lambda h: (h.ngay, h.gio)):
    hd.tien_hang = tien_hang(hd)
    hd.phi_ship = PHI_SHIP_NOIBO if hd.nguon == "POS" and hd.loai == "GiaoHang" else 0
    hd.tien_giam = hd.km_tien + hd.giam_hang + hd.diem_tru * 1000
    hd.tong = hd.tien_hang - hd.tien_giam + hd.phi_ship
    hd.diem_cong = (hd.tong - hd.phi_ship) // 10000 if hd.kh else 0
    t_tao = dt.datetime.combine(NGAY[hd.ngay], hd.gio)
    hd.giao = None
    if hd.loai == "GiaoHang":
        lay = t_tao + dt.timedelta(minutes=rng.randrange(8, 16))
        giao = lay + dt.timedelta(minutes=rng.randrange(10, 31))
        if hd.nguon == "POS":
            ten = NV[hd.ship_nv]["ten"]
            hd.giao = ("NoiBo", ten, NV[hd.ship_nv]["sdt"], rng.choice(CUA_KHAU[hd.cn]), lay, giao)
        else:
            ten = rng.choice(TAI_XE)
            hd.giao = ("Grab" if hd.nguon == "Grab" else "Shopee", ten, TAI_XE_SDT[ten], rng.choice(CUA_KHAU[hd.cn]), lay, giao)
    hd.thanh_toan = []
    hd.ngay_tt = None
    if hd.trang_thai != "HoanThanh":
        continue
    hd.ngay_tt = t_tao + dt.timedelta(seconds=rng.randrange(30, 150))
    mmdd = f"{hd.ngay + 1:02d}"

    def ma_ck():
        return f"FT2609{mmdd}{rng.randrange(10**6):06d}"

    if hd.nguon in ("Grab", "ShopeeFood"):
        pre = "GF-" if hd.nguon == "Grab" else "SPF-"
        hd.thanh_toan = [("Vi", hd.tong, f"{pre}2609{mmdd}{rng.randrange(10**6):06d}")]
    elif hd.loai == "GiaoHang":
        hd.ngay_tt = hd.giao[5]
        hd.thanh_toan = [("TienMat", hd.tong, None)] if rng.random() < 0.7 else [("ChuyenKhoan", hd.tong, ma_ck())]
    else:
        r = rng.random()
        if hd.tong >= 150000 and r < 0.06:
            tm = hd.tong // 2 // 10000 * 10000
            hd.thanh_toan = [("TienMat", tm, None), ("ChuyenKhoan", hd.tong - tm, ma_ck())]
        elif r < 0.48:
            hd.thanh_toan = [("TienMat", hd.tong, None)]
        elif r < 0.83 or (r < 0.93 and hd.tong < 100000):
            hd.thanh_toan = [("ChuyenKhoan", hd.tong, ma_ck())]
        elif r < 0.93:
            hd.thanh_toan = [("The", hd.tong, f"POS{rng.randrange(10**8):08d}")]
        else:
            hd.thanh_toan = [("Vi", hd.tong, f"MOMO{rng.randrange(10**10):010d}")]
    if hd.kh:
        if hd.diem_tru:
            diem_kh[hd.kh] -= hd.diem_tru
            TICH_DIEM.append((hd.ma, hd.kh, -hd.diem_tru, diem_kh[hd.kh], "Tieu"))
        if hd.diem_cong:
            diem_kh[hd.kh] += hd.diem_cong
            TICH_DIEM.append((hd.ma, hd.kh, hd.diem_cong, diem_kh[hd.kh], "Tich"))

for kh in KH:
    assert diem_kh[kh["sdt"]] == kh["diem"], (kh["ten"], diem_kh[kh["sdt"]], kh["diem"])

# ---------------------------------------------------------------- tiêu hao & chia lại đợt nhập
TIEU_HAO = defaultdict(lambda: [0.0] * 30)      # (NL, CN) -> theo ngày
for hd in HOA_DON:
    if hd.trang_thai != "HoanThanh":
        continue
    for sp, sl, _, _ in hd.lines:
        thanh_phan = COMBO[sp].items() if sp.startswith("CB") else [(sp, 1)]
        for tp, k in thanh_phan:
            for nl, q in RECIPE[tp]:
                TIEU_HAO[(nl, hd.cn)][hd.ngay] += q * k * sl

for cn in CN:
    for sp, n in PLAN[cn].items():
        ban = sum(sl * (COMBO[x].get(sp, 0) if x.startswith("CB") else (x == sp))
                  for h in HOA_DON if h.cn == cn and h.trang_thai == "HoanThanh" for x, sl, _, _ in h.lines)
        assert ban == n, (cn, sp, ban, n)

NHAP_MOI = {}
for (nl, cn), v in NHAP.items():
    tong = sum(d or 0 for d in v["dots"])
    ngay_th = TIEU_HAO[(nl, cn)]
    hao = tong - round(sum(ngay_th)) - TON[(nl, cn)]
    assert hao >= 0, (nl, cn, hao)
    tuan = v["ncc"] not in NCC_NUA_THANG
    moc = NGAY_NHAN_TUAN if tuan else NGAY_NHAN_NUA
    buoc = BUOC.get(nl, 100)
    luy_ke, da = [], 0
    for k, m in enumerate(moc):
        het = (moc[k + 1] - 1) if k + 1 < len(moc) else 30          # ngày cuối kỳ (1..30)
        can = sum(ngay_th[:het]) + hao * het / 30
        c = tong if k + 1 == len(moc) else min(tong, math.ceil(round(can, 6) / buoc) * buoc)
        c = max(c, da)
        luy_ke.append(c)
        da = c
    so = [b - a for a, b in zip([0] + luy_ke[:-1], luy_ke)]
    ton = 0.0
    for d in range(1, 31):
        ton += sum(s for s, m in zip(so, moc) if m == d or (m == 0 and d == 1))
        ton -= ngay_th[d - 1] + hao / 30
        assert ton >= -1e-6, (nl, cn, d, ton)
    if tuan:
        NHAP_MOI[(nl, cn)] = [x if x > 0 else None for x in so]
    else:
        NHAP_MOI[(nl, cn)] = [so[0] or None, None, so[1] or None, None]

# ---------------------------------------------------------------- sinh mục 26-30
def q(s):
    return "NULL" if s is None else "N'" + s.replace("'", "''") + "'"


def ts(t):
    return f"'{t:%Y-%m-%d %H:%M:%S}'"


def chunks(rows, n=1000):
    for i in range(0, len(rows), n):
        yield rows[i:i + n]


hd_ok = [h for h in HOA_DON if h.trang_thai == "HoanThanh"]
n_ct = sum(len(h.lines) for h in HOA_DON)
n_tt = sum(len(h.thanh_toan) for h in HOA_DON)
n_gh = sum(1 for h in HOA_DON if h.giao)

def so_vn(n):
    return f"{n:,}".replace(",", ".")


L = []
w = L.append
tong_dt = {cn: sum(h.tong - h.phi_ship for h in hd_ok if h.cn == cn) for cn in CN}
w("/*==============================================================================")
w("  NHÓM 6: BÁN HÀNG THÁNG 9/2026 (3 chi nhánh, 01/09 - 30/09)")
w("  HoaDon | ChiTiet_HoaDon | ThanhToan_HoaDon | NhatKy_GiaoHang | GiaoDich_TichDiem")
w("  Sinh bởi gen_banhang.py - sửa quy tắc trong script rồi chạy lại, không sửa tay")
w("  (script cũng cập nhật tiền ca làm mục 7 và số lượng đợt nhập mục 22).")
w("")
w("  Quy tắc:")
w("  - Số món bán (tính cả món trong combo) khớp đúng kế hoạch bán ghi ở mục 21,")
w("    nên tổng nhập - xuất bán - hao hụt = tồn kho 30/09")
w("  - Mã hóa đơn MaDonLocal: CN01-260901-001 (chi nhánh - ngày - số thứ tự)")
w("  - Nguồn đơn: POS (tại quầy, mang đi, quán tự giao phí 15.000đ), Grab, ShopeeFood (tài xế app giao,")
w("    phí ship khách trả cho app nên PhiShip = 0)")
w("  - TienGiam = khuyến mãi + giảm theo hạng thành viên + đổi điểm (1 điểm = 1.000đ)")
w("    + Khuyến mãi chỉ cho đơn POS, tính trên phần không phải combo, 1 đơn tối đa 1 mã")
w("    + Hạng xét theo điểm lúc mua: Bạc giảm 3%, Vàng giảm 5% (làm tròn xuống 1.000đ)")
w("  - Tích điểm: 10.000đ (không tính phí ship) = 1 điểm; tổng giao dịch điểm = KhachHang.DiemHienTai")
w("  - Tiền mặt thu trong ca = TienHeThong - TienDauCa của NhatKy_CaLam")
w("  - 9 đơn hủy (khách đổi ý): không thanh toán, không trừ kho, không tích điểm")
w("")
for cn in CN:
    so = sum(1 for h in hd_ok if h.cn == cn)
    w(f"  {cn}: {so} hóa đơn | doanh thu (sau giảm, không tính ship) {so_vn(tong_dt[cn])}đ | TB {so_vn(tong_dt[cn] // 30)}đ/ngày")
w("==============================================================================*/")
w("")
w("/*------------------------------------------------------------------------------")
w(f"  26. HÓA ĐƠN ({len(HOA_DON)} đơn)")
w("     Ca làm tra theo chi nhánh + giờ tạo; khách tra theo số điện thoại; khuyến mãi tra theo mã")
w("------------------------------------------------------------------------------*/")
for part in chunks(HOA_DON):
    w("INSERT INTO dbo.HoaDon (MaDonLocal, MaChiNhanh, MaThuNgan, MaKhachHang, MaCa, MaKhuyenMai, NguonDon, LoaiDon,")
    w("    TongTienHang, TienGiam, PhiShip, TongThanhToan, DiemCong, DiemTru, TrangThaiDon, TrangThaiThanhToan, NgayTao, NgayThanhToan)")
    w("SELECT v.MaDon, cn.MaChiNhanh, nv.MaNhanVien, kh.MaKhachHang, ca.MaCa, km.MaKhuyenMai, v.NguonDon, v.LoaiDon,")
    w("    v.TienHang, v.TienGiam, v.PhiShip, v.TongTT, v.DiemCong, v.DiemTru, v.TTDon, v.TTThanhToan,")
    w("    CAST(v.NgayTao AS DATETIME2(0)), CAST(v.NgayTT AS DATETIME2(0))")
    w("FROM (VALUES")
    rows = []
    for h in part:
        t = dt.datetime.combine(NGAY[h.ngay], h.gio)
        rows.append(f"    (N'{h.ma}', N'{h.cn}', N'{h.thu_ngan}', {q(h.kh) if h.kh else 'NULL'}, {q(h.km)}, "
                    f"N'{h.nguon}', N'{h.loai}', {h.tien_hang}, {h.tien_giam}, {h.phi_ship}, {h.tong}, "
                    f"{h.diem_cong}, {h.diem_tru}, N'{h.trang_thai}', N'{h.tt}', {ts(t)}, "
                    f"{ts(h.ngay_tt) if h.ngay_tt else 'NULL'})")
    w(",\n".join(rows))
    w(") AS v (MaDon, MaCN, MaNV, SDT, MaKM, NguonDon, LoaiDon, TienHang, TienGiam, PhiShip, TongTT,")
    w("        DiemCong, DiemTru, TTDon, TTThanhToan, NgayTao, NgayTT)")
    w("JOIN dbo.ChiNhanh    cn ON cn.MaCode  = v.MaCN")
    w("JOIN dbo.NhanVien    nv ON nv.MaCode  = v.MaNV")
    w("JOIN dbo.NhatKy_CaLam ca ON ca.MaChiNhanh = cn.MaChiNhanh")
    w("                       AND CAST(v.NgayTao AS DATETIME2(0)) >= ca.GioMo")
    w("                       AND CAST(v.NgayTao AS DATETIME2(0)) <  ca.GioDong")
    w("LEFT JOIN dbo.KhachHang kh ON kh.SoDienThoai = v.SDT")
    w("LEFT JOIN dbo.KhuyenMai km ON km.MaCode      = v.MaKM;")
    w("")
w(f"IF (SELECT COUNT(*) FROM dbo.HoaDon) <> {len(HOA_DON)}")
w("    THROW 50101, N'HoaDon thiếu dòng: sai mã chi nhánh / nhân viên hoặc giờ tạo ngoài ca', 1;")
w("")
w("/*------------------------------------------------------------------------------")
w(f"  27. CHI TIẾT HÓA ĐƠN ({n_ct} dòng) - tên và giá đóng băng lúc bán, combo là 1 dòng")
w("------------------------------------------------------------------------------*/")
ct_rows = [(h, ln) for h in HOA_DON for ln in h.lines]
for part in chunks(ct_rows):
    w("INSERT INTO dbo.ChiTiet_HoaDon (MaHoaDon, MaSanPham, TenSanPhamChup, SoLuong, DonGia, GiamDong, ThanhTien, GhiChu)")
    w("SELECT hd.MaHoaDon, sp.MaSanPham, sp.TenSanPham, v.SoLuong, v.DonGia, 0, v.SoLuong * v.DonGia, v.GhiChu")
    w("FROM (VALUES")
    w(",\n".join(f"    (N'{h.ma}', N'{sp}', {sl}, {g}, {q(gc)})" for h, (sp, sl, g, gc) in part))
    w(") AS v (MaDon, MaSKU, SoLuong, DonGia, GhiChu)")
    w("JOIN dbo.HoaDon  hd ON hd.MaDonLocal = v.MaDon")
    w("JOIN dbo.SanPham sp ON sp.MaSKU      = v.MaSKU;")
    w("")
w(f"IF (SELECT COUNT(*) FROM dbo.ChiTiet_HoaDon) <> {n_ct}")
w("    THROW 50102, N'ChiTiet_HoaDon thiếu dòng', 1;")
w("")
w("IF EXISTS (SELECT 1 FROM dbo.HoaDon hd")
w("           WHERE hd.TongTienHang <> (SELECT SUM(ct.ThanhTien) FROM dbo.ChiTiet_HoaDon ct WHERE ct.MaHoaDon = hd.MaHoaDon)")
w("              OR hd.TongThanhToan <> hd.TongTienHang - hd.TienGiam + hd.PhiShip)")
w("    THROW 50103, N'HoaDon: tổng tiền không khớp chi tiết', 1;")
w("")
w("/*------------------------------------------------------------------------------")
w(f"  28. THANH TOÁN ({n_tt} dòng) - thời gian = lúc thanh toán, người thu = thu ngân")
w("     Grab / ShopeeFood: ví app (mã GF- / SPF-); quán tự giao: thu khi giao")
w("------------------------------------------------------------------------------*/")
tt_rows = [(h, x) for h in HOA_DON for x in h.thanh_toan]
for part in chunks(tt_rows):
    w("INSERT INTO dbo.ThanhToan_HoaDon (MaHoaDon, HinhThuc, SoTien, ThoiGian, MaThamChieu, NguoiThu)")
    w("SELECT hd.MaHoaDon, v.HinhThuc, v.SoTien, hd.NgayThanhToan, v.MaThamChieu, hd.MaThuNgan")
    w("FROM (VALUES")
    w(",\n".join(f"    (N'{h.ma}', N'{ht}', {st}, {q(mtc)})" for h, (ht, st, mtc) in part))
    w(") AS v (MaDon, HinhThuc, SoTien, MaThamChieu)")
    w("JOIN dbo.HoaDon hd ON hd.MaDonLocal = v.MaDon;")
    w("")
w(f"IF (SELECT COUNT(*) FROM dbo.ThanhToan_HoaDon) <> {n_tt}")
w("    THROW 50104, N'ThanhToan_HoaDon thiếu dòng', 1;")
w("")
w("IF EXISTS (SELECT 1 FROM dbo.HoaDon hd")
w("           WHERE hd.TrangThaiThanhToan = N'DaTT'")
w("             AND hd.TongThanhToan <> (SELECT SUM(t.SoTien) FROM dbo.ThanhToan_HoaDon t WHERE t.MaHoaDon = hd.MaHoaDon))")
w("    THROW 50105, N'ThanhToan_HoaDon: tổng tiền thanh toán không khớp hóa đơn', 1;")
w("")
w("/*------------------------------------------------------------------------------")
w(f"  29. GIAO HÀNG ({n_gh} đơn) - NoiBo: nhân viên đứng ca đi giao; Grab / Shopee: tài xế app")
w("------------------------------------------------------------------------------*/")
gh_rows = [h for h in HOA_DON if h.giao]
for part in chunks(gh_rows):
    w("INSERT INTO dbo.NhatKy_GiaoHang (MaHoaDon, LoaiShip, TenShipper, SoDienThoai, DiaChi, TrangThai, GioLay, GioGiao)")
    w("SELECT hd.MaHoaDon, v.LoaiShip, v.TenShipper, v.SDT, v.DiaChi, N'DaGiao',")
    w("       CAST(v.GioLay AS DATETIME2(0)), CAST(v.GioGiao AS DATETIME2(0))")
    w("FROM (VALUES")
    w(",\n".join(f"    (N'{h.ma}', N'{g[0]}', {q(g[1])}, '{g[2]}', {q(g[3])}, {ts(g[4])}, {ts(g[5])})"
                 for h in part for g in [h.giao]))
    w(") AS v (MaDon, LoaiShip, TenShipper, SDT, DiaChi, GioLay, GioGiao)")
    w("JOIN dbo.HoaDon hd ON hd.MaDonLocal = v.MaDon;")
    w("")
w(f"IF (SELECT COUNT(*) FROM dbo.NhatKy_GiaoHang) <> {n_gh}")
w("    THROW 50106, N'NhatKy_GiaoHang thiếu dòng', 1;")
w("")
w("/*------------------------------------------------------------------------------")
w(f"  30. GIAO DỊCH TÍCH ĐIỂM ({len(TICH_DIEM)} dòng) - Tich: +điểm, Tieu: đổi điểm trừ tiền")
w("------------------------------------------------------------------------------*/")
for part in chunks(TICH_DIEM):
    w("INSERT INTO dbo.GiaoDich_TichDiem (MaKhachHang, MaHoaDon, SoDiemThayDoi, DiemSauGD, LyDo, NgayTao)")
    w("SELECT kh.MaKhachHang, hd.MaHoaDon, v.SoDiem, v.DiemSau, v.LyDo, hd.NgayTao")
    w("FROM (VALUES")
    w(",\n".join(f"    (N'{ma}', '{sdt}', {sd}, {ds}, N'{ld}')" for ma, sdt, sd, ds, ld in part))
    w(") AS v (MaDon, SDT, SoDiem, DiemSau, LyDo)")
    w("JOIN dbo.HoaDon    hd ON hd.MaDonLocal  = v.MaDon")
    w("JOIN dbo.KhachHang kh ON kh.SoDienThoai = v.SDT;")
    w("")
w(f"IF (SELECT COUNT(*) FROM dbo.GiaoDich_TichDiem) <> {len(TICH_DIEM)}")
w("    THROW 50107, N'GiaoDich_TichDiem thiếu dòng', 1;")
w("")
w("-- Điểm hiện tại của khách = tổng các giao dịch điểm")
w("IF EXISTS (SELECT 1 FROM dbo.KhachHang kh")
w("           WHERE kh.DiemHienTai <> ISNULL((SELECT SUM(g.SoDiemThayDoi) FROM dbo.GiaoDich_TichDiem g")
w("                                           WHERE g.MaKhachHang = kh.MaKhachHang), 0))")
w("    THROW 50108, N'KhachHang.DiemHienTai không khớp GiaoDich_TichDiem', 1;")
w("")
w("-- Tiền mặt thu trong ca = TienHeThong - TienDauCa")
w("IF EXISTS (SELECT 1 FROM dbo.NhatKy_CaLam ca")
w("           WHERE ca.TienHeThong - ca.TienDauCa <> ISNULL((")
w("                 SELECT SUM(t.SoTien) FROM dbo.ThanhToan_HoaDon t JOIN dbo.HoaDon hd ON hd.MaHoaDon = t.MaHoaDon")
w("                 WHERE hd.MaCa = ca.MaCa AND t.HinhThuc = N'TienMat'), 0))")
w("    THROW 50109, N'NhatKy_CaLam: tiền hệ thống không khớp tiền mặt thu trong ca', 1;")
w("")
w("-- Nhập - xuất bán (qua công thức, combo tách thành món) - tồn kho 30/09 = hao hụt, không được âm")
w("IF EXISTS (")
w("    SELECT 1")
w("    FROM dbo.TonKho_ChiNhanh tk")
w("    WHERE (SELECT ISNULL(SUM(ct.SoLuong), 0) FROM dbo.ChiTiet_NhapHang ct")
w("           JOIN dbo.Phieu_NhapHang pn ON pn.MaPhieuNhap = ct.MaPhieuNhap")
w("           WHERE pn.MaChiNhanh = tk.MaChiNhanh AND ct.MaNguyenLieu = tk.MaNguyenLieu)")
w("        - (SELECT ISNULL(SUM(x.SoLuong * c.SoLuongTrenMotPhan), 0)")
w("           FROM (SELECT hd.MaChiNhanh, ISNULL(cb.MaSanPham, ct.MaSanPham) AS MaSanPham,")
w("                        ct.SoLuong * ISNULL(cb.SoLuong, 1) AS SoLuong")
w("                 FROM dbo.ChiTiet_HoaDon ct")
w("                 JOIN dbo.HoaDon hd ON hd.MaHoaDon = ct.MaHoaDon AND hd.TrangThaiDon = N'HoanThanh'")
w("                 LEFT JOIN dbo.Combo_ChiTiet cb ON cb.MaCombo = ct.MaSanPham) x")
w("           JOIN dbo.CongThuc c ON c.MaSanPham = x.MaSanPham")
w("           WHERE x.MaChiNhanh = tk.MaChiNhanh AND c.MaNguyenLieu = tk.MaNguyenLieu)")
w("        - tk.SoLuongTon < 0")
w(")")
w("    THROW 50110, N'Kho âm: xuất bán + tồn cuối lớn hơn tổng nhập', 1;")

# ---------------------------------------------------------------- ghi vào 02_DuLieuMau.sql
tien_mat = Counter()
for h in hd_ok:
    for ht, st, _ in h.thanh_toan:
        if ht == "TienMat":
            tien_mat[(h.cn, h.ngay + 1, CA[h.ca][0])] += st

CA_RE = re.compile(r"(\(N'(CN0\d)', N'NV\d{3}', CAST\('2026-09-(\d\d) (\d\d):00:00' AS DATETIME2\(0\)\), "
                   r"CAST\('[^']+' AS DATETIME2\(0\)\), ([\d.]+), )[\d.]+, [\d.]+(, N'[^']*'\))")
so_ca = 0


def sua_ca(m):
    global so_ca
    so_ca += 1
    dau = float(m.group(5))
    ht = dau + tien_mat[(m.group(2), int(m.group(3)), int(m.group(4)))]
    return f"{m.group(1)}{ht:.2f}, {ht:.2f}{m.group(6)}"


moi = CA_RE.sub(sua_ca, SQL)
assert so_ca == 270, so_ca


def sua_nhap(m):
    so = NHAP_MOI[(m.group(2), m.group(5))]
    cot = ", ".join(f"{'NULL' if x is None else x:>6}" for x in so)
    return f"{m.group(1)} {cot})"


i = moi.index("22. CHI TIẾT PHIẾU NHẬP")
j = moi.index("WHERE d.SoLuong > 0", i)
moi = moi[:i] + NHAP_RE.sub(sua_nhap, moi[i:j]) + moi[j:]
so_dong_nhap = sum(1 for v in NHAP_MOI.values() for x in v if x)
moi = re.sub(r"IF \(SELECT COUNT\(\*\) FROM dbo\.ChiTiet_NhapHang\) <> \d+",
             f"IF (SELECT COUNT(*) FROM dbo.ChiTiet_NhapHang) <> {so_dong_nhap}", moi)
moi = re.sub(r"Phieu_NhapHang 132 \| ChiTiet_NhapHang \d+", f"Phieu_NhapHang 132 | ChiTiet_NhapHang {so_dong_nhap}", moi)
moi = re.sub(r"HoaDon \d+ \| ChiTiet_HoaDon \d+ \| ThanhToan_HoaDon \d+ \| NhatKy_GiaoHang \d+ \| GiaoDich_TichDiem \d+",
             f"HoaDon {len(HOA_DON)} | ChiTiet_HoaDon {n_ct} | ThanhToan_HoaDon {n_tt} | "
             f"NhatKy_GiaoHang {n_gh} | GiaoDich_TichDiem {len(TICH_DIEM)}", moi)

i = moi.index(MOC_DAU) + len(MOC_DAU)
j = moi.index(MOC_CUOI, i)
moi = moi[:i] + "\n" + "\n".join(L) + "\n" + moi[j:]
SEED.write_text(moi, encoding="utf-8")

# ---------------------------------------------------------------- báo cáo
print(f"Hóa đơn {len(HOA_DON)} | chi tiết {n_ct} | thanh toán {n_tt} | giao hàng {n_gh} | tích điểm {len(TICH_DIEM)} | dòng nhập {so_dong_nhap}")
for cn in CN:
    hs = [h for h in hd_ok if h.cn == cn]
    print(f"  {cn}: {len(hs)} HĐ | tiền hàng {sum(h.tien_hang for h in hs):,} | giảm {sum(h.tien_giam for h in hs):,} "
          f"| doanh thu {tong_dt[cn]:,} ({tong_dt[cn] / 30:,.0f}/ngày) | combo {sum(sl for h in hs for sp, sl, _, _ in h.lines if sp.startswith('CB'))} "
          f"| KM {sum(1 for h in hs if h.km)} | app {sum(1 for h in hs if h.nguon != 'POS')} | quán giao {sum(1 for h in hs if h.nguon == 'POS' and h.loai == 'GiaoHang')}")
