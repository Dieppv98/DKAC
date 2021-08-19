namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissionAction")]
    public partial class PermissionAction
    {
        public int id { get; set; }

        public int? PageId { get; set; }

        public int? RoleId { get; set; }

        public int? ActionKey { get; set; }
    }
}
