using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class ChatReceiverInfo
    {
        public int? SenderId { get; set; }//id người gửi
        public int? ReceiverId { get; set; }//id người nhận
        public string Message { get; set; }//nội dung message
    }
}