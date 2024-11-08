using BlazingChat.Server.Data;
using BlazingChat.Server.Data.Entities;
using BlazingChat.Server.Hubs;
using BlazingChat.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazingChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ChatContext chatContext;
        private readonly TokenService tokenService;
        private readonly IHubContext<BlazingChatHub, IBlazingChatHubClient> hubContext;

        public AccountController(ChatContext _chatContext, TokenService _tokenService, IHubContext<BlazingChatHub, IBlazingChatHubClient> _hubContext)
        {
            chatContext = _chatContext;
            tokenService = _tokenService;
            hubContext = _hubContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto, CancellationToken cancellationToken)
        {
            var dbUser = await chatContext.Users
                                .AsNoTracking()
                                .AnyAsync(u => u.Username == dto.Username);

            if (dbUser)
            {
                return BadRequest($"{dto.Username} already exists");
            }

            var user = new User
            {
                Name = dto.Name,
                Username = dto.Username,
                Password = dto.Password,

            };
            await chatContext.Users.AddAsync(user, cancellationToken);
            await chatContext.SaveChangesAsync(cancellationToken);

            await hubContext.Clients.All.UserConnected(new UserDto(user.Id, user.Name));

            return Ok(GenerateToken(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var dbUser = await chatContext.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Password == loginDto.Password);

            if (dbUser is null)
            {
                return BadRequest("Incorrect username / password");
            }

            return Ok(GenerateToken(dbUser));
        }

        private AuthResponseDto GenerateToken(User user)
        {
            var token = tokenService.GenerateJwt(user);
            return new AuthResponseDto(new UserDto(user.Id, user.Name), token);
        }
    }
}
