using AutoMapper;
using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CategoryMapping;

namespace DataService.CategoryMappingServices
{
    public interface ICategoryMappingService
    {
        Task<bool> RegisterCategoryForExpertAsync(string idCategory, CreateCategoryMappingViewModel newCategoryMapping, string idAccountExpert);
        Task<bool> UpdateCategoryMappingAsync(string idCategoryMapping, string accIdUser, CreateCategoryMappingViewModel categoryMappingModel);
        Task<bool> UpdateCategoryMappingActiveAsync(string idCategoryMapping, string accIdExpert);
        Task<List<CategoryMappingViewModel>> GetAllCategoryMappingAsync();
        Task<List<CategoryMapping>> GetAllCategoryMappingByCategoryAsync(string IdCategory);
        Task<bool> DeleteMappingCategoryAsync(string idCategoryMapping, string accIdExpert);

        Task<List<CategoryMapping>> GetAllCategoryMappingUnConfirmAsync();
    }
    public class CategoryMappingService : ICategoryMappingService
    {
        private readonly ExpertConectionContext _context;
        private readonly Mapper _mapper;

        public CategoryMappingService(ExpertConectionContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg => {

                cfg.AddProfile<MappingProfile>();
            });
            _mapper = new Mapper(config);

        }

        public async Task<bool> RegisterCategoryForExpertAsync(string idCategory, CreateCategoryMappingViewModel newCategoryMapping, string idAccountExpert)
        {
            if (!string.IsNullOrEmpty(idCategory) && !string.IsNullOrEmpty(idAccountExpert))
            {
                var CurrentExpert = await _context.Experts.Where(p => p.AcId == idAccountExpert && p.IsActive).FirstOrDefaultAsync();
                if (newCategoryMapping != null && CurrentExpert != null)
                {
                    var findingCategory = await _context.Categories.Where(p => p.Id == idCategory).FirstOrDefaultAsync();
                    var isExist = await _context.CategoryMappings.Where(p => p.Expert.Id == CurrentExpert.Id && p.CategoryId == findingCategory.Id).FirstOrDefaultAsync();
                    if (findingCategory != null)
                    {
                        if (isExist == null)
                        {
                            var newCategoryMappingExpert = new CategoryMapping()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ExpertId = CurrentExpert.Id,
                                CategoryId = idCategory,
                                Price = newCategoryMapping.Price,
                                ExperienceYear = newCategoryMapping.ExperienceYear,
                                SummaryRating = 0,
                                Introduction = newCategoryMapping.Introduction,
                                Description = newCategoryMapping.Description,
                                IsActive = true,
                                IsConfirm = false,
                                Name = newCategoryMapping.Name
                            };

                            await _context.CategoryMappings.AddAsync(newCategoryMappingExpert);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                        else return false;
                    }
                    else return false;              
                }
                else return false;
            }
            else return false;
        }


        public async Task<bool> UpdateCategoryMappingActiveAsync(string idCategoryMapping, string accIdExpert)
        {
            if (!string.IsNullOrEmpty(idCategoryMapping) && !string.IsNullOrEmpty(accIdExpert))
            {
                var ExpertCurrent = await _context.Experts.Where(p => p.AcId == accIdExpert && p.IsActive).FirstOrDefaultAsync();
                var currentCategoryMapping = await _context.CategoryMappings.Where(p => p.Id == idCategoryMapping && p.IsActive == true).FirstOrDefaultAsync();
                if (currentCategoryMapping != null && ExpertCurrent != null)
                {
                    if (currentCategoryMapping.ExpertId == ExpertCurrent.Id)
                    {
                        if (currentCategoryMapping.IsActive == true)
                        {
                            currentCategoryMapping.IsActive = false;
                            _context.CategoryMappings.Update(currentCategoryMapping);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            currentCategoryMapping.IsActive = false;
                            _context.CategoryMappings.Update(currentCategoryMapping);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> UpdateCategoryMappingAsync(string idCategoryMapping, string accIdExpert, CreateCategoryMappingViewModel categoryMappingModel)
        {
            if (!string.IsNullOrEmpty(idCategoryMapping) && !string.IsNullOrEmpty(accIdExpert))
            { 
                var currentCategoryMapping = await _context.CategoryMappings.Where(p => p.Id == idCategoryMapping && p.IsActive == true).FirstOrDefaultAsync();
                var ExpertCurrent = await _context.Experts.Where(p => p.AcId == accIdExpert && p.IsActive).FirstOrDefaultAsync();
                if (currentCategoryMapping != null && ExpertCurrent != null)
                {
                    if(currentCategoryMapping.ExpertId == ExpertCurrent.Id)
                    {
                        var isChange = false;
                        if(categoryMappingModel.Price != null || categoryMappingModel.Price > 0) { 
                            currentCategoryMapping.Price = categoryMappingModel.Price;
                            isChange = true;
                        }
                        if(categoryMappingModel.ExperienceYear != null || categoryMappingModel.ExperienceYear > 0)
                        {
                            currentCategoryMapping.ExperienceYear = categoryMappingModel.ExperienceYear;
                            isChange = true;
                        }
                        if (!string.IsNullOrEmpty(categoryMappingModel.Introduction) && categoryMappingModel.Introduction != "string")
                        {
                            currentCategoryMapping.Introduction = categoryMappingModel.Introduction;
                            isChange = true;
                        }
                        if (!string.IsNullOrEmpty(categoryMappingModel.Description) && categoryMappingModel.Description != "string")
                        {
                            currentCategoryMapping.Description = categoryMappingModel.Description;
                            isChange = true;
                        }

                        if (isChange) currentCategoryMapping.IsConfirm = false;
                        _context.CategoryMappings.Update(currentCategoryMapping);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }else return false;
            }
            else return false;
        }

        public async Task<List<CategoryMappingViewModel>> GetAllCategoryMappingAsync()
        {
            var categorymappings = await _context.CategoryMappings.Where(p => p.IsActive && p.Expert.Ac.IsActive && p.IsConfirm).ToListAsync();
            return _mapper.Map<List<CategoryMappingViewModel>>(categorymappings);

        }
        public async Task<List<CategoryMapping>> GetAllCategoryMappingByCategoryAsync(string IdCategory)
        {
            if (!string.IsNullOrEmpty(IdCategory))
            {
                return await _context.CategoryMappings.Where(p => p.CategoryId == IdCategory && p.IsActive && p.Expert.IsActive && p.IsConfirm).ToListAsync();
            }
            else return null;
        }

        public async Task<bool> DeleteMappingCategoryAsync(string idCategoryMapping, string accIdExpert)
        {
            if (!string.IsNullOrEmpty(idCategoryMapping) && !string.IsNullOrEmpty(accIdExpert))
            {
                var ExpertCurrent = await _context.Experts.Where(p => p.AcId == accIdExpert && p.IsActive).FirstOrDefaultAsync();
                var currentCategoryMapping = await _context.CategoryMappings.Where(p => p.Id == idCategoryMapping && p.IsActive == true).FirstOrDefaultAsync();
                if (currentCategoryMapping != null && ExpertCurrent != null)
                {
                    if (currentCategoryMapping.ExpertId == ExpertCurrent.Id)
                    {            
                            _context.CategoryMappings.Remove(currentCategoryMapping);
                            await _context.SaveChangesAsync();
                            return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<CategoryMapping>> GetAllCategoryMappingUnConfirmAsync()
        {
            return await _context.CategoryMappings.Where(p => p.IsConfirm == false && p.IsActive == true).ToListAsync();
        }
    }
}
