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

        public List<SelectChatInfo> LoadPopupSelectChat()
        {
            var data = (from c in db.ChatHistoryGroupBies
                        join u in db.Users on c.AdminTo equals u.id
                        select new SelectChatInfo()
                        {
                            Id = c.Id,
                            IdSender = c.SenderId,
                            IdReceiver = c.ReceiverId,
                            AdminTo = c.AdminTo,
                            AdminToUserName = u.FullName + "(" + u.UserName + ")",
                            CreatedDate = c.CreatedDate,
                            Message = c.Message,
                            Status = c.NumberMessageNotSeen,
                        }).OrderByDescending(x => x.CreatedDate).ToList() ?? new List<SelectChatInfo>();

            return data;
        }

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

            int? adminto = -1;
            if (model.SenderId == (int)MessageOfAdmin.admin)
                adminto = model.ReceiverId;
            if (model.ReceiverId == (int)MessageOfAdmin.admin)
                adminto = model.SenderId;

            var chatHistory = db.ChatHistoryGroupBies.FirstOrDefault(x => x.AdminTo == adminto);
            if (chatHistory == null)
            {
                ChatHistoryGroupBy groupBy = new ChatHistoryGroupBy()
                {
                    CreatedDate = DateTime.Now,
                    Message = model.Message,
                    SenderId = model.SenderId,
                    ReceiverId = model.ReceiverId,
                    NumberMessageNotSeen = 1,
                    AdminTo = adminto,
                };
                db.ChatHistoryGroupBies.Add(groupBy);
            }
            else
            {
                chatHistory.Message = model.Message;
                chatHistory.CreatedDate = DateTime.Now;
                chatHistory.NumberMessageNotSeen = ((chatHistory.NumberMessageNotSeen ?? 0) + 1);
            }

            return Task.FromResult(db.SaveChanges());
        }

        public List<ChatGroupByInfo> GetChatByClient(int page, int size, int userId, int toId)
        {
            var now = DateTime.Now.Date;
            page = (page - 1);
            //lst chat do client gửi và nhận
            List<Chat> lstChatSend = new List<Chat>();
            if(toId == -1)
            {
                lstChatSend = db.Chats.Where(x => (x.SenderId == (int)MessageOfAdmin.admin && x.ReceiverId == userId) || (x.SenderId == userId && x.ReceiverId == (int)MessageOfAdmin.admin)).OrderByDescending(x => x.CreatedDate).Skip(page * size).Take(size).ToList() ?? new List<Chat>();
            }
            else
            {
                lstChatSend = db.Chats.Where(x => (x.SenderId == (int)MessageOfAdmin.admin && x.ReceiverId == toId) || (x.SenderId == toId && x.ReceiverId == (int)MessageOfAdmin.admin)).OrderByDescending(x => x.CreatedDate).Skip(page * size).Take(size).ToList() ?? new List<Chat>();
            }
            
            var groupDay = lstChatSend.GroupBy(x => x.CreatedDate.Value.Date)
                .Select(x => x.FirstOrDefault().CreatedDate).OrderBy(x => x.Value).ToList();

            List<ChatGroupByInfo> lstChatGroup = new List<ChatGroupByInfo>();

            foreach (var item in groupDay)
            {
                if (item.Value.Date == now)
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
                    var lst = lstChatSend.Where(x => x.CreatedDate.Value.Date == item.Value.Date).OrderBy(x => x.CreatedDate).ToList();
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