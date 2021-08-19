using DKAC.Models.EntityModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface IRoomRepository
    {
        int Add(Room room);
        int Delete(int id);
        Room GetById(int id);
        RoomRequestModel GetListAllRoom(Employee employee, User user, string KeySearch, int page, int pageSize);
        int Update(Room room);
        Room GetByShortName(string RoomShortName, int? id);
        Room GetByRoomName(string RoomName, int? id);
        List<Employee> GetAllEmployeeByRoom(int roomId);
    }
}