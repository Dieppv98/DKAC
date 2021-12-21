namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        public int id { get; set; }
        
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        
        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }

        [StringLength(50)]
        public string MenuCode { get; set; }

        [StringLength(200)]
        public string MenuName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime? Date { get; set; }
        public int? Ca { get; set; }
    }
}
