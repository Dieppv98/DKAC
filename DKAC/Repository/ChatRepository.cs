using DKAC.IRepository;
using DKAC.Models.EntityModel;
using DKAC.Models.Enum;
using DKAC.Models.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DKAC.Repository
{
    public class ChatRepository : IChatRepository
    {
        DKACDbContext db = new DKACDbContext();

        public Task<int> AddMessage(ChatReceiverInfo model)
        {
            Chat chat = new Chat()
            {
                CreatedDate = DateTime.Now,
                Message = model.Message,
                SenderId = model.SenderId,
                ReceiverId = model.ReceiverId,
            };
            db.Chats.Add(chat);
            return Task.FromResult(db.SaveChanges());
        }

        public List<ChatGroupByInfo> GetChatByClinet(int page, int size, int id)//  id của client
        {
            var now = DateTime.Now.Date;
            page = (page - 1);
            //lst chat do client gửi và nhận
            var lstChatSend = db.Chats.Where(x => (x.SenderId == (int)MesageOfAdmin.admin && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == (int)MesageOfAdmin.admin)).OrderBy(x => x.CreatedDate).Take(20).Skip(page * size).Take(size).ToList() ?? new List<Chat>();

            var groupDay = lstChatSend.GroupBy(x => x.CreatedDate.Value.Date)
                .Select(x => x.FirstOrDefault().CreatedDate).OrderBy(x => x.Value).ToList();

            List<ChatGroupByInfo> lstChatGroup = new List<ChatGroupByInfo>();
            
            foreach (var item in groupDay)
            {
                if(item.Value.Date == now)
                {
                    ChatGroupByInfo chatGroup = new ChatGroupByInfo();
                    List<ChatGroupByHourInfo> lstChatGroupHour = new List<ChatGroupByHourInfo>();

                    var lst = lstChatSend.Where(x => x.CreatedDate.Value.Date == item.Value.Date).ToList() ?? new List<Chat>();
                    var groupHour = lst.GroupBy(x => x.CreatedDate.Value.Hour).Select(x => x.FirstOrDefault()).OrderBy(x => x.CreatedDate).ToList() ?? new List<Chat>();
                    foreach (var h in groupHour)
                    {
                        ChatGroupByHourInfo chatHour = new ChatGroupByHourInfo();
                        var lstHour = lst.Where(x => x.CreatedDate.Value.Hour == h.CreatedDate.Value.Hour).OrderBy(x => x.CreatedDate).ToList() ?? new List<Chat>();
                        chatHour.Hour = h.CreatedDate.Value.ToString("HH:mm");
                        chatHour.lstChat = lstHour;
                        lstChatGroupHour.Add(chatHour);
                    }

                    chatGroup.CreatedDate = null;
                    chatGroup.lstChat = lst;
                    chatGroup.lstChatHour = lstChatGroupHour;
                    lstChatGroup.Add(chatGroup);
                }
                else
                {
                    ChatGroupByInfo chatGroup = new ChatGroupByInfo();
                    var lst = lstChatSend.Where(x => x.CreatedDate.Value.Date == item.Value.Date).ToList();
                    chatGroup.CreatedDate = item.Value.Date;
                    chatGroup.lstChat = lst;
                    lstChatGroup.Add(chatGroup);
                }
            }
            
            return lstChatGroup;
        }
        
        public List<ChatInfo> GetReceiverChatById(int id)
        {
            var rs = (from u in db.Users
                      where u.IsDeleted != 1 && u.id != id
                      select new ChatInfo()
                      {
                          id = u.id,
                          FullName = u.FullName,
                          SeenStatusMaesage = db.Chats.Where(x => x.ReceiverId == u.id && x.SenderId == id && x.Status != (int)SeenStatus.Seen).Count(),
                          CreatedDate = db.Chats.Where(x => x.ReceiverId == u.id && x.SenderId == id && x.Status != (int)SeenStatus.Seen).OrderByDescending(x => x.CreatedDate).FirstOrDefault().CreatedDate,
                      }).OrderByDescending(x => x.CreatedDate).Take(20).ToList() ?? new List<ChatInfo>();

            return rs;
        }
    }
}