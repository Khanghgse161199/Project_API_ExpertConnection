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
    public class EmployeeController : ControllerBase
    {
        private readonly IAccountService _acc;
        private readonly IEmployeeService _employee;
        private readonly IAuthService _authService;
        public EmployeeController(IAccountService acc, IEmployeeService employee, IAuthService authService)
        {
            _acc = acc;
            _employee = employee;
            _authService = authService;
        }

        [HttpPost("SignUpEmployee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel emInfom)
        {
            var headerToken = Request.Headers["token"].ToString();
            if (headerToken != null && headerToken != string.Empty)
            {
                if (ModelState.IsValid)
                {
                    // check xem co phai admin hay khong ?
                    CheckTokenResultViewModel TokenCheck = await _authService.checkTokenAsync(headerToken);
                    if (TokenCheck != null && TokenCheck.RoleName == "Admin")
                    {
                        // tao accId cho account va employee ben duoi
                        string accId = Guid.NewGuid().ToString();

                        // tao mot account voi role la Emplyee
                        bool accCreated = await _acc.CreateAccountAsync(emInfom.Username, emInfom.Password, "Employee", accId);
                        if (accCreated)
                        {
                            // tao mot Employee (lay accId tao ben tren => trung id)
                            bool empCreated = await _employee.CreateEmployeeAsync(accId, emInfom.Fullname);
                            if (empCreated)
                            {
                                return Ok("Create employee successful");
                            }
                            else
                            {
                                return NotFound("Error Occured");
                            }
                        }
                        else return NotFound("Error Occured");
                    }
                    else return NotFound();

                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
                    return BadRequest(errors);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllExpert")]

        public async Task<IActionResult> GetAllExpertAsync()
        {
            var TokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(TokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    CheckTokenResultViewModel checkToken = await _authService.checkTokenAsync(TokenInHeader);
                    if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Employee")
                    {
                        var listAllExpert = await _employee.GetAllExpertAsync();
                        if (listAllExpert != null || listAllExpert.Count() > 0)
                        {
                            return Ok(listAllExpert);
                        }
                        else return NotFound();
                    }
                    else return BadRequest();
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                    return BadRequest(errors);
                }
            }
            else return BadRequest();
        }

        [HttpGet("GetAllUser")]

        public async Task<IActionResult> GetAllUserAsync()
        {
            var TokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(TokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    CheckTokenResultViewModel checkToken = await _authService.checkTokenAsync(TokenInHeader);
                    if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Employee")
                    {
                        var listAllExpert = await _employee.GetAllUserAsync();
                        if (listAllExpert != null || listAllExpert.Count() > 0)
                        {
                            return Ok(listAllExpert);
                        }
                        else return NotFound();
                    }
                    else return BadRequest();
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                    return BadRequest(errors);
                }
            }
            else return BadRequest();
        }

        [HttpPut("ConfirmExpert")]
        public async Task<IActionResult> ConfirmExpert(Guid id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    CheckTokenResultViewModel checkTokenResultViewModel = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkTokenResultViewModel.RoleName == "Employee" || checkTokenResultViewModel.RoleName == "Admin")
                    {
                        bool isConfrim = await _employee.ConfirmExpertAsync(id.ToString());

                        if (isConfrim)
                        {
                            return Ok("Confirm Expert is success");
                        }
                        else return BadRequest();
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
            else return BadRequest();
        }

        [HttpPut("ConfirmUser")]
        public async Task<IActionResult> ConfirmUser(Guid id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    CheckTokenResultViewModel checkTokenResultViewModel = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkTokenResultViewModel.RoleName == "Employee" || checkTokenResultViewModel.RoleName == "Admin")
                    {
                        bool isConfrim = await _employee.ConfirmUserAsync(id.ToString());

                        if (isConfrim)
                        {
                            return Ok("Confirm User is success");
                        }
                        else return BadRequest();
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
            else return BadRequest();
        }

        [HttpPut("ConfirmExpertRegisterCategoryMapping")]

        public async Task<IActionResult> ConfirmExpertRegisterCategory(Guid IdCategoryMapping)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if(checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                {
                    if (!string.IsNullOrEmpty(IdCategoryMapping.ToString()))
                    {
                        if (ModelState.IsValid)
                        {
                            bool IsConfirm = await _employee.ConfirmRegisterCategoryMappingExpertAsync(IdCategoryMapping.ToString());
                            if (IsConfirm)
                            {
                                return Ok("Confirm Cattegroy Mapping Success");
                            }
                            else return BadRequest();
                        }
                        else
                        {
                            var errors = ModelState.Select(x => x.Value.Errors)
                                .Where(y => y.Count > 0)
                                .ToList();
                            return BadRequest(errors);
                        }
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            else return BadRequest();  
        }

        [HttpPut("ConfirmExpertUpdateCategoryMapping")]

        public async Task<IActionResult> ConfirmUpdateCategoryMapping(Guid IdCategoryMapping)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee")
                {
                    if (!string.IsNullOrEmpty(IdCategoryMapping.ToString()))
                    {
                        if (ModelState.IsValid)
                        {
                            bool IsConfirm = await _employee.ConfirmExpertUpdateCategoryMappingAsync(IdCategoryMapping.ToString());
                            if (IsConfirm)
                            {
                                return Ok("Confirm Update Cattegroy-Mapping Success");
                            }
                            else return BadRequest();
                        }
                        else
                        {
                            var errors = ModelState.Select(x => x.Value.Errors)
                                .Where(y => y.Count > 0)
                                .ToList();
                            return BadRequest(errors);
                        }
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            else return BadRequest();
        }
    }
}
