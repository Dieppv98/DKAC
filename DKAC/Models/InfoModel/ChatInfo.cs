using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class ChatInfo
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public int? SeenStatusMaesage { get; set; } //số lượng tin nhắn chưa đọc
        public DateTime? CreatedDate { get; set; } //ngày tin nhắn được tạo ra gần đây nhất
    }
}