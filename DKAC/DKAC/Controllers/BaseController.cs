using DKAC.Common;
using DKAC.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DKAC.Controllers
{
    public class BaseController : Controller
    {
        DKACDbContext db = new DKACDbContext();
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext fillterContext)
        {
            var em = (Employee)Session[CommonConstants.EMPLOYEE_SESSION];
            var user = (User)Session[CommonConstants.USER_SESSION];
            if (user == null && em == null)
            {
                fillterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            base.OnActionExecuting(fillterContext);
        }

        public bool CheckPermission(int pageId, int action, User currentUser)
        {
            var lstRole = db.UserRoles.Where(x => x.UserId == currentUser.id).ToList();
            List<PermissionAction> lstPer = new List<PermissionAction>();
            foreach (var item in lstRole)
            {
                var lstPerAction = db.PermissionActions.Where(x => x.RoleId == item.RoleId).ToList();
                if (lstPerAction != null) lstPer.AddRange(lstPerAction);
            }
            lstPer.Distinct().ToList();
            if (currentUser.UserName == "admin") return true;
            int actionkey = 7;
            int action1 = 4;
            var r = actionkey & (byte)action1;
            var check = lstPer.Any(x => x.PageId == pageId && (x.ActionKey & (byte)action) == (byte)action);
            return check;
        }
    }
}