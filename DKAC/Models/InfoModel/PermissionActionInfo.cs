using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Models.InfoModel
{
    public class PermissionActionInfo
    {
        public int id { get; set; }

        public int? PageId { get; set; }

        public string PageName { get; set; }

        public int? RoleId { get; set; }

        public string RoleName { get; set; }

        public int? ActionKey { get; set; }

        public User User { get; set; }

        public Page page { get; set; }

        public List<PermissionAction> ListPermissionActions { get; set; }

        public List<Page> ListPage { get; set; }

        public List<Role> ListRole { get; set; }

        public List<EntityModel.Action> ListAction { get; set; }

        public List<Modul> ListModul { get; set; }

        public List<User> ListUser { get; set; }

        public List<UserRole> ListUserRole { get; set; }

        public int? ModulId { get; set; }
        public string Url { get; set; }
    }
}