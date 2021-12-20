using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class ReportByDishInfo
    {
        public int Id { get; set; }
        public int? DishId { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public string DishName { get; set; }
        public int? NumberTotal { get; set; } //tổng số lượng đã đặt
        public List<ListReportByDish> lstData { get; set; }
    }
    
    public class ListReportByDish
    {
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public int? NumberRegister { get; set; }
    }
}