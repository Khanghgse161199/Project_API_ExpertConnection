using DatabaseConection.Entities;
using DataService.HashService;

using Microsoft.EntityFrameworkCore;
using ViewMode.Auth;
using ViewModel.Expert;

namespace DataService.AccountService
{
    public interface IAccountService
    {

        Task<bool> CreateAccountAsync(string username, string password, string role, string acId);

    }
    public class AccountService : IAccountService
    {
        private readonly ExpertConectionContext _context;
        private readonly IHashService _hashcode;
        private readonly string serectKey = "asknf,jasf2241824y124";
        public AccountService(ExpertConectionContext context, IHashService hash)
        {
            _context = new ExpertConectionContext();
            _hashcode = hash;
        }

      

        public async Task<bool> CreateAccountAsync(string username, string password, string role,string acId)
        {
            if (!string.IsNullOrEmpty(acId) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(role) && username != "string" && password != "string")
            {
                var accDb = await _context.Accounts.Where(p => p.Username == username && p.Password == password && p.IsActive).FirstOrDefaultAsync();
                if (accDb == null)
                {
                    var roleDb = await _context.Roles.Where(p => p.IsActive && p.Name == role).FirstOrDefaultAsync();
                    if (roleDb != null)
                    {
                        Account newAccount = new Account
                        {
                            Id = acId,
                            Username = username,
                            Password = _hashcode.SHA256(password + serectKey),
                            IsActive = true,
                            RoleId = roleDb.Id,
                        };
                        await _context.AddAsync(newAccount);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
    }
}
