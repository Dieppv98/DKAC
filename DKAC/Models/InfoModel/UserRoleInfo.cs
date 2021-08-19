using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class UserRoleInfo
    {
        public int Id { get; set; }

        public int? RoleId { get; set; }

        public int? UserId { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool Check { get; set; }
    }
}