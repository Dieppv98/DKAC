using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class RegisterByMenuInfo
    {
        public int? MenuId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime? DateApply { get; set; }// ngày bắt đầu áp dụng
        public List<SelectListItem> lstMenu { get; set; }
    }
}