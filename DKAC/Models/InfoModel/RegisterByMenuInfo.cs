using DKAC.Models.EntityModel;
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
        public int? RegisterId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public int? Quantity { get; set; } // số lượng
        public DateTime? DateApply { get; set; }// ngày bắt đầu áp dụng
        public DateTime? RegDate { get; set; }// ngày đăng ký
        public string RegDateString { get; set; }// ngày đăng ký string
        public int? Ca { get; set; } // ca đăng ký
        public int? DishId { get; set; }
        public int? ModifyBy { get; set; }
        public string DishName { get; set; }
        public List<SelectListItem> lstMenu { get; set; }
        public List<RegisterByDuplplicate> lstDupplicate { get; set; }
        public List<User> lstUser { get; set; }
        public List<int> lstUserId { get; set; }
    }

    public class RegisterByDuplplicate
    {
        public int RegisterId { get; set; }
        public bool? Replace { get; set; }
        public bool? Plus { get; set; }
        public bool? Skip { get; set; }
        public bool? Remove { get; set; }
    }
}