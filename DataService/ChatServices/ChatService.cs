using AutoMapper;
using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Chat;

namespace DataService.ChatServices
{
    public interface IChatService
    {
        Task<bool> CreateChatAsync(string AccId,CreateChatViewModel createChatViewModel);
        Task<List<ChatViewModel>> GetChatAsync(string IdAdvise, string AcId);

        Task<bool> DeleteChatAsync(string IdChat, string AcId);
        
    }
    public class ChatService : IChatService
    {
        private readonly ExpertConectionContext _expertConectionContext;
        private readonly Mapper _mapper;
        public static int PAGE_SIZE { get; set; } = 5;
        public ChatService(ExpertConectionContext expertConectionContext)
        {
            _expertConectionContext = expertConectionContext;
            var config = new MapperConfiguration(cfg => {

                cfg.AddProfile<MappingProfile>();
            });
            _mapper = new Mapper(config);
        }
        public async Task<bool> CreateChatAsync(string AccId, CreateChatViewModel createChatViewModel)
        {
            if(createChatViewModel != null && !string.IsNullOrEmpty(AccId))
            {
                var currentAdvise = await _expertConectionContext.Advises.Where(p => p.Id == createChatViewModel.IdAvise && p.IsActive && p.ExpertConfirm && p.UserConfirm).Include("User").Include("CategoryMapping").Include("CategoryMapping.Expert").FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(createChatViewModel.ImageUrl) || !string.IsNullOrEmpty(createChatViewModel.ContentChat))
                {
                    if (currentAdvise.User.AcountId == AccId)
                    {
                        var newChat = new Chat
                        {
                            Id = Guid.NewGuid().ToString(),
                            AdviseId = createChatViewModel.IdAvise,
                            FromAcc = AccId,
                            ToAc = currentAdvise.CategoryMapping.Expert.AcId,
                            CreateDate = DateTime.Now,
                            ImageUrl = createChatViewModel.ImageUrl,
                            ContentChat = createChatViewModel.ContentChat,
                            IsActive = true,
                        };

                        await _expertConectionContext.Chats.AddAsync(newChat);
                        await _expertConectionContext.SaveChangesAsync();
                        return true;
                    }
                    else if (currentAdvise.CategoryMapping.Expert.AcId == AccId)
                    {
                        var newChat = new Chat
                        {
                            Id = Guid.NewGuid().ToString(),
                            AdviseId = createChatViewModel.IdAvise,
                            FromAcc = AccId,
                            ToAc = currentAdvise.User.AcountId,
                            CreateDate = DateTime.Now,
                            ImageUrl = createChatViewModel.ImageUrl,
                            ContentChat = createChatViewModel.ContentChat,
                            IsActive = true,
                        };
                        await _expertConectionContext.Chats.AddAsync(newChat);
                        await _expertConectionContext.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }else return false;
            }
            else return false;
        }

        public async Task<List<ChatViewModel>> GetChatAsync(string IdAdvise, string AcId)
        {
            if(!string.IsNullOrEmpty(IdAdvise) || !string.IsNullOrEmpty(AcId))
            {
                var currentAddvise = await _expertConectionContext.Advises.Where(p => p.Id == IdAdvise && p.IsActive && p.UserConfirm && p.ExpertConfirm).Include("User").Include("CategoryMapping.Expert").FirstOrDefaultAsync();
                if (currentAddvise.User.AcountId == AcId || currentAddvise.CategoryMapping.Expert.AcId == AcId)
                {
                    var AllChat = _expertConectionContext.Chats.Where(p => p.AdviseId == IdAdvise && p.IsActive).AsQueryable();
                    if (AllChat.Count() > 0)
                    {
                        AllChat = AllChat.OrderByDescending(p => p.CreateDate);
                        var AllChatPaging = PageServices.PaginateList<Chat>.CreatePaginateList(AllChat, 6, 1);
                        return _mapper.Map<List<ChatViewModel>>(AllChatPaging);
                    }
                    else return null;
                } else return null; 
            } else return null;

        }

        public Task<bool> DeleteChat(string IdChat, string AcId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteChatAsync(string IdChat, string AcId)
        {
            if (!string.IsNullOrEmpty(IdChat) || !string.IsNullOrEmpty(AcId))
            {
                var currentChat = await _expertConectionContext.Chats.Where(p => p.Id == IdChat && p.IsActive).FirstOrDefaultAsync();
                if (currentChat.FromAcc == AcId)
                {
                    currentChat.IsActive = false;
                    _expertConectionContext.Chats.Update(currentChat);
                    await _expertConectionContext.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
