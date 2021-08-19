using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models
{
    public class BaseModel
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public int totalRecord { get; set; }
    }
}