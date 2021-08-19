using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKAC.Models.InfoModel
{
    public class RoomInfo
    {
        private const string RegexCode = @"^[a-zA-Z0-9]+$";
        private const string RegexPhone = @"^0[0-9]{9,10}$";

        public int? Id { get; set; }

        [StringLength(200)]
        [Display(Name = "Tên phòng")]
        [Required(ErrorMessage = "Vui lòng nhập tên phòng")]
        [Remote("CheckDuplicatedRoomName", "Room", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tên phòng bị trùng, mời nhập tên khác")]
        public string RoomName { get; set; }

        [Display(Name = "Mã phòng")]
        [Required(ErrorMessage = "Vui lòng nhập mã phòng")]
        [RegularExpression(RegexCode, ErrorMessage = "Mã phòng không được chứa kí tự đặc biệt")]
        [StringLength(100)]
        [Remote("CheckDuplicatedShortName", "Room", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Mã đã bị trùng, mời nhập mã khác")]
        public string RoomShortName { get; set; }

        public int? Members { get; set; }

        public int? Manager { get; set; }

        [StringLength(100)]
        public string DiaChi { get; set; }

        [StringLength(15)]
        [RegularExpression(RegexPhone, ErrorMessage = "Chỉ được nhập số, độ dài từ 10 đến 11 ký tự, phải bắt đầu bằng số 0")]
        public string SDT { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte IsDeleted { get; set; }
    }
}