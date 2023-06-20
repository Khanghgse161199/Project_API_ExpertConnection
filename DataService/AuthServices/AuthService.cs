using DatabaseConection.Entities;
using DataService.HashService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMode.Auth;

namespace DataService.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResultViewModel> LoginEmployeeAsync(LoginViewModel loginInfo);
        Task<LoginResultViewModel> LoginUserAsync(LoginViewModel loginInfo);
        Task<LoginResultViewModel> LoginExpertAsync(LoginViewModel loginInfo);

        Task<CheckTokenResultViewModel> checkTokenAsync(string tokenCheck);
    }
    public class AuthService: IAuthService
    {
        private readonly ExpertConectionContext _context;
        private readonly IHashService _hashService;
        private readonly string serectKey = "asknf,jasf2241824y124";
        public AuthService(ExpertConectionContext context, IHashService hash)
        {
            _context = context;
            _hashService = hash;
        }

        public async Task<LoginResultViewModel> LoginEmployeeAsync( LoginViewModel loginInfo)
        {
            
            var account = await _context.Accounts.Where(p => p.Username == loginInfo.Username && p.Password == _hashService.SHA256(loginInfo.Password + serectKey) && p.IsActive == true).FirstOrDefaultAsync();  
            if (account != null) {
                var role = await _context.Roles.Where(p => p.IsActive && p.Id == account.RoleId).FirstOrDefaultAsync();
                if (role != null && role.Name == "Employee" || role != null && role.Name == "Admin")
                {
                    string tokenAccess = Guid.NewGuid().ToString();
                    var tokendb = await _context.Tokens.Where(p => p.AccId == account.Id).FirstOrDefaultAsync(); 
                    if(tokendb != null)
                    {
                        tokendb.AccessToken = tokenAccess;
                        tokendb.IsActive = true;
                        tokendb.CreateDate = DateTime.Now;
                        _context.Tokens.Update(tokendb);
                        await _context.SaveChangesAsync();
                        return new LoginResultViewModel()
                        {
                            AccessToken = tokenAccess,
                        };
                    }
                    else
                    {
                        Token newToken = new Token()
                        {
                            AccessToken = tokenAccess,
                            AccId = account.Id,
                            Id = Guid.NewGuid().ToString(),
                            CreateDate = DateTime.Now,
                            IsActive = true,
                        };

                        await _context.Tokens.AddAsync(newToken);
                        await _context.SaveChangesAsync();
                        return new LoginResultViewModel()
                        {
                            AccessToken = tokenAccess,
                        };        
                }                  
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<LoginResultViewModel> LoginUserAsync(LoginViewModel loginInfo)
        {

            var account = await _context.Accounts.Where(p => p.Username == loginInfo.Username && p.Password == _hashService.SHA256(loginInfo.Password + serectKey) && p.IsActive == true).FirstOrDefaultAsync();
            if (account != null)
            {
                var role = await _context.Roles.Where(p => p.IsActive && p.Id == account.RoleId).FirstOrDefaultAsync();
                if (role != null && role.Name == "User")
                {
                    string tokenAccess = Guid.NewGuid().ToString();
                    var tokendb = await _context.Tokens.Where(p => p.AccId == account.Id).FirstOrDefaultAsync();
                    if (tokendb != null)
                    {
                        tokendb.AccessToken = tokenAccess;
                        tokendb.IsActive = true;
                        tokendb.CreateDate = DateTime.Now;
                        _context.Tokens.Update(tokendb);
                        await _context.SaveChangesAsync();
                        return new LoginResultViewModel()
                        {
                            AccessToken = tokenAccess,
                        };
                    }
                    else
                    {
                        Token newToken = new Token()
                        {
                            AccessToken = tokenAccess,
                            AccId = account.Id,
                            Id = Guid.NewGuid().ToString(),
                            CreateDate = DateTime.Now,
                            IsActive = true,
                        };

                        await _context.Tokens.AddAsync(newToken);
                        await _context.SaveChangesAsync();
                        return new LoginResultViewModel()
                        {
                            AccessToken = tokenAccess,
                        };
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<LoginResultViewModel> LoginExpertAsync(LoginViewModel loginInfo)
        {

            var account = await _context.Accounts.Where(p => p.Username == loginInfo.Username && p.Password == _hashService.SHA256(loginInfo.Password + serectKey) && p.IsActive == true).FirstOrDefaultAsync();
            if (account != null)
            {
                var role = await _context.Roles.Where(p => p.IsActive && p.Id == account.RoleId).FirstOrDefaultAsync();
                if (role != null && role.Name == "Expert")
                {
                    string tokenAccess = Guid.NewGuid().ToString();
                    var tokendb = await _context.Tokens.Where(p => p.AccId == account.Id).FirstOrDefaultAsync();
                    if (tokendb != null)
                    {
                        tokendb.AccessToken = tokenAccess;
                        tokendb.IsActive = true;
                        tokendb.CreateDate = DateTime.Now;
                        _context.Tokens.Update(tokendb);
                        await _context.SaveChangesAsync();
                        return new LoginResultViewModel()
                        {
                            AccessToken = tokenAccess,
                        };
                    }
                    else
                    {
                        Token newToken = new Token()
                        {
                            AccessToken = tokenAccess,
                            AccId = account.Id,
                            Id = Guid.NewGuid().ToString(),
                            CreateDate = DateTime.Now,
                            IsActive = true,
                        };

                        await _context.Tokens.AddAsync(newToken);
                        await _context.SaveChangesAsync();
                        return new LoginResultViewModel()
                        {
                            AccessToken = tokenAccess,
                        };
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<CheckTokenResultViewModel> checkTokenAsync(string tokenCheck)
        {
            var tokenDb = await _context.Tokens.Where(p => p.IsActive && p.AccessToken == tokenCheck && ((DateTime.Now.Day - p.CreateDate.Day) <= 2)).Include("Acc").FirstOrDefaultAsync();
            if (tokenDb != null)
            {             
                var role = await _context.Roles.Where(p => p.Id == tokenDb.Acc.RoleId).FirstOrDefaultAsync();
                return new CheckTokenResultViewModel()
                {
                    accId = tokenDb.Acc.Id,
                    RoleId = tokenDb.Acc.RoleId,
                    RoleName = role.Name,
                    Username = tokenDb.Acc.Username,
                };
            }
            return null;
        }
    }
}
