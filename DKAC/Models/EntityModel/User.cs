namespace DKAC.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int id { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifyBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        public byte? IsDeleted { get; set; }
        
        public string Code { get; set; }
        
        public string FullName { get; set; }
        
        public string UserName { get; set; }
        
        public string PassWord { get; set; }
   
        public string Position { get; set; }

        public int? UserGroupId { get; set; }
        public int? RoomID { get; set; }
        public int? Role { get; set; }
    }
}
