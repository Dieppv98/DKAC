namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Jugment")]
    public partial class Jugment
    {
        public int id { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }

        public int? EmployeeId { get; set; }

        public int? DishId { get; set; }

        public int? Point { get; set; }

        [Column("Jugment")]
        [StringLength(1000)]
        public string Jugment1 { get; set; }

        public DateTime? JugmentDate { get; set; }
    }
}
