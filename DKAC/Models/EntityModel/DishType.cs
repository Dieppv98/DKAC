namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DishType")]
    public partial class DishType
    {
        public int id { get; set; }

        [StringLength(50)]
        public string DishTypeCode { get; set; }

        [StringLength(200)]
        public string DishTypeName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }
    }
}
