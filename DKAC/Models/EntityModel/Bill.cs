namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill")]
    public partial class Bill
    {
        public int id { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }

        [StringLength(50)]
        public string BillCode { get; set; }

        [StringLength(200)]
        public string BillName { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? RegisterDate { get; set; }

        public int? Total { get; set; }

        public int? Status { get; set; }
    }
}
