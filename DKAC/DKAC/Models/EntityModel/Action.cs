namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Action")]
    public partial class Action
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string ActionName { get; set; }

        public int? Value { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
