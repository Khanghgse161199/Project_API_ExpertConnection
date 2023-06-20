using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Advise;

namespace DataService.AdviseServices
{
    public interface IAdviseService
    {
        Task<bool> CreateAdviseAsync(string IdCategoryMapping,string accId);
        Task<List<AdviseExpertViewModel>> GetAllAdviseConfrimOfUSerAsync(string accId);
        Task<List<AdviseExpertViewModel>> GetAllAdviseUnConfirmOfUserAsync(string accId);
        Task<List<AdviseUserViewModel>> GetAllAdviseConfrimOfExpertAsync(string accId);
        Task<List<AdviseUserViewModel>> GetAllAdviseUnConfirmOfExpert(string accId);
        Task<bool> ConfirmAdviseOfUserAsync(string accId, string AdviseId);
        Task<bool> ConfirmAdviseOfExpertAsync(string accId, string AdviseId);
        Task<bool> DeleteAviseAsync(string accId, string AdviseId);
    }
    public class AdviseService : IAdviseService
    {
        private readonly ExpertConectionContext _expertConectionContext;

        public AdviseService(ExpertConectionContext expertConectionContext)
        {
            _expertConectionContext = expertConectionContext;
        }

        public async Task<bool> CreateAdviseAsync(string IdCategoryMapping, string accId)
        {
            if (!string.IsNullOrEmpty(IdCategoryMapping) && !string.IsNullOrEmpty(accId))
            {
                var currentUser = await _expertConectionContext.Users.Where(p => p.AcountId == accId && p.IsActive).FirstOrDefaultAsync();
                var checkExist = await _expertConectionContext.Advises.Where(p => p.UserId == currentUser.Id && p.CategoryMappingId == IdCategoryMapping && p.IsActive).FirstOrDefaultAsync();
                if (checkExist == null && currentUser != null)
                {
                    string idRating = Guid.NewGuid().ToString();
                    var newRating = new Rating
                    {
                        Id = idRating,
                        RatingPoint = 0,
                        Comment = "",
                        IsActice = true,
                        UserId = currentUser.Id,
                    };

                    var newAdvise = new Advise
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = currentUser.Id,
                        CategoryMappingId = IdCategoryMapping,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        UserConfirm = false,
                        ExpertConfirm = false,
                        IdRating = idRating,
                        IsRating = false,
                    };
                    
                    await _expertConectionContext.Advises.AddAsync(newAdvise);
                    await _expertConectionContext.Ratings.AddAsync(newRating);
                    await _expertConectionContext.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<AdviseExpertViewModel>> GetAllAdviseConfrimOfUSerAsync(string accId)
        {
            if (!string.IsNullOrEmpty(accId))
            {
                var currentUser = await _expertConectionContext.Users.Where(p => p.AcountId == accId && p.IsActive).FirstOrDefaultAsync();
                if (currentUser != null)
                {
                    var listAllAdvises = _expertConectionContext.Advises.Where(p => p.UserId == currentUser.Id && p.IsActive && p.UserConfirm && p.ExpertConfirm).Include("IdRatingNavigation").Select(p => new AdviseExpertViewModel
                    {
                        IdAdvise = p.Id,
                        NameCategoryMapping = p.CategoryMapping.Name,
                        NameExpert = p.CategoryMapping.Expert.Fullname,
                        CreateDay = p.CreatedDate.ToString("dd/MM/yyyy_HH:mm"),
                    });
                    return listAllAdvises.ToList();
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<AdviseExpertViewModel>> GetAllAdviseUnConfirmOfUserAsync(string accId)
        {
            if (!string.IsNullOrEmpty(accId))
            {
                var currentUser = await _expertConectionContext.Users.Where(p => p.AcountId == accId && p.IsActive).FirstOrDefaultAsync();
                if (currentUser != null)
                {
                    var listAllAdvises = _expertConectionContext.Advises.Where(p => p.UserId == currentUser.Id && p.IsActive && !p.UserConfirm).Select(p => new AdviseExpertViewModel
                    {
                        IdAdvise = p.Id,
                        NameCategoryMapping = p.CategoryMapping.Name,
                        NameExpert = p.CategoryMapping.Expert.Fullname,
                        CreateDay = p.CreatedDate.ToString("dd/MM/yyyy_HH:mm"),
                    });
                    return listAllAdvises.ToList();
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<AdviseUserViewModel>> GetAllAdviseConfrimOfExpertAsync(string accId)
        {
            if (!string.IsNullOrEmpty(accId))
            {
                var currentExpert = await _expertConectionContext.Experts.Where(p => p.AcId == accId && p.IsActive).FirstOrDefaultAsync();
                if (currentExpert != null)
                {
                    var listAllConfirm = _expertConectionContext.Advises.Where(p => p.CategoryMapping.ExpertId == currentExpert.Id && p.ExpertConfirm && p.UserConfirm && p.IsActive).Select(p => new AdviseUserViewModel
                    {
                        IdAdvise = p.Id,
                        NameCategoryMapping = p.CategoryMapping.Name,
                        NameUser = p.User.FullName,
                        PhoneNumber = p.User.PhoneNumber,
                        Email = p.User.Email,
                        BirthDay = p.CreatedDate.ToString("dd/MM/yyyy"),
                        Introduction = p.User.Introduction,
                        CreateDay = p.CreatedDate.ToString("dd/MM/yyyy_HH:mm"),
                    });
                    return listAllConfirm.ToList();
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<AdviseUserViewModel>> GetAllAdviseUnConfirmOfExpert(string accId)
        {
            if (!string.IsNullOrEmpty(accId))
            {
                var currentExpert = await _expertConectionContext.Experts.Where(p => p.AcId == accId && p.IsActive).FirstOrDefaultAsync();
                if (currentExpert != null)
                {
                    var listAllWaittingConfirm = _expertConectionContext.Advises.Where(p => p.CategoryMapping.ExpertId == currentExpert.Id && !p.ExpertConfirm && p.IsActive).Select(p => new AdviseUserViewModel
                    {
                        IdAdvise = p.Id,
                        NameCategoryMapping = p.CategoryMapping.Name,
                        NameUser = p.User.FullName,
                        PhoneNumber = p.User.PhoneNumber,
                        Email = p.User.Email,
                        BirthDay = p.CreatedDate.ToString("dd/MM/yyyy"),
                        Introduction = p.User.Introduction,
                        CreateDay = p.CreatedDate.ToString("dd/MM/yyyy_HH:mm"),                       
                    });
                    return listAllWaittingConfirm.ToList();
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> ConfirmAdviseOfUserAsync(string accId, string AdviseId)
        {
            if (!string.IsNullOrEmpty(accId) && !string.IsNullOrEmpty(AdviseId))
            {
                var currentUser = await _expertConectionContext.Users.Where(p => p.AcountId == accId && p.IsActive && p.UserConfirm).FirstOrDefaultAsync();
                if (currentUser != null)
                {
                    var currentAdvise = await _expertConectionContext.Advises.Where(p => p.Id == AdviseId && p.UserId == currentUser.Id && p.IsActive && !p.UserConfirm).FirstOrDefaultAsync();
                    if (currentAdvise != null)
                    {
                        currentAdvise.UserConfirm = true;
                        _expertConectionContext.Advises.Update(currentAdvise);
                        await _expertConectionContext.SaveChangesAsync();
                        return true;
                    }else return false;
                }
                else return false;
            } else return false;
        }

        public async Task<bool> ConfirmAdviseOfExpertAsync(string accId, string AdviseId)
        {
            if (!string.IsNullOrEmpty(accId) && !string.IsNullOrEmpty(AdviseId))
            {
                var currentExpert = await _expertConectionContext.Experts.Where(p => p.AcId == accId && p.IsActive && p.ExpertConfirm).FirstOrDefaultAsync();
                if (currentExpert != null)
                {
                    var currentAdvise = await _expertConectionContext.Advises.Where(p => p.Id == AdviseId && p.CategoryMapping.ExpertId == currentExpert.Id && p.IsActive && !p.ExpertConfirm).FirstOrDefaultAsync();
                    if (currentAdvise != null)
                    {
                        currentAdvise.ExpertConfirm = true;
                        _expertConectionContext.Advises.Update(currentAdvise);
                        await _expertConectionContext.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteAviseAsync(string accId, string AdviseId)
        {
            if(!string.IsNullOrEmpty(accId) && !string.IsNullOrEmpty(AdviseId))
            {
                var currentUser = await _expertConectionContext.Users.Where(p => p.AcountId == accId && p.IsActive && p.UserConfirm).FirstOrDefaultAsync();
                if (currentUser != null)
                {
                    var currentAdvise = await _expertConectionContext.Advises.Where(p => p.Id == AdviseId && p.UserId == currentUser.Id && !p.UserConfirm).FirstOrDefaultAsync();
                    if (currentAdvise != null)
                    {
                        currentAdvise.IsActive = false;
                        _expertConectionContext.Advises.Update(currentAdvise);
                        await _expertConectionContext.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            return false;
        }
    }
}
    