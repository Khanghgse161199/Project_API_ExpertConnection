using DatabaseConection.Entities;
using DataService.AccountService;
using DataService.AuthServices;
using DataService.EmployeeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewMode.Auth;
using ViewMode.Employee;
using ViewModel.User;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _acc;
        private readonly IEmployeeService _employee;
        private readonly IAuthService _authService;
        public AuthController(IAccountService acc, IEmployeeService employee, IAuthService authService)
        {
            _acc = acc;
            _employee = employee;
            _authService = authService;
        }

        [HttpPost("LoginEmployee")]
        public async Task<IActionResult> LoginEmployee(LoginViewModel login)
        {

            if (ModelState.IsValid)
            {
                var token = await _authService.LoginEmployeeAsync(login);
                if (token != null)
                {
                    return Ok(token);
                }
                else return NotFound("User Name or PassWord is incoreect");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                return BadRequest(errors);
            }
        }

        [HttpPost("LoginExpert")]
        public async Task<IActionResult> LoginExpert(LoginViewModel login)
        {

            if (ModelState.IsValid)
            {
                var token = await _authService.LoginExpertAsync(login);
                if (token != null)
                {
                    return Ok(token);
                }
                else return NotFound("User Name or PassWord is incoreect");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                return BadRequest(errors);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginViewModel login)
        {

            if (ModelState.IsValid)
            {
                var token = await _authService.LoginUserAsync(login);
                if (token != null)
                {
                    return Ok(token);
                }
                else return NotFound("User Name or PassWord is incoreect");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                return BadRequest(errors);
            }
        }



    }
}
