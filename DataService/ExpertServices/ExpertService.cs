using DatabaseConection.Entities;
using DataService.RatingServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CategoryMapping;
using ViewModel.Expert;
using ViewModel.Rating;

namespace DataService.ExpertServices
{
    public interface IExpertService
    {
        Task<bool> CreateExpertAsync(string accId, ExpertModel expertModel);
        Task<ExpertProfileModel> GetProfileExpert(string expertId);
        Task<List<CategoryMapping>> GetCategoryMappingOfExpertAsync(string accIdExpert);
        Task<bool> UpdateProfileExpert(string expertId, ExpertUpdateProfileModel expertUpdateProfileModel);

    }
    public class ExpertService : IExpertService
    {
        private readonly ExpertConectionContext _context;
        private readonly IRatingService _ratingService;

        public ExpertService(ExpertConectionContext context, IRatingService ratingService)
        {
            _context = context;
            _ratingService = ratingService;
        }
        public async Task<bool> CreateExpertAsync(string accId, ExpertModel expertModel)
        {
            if (expertModel != null)
            {
                if (checkEmailExist(expertModel.Email) == false)
                {
                    try
                    {
                        var newExpert = new Expert()
                        {
                            Id = Guid.NewGuid().ToString(),
                            AcId = accId,
                            Fullname = expertModel.Fullname,
                            CerfificateLink = expertModel.CerfificateLink,
                            Introduction = expertModel.Introduction,
                            RatingSummary = 0,
                            WorlkRole = expertModel.WorlkRole,
                            Email = expertModel.Email,
                            Phone = expertModel.Phone,
                            EmailConfirm = true,
                            ExpertConfirm = false,
                            IsActive = true,
                        };

                        await _context.Experts.AddAsync(newExpert);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else return false;
            }
            else return false;
        }

        public async Task<ExpertProfileModel> GetProfileExpert(string expertId)
        {
            if (!string.IsNullOrEmpty(expertId))
            {
                var CurrentExpet = await _context.Experts.Where(p => p.Id == expertId && p.IsActive).Include("Ac").Include("CategoryMappings").FirstOrDefaultAsync();

                if (CurrentExpet.CategoryMappings.Count() > 0)
                {
                    var username = CurrentExpet.Ac.Username;
                    var allListOfCategorymapping = await _context.CategoryMappings.Where(p => p.ExpertId == expertId && p.IsActive).Include("Advises").ToListAsync();
                    var listRatingDetail = await CreateExpertProfileOnCategorymapping(allListOfCategorymapping);
                    var expertProfile =  new ExpertProfileModel
                    {
                        Id = CurrentExpet.Id,
                        UserName = username,
                        Fullname = CurrentExpet.Fullname,
                        CerfificateLink = CurrentExpet.CerfificateLink,
                        Introduction = CurrentExpet.Introduction,
                        RatingSummary = CurrentExpet.RatingSummary,
                        WorlkRole = CurrentExpet.WorlkRole,
                        Phone = CurrentExpet.Phone,
                        listCategoryMapping = listRatingDetail,
                    };
                    return expertProfile;
                }
                else return null;
            }
            else return null;
        }
        
        public async Task<List<CategoryMappingProfileModel>> CreateExpertProfileOnCategorymapping(List<CategoryMapping> categoryMappings)
        {
            if(categoryMappings.Count > 0)
            {
                List<CategoryMappingProfileModel> temp = new List<CategoryMappingProfileModel>();
                foreach (var item in categoryMappings)
                {
                    if(item != null)
                    {
                        var resultListRating = await getCategoryMappingProfileModel(item.Id);
                        temp.Add(new CategoryMappingProfileModel
                        {
                            IdCategoryMapping = item.Id,
                            NameOfCategoryMapping = item.Name,
                            SummaryRating = item.SummaryRating,
                            ratingViewModels = resultListRating
                        });
                    }                   
                }
                return temp;
            }
            else return null;
        }

        public async Task<List<RatingInCategoryMappingViewModel>> getCategoryMappingProfileModel(string categoryMappingId)
        {
            List<RatingInCategoryMappingViewModel> tmp = new List<RatingInCategoryMappingViewModel>();
            var allAdvise = await _context.Advises.Where(p => p.CategoryMappingId == categoryMappingId && p.IsActive && p.IsRating).Include("IdRatingNavigation").ToListAsync();
            if (allAdvise.Count > 0)
            {
                foreach (var item in allAdvise) {
                   if(item != null)
                    {
                        tmp.Add(new RatingInCategoryMappingViewModel
                        {
                            IdRating = item.IdRating,
                            FromUser = item.IdRatingNavigation.UserId,
                            RatingPoint = item.IdRatingNavigation.RatingPoint,
                            comment = item.IdRatingNavigation.Comment,
                        });
                    }
                }
                return tmp;
            }
            else return null;
        }

        public async Task<bool> UpdateProfileExpert(string expertAccId, ExpertUpdateProfileModel expertUpdateProfileModel)
        {
            if (!string.IsNullOrEmpty(expertAccId))
            {
                if (checkEmailExist(expertUpdateProfileModel.Email) == false)
                {
                    var currentExpert = await _context.Experts.Where(p => p.AcId == expertAccId && p.IsActive == true).FirstOrDefaultAsync();
                    if (currentExpert != null)
                    {
                        if (!string.IsNullOrEmpty(expertUpdateProfileModel.Fullname) && expertUpdateProfileModel.Fullname != "string")
                        {
                            currentExpert.Fullname = expertUpdateProfileModel.Fullname;
                        }
                        if (!string.IsNullOrEmpty(expertUpdateProfileModel.CerfificateLink) && expertUpdateProfileModel.CerfificateLink != "string")
                        {
                            currentExpert.CerfificateLink = expertUpdateProfileModel.CerfificateLink;
                        }
                        if (!string.IsNullOrEmpty(expertUpdateProfileModel.Introduction) && expertUpdateProfileModel.Introduction != "string")
                        {
                            currentExpert.Introduction = expertUpdateProfileModel.Introduction;
                        }
                        if (!string.IsNullOrEmpty(expertUpdateProfileModel.WorlkRole) && expertUpdateProfileModel.WorlkRole != "string")
                        {
                            currentExpert.WorlkRole = expertUpdateProfileModel.WorlkRole;
                        }
                        if (!string.IsNullOrEmpty(expertUpdateProfileModel.Email) && expertUpdateProfileModel.Email != "string")
                        {
                            currentExpert.Email = expertUpdateProfileModel.Email;
                        }
                        if (!string.IsNullOrEmpty(expertUpdateProfileModel.Phone) && expertUpdateProfileModel.Phone != "string")
                        {
                            currentExpert.Phone = expertUpdateProfileModel.Phone;
                        }
                        _context.Experts.Update(currentExpert);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<List<CategoryMapping>> GetCategoryMappingOfExpertAsync(string accIdExpert)
        {
            if (!string.IsNullOrEmpty(accIdExpert))
            {
                return await _context.CategoryMappings.Where(p => p.Expert.AcId == accIdExpert && p.IsActive).ToListAsync();
            }
            else return null;
        }

        private bool checkEmailExist(string email)
        {
            foreach (var item in _context.Experts)
            {
                if (item.Email == email)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
