using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class EmployeeInfo
    {
        private const string RegexCode = @"^[a-zA-Z0-9]+$";

        public int id { get; set; }

        [StringLength(200)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [RegularExpression(RegexCode, ErrorMessage = "Tên tài khoản không được chứa kí tự đặc biệt")]
        [StringLength(100)]
        [Remote("CheckDuplicatedUserName", "Employee", AdditionalFields = "id", HttpMethod = "POST", ErrorMessage = "Tên tài khoản đã tồn tại")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [StringLength(20)]
        public string PassWord { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("PassWord", ErrorMessage = "Xác nhận mật khẩu không đúng, mời nhập lại")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(20)]
        public string ConfirmPassWord { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        [StringLength(50)]
        public int? Gender { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Tel { get; set; }

        [StringLength(50)]
        public string CMND { get; set; }

        public int? RoomID { get; set; }

        public string RoomName { get; set; }

        public int? Role { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte IsDeleted { get; set; }
    }
}