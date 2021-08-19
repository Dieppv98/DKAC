using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DKAC.Models.EntityModel
{
    [Table("Notify")]
    public class Notify
    {
        public int Id { get; set; }
        public int? ReceiveUserId { get; set; } // id của người nhận
        public int? DishId { get; set; } // id của món ăn vừa đc thêm
        public int? SeenStatus { get; set; } // Trạng thái đã xem hay chưa
        public int? TypeNoti { get; set; } //Loại thông báo
        public string ContentNoti { get; set; } // nội dung của thông báo
        public string Image { get; set; } // ảnh của thông báo
        public DateTime? CreatedDate { get; set; } //Thời gian thông báo đc tạo ra
    }
}