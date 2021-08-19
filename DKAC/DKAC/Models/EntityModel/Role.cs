namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string RoleName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
