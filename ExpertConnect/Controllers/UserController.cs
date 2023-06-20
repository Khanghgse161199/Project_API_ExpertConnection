using DatabaseConection.Entities;
using DataService.AccountService;
using DataService.AuthServices;
using DataService.EmployeeServices;
using DataService.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewMode.Auth;
using ViewModel.User;

namespace ExpertConnect.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _acc;
        private readonly IUserService _userService;
        private readonly IAuthService _auth;
        public UserController(IAccountService acc, IUserService userService, IAuthService auth)
        {
            _acc = acc;
            _userService = userService;
            _auth = auth;
        }

        //Sign-Up
        
        [HttpPost("SignUpUser")]
        public async Task<IActionResult> CreateUser(UserModel usMdel)
        {
            if (usMdel != null)
            {
                if (ModelState.IsValid)
                {
                    string accId = Guid.NewGuid().ToString();
                    bool accCreated = await _acc.CreateAccountAsync(usMdel.Username, usMdel.Password, "User", accId);
                    if (accCreated)
                    {
                        bool userCreated = await _userService.CreateUserAsync(accId, usMdel);
                        if (userCreated)
                        {
                            return Ok("Sign-Up User Success.");
                        }
                        else return NotFound("Error Occured");  
                    }
                    else return NotFound("Error Occured"); ;
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                    return BadRequest(errors);
                }
            }
            else return NoContent();
        }

        [HttpGet("GetProfileUser")]
        public async Task<IActionResult> GetProfile()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _auth.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        var profileExpert = await _userService.GetUserProfileAsync(checkToken.accId, checkToken.Username);
                        if (profileExpert != null)
                        {
                            return Ok(profileExpert);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else { return BadRequest(); }
            }
            else return BadRequest();
        }

        [HttpPut("UpdateProfileUser")]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileModel userUpdateProfileModel)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _auth.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdateprofileExpert = await _userService.UpdateProfileUser(checkToken.accId, userUpdateProfileModel);
                        if (isUpdateprofileExpert)
                        {
                            return Ok("Update Profile Success");
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else { return BadRequest(); }
            }
            else return BadRequest();
        }
    }
}
