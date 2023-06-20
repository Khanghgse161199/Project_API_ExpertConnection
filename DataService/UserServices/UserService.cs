using DatabaseConection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModel.User;

namespace DataService.UserServices
{
    public interface IUserService
    {
       Task<bool> CreateUserAsync(string accId, UserModel userModel);

       Task<List<CategoryMapping>> GetAllCategoryMapping();
       Task<UserProfileModel> GetUserProfileAsync(string idUser, string username);
       Task<bool> UpdateProfileUser(string idUser, UserUpdateProfileModel userUpdateProfileModel);
    }
    public class UserService : IUserService
    {
        private readonly ExpertConectionContext _context;

        public UserService(ExpertConectionContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateUserAsync(string accId, UserModel userModel)
        {
            if (!string.IsNullOrEmpty(accId) && userModel != null)
            {
                try
                {
                    User newUser = new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        AcountId = accId,
                        FullName = userModel.FullName,
                        Birthday = ConvertToDateTime(userModel.Birthday),
                        Address = userModel.Address,
                        Introduction = userModel.Introduction,
                        PhoneNumber = userModel.PhoneNumber,
                        Email = userModel.Email,
                        EmailActivated = true,
                        UserConfirm = false,
                        IsActive = true,
                    };
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else return false;
        }     

        public async Task<UserProfileModel> GetUserProfileAsync(string accidUser, string username)
        {
            if (!string.IsNullOrEmpty(accidUser))
            {
                var currentUser = await _context.Users.Where(p => p.AcountId == accidUser && p.IsActive).FirstOrDefaultAsync();
                if (currentUser != null)
                {
                    var profileCurrentUser = new UserProfileModel() { 
                        Id = currentUser.Id,
                        Username = username,
                        FullName = currentUser.FullName,
                        Birthday = currentUser.Birthday,
                        Address = currentUser.Address,
                        Introduction = currentUser.Introduction,
                        PhoneNumber = currentUser.PhoneNumber,
                        Email = currentUser.Email,
                        EmailActivated = currentUser.EmailActivated,
                        UserConfirm = currentUser.UserConfirm,
                        IsActive = currentUser.IsActive,
                    };
                    return profileCurrentUser;
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateProfileUser(string accidUser, UserUpdateProfileModel userUpdateProfileModel)
        {
            if (!string.IsNullOrEmpty(accidUser))
            {            
                    var currentUser = await _context.Users.Where(p => p.AcountId == accidUser).FirstOrDefaultAsync();
                        if (currentUser != null)
                        {
                            if (!string.IsNullOrEmpty(userUpdateProfileModel.FullName) && userUpdateProfileModel.FullName != "string")
                            {
                                currentUser.FullName = userUpdateProfileModel.FullName;
                            }
                            if (!string.IsNullOrEmpty(userUpdateProfileModel.Birthday) && userUpdateProfileModel.Birthday != "string")
                            {
                                currentUser.Birthday = ConvertToDateTime(userUpdateProfileModel.Birthday);
                            }
                            if (!string.IsNullOrEmpty(userUpdateProfileModel.Introduction) && userUpdateProfileModel.Introduction != "string")
                            {
                                currentUser.Introduction = userUpdateProfileModel.Introduction;
                            }
                            if (!string.IsNullOrEmpty(userUpdateProfileModel.Address) && userUpdateProfileModel.Address != "string")
                            {
                                currentUser.Address = userUpdateProfileModel.Address;
                            }
                            if (!string.IsNullOrEmpty(userUpdateProfileModel.PhoneNumber) && userUpdateProfileModel.PhoneNumber != "string")
                            {
                                if (checkPhoneExist(userUpdateProfileModel.PhoneNumber))
                                {
                                    currentUser.PhoneNumber = userUpdateProfileModel.PhoneNumber;                                     
                                }else return false;
                            }
                            if (!string.IsNullOrEmpty(userUpdateProfileModel.Email) && userUpdateProfileModel.Email != "string")
                            {
                                if (checkEmailExist(userUpdateProfileModel.Email))
                                {
                                    currentUser.Email = userUpdateProfileModel.Email;
                                }
                                else return false;
                            }
                            _context.Users.Update(currentUser);
                            await _context.SaveChangesAsync();
                            return true;
                            }
                       
                    else return false;
             
            }
            else return false;
        }

        public async Task<List<CategoryMapping>> GetAllCategoryMapping()
        {
            if (_context.CategoryMappings.Count() > 0)
            {
                return await _context.CategoryMappings.ToListAsync();
            }
            else return null;
        }

        public async Task<List<CategoryMapping>> GetCategoryMappingById(string idCategory)
        {
            if (!string.IsNullOrEmpty(idCategory))
            {
                return await _context.CategoryMappings.Where(p => p.CategoryId == idCategory && p.IsActive).ToListAsync();
            }
            else return null;
        }

        public DateTime ConvertToDateTime(string dateString)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString, "d/M/yyyy", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            throw new ArgumentException("Wrong Fortmat d/M/yyyy");
        }

        private bool checkEmailExist(string email)
        {
            bool check = IsEmail(email);
            if (check)
            {
                foreach (var item in _context.Users)
                {
                    if (item.Email == email)
                    {
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        private bool IsEmail(string inputString)
        {
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Match match = Regex.Match(inputString, pattern);
            return match.Success;
        }

        private bool checkPhoneExist(string phone)
        {
            bool check = IsPhoneNumber(phone);
            if (check)
            {
                foreach (var item in _context.Users)
                {
                    if (item.PhoneNumber == phone)
                    {
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        private bool IsPhoneNumber(string inputString)
        {
            string pattern = @"^\+?[0-9]{1,3}-?[0-9]{1,4}-?[0-9]{4,10}$";
            Match match = Regex.Match(inputString, pattern);
            return match.Success;
        }
    }
}
