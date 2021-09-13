using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DKAC.Models.EntityModel
{
    [Table("Chat")]
    public class Chat
    {
        public int Id { get; set; }
        [Index("IX_Chat_SenderId", IsClustered = false, IsUnique = false)]
        public int? SenderId { get; set; } // id của người gửi
        [Index("IX_Chat_ReceiverId", IsClustered = false, IsUnique = false)]
        public int? ReceiverId { get; set; } // id của người nhận
        public int? Status { get; set; } // Trạng thái đã xem hay chưa
        public string Message { get; set; } // nội dung của tin nhắn
        [Index("IX_Chat_CreatedDate", IsClustered = false, IsUnique = false)]
        public DateTime? CreatedDate { get; set; } //Thời gian gửi tin nhắn
    }
}