using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Rating;

namespace DataService.RatingServices
{
    public interface IRatingService
    {
        Task<bool> RatingAdviseAsync(string IdAdvise, string AcId, float pointRating, string comment);
        Task<List<RatingViewModel>> GetAllRatingAsync();
        Task<List<RatingViewModel>> GetRatingByIdAsync(string idRating);
        Task<List<RatingViewModelSecond>> GetRatingByCategoryMappingAsync(string categoryMappingId);
        Task<List<RatingViewModel>> GetRatingByCategoryByUserAsync(string UserId);
        Task<List<RatingViewModelThird>> GetRatingByCategoryByExpertrAsync(string Id);
        Task<bool> UpdateRatingAsync(string IdRating, string AccId, RatingUpdateModel ratingUpdateModel);
        Task<bool> DeleteRatingAsync(string IdRating, string AccId);
        public class RatingService : IRatingService
        {
            private readonly ExpertConectionContext _context;
            public RatingService(ExpertConectionContext context)
            {
                _context = context;
            }
            public async Task<bool> RatingAdviseAsync(string IdAdvise, string AcId, float pointRating, string comment)
            {
                if (!string.IsNullOrEmpty(IdAdvise) && !string.IsNullOrEmpty(AcId) && !string.IsNullOrEmpty(comment))
                {
                    var currentAdvise = await _context.Advises.Where(p => p.Id == IdAdvise && p.ExpertConfirm && p.UserConfirm && !p.IsRating).Include("User").FirstOrDefaultAsync();
                    if (currentAdvise.User.AcountId == AcId)
                    {
                        var currentRating = await _context.Ratings.Where(p => p.Id == currentAdvise.IdRating && p.IsActice).FirstOrDefaultAsync();
                        if (currentRating != null)
                        {
                            currentRating.RatingPoint = pointRating;
                            currentRating.Comment = comment;
                            _context.Ratings.Update(currentRating);
                            await _context.SaveChangesAsync();
                            var isUpdate = await UpdateSummyRatingCategoryMapping(currentAdvise.CategoryMappingId);
                            if (isUpdate)
                            {
                                currentAdvise.IsRating = true;
                                await _context.SaveChangesAsync();
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else { return false; }
                }
                else return false;
            }

            private async Task<bool> UpdateSummyRatingCategoryMapping(string IdCategoryMapping)
            {
                if (!string.IsNullOrEmpty(IdCategoryMapping))
                {
                    float countOne = 0;
                    float countTwo = 0;
                    float countThree = 0;
                    float countFour = 0;
                    float countFive = 0;
                    float countNumber = 0;
                    CategoryMapping CurrentCategoryMapping = await _context.CategoryMappings.Where(p => p.Id == IdCategoryMapping && p.IsActive).FirstOrDefaultAsync();

                    if (CurrentCategoryMapping != null)
                    {
                        var allAdviseOfCategoryMapping = await _context.Advises.Where(p => p.CategoryMappingId == IdCategoryMapping && p.IsActive).Include("IdRatingNavigation").ToListAsync();
                        if (allAdviseOfCategoryMapping.Count() > 0)
                        {
                            foreach (var item in allAdviseOfCategoryMapping)
                            {
                                if (item.IsRating)
                                {
                                    if (item.IdRatingNavigation.RatingPoint == 1)
                                    {
                                        countOne++;
                                    }
                                    if (item.IdRatingNavigation.RatingPoint == 2)
                                    {
                                        countTwo++;
                                    }
                                    if (item.IdRatingNavigation.RatingPoint == 3)
                                    {
                                        countThree++;
                                    }
                                    if (item.IdRatingNavigation.RatingPoint == 4)
                                    {
                                        countFour++;
                                    }
                                    if (item.IdRatingNavigation.RatingPoint == 5)
                                    {
                                        countFive++;
                                    }
                                    countNumber++;
                                }
                            }
                            float summaryRating = (((countOne * 1) + (countTwo * 2) + (countThree * 3) + (countFour * 4) + (countFive * 5)) / countNumber);
                            CurrentCategoryMapping.SummaryRating = summaryRating;
                            var IsUpdate = await UpdateRatingSummaryExpert(CurrentCategoryMapping.ExpertId);
                            _context.CategoryMappings.Update(CurrentCategoryMapping);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }

            private async Task<bool> UpdateRatingSummaryExpert(string IdExpert)
            {
                if (!string.IsNullOrEmpty(IdExpert))
                {
                    double totalRatingSummary = 0;
                    var currentExpert = await _context.Experts.Where(p => p.Id == IdExpert && p.IsActive).Include("CategoryMappings").FirstOrDefaultAsync();
                    if (currentExpert.CategoryMappings.Count() > 0)
                    {
                        var count = 0;
                        foreach (var item in currentExpert.CategoryMappings)
                        {
                            if (item.SummaryRating > 0)
                            {
                                totalRatingSummary = totalRatingSummary + item.SummaryRating;
                                count++;    
                            }
                        }
                        currentExpert.RatingSummary = (totalRatingSummary / count);
                        _context.Experts.Update(currentExpert);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

            public async Task<List<RatingViewModel>> GetAllRatingAsync()
            {
                var AllRating = await _context.Advises.Where(p => p.IsActive && p.UserConfirm && p.ExpertConfirm && p.IsRating).Include("IdRatingNavigation").Select(p => new RatingViewModel
                {
                    IdRating = p.IdRating,
                    FromUser = p.UserId,
                    ToCategoryMapping = p.CategoryMappingId,
                    RatingPoint = p.IdRatingNavigation.RatingPoint,
                    comment = p.IdRatingNavigation.Comment,
                }).ToListAsync();

                return AllRating;
            }

            public async Task<List<RatingViewModel>> GetRatingByIdAsync(string idRating)
            {
                var AllRating = await _context.Advises.Where(p => p.IdRating == idRating && p.IsRating && p.IsActive && p.UserConfirm && p.ExpertConfirm).Include("IdRatingNavigation").Select(p => new RatingViewModel
                {
                    IdRating = p.IdRating,
                    FromUser = p.UserId,
                    ToCategoryMapping = p.CategoryMappingId,
                    RatingPoint = p.IdRatingNavigation.RatingPoint,
                    comment = p.IdRatingNavigation.Comment,
                }).ToListAsync();

                return AllRating;
            }

            public async Task<List<RatingViewModelSecond>> GetRatingByCategoryMappingAsync(string categoryMappingId)
            {
                if (!string.IsNullOrEmpty(categoryMappingId))
                {
                    var allAdviseOfCategoryMapping = await _context.Advises.Where(p => p.CategoryMappingId == categoryMappingId && p.IsActive && p.ExpertConfirm && p.UserConfirm && p.IsRating).Include("IdRatingNavigation").ToListAsync();
                    if (allAdviseOfCategoryMapping.Count() > 0)
                    {
                        return allAdviseOfCategoryMapping.Select(p => new RatingViewModelSecond
                        {                       
                            FromUser = p.UserId,
                            RatingPoint = p.IdRatingNavigation.RatingPoint,
                            comment = p.IdRatingNavigation.Comment,
                        }).ToList();
                    }
                    else return null;
                }
                return null;
            }

            public async Task<List<RatingViewModel>> GetRatingByCategoryByUserAsync(string accId)
            {
                if (!string.IsNullOrEmpty(accId))
                {
                    var allAdviseOfCategoryMapping = await _context.Advises.Where(p => p.User.AcountId == accId && p.IsActive && p.ExpertConfirm && p.UserConfirm && p.IsRating).Include("IdRatingNavigation").ToListAsync();
                    if (allAdviseOfCategoryMapping.Count() > 0)
                    {
                        return allAdviseOfCategoryMapping.Select(p => new RatingViewModel
                        {
                            IdRating = p.IdRating,
                            FromUser = p.UserId,
                            ToCategoryMapping = p.CategoryMappingId,
                            RatingPoint = p.IdRatingNavigation.RatingPoint,
                            comment = p.IdRatingNavigation.Comment,
                        }).ToList();
                    }
                    else return null;
                }
                return null;
            }
            public async Task<List<RatingViewModelThird>> GetRatingByCategoryByExpertrAsync(string Id)
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    var allAdviseOfCategoryMapping = await _context.Advises.Where(p => p.CategoryMapping.ExpertId == Id && p.IsActive && p.ExpertConfirm && p.UserConfirm && p.IsRating).Include("IdRatingNavigation").ToListAsync();
                    if (allAdviseOfCategoryMapping.Count() > 0)
                    {
                        return allAdviseOfCategoryMapping.Select(p => new RatingViewModelThird
                        {
                            IdRating = p.IdRating,
                            FromUser = p.UserId,                         
                            RatingPoint = p.IdRatingNavigation.RatingPoint,
                            comment = p.IdRatingNavigation.Comment,
                        }).ToList();
                    }
                    else return null;
                }
                return null;
            }
            public async Task<bool> UpdateRatingAsync(string IdRating, string AccId,RatingUpdateModel ratingUpdateModel)
            {
                if (!string.IsNullOrEmpty(IdRating) && !string.IsNullOrEmpty(AccId))
                {
                    var adviseOfRating = await _context.Advises.Where(p => p.IdRating == IdRating && p.IsRating && p.IsActive && p.IsRating).Include("User").Include("CategoryMapping").FirstOrDefaultAsync();
                    if (adviseOfRating.User.AcountId == AccId)
                    {
                        var currentRating = await _context.Ratings.Where(p => p.Id == IdRating && p.IsActice).FirstOrDefaultAsync();
                        currentRating.Comment = ratingUpdateModel.newComment;
                        currentRating.RatingPoint = ratingUpdateModel.newPoint;
                        var isUpdate = await UpdateSummyRatingCategoryMapping(adviseOfRating.CategoryMappingId);
                        if (isUpdate)
                        {
                            var UpdateExpert = await UpdateRatingSummaryExpert(adviseOfRating.CategoryMapping.ExpertId);
                            if (UpdateExpert)
                            {
                                _context.Ratings.Update(currentRating);
                                await _context.SaveChangesAsync();
                                return true;
                            }
                            else return false;
                        } else return false;               
                    }
                    else return false;
                }
                else return false;
            }

            public async Task<bool> DeleteRatingAsync(string IdRating, string AccId)
            {
                if (!string.IsNullOrEmpty(IdRating) && !string.IsNullOrEmpty(AccId))
                {
                    var adviseOfRating = await _context.Advises.Where(p => p.IdRating == IdRating && p.IsRating && p.IsActive).Include("User").FirstOrDefaultAsync();
                    if (adviseOfRating.User.AcountId == AccId)
                    {
                        _context.Ratings.Where(p => p.Id == IdRating).FirstOrDefault().IsActice = false;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
        }
    }
}
