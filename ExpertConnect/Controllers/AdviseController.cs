using DataService.AdviseServices;
using DataService.AuthServices;
using DataService.RatingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Rating;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdviseController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdviseService _AdviseService;
        private readonly IRatingService _ratingService;
        public AdviseController(IAdviseService AdviseService, IAuthService authService, IRatingService ratingService)
        {
            _authService = authService;
            _AdviseService = AdviseService;
            _ratingService = ratingService;
        }

        [HttpPost("CreateAdvise")]

        public async Task<IActionResult> CreateAdvise(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(Id.ToString()) && !string.IsNullOrEmpty(tokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "User")
                        {
                            var IsCreate = await _AdviseService.CreateAdviseAsync(Id.ToString(),checkToken.accId);
                            if(IsCreate)
                            {
                                return Ok("Create Advise Success");
                            }
                            else
                            {
                                return BadRequest("Error Occur");
                            }
                        }
                        else return BadRequest();
                    }
                    else return NotFound();
                }
                else { 
                    var error = ModelState.Select(p => p.Value.Errors)
                        .Where(p => p.Count > 0)
                        .ToList();
                    return BadRequest(error);
                }
            }
            else return BadRequest();
        }

        [HttpPost("RatingAdvise")]
        public async Task<IActionResult> RatingAdvise(CreateRatingViewModel createRatingViewModel)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "User")
                {
                    if (ModelState.IsValid)
                    {
                        var isCrating = await _ratingService.RatingAdviseAsync(createRatingViewModel.IdAdvise, checkToken.accId, createRatingViewModel.RatingPoint, createRatingViewModel.Comment);
                        if (isCrating)
                        {
                            return Ok("Rating Success");
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

        [HttpGet("GetAllAdviseConfrimOfUSer")]
        public async Task<IActionResult> GetAllAdviseConfrimOfUSer()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "User")
                        {
                            var IsGet = await _AdviseService.GetAllAdviseConfrimOfUSerAsync(checkToken.accId);
                            if (IsGet != null)
                            {
                                return Ok(IsGet);
                            }
                            else
                            {
                                return NotFound("Not Have Advise Now!");
                            }
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
            }
            else return BadRequest();
        }

        [HttpGet("GetAllAdviseUnConfrimOfUSer")]
        public async Task<IActionResult> GetAllAdviseUnConfrimOfUSer()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "User")
                        {
                            var IsGet = await _AdviseService.GetAllAdviseUnConfirmOfUserAsync(checkToken.accId);
                            if (IsGet != null)
                            {
                                return Ok(IsGet);
                            }
                            else
                            {
                                return NotFound("Not Have Advise Now!");
                            }
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
            }
            else return BadRequest();
        }

        [HttpGet("GetAllAdviseConfrimOfExpert")]
        public async Task<IActionResult> GetAllAdviseConfrimOfExpert()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "Expert")
                        {
                            var IsGet = await _AdviseService.GetAllAdviseConfrimOfExpertAsync(checkToken.accId);
                            if (IsGet != null)
                            {
                                return Ok(IsGet);
                            }
                            else
                            {
                                return NotFound("Not Have Advise Now!");
                            }
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
            }
            else return BadRequest();
        }

        [HttpGet("GetAllAdviseUnConfrimOfExpert")]
        public async Task<IActionResult> GetAllAdviseUnConfrimOfExpert()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "Expert")
                        {
                            var IsGet = await _AdviseService.GetAllAdviseUnConfirmOfExpert(checkToken.accId);
                            if (IsGet != null)
                            {
                                return Ok(IsGet);
                            }
                            else
                            {
                                return NotFound("Not Have Advise Now!");
                            }
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
            }
            else return BadRequest();
        }


        [HttpPut("ConfirmUserAdvise")]
        public async Task<IActionResult> ConfirmUserAdvise(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "User")
                        {
                            var IsConfirm = await _AdviseService.ConfirmAdviseOfUserAsync(checkToken.accId, Id.ToString());
                            if (IsConfirm)
                            {
                                return Ok("Confirm Advise Success");
                            }
                            else return BadRequest();                            
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
            }
            else return BadRequest();
        }

        [HttpPut("ConfirmExpertAdvise")]
        public async Task<IActionResult> ConfirmExpertAdvise(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "Expert")
                        {
                            var IsConfirm = await _AdviseService.ConfirmAdviseOfExpertAsync(checkToken.accId, Id.ToString());
                            if (IsConfirm)
                            {
                                return Ok("Confirm Advise Success");
                            }
                            else return BadRequest();
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
            }
            else return BadRequest();
        }

        [HttpPut("DeleteAdvise")]
        public async Task<IActionResult> DeleteAdvise(Guid Id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken != null)
                    {
                        if (checkToken.RoleName == "User")
                        {
                            var IsDelete = await _AdviseService.DeleteAviseAsync(checkToken.accId, Id.ToString());
                            if (IsDelete)
                            {
                                return Ok("Delete Advise Success");
                            }
                            else return BadRequest();
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
            }
            else return BadRequest();
        }
    }
}
