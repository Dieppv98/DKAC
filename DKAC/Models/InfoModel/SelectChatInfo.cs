using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class SelectChatInfo
    {
        public int Id { get; set; }
        public int? IdSender { get; set; }
        public int? IdReceiver { get; set; }
        public string Message { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TimeDisplay { get; set; }
        public int? AdminTo { get; set; } //từ admin gửi đến ai hoặc từ ai gửi đến admin
        public string AdminToUserName { get; set; } //tên của user gửi đến admin hoặc từ admin -> user
    }
}