using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class RegisterByPersonalInfo
    {
        public int? id { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }

        [StringLength(50)]
        public string RegisterCode { get; set; }

        public int? UserId { get; set; }

        public string EmployeeName { get; set; }

        public int? RoomId { get; set; }

        public string RoomName { get; set; }

        public int? DishId { get; set; }

        public Dish Dish { get; set; }

        public int? Ca { get; set; }

        public int? Quantity { get; set; }

        public DateTime RegisterDate { get; set; }

        public List<SelectListItem> lstU { get; set; }
        public List<SelectListItem> lstR { get; set; }
        public List<SelectListItem> lstDish { get; set; }
    }
}