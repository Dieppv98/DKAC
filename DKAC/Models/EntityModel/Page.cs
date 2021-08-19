namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Page")]
    public partial class Page
    {
        public int id { get; set; }

        [StringLength(1000)]
        public string PageName { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
        public string Url { get; set; }

        public int? ModulId { get; set; }
    }
}
