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
        RoomRequestModel GetListAllRoom(RoomRequestModel request, int pageIndex, int recordPerPage, out int totalCount);
        int Update(Room room);
        Room GetByShortName(string RoomShortName, int? id);
        Room GetByRoomName(string RoomName, int? id);
        List<User> GetAllEmployeeByRoom(int roomId);
    }
}