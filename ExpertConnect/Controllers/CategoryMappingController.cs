using DatabaseConection.Entities;
using DataService.AuthServices;
using DataService.CategoryMappingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewMode.Auth;
using ViewModel.CategoryMapping;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryMappingController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICategoryMappingService _categoryMappingService;
        public CategoryMappingController(ICategoryMappingService categoryMappingService, IAuthService authService)
        {
            _authService = authService;
            _categoryMappingService = categoryMappingService;
        }
      
        [HttpGet("GetAllCategoryMapping")]

        public async Task<IActionResult> getAllCategoryMapping()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "User" || checkToken.RoleName == "Expert" || checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                    {

                        if (ModelState.IsValid)
                        {
                            var listCategoryMapping = await _categoryMappingService.GetAllCategoryMappingAsync();
                            return Ok(listCategoryMapping);
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
                }else { return BadRequest(); }
            }else return BadRequest();
        }


        [HttpGet("GetAllCategoryMappingByCategory")]

        public async Task<IActionResult> GetAllCategoryMappingByCategoryId(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "User" || checkToken.RoleName == "Expert" || checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                    {
                        if (!string.IsNullOrEmpty(Id.ToString()))
                        {
                            if (ModelState.IsValid)
                            {
                                var listCategoryMapping = await _categoryMappingService.GetAllCategoryMappingByCategoryAsync(Id.ToString());
                                return Ok(listCategoryMapping);
                            }
                            else
                            {
                                var error = ModelState.Select(p => p.Value.Errors)
                                    .Where(p => p.Count > 0)
                                    .ToList();
                                return BadRequest(error);
                            }
                        }
                        else return NotFound("Not Have Id Category-Mapping To Find");
                    }
                    else return BadRequest();
                }
                else { return BadRequest(); }
            }
            else return BadRequest();
        }

        [HttpGet("GetAllCategoryMappingUnConfirm")]

        public async Task<IActionResult> GetAllCategoryMappingUnConfirm()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                if(ModelState.IsValid)
                {
                    var tokenDb = await _authService.checkTokenAsync(tokenInHeader);
                    if (tokenDb != null)
                    {
                        if (tokenDb.RoleName == "Admin" || tokenDb.RoleName == "Employee")
                        {
                            var listCategoryMappingUnConfirm = await _categoryMappingService.GetAllCategoryMappingUnConfirmAsync();
                            return Ok(listCategoryMappingUnConfirm);
                        }
                        else return BadRequest();
                    }
                    else return NotFound();
                }
                else
                {
                    var error = ModelState.Select(p => p.Value.Errors)
                        .Where(p => p.Count > 0)
                        .ToList();
                    return BadRequest(error);
                }
            }else return BadRequest();
        }

        [HttpPut("UpdateCategoryMapping")]

        public async Task<IActionResult> UpdateCategoryMapping(Guid Id, CreateCategoryMappingViewModel categoryMappingModel)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Expert")
                    {
                        if (!string.IsNullOrEmpty(Id.ToString()))
                        {

                            if (ModelState.IsValid)
                            {
                                var IsUpdate = await _categoryMappingService.UpdateCategoryMappingAsync(Id.ToString(), checkToken.accId, categoryMappingModel);
                                if (IsUpdate)
                                {
                                    return Ok("Update Category-Mapping Success");
                                }
                                else return BadRequest();
                            }
                            else
                            {
                                var error = ModelState.Select(p => p.Value.Errors)
                                    .Where(p => p.Count > 0)
                                    .ToList();
                                return BadRequest(error);
                            }
                        }
                        else return NotFound("Not Have Id To Finding");
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            else return BadRequest();
        }


        [HttpPut("UpdateCategoryMappingIsActive")]

        public async Task<IActionResult> UpdateCategoryMappingIsActive(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Expert")
                    {
                        if (!string.IsNullOrEmpty(Id.ToString()))
                        {
                            if (ModelState.IsValid)
                            {
                                var IsDelete = await _categoryMappingService.UpdateCategoryMappingActiveAsync(Id.ToString(), checkToken.accId);
                                if (IsDelete)
                                {
                                    return Ok("Update IsActive Category-Mapping Success");
                                }
                                else return BadRequest();
                            }
                            else
                            {
                                var error = ModelState.Select(p => p.Value.Errors)
                                    .Where(p => p.Count > 0)
                                    .ToList();
                                return BadRequest(error);
                            }
                        }
                        else return NotFound("Not Have Id To Find Category-Mapping");
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            else return BadRequest();
        }

        [HttpDelete("DeleteCategoryMapping")]

        public async Task<IActionResult> DeleteCategoryMapping(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Expert")
                    {

                        if (!string.IsNullOrEmpty(Id.ToString()))
                        {
                            if (ModelState.IsValid)
                            {
                                var IsDelete = await _categoryMappingService.DeleteMappingCategoryAsync(Id.ToString(), checkToken.accId);
                                if (IsDelete)
                                {
                                    return Ok("Delete Category-Mapping Success");
                                }
                                else return BadRequest();
                            }
                            else
                            {
                                var error = ModelState.Select(p => p.Value.Errors)
                                    .Where(p => p.Count > 0)
                                    .ToList();
                                return BadRequest(error);
                            }
                        }
                        else return NotFound("Not Have Id Category-Mapping To Find");
                    }
                    else return BadRequest();
                }
                else { return BadRequest(); }
            }
            else return BadRequest();
        }

    }
}
