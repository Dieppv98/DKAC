using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.RequestModel
{
    public class SupplierRequestModel : BaseModel
    {
        public string KeyWord { get; set; }
        public List<Supplier> data { get; set; }

        public User currentUser { get; set; }

        public bool hasViewPermission { get; set; }

        public bool hasAddPermission { get; set; }

        public bool hasUpdatePermission { get; set; }

        public bool hasDeletePermission { get; set; }
        public int? hasDataPermission { get; set; }
    }
}