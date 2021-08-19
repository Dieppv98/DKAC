using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface ILoginRepository
    {
        bool Login(string UserName, string Password);
        Employee GetEmployeeByUserName(string userName);
        bool SignUp(EmployeeInfo model);
    }
}