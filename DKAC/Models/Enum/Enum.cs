using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.Enum
{
    public enum CacheType
    {
        MemoryCache_GetAllSupplier = 1,
        MemoryCache_GetAllDish = 2,
    }

    public enum SeenStatus
    {
        Seen = 1, // đã xem
    }

    public enum TypeOfNoti
    {
        TypeRegister = 1, //Thông báo đăng ký ăn hàng ngày
        TypeDishNew = 2, //Thông báo khi có món mới
    }

    public enum PageId
    {
        QuanLyNhaCungCap = 1,
        QuanLyNguoiDung = 2,
        QuanLyPhongBan = 3,
        QuanLyThucDon = 4,
        QuanLySuatAn = 5,
        ThongKeDKACCaNhanAdmin = 6,
        ThongKeDKACCaNhanNhanVien = 7,
        ThongKeDKACTheoPhongAdmin = 8,
        ThongKeDKACTheoPhongNhanVien = 9,
        ThongKeDanhGiaNguoiDung = 10,
        DanhGiaNhaCC = 11,
        QuanLyQuyenTruyCap = 12,
        QuanLyVaiTro = 13,
        ThongKeLuotSuDungHeThong = 14,
    }

    public enum Actions
    {
        Xem = 1,
        Them = 2,
        Sua = 4,
        Xoa = 8,
    }
}