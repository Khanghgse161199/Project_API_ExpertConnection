using DatabaseConection.Entities;
using DataService.AuthServices;
using DataService.CategoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewMode.Auth;
using ViewModel.Category;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthService _auth;
        public CategoryController(ICategoryService categoryService, IAuthService anthService)
        {
            _categoryService = categoryService;
            _auth = anthService;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CategoryModel ctMDel)
        {
            var headerCheckToken = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(headerCheckToken))
            {
                if (ModelState.IsValid)
                {
                    CheckTokenResultViewModel checkTokenResultViewModel = await _auth.checkTokenAsync(headerCheckToken);
                    if (checkTokenResultViewModel != null && checkTokenResultViewModel.RoleName == "Admin")
                    {
                        bool isCreate = await _categoryService.CreateCategoryAsync(ctMDel);
                        if (isCreate)
                        {
                            return Ok("Create Category successful");
                        }else return NotFound("Error Occured");
                    }
                    else return NotFound("Error Occured");
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .ToList();
                    return BadRequest(errors);
                }
            }
            else return NotFound("Error Occured");
        }

        [HttpGet("GetAllCategory")]

        public async Task<IActionResult> GetAllCategory()
        {
            var headerCheck = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(headerCheck))
            {
                var checkToken = await _auth.checkTokenAsync(headerCheck);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin" || checkToken.RoleName == "Expert" || checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        var ListCategory = await _categoryService.GetAllCategoryAsync();
                        return Ok(ListCategory);
                    }
                    else
                    {
                        var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                        return BadRequest(errors);
                    }
                }
                return BadRequest();
            }
            else return BadRequest();
        }

        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(Guid Id)
        {
            var headerCheck = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(headerCheck))
            {
                var checkToken = await _auth.checkTokenAsync(headerCheck);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin" || checkToken.RoleName == "Expert" || checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(Id.ToString()))
                        {
                            var categoryFinding = await _categoryService.GetCategoryByIdAsync(Id.ToString());
                            return Ok(categoryFinding);
                        }
                        else return BadRequest("Not Have Id To Find Category");
                    }
                    else
                    {
                        var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                        return BadRequest(errors);
                    }
                }
                return BadRequest();
            }
            else return BadRequest();
        }

        [HttpPut("UpdateCategory")]

        public async Task<IActionResult> UpdateCategory(Guid Id, CategoryModel tempCategoryModel)
        {
            var headerCheck = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(headerCheck))
            {
                var checkToken = await _auth.checkTokenAsync(headerCheck);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin") {
                    if (!string.IsNullOrEmpty(Id.ToString()) && tempCategoryModel != null)
                    {
                        if (ModelState.IsValid)
                        {
                            var check = await _categoryService.UpdateCategoryAsync(tempCategoryModel, Id.ToString());
                            if (check)
                            {
                                return Ok("Update CategorySuccess");
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
                    else return NotFound("Not Have Id To Find Category");
                }
                return BadRequest();
            }
            else return BadRequest();
        }

        [HttpPut("UpdateCategoryIsActive")]
        public async Task<IActionResult> UpdateCategoryIsActive(Guid Id)
        {
            var headerCheck = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(headerCheck))
            {
                var checkToken = await _auth.checkTokenAsync(headerCheck);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(Id.ToString()))
                        {
                            var IsUpdate = await _categoryService.UpdateToActiveAsync(Id.ToString());
                            if (IsUpdate)
                            {
                                return Ok("Update Category To UnActive Success");
                            }
                            else return BadRequest();
                            
                        }
                        else return BadRequest("Not Have Id To Find Category");
                    }
                    else
                    {
                        var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y => y.Count > 0)
                            .ToList();
                        return BadRequest(errors);
                    }
                }
                return BadRequest();
            }
            else return BadRequest();
        }    

        //[HttpDelete("DeleteCategory")]
        //public async Task<IActionResult> DeleteCategory(Guid Id)
        //{
        //    var headerCheck = Request.Headers["token"].ToString();
        //    if (!string.IsNullOrEmpty(headerCheck))
        //    {
        //        var checkToken = await _auth.checkTokenAsync(headerCheck);
        //        if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                if (!string.IsNullOrEmpty(Id.ToString()))
        //                {
        //                    var IsDelete = await _categoryService.DeleteCategoryByIdAsync(Id.ToString());
        //                    if (IsDelete)
        //                    {
        //                        return Ok("Delete Category Success");
        //                    }
        //                    else return BadRequest();
        //                }
        //                else return NotFound("Not Have Id To Find Category");
        //            }
        //            else
        //            {
        //                var errors = ModelState.Select(x => x.Value.Errors)
        //                    .Where(y => y.Count > 0)
        //                    .ToList();
        //                return BadRequest(errors);
        //            }
        //        }
        //        return BadRequest();
        //    }
        //    else return BadRequest();
        //}
    }
}
