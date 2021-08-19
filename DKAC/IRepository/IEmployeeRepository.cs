using DKAC.Models.EntityModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.Repository
{
    public interface IEmployeeRepository
    {
        int Add(Employee employee);
        int Delete(int id);
        Employee GetById(int? id);
        EmployeeRequestModel GetListAllEmployee(Employee employee, User user, string KeySearch, int page, int pageSize);
        int Update(Employee employee);
        Employee GetByUserName(string UserName, int? id);
        List<Room> GetAllRoom();

    }
}