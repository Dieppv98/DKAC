using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class SupplierInfo
    {
        private const string RegexCode = @"^[a-zA-Z0-9]+$";
        private const string RegexPhone = @"^0[0-9]{9,10}$";

        public int id { get; set; }

        [StringLength(50, ErrorMessage = "Mã không được quá 50 ký tự")]
        [Required(ErrorMessage = "Mã không được để trống")]
        [RegularExpression(RegexCode, ErrorMessage = "Mã không được chứa ký tự đặc biệt")]
        [Remote("CheckDuplicatedCode", "Supplier", HttpMethod = "POST", AdditionalFields = "id", ErrorMessage = "Mã bị trùng, vui lòng thử mã khác")]
        public string SupplierCode { get; set; }

        [StringLength(200, ErrorMessage = "Tên không được quá 200 ký tự")]
        public string SupplierName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        [RegularExpression(RegexPhone, ErrorMessage = "Số điện thoại chỉ được nhập số, có 10 hoặc 11 ký tự, và bắt đầu bằng số 0")]
        public string Tel { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }
    }
}