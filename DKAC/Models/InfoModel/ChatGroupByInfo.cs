using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class ChatGroupByInfo
    {
        public DateTime? CreatedDate { get; set; }

        public List<Chat> lstChat { get; set; }

        public List<ChatGroupByHourInfo> lstChatHour { get; set; }
    }

    public class ChatGroupByHourInfo
    {
        public string Hour { get; set; }

        public List<Chat> lstChat { get; set; }


    }
}