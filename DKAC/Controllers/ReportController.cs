using DKAC.Common;
using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using DKAC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DKAC.Controllers
{
    public class ReportController : BaseController
    {
        ReportRepository _reportRepo = new ReportRepository();
        RoomRepository _roomRepo = new RoomRepository();
        EmployeeRepository _emRepo = new EmployeeRepository();

        // GET: Report
        public ActionResult ReportByPersonal()
        {
            var em = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.emName = em.FullName;
            var roomName = _roomRepo.GetRoomNameByRoomId(em.RoomID);
            ViewBag.RoomName = roomName;
            return View();
        }

        public PartialViewResult ReportByPersonalSearch(DateTime? fDate, DateTime? tDate, string dish)
        {
            var em = (User)Session[CommonConstants.USER_SESSION];
            var lstReg = _reportRepo.GetListRegisterReport(fDate, tDate, em.id, dish);
            ViewBag.emName = em.FullName;
            var roomName = _roomRepo.GetRoomNameByRoomId(em.RoomID);
            ViewBag.RoomName = roomName;
            return PartialView(lstReg);
        }

        public ActionResult ExportExcelReportByPersonal(DateTime? fDate, DateTime? tDate, string dish)
        {
            #region Lấy dữ liệu
            var em = (User)Session[CommonConstants.USER_SESSION];
            var roomName = _roomRepo.GetRoomNameByRoomId(em.RoomID);
            var lstReg = _reportRepo.GetListRegisterReport(fDate, tDate, em.id, dish);
            int? totalQty = 0;
            int? totalMoney = 0;
            foreach (var item in lstReg)
            {
                totalQty += item.Quantity;
                totalMoney += item.Quantity * item.Dish.Cost;
            }
            #endregion

            #region Xuất excel
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string filepath = string.Concat(rootPath + Ultilities.GetPathTemplateExcel(), "/DanhSachThongKe.xlsx");
            string v_filename = "Thống kê đăng ký ăn ca theo tháng_" + DateTime.Now.Date.ToString("dd/MM/yyyy/hhmmss");
            string filepathtemp = string.Concat(rootPath + Ultilities.GetPathTempFolder(), "/Temp/DanhSachThongKe.xlsx");

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filepath);
            System.IO.FileInfo fileTemp = fileInfo.CopyTo(filepathtemp, true);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage p = new ExcelPackage(fileTemp, true))
            {
                try
                {
                    var tongcot = 6;
                    int v_vt = 3;
                    #region Thống kê đăng ký ăn ca cá nhân
                    ExcelWorksheet ws = p.Workbook.Worksheets[0];
                    #region - Khu vực header chung ở trên
                    ws.Cells[1, 1].Value = "THỐNG KÊ ĂN CA CÁ NHÂN " + (fDate.HasValue ? "TỪ " + fDate.Value.ToString("dd/MM/yyyy") : "TỪ TRƯỚC") + (tDate.HasValue ? " ĐẾN " + tDate.Value.ToString("dd/MM/yyyy") : " ĐẾN NAY");
                    ws.Cells[1, 1, 1, tongcot].Merge = true;
                    ws.Cells[1, 1, 1, tongcot].Style.Font.Bold = true;

                    ws.Cells[2, 1].Value = "Họ và tên: " + em.FullName;
                    ws.Cells[2, 1, 2, 3].Merge = true;
                    ws.Cells[2, 1, 2, 3].Style.Font.Bold = true;

                    ws.Cells[2, 4].Value = "Phòng ban: " + roomName;
                    ws.Cells[2, 4, 2, tongcot].Merge = true;
                    ws.Cells[2, 4, 2, tongcot].Style.Font.Bold = true;

                    #endregion
                    #region - Thiết lập header
                    ws.Cells[v_vt, 1].Value = "STT";
                    ws.Cells[v_vt, 1].Style.WrapText = true;
                    ws.Cells[v_vt, 2].Value = "Tên suất ăn";
                    ws.Cells[v_vt, 2].Style.WrapText = true;
                    ws.Cells[v_vt, 3].Value = "Ca";
                    ws.Cells[v_vt, 3].Style.WrapText = true;
                    ws.Cells[v_vt, 4].Value = "Số lượng";
                    ws.Cells[v_vt, 4].Style.WrapText = true;
                    ws.Cells[v_vt, 5].Value = "Ngày đăng ký";
                    ws.Cells[v_vt, 5].Style.WrapText = true;
                    ws.Cells[v_vt, 6].Value = "Tổng tiền";
                    ws.Cells[v_vt, 6].Style.WrapText = true;
                    ws.Cells[v_vt, 1, v_vt, tongcot].Style.Font.Bold = true;
                    #endregion
                    v_vt++;
                    #region - Fill thông tin danh sach
                    var count = 0;
                    foreach (var item in lstReg)
                    {
                        count++;
                        ws.Cells[v_vt, 1].Value = count;
                        ws.Cells[v_vt, 2].Value = item.Dish.DishName;
                        ws.Cells[v_vt, 3].Value = item.Ca;
                        ws.Cells[v_vt, 4].Value = item.Quantity;
                        ws.Cells[v_vt, 5].Value = item.RegisterDate.ToString("dd/MM/yyyy");
                        ws.Cells[v_vt, 6].Value = String.Format("{0:N0} VNĐ", item.Quantity * item.Dish.Cost);
                        v_vt++;
                    }
                    ws.Cells[v_vt, 1].Value = "Tổng";
                    ws.Cells[v_vt, 1, v_vt, 3].Merge = true;
                    ws.Cells[v_vt, 4].Value = totalQty;
                    ws.Cells[v_vt, tongcot].Value = String.Format("{0:N0} VNĐ", totalMoney); ;
                    ws.Cells[v_vt, 1, v_vt, 6].Style.Font.Bold = true;
                    #endregion
                    #endregion

                    #region body style
                    //sheet 1
                    ws.Cells.Style.Font.Size = 13;
                    ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[v_vt, 1, v_vt, tongcot].AutoFitColumns();
                    ws.DefaultColWidth = 20;
                    ws.Column(1).Width = 7;
                    ws.Column(2).Width = 30;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 30;
                    ws.Column(6).Width = 25;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    #endregion

                    p.SaveAs(fileTemp);
                    using (ExcelPackage p2 = new ExcelPackage(fileTemp, true))
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment; filename=\"" + v_filename + ".xlsx\"");
                        Response.AddHeader("Content-Type", "application/Excel");
                        Response.BinaryWrite(p2.GetAsByteArray());
                        Response.End();
                    }
                    fileTemp.Delete();
                    return new EmptyResult();
                }
                catch (Exception)
                {
                    return RedirectToAction("ReportByPersonal", "Report");
                }
            }
            #endregion
        }

        public ActionResult ReportByGroup()
        {
            var em = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.emName = em.FullName;
            var roomName = _roomRepo.GetRoomNameByRoomId(em.RoomID);
            ViewBag.RoomName = roomName;

            var lstUser = _reportRepo.GetLstUserByRoomId(em.RoomID);
            RegisterByPersonalInfo model = new RegisterByPersonalInfo();
            List<SelectListItem> lstU = lstUser.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = $"{a.FullName} ({a.UserName})",
                    Value = a.id.ToString()
                };
            });
            model.lstU = lstU;
            return View(model);
        }

        public PartialViewResult ReportByGroupSearch(DateTime? fDate, DateTime? tDate, int? userId)
        {
            var em = (User)Session[CommonConstants.USER_SESSION];
            var lstReg = _reportRepo.GetListRegisterGroupByRoomId(fDate, tDate, em.RoomID, userId);
            ViewBag.emName = em.FullName;
            var roomName = _roomRepo.GetRoomNameByRoomId(em.RoomID);
            ViewBag.RoomName = roomName;
            return PartialView(lstReg);
        }

        public ActionResult ExportExcelReportByGroup(DateTime? fDate, DateTime? tDate, int? userId)
        {
            #region Lấy dữ liệu
            var em = (User)Session[CommonConstants.USER_SESSION];
            var roomName = _roomRepo.GetRoomNameByRoomId(em.RoomID);
            var lstReg = _reportRepo.GetListRegisterGroupByRoomId(fDate, tDate, em.RoomID, userId);
            int? totalQty = 0;
            int? totalMoney = 0;
            foreach (var item in lstReg)
            {
                totalQty += item.Quantity;
                totalMoney += item.Quantity * item.Dish.Cost;
            }
            #endregion

            #region Xuất excel
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string filepath = string.Concat(rootPath + Ultilities.GetPathTemplateExcel(), "/DanhSachThongKe.xlsx");
            string v_filename = "Thống kê đăng ký ăn ca tập thể " + (fDate.HasValue ? $"từ {fDate.Value.ToString("dd/MM/yyyy")}" : "từ trước") + (tDate.HasValue ? $" đến {tDate.Value.ToString("dd/MM/yyyy")}" : " đến nay");
            string filepathtemp = string.Concat(rootPath + Ultilities.GetPathTempFolder(), "/Temp/DanhSachThongKe.xlsx");

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filepath);
            System.IO.FileInfo fileTemp = fileInfo.CopyTo(filepathtemp, true);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage p = new ExcelPackage(fileTemp, true))
            {
                try
                {
                    var tongcot = 4;
                    int v_vt = 3;
                    #region Thống kê đăng ký ăn ca cá nhân
                    ExcelWorksheet ws = p.Workbook.Worksheets[0];
                    #region - Khu vực header chung ở trên
                    ws.Cells[1, 1].Value = "THỐNG KÊ ĂN CA TẬP THỂ THÁNG " + (fDate.HasValue ? $"TỪ {fDate.Value.ToString("dd/MM/yyyy")}" : " TỪ TRƯỚC") + (tDate.HasValue ? $" ĐẾN {tDate.Value.ToString("dd/MM/yyyy")}" : " ĐẾN NAY");
                    ws.Cells[1, 1, 1, tongcot].Merge = true;
                    ws.Cells[1, 1, 1, tongcot].Style.Font.Bold = true;

                    ws.Cells[2, 1].Value = "Trưởng phòng: " + em.FullName;
                    ws.Cells[2, 1, 2, 2].Merge = true;
                    ws.Cells[2, 1, 2, 2].Style.Font.Bold = true;

                    ws.Cells[2, 3].Value = "Phòng ban: " + roomName;
                    ws.Cells[2, 3, 2, tongcot].Merge = true;
                    ws.Cells[2, 3, 2, tongcot].Style.Font.Bold = true;

                    #endregion
                    #region - Thiết lập header
                    ws.Cells[v_vt, 1].Value = "STT";
                    ws.Cells[v_vt, 1].Style.WrapText = true;
                    ws.Cells[v_vt, 2].Value = "Nhân viên";
                    ws.Cells[v_vt, 2].Style.WrapText = true;
                    ws.Cells[v_vt, 3].Value = "Số lượng đăng ký";
                    ws.Cells[v_vt, 3].Style.WrapText = true;
                    ws.Cells[v_vt, 4].Value = "Tổng tiền";
                    ws.Cells[v_vt, 4].Style.WrapText = true;
                    ws.Cells[v_vt, 1, v_vt, tongcot].Style.Font.Bold = true;
                    #endregion
                    v_vt++;
                    #region - Fill thông tin danh sach
                    var count = 0;
                    var lstEm = _emRepo.GetEmployees(em.RoomID);
                    foreach (var e in lstEm)
                    {
                        count++;
                        int? sl = 0;
                        int? m = 0;
                        foreach (var reg in lstReg)
                        {
                            if (reg.UserId == e.id)
                            {
                                sl += reg.Quantity;
                                m += reg.Quantity * reg.Dish.Cost;
                            }
                        }
                        ws.Cells[v_vt, 1].Value = count;
                        ws.Cells[v_vt, 2].Value = e.FullName;
                        ws.Cells[v_vt, 3].Value = sl;
                        ws.Cells[v_vt, 4].Value = String.Format("{0:N0} VNĐ", m);
                        v_vt++;
                    }
                    ws.Cells[v_vt, 1].Value = "Tổng";
                    ws.Cells[v_vt, 1, v_vt, 2].Merge = true;
                    ws.Cells[v_vt, 3].Value = totalQty;
                    ws.Cells[v_vt, tongcot].Value = String.Format("{0:N0} VNĐ", totalMoney);
                    ws.Cells[v_vt, 1, v_vt, tongcot].Style.Font.Bold = true;
                    #endregion
                    #endregion

                    #region body style
                    //sheet 1
                    ws.Cells.Style.Font.Size = 13;
                    ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[v_vt, 1, v_vt, tongcot].AutoFitColumns();
                    ws.DefaultColWidth = 20;
                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 45;
                    ws.Column(3).Width = 25;
                    ws.Column(4).Width = 25;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[1, 1, v_vt, tongcot].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    #endregion

                    p.SaveAs(fileTemp);
                    using (ExcelPackage p2 = new ExcelPackage(fileTemp, true))
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment; filename=\"" + v_filename + ".xlsx\"");
                        Response.AddHeader("Content-Type", "application/Excel");
                        Response.BinaryWrite(p2.GetAsByteArray());
                        Response.End();
                    }
                    fileTemp.Delete();
                    return new EmptyResult();
                }
                catch (Exception)
                {
                    return RedirectToAction("ReportByGroup", "Report");
                }
            }
            #endregion
        }
    }
}