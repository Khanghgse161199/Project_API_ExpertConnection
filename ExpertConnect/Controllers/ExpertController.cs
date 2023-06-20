using DatabaseConection.Entities;
using DataService.AccountService;
using DataService.AuthServices;
using DataService.CategoryMappingServices;
using DataService.ExpertServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewMode.Auth;
using ViewModel.CategoryMapping;
using ViewModel.Expert;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IExpertService _expertService;
        private readonly IAccountService _accountService;
        private readonly ICategoryMappingService _categoryMappingService;
        private readonly IAuthService _auth;
        
        public ExpertController(IExpertService expertService, IAccountService accountService, IAuthService auth, ICategoryMappingService categoryMappingService)
        {
            _expertService = expertService;
            _accountService = accountService;
            _categoryMappingService = categoryMappingService;
            _auth = auth;
        }

        [HttpPost("SignUpExpert")]

        public async Task<IActionResult> createExpertAsync(ExpertModel expertModel)
        {
            if (expertModel != null)
            {
                if (ModelState.IsValid)
                {
                    var accId = Guid.NewGuid().ToString();
                    bool isCreateAcount = await _accountService.CreateAccountAsync(expertModel.Username, expertModel.Password, "Expert", accId);
                    if (isCreateAcount)
                    {
                        bool isCreateExpert = await _expertService.CreateExpertAsync(accId, expertModel);
                        if (isCreateExpert)
                        {
                            return Ok("Create expert success");
                        }
                        else return NotFound("Error Occured"); ;
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
            } else return NoContent(); ;
        }

        [HttpPost("RegisterCategoryForExpert")]

        public async Task<IActionResult> RegisterCategoryForExpert(Guid IdCategory, CreateCategoryMappingViewModel categoryMappingModel)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                if (!string.IsNullOrEmpty(IdCategory.ToString()) && categoryMappingModel != null)
                {
                    if (ModelState.IsValid)
                    {
                        CheckTokenResultViewModel checkTokenResultViewModel = await _auth.checkTokenAsync(tokenInHeader);
                        if (checkTokenResultViewModel.RoleName == "Expert")
                        {
                            var IsRegister = await _categoryMappingService.RegisterCategoryForExpertAsync(IdCategory.ToString(), categoryMappingModel, checkTokenResultViewModel.accId);
                            if (IsRegister)
                            {
                                return Ok("Register Category Success");
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
                else return NotFound("Not Have Id To Find");
            }
            else return BadRequest();
        }

        [HttpGet("GetProfileExpert")]      
        public async Task<IActionResult> GetProfileExpert(string Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id))
            {
                var checkToken = await _auth.checkTokenAsync(tokenInHeader);
                if(checkToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var profileExpert = await _expertService.GetProfileExpert(Id);
                        if (profileExpert != null)
                        {
                            return Ok(profileExpert);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(x => x.Value.Errors)
                            .Where (y => y.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }else { return BadRequest(); }
            }
            else return BadRequest();
        }

        [HttpGet("GetAllCategoryMappingOfExpert")]

        public async Task<IActionResult> GetAllCategoryMappingOfExpert()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _auth.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Expert")
                    {
                        if (ModelState.IsValid)
                        {
                            var getCategoryMappings = await _expertService.GetCategoryMappingOfExpertAsync(checkToken.accId);                           
                            return Ok(getCategoryMappings);
                        }
                        else
                        {
                            var error = ModelState.Select(p => p.Value.Errors)
                                .Where(p => p.Count > 0)
                                .ToList();
                            return BadRequest(error);
                        }
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            else return BadRequest();
        }

        [HttpPut("UpdateProfileExpert")]
        public async Task<IActionResult> UpdateProfile(ExpertUpdateProfileModel expertUpdateProfileModel)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _auth.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Expert")
                {
                    if (ModelState.IsValid)
                    {
                        var profileExpert = await _expertService.UpdateProfileExpert(checkToken.accId, expertUpdateProfileModel);
                        if (profileExpert)
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
