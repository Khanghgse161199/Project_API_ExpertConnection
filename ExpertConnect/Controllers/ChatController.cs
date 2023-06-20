using DatabaseConection.Entities;
using DataService.AuthServices;
using DataService.ChatServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Advise;
using ViewModel.Chat;

namespace ExpertConnect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ExpertConectionContext _expertConectionContext;
        private readonly IAuthService _authService; 
        private readonly IChatService _chatService;

        public ChatController(ExpertConectionContext expertConectionContext, IChatService chatService, IAuthService authService)
        {
            _expertConectionContext = expertConectionContext;
            _authService = authService;
            _chatService = chatService;
        }

        [HttpPost("CreateChat")]
        public async Task<IActionResult> CreateChat(CreateChatViewModel createChatViewModel)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if(!string.IsNullOrEmpty(tokenInHeader) && createChatViewModel != null)
            {
                if (ModelState.IsValid)
                {
                    var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                    if (checkToken.RoleName == "User" || checkToken.RoleName == "Expert")
                    {
                        var IsCreate = await _chatService.CreateChatAsync(checkToken.accId, createChatViewModel);
                        if (IsCreate)
                        {
                            return Ok("Send Message Success");
                        }
                        else return BadRequest();
                    }
                    else return BadRequest();
                }else
                {
                    var error = ModelState.Select(p => p.Value.Errors)
                        .Where(p => p.Count > 0)
                        .ToList();
                    return BadRequest(error);
                }
            }else return NotFound();
        }

        [HttpGet("GetChats")]
        public async Task<IActionResult> GetChats(Guid Id)
        {
            if (!string.IsNullOrEmpty(Id.ToString()))
            {
                if (ModelState.IsValid)
                {
                    string tokenInHeader = Request.Headers["token"].ToString();
                    if (!string.IsNullOrEmpty(tokenInHeader))
                    {
                        var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                        if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin" || checkToken.RoleName == "Expert" || checkToken.RoleName == "User")
                        {
                            var chats = await _chatService.GetChatAsync(Id.ToString(),checkToken.accId);
                            return Ok(chats);
                        }
                        else return NotFound();
                    }else return BadRequest();
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

        [HttpPut("DeleteChat")]
        public async Task<IActionResult> DeleteChat(DeleteChatViewModel deleteChatViewModel)
        {
            if (!string.IsNullOrEmpty(deleteChatViewModel.ChatId))
            {
                if (ModelState.IsValid)
                {
                    string tokenInHeader = Request.Headers["token"].ToString();
                    if (!string.IsNullOrEmpty(tokenInHeader))
                    {
                        var checkToken = await _authService.checkTokenAsync(tokenInHeader);
                        if (checkToken.RoleName == "Expert" || checkToken.RoleName == "User")
                        {
                            var IsDelete = await _chatService.DeleteChatAsync(deleteChatViewModel.ChatId, checkToken.accId);
                            if (IsDelete)
                            {
                                return Ok("Delete Chat Success");
                            }
                            else return BadRequest();
                        }
                        else return NotFound();
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
            else return BadRequest();
        }
    }
}
