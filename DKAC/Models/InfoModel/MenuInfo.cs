using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class MenuInfo
    {
        private const string RegexCode = @"^[a-zA-Z0-9]+$";

        public int id { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }

        [StringLength(50)]
        [Display(Name = "Mã thực đơn")]
        [Required(ErrorMessage = "Vui lòng nhập mã thực đơn")]
        [RegularExpression(RegexCode, ErrorMessage = "Mã thực đơn không được chứa kí tự đặc biệt")]
        [Remote("CheckDuplicatedMenuCode", "Menu", AdditionalFields = "id", HttpMethod = "POST", ErrorMessage = "Mã thực đơn đã tồn tại")]
        public string MenuCode { get; set; }

        [StringLength(200)]
        [Display(Name = "Tên thực đơn")]
        [Required(ErrorMessage = "Vui lòng nhập tên thực đơn")]
        public string MenuName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime? Date { get; set; }
        public int? Ca { get; set; }
        public int? DishId { get; set; }

        public List<MenuDetail> details { get; set; }
        public List<MenuDetailInfo> detailInfos { get; set; }
        public List<SelectListItem> lstDish { get; set; }
    }
}