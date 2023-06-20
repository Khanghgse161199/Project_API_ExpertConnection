using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Category;

namespace DataService.CategoryServices
{
    public interface ICategoryService
    {
        Task<bool> CreateCategoryAsync(CategoryModel categoryModel);
        Task<bool> UpdateCategoryAsync(CategoryModel tempCategoryModel, string categoryIdCurrent);
        Task<Category> GetCategoryByIdAsync(string categoryIdCurrent);
        Task<bool> UpdateToActiveAsync(string categoryIdCurrent);


        Task<List<Category>> GetAllCategoryAsync();
    }
    public class CategoryService : ICategoryService
    {
        private readonly ExpertConectionContext _context;

        public CategoryService(ExpertConectionContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCategoryAsync(CategoryModel categoryModel)
        {
            if (categoryModel != null)
            {
                try
            {            
                    var newCategory = new Category()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = categoryModel.Name,
                        IsActive = true,
                    };
                    await _context.Categories.AddAsync(newCategory);
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

        public async Task<bool> UpdateCategoryAsync(CategoryModel tempCategoryModel, string categoryIdCurrent)
        {
            if (!string.IsNullOrEmpty(categoryIdCurrent))
            {
                var currentCategory = await _context.Categories.Where(p => p.Id == categoryIdCurrent && p.IsActive == true).FirstOrDefaultAsync();
                if (currentCategory != null)
                {
                    if (!string.IsNullOrEmpty(tempCategoryModel.Name))
                    {
                        currentCategory.Name = tempCategoryModel.Name;
                        _context.Categories.Update(currentCategory);
                        await _context.SaveChangesAsync();
                        return true;
                    }else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<Category> GetCategoryByIdAsync(string categoryIdCurrent)
        {
            if (!string.IsNullOrEmpty(categoryIdCurrent))
            {
                Category currentCategory = await _context.Categories.Where(p => p.Id == categoryIdCurrent && p.IsActive == true).FirstOrDefaultAsync();
                if (currentCategory != null)
                {
                    return currentCategory;
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<bool> UpdateToActiveAsync(string categoryIdCurrent)
        {
            if (!string.IsNullOrEmpty(categoryIdCurrent))
            {
                Category currentCategory = await _context.Categories.Where(p => p.Id == categoryIdCurrent && p.IsActive == true).FirstOrDefaultAsync();
                if (currentCategory != null && currentCategory.CategoryMappings.Count() == 0)
                {
                    if (currentCategory.CategoryMappings == null )
                    {
                        if(currentCategory.IsActive = true)
                        {
                            currentCategory.IsActive = false;
                            _context.Categories.Update(currentCategory);
                            await _context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            currentCategory.IsActive = true;
                            _context.Categories.Update(currentCategory);
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
    }
}
