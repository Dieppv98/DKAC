using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DKAC.Models.EntityModel
{
    [Table("LogTimeCache")]
    public partial class LogTimeCache
    {
        public int id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? PreOder { get; set; }
        public int? CacheType { get; set; }
        public string Description { get; set; }
    }
}