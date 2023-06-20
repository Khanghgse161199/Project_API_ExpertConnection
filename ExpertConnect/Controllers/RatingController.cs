using DataService.AuthServices;
using DataService.RatingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Rating;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IAuthService _authService;
        public RatingController(IRatingService ratingService, IAuthService authService)
        {
            _ratingService = ratingService;
            _authService = authService;
        } 

        [HttpGet("GetAllRating")]
        public async Task<IActionResult> GetAllRating()
        {

            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _ratingService.GetAllRatingAsync();
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count() > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return BadRequest();
        }

        [HttpGet("GetRatingById")]
        public async Task<IActionResult> GetRatingById(Guid Id)
        {

            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _ratingService.GetRatingByIdAsync(Id.ToString());
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count() > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return BadRequest();
        }

        [HttpGet("GetRatingByCategoryMapping")]
        public async Task<IActionResult> GetRatingByCategoryMapping(Guid Id)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _ratingService.GetRatingByCategoryMappingAsync(Id.ToString());
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count() > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return BadRequest();
        }

        [HttpGet("GetRatingByUserId")]
        public async Task<IActionResult> GetRatingByUserId()
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _ratingService.GetRatingByCategoryByUserAsync(checkToken.accId);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count() > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return BadRequest();
        }

        [HttpGet("GetRatingByExpertId")]
        public async Task<IActionResult> GetRatingByExpertId(string Id)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _ratingService.GetRatingByCategoryByExpertrAsync(Id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count() > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return BadRequest();
        }

        [HttpPut("UpdateRating")]
        public async Task<IActionResult> UpdateRating(Guid Id,RatingUpdateModel ratingUpdateModel)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _ratingService.UpdateRatingAsync(Id.ToString(), checkToken.accId, ratingUpdateModel);
                        if (isUpdate)
                        {
                            return Ok("Update Rating Success");
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count() > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return BadRequest();
        }

        //[HttpPut("DeleteRating")]
        //public async Task<IActionResult> DeleteRating(string Id)
        //{
        //    string tokenInHeader = Request.Headers["token"].ToString();
        //    if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
        //    {
        //        var checkToken = await _authService.checkTokenAsync(tokenInHeader);
        //        if (checkToken.RoleName == "User")
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var isUpdate = await _ratingService.DeleteRatingAsync(Id.ToString(), checkToken.accId);
        //                if (isUpdate)
        //                {
        //                    return Ok("Delete Rating Success");
        //                }
        //                else return BadRequest();
        //            }
        //            else
        //            {
        //                var error = ModelState.Select(p => p.Value.Errors)
        //                    .Where(p => p.Count() > 0)
        //                    .ToList();
        //                return BadRequest(error);
        //            }
        //        }
        //        else return NotFound();
        //    }
        //    else return BadRequest();
        //}

    }
}
