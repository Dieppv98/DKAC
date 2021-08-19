namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Modul")]
    public partial class Modul
    {
        public int id { get; set; }

        [StringLength(500)]
        public string ModulName { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
    }
}
