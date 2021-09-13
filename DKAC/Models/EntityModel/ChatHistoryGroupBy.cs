using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DKAC.Models.EntityModel
{
    [Table("ChatHistoryGroupBy")]
    public class ChatHistoryGroupBy
    {
        public int Id { get; set; }
        public int? SenderId { get; set; } // id của người gửi
        public int? ReceiverId { get; set; } // id của người nhận
        public int? NumberMessageNotSeen { get; set; } // số tin nhắn chưa xem
        public string Message { get; set; } // nội dung của tin nhắn
        public DateTime? CreatedDate { get; set; } //Thời gian gửi tin nhắn
        public int? AdminTo { get; set; } //từ admin gửi đến ai hoặc từ ai gửi đến admin
    }
}