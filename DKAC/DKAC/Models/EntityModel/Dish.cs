namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dish")]
    public partial class Dish
    {
        public int id { get; set; }

        [StringLength(50)]
        public string DishCode { get; set; }

        [StringLength(200)]
        public string DishName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? DishTypeId { get; set; }

        public int? SupplierId { get; set; }

        public int? Cost { get; set; }

        public int? JugmentQty { get; set; }

        public int? RegisterQty { get; set; }

        public double? JugmentPoint { get; set; }

        [StringLength(1000)]
        public string Image { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }
    }
}
