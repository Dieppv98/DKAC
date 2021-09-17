using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKAC.IRepository
{
    public interface IChatRepository
    {
        List<ChatInfo> GetReceiverChatById(int id);
        List<ChatGroupByInfo> GetChatByClient(int page, int size, int userId, int toId);
        Task<int> AddMessage(ChatReceiverInfo model);
        List<SelectChatInfo> LoadPopupSelectChat();
        int UpdateSeenById(int updateSeeById);
        int GetMessageCoutByUserId(User user);
    }
}
