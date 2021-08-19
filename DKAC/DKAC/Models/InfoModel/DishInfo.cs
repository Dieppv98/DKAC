using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class DishInfo
    {
        private const string RegexCode = @"^[a-zA-Z0-9]+$";

        public int id { get; set; }

        [StringLength(50, ErrorMessage = "Mã không được quá 50 ký tự")]
        [Required(ErrorMessage = "Mã không được để trống")]
        [RegularExpression(RegexCode, ErrorMessage = "Mã không được chứa ký tự đặc biệt")]
        [Remote("CheckDuplicatedCode", "Dish", HttpMethod = "POST", AdditionalFields = "id", ErrorMessage = "Mã bị trùng, vui lòng thử mã khác")]
        public string DishCode { get; set; }

        [StringLength(200)]
        public string DishName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? SupplierId { get; set; }

        public int? DishTypeId { get; set; }

        public string DishTypeName { get; set; }

        [StringLength(50)]
        public string SupplierCode { get; set; }

        [StringLength(200)]
        public string SupplierName { get; set; }

        public int? Cost { get; set; }

        public int? JugmentQty { get; set; }

        public int? RegisterQty { get; set; }

        public double? JugmentPoint { get; set; }

        [StringLength(1000)]
        public string Image { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }

        public int JugStatus { get; set; }
    }
}