using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<bool> CreateEmployeeAsync(string accId, string fullName);
        Task<IEnumerable<Expert>> GetAllExpertAsync();
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<bool> ConfirmRegisterCategoryMappingExpertAsync(string idCategoryMapping);
        Task<bool> ConfirmExpertUpdateCategoryMappingAsync(string idCategoryMapping);
        Task<bool> ConfirmExpertAsync(string expertId);
        Task<bool> ConfirmUserAsync(string idUser);
    }
    public class EmployeeService:IEmployeeService
    {
        private readonly ExpertConectionContext _context;
        public EmployeeService(ExpertConectionContext context)
        {
            _context = context;
        }

        public async Task<bool> ConfirmRegisterCategoryMappingExpertAsync(string idCategoryMapping)
        {
            if (!string.IsNullOrEmpty(idCategoryMapping))
            {
                var currentCategoryMapping = await _context.CategoryMappings.Where(p => p.Id == idCategoryMapping && p.IsActive == true).Include("Category").Include("Expert").FirstOrDefaultAsync();
                if (currentCategoryMapping != null && currentCategoryMapping.IsConfirm == false)
                {
                    currentCategoryMapping.IsConfirm = true;
                    _context.CategoryMappings.Update(currentCategoryMapping);
                    await _context.SaveChangesAsync();
                    return true;
                }else return false;
            }
            else return false;
        }

        public async Task<bool> ConfirmExpertUpdateCategoryMappingAsync(string idCategoryMapping)
        {
            if (!string.IsNullOrEmpty(idCategoryMapping)){
                var currentCategoryMapping = await _context.CategoryMappings.Where(p => p.Id == idCategoryMapping && p.IsActive == true && p.IsConfirm == false).FirstOrDefaultAsync();
                if(currentCategoryMapping != null)
                {
                    currentCategoryMapping.IsConfirm = true;
                    _context.CategoryMappings.Update(currentCategoryMapping);
                    await _context.SaveChangesAsync();
                    return true;
                }else return false;
            } else return false;
        }

        public async Task<bool> CreateEmployeeAsync(string accId,string fullName)
        {
            if(!string.IsNullOrEmpty(accId) && !string.IsNullOrEmpty(fullName))
            {
                try
                {
                    Employee newEmployee = new Employee()
                    {
                        AcId = accId,
                        Fullname = fullName,
                        Id = Guid.NewGuid().ToString(),
                        IsActive = true,
                    };
                    await _context.Employees.AddAsync(newEmployee);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }else { return false; }
        }

        public async Task<bool> ConfirmExpertAsync(string expertId)
        {
            if (!string.IsNullOrEmpty(expertId))
            {
                var expertCurrent = await _context.Experts.Where(p => p.Id == expertId).FirstOrDefaultAsync();
                if (expertCurrent != null)
                {
                    expertCurrent.ExpertConfirm = true;
                    _context.Experts.Update(expertCurrent);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else { return false; }
        }

        public async Task<bool> ConfirmUserAsync(string idUser)
        {
            if (!string.IsNullOrEmpty(idUser))
            {
                var userCurrent = await _context.Users.Where(p => p.Id == idUser).FirstOrDefaultAsync();
                if (userCurrent != null)
                {
                    userCurrent.UserConfirm = true;
                    _context.Users.Update(userCurrent);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else { return false; }
        }

        public async Task<IEnumerable<Expert>> GetAllExpertAsync()
        {
            var listTemp = await _context.Experts.ToListAsync();
            return listTemp;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            var listTemp = await _context.Users.ToListAsync();
            return listTemp;
        }
    }
}
