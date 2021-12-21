using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public partial class MenuDetailInfo
    {
        public int id { get; set; }

        public int? MenuId { get; set; }

        public int? DishId { get; set; }

        public int? IndexDate { get; set; }//Thứ tự ngày trong menu từ 1-7
        
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        
        public string ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }
        public string ImageDishUrl { get; set; }
    }
}