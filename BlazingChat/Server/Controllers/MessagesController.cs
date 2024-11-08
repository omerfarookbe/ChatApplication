using BlazingChat.Server.Data;
using BlazingChat.Server.Data.Entities;
using BlazingChat.Server.Hubs;
using BlazingChat.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazingChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : BaseController
    {
        public ChatContext chatContext { get; }
        private readonly IHubContext<BlazingChatHub, IBlazingChatHubClient> hubContext;

        public MessagesController(ChatContext _chatContext, IHubContext<BlazingChatHub, IBlazingChatHubClient> _hubContext)
        {
            chatContext = _chatContext;
            hubContext = _hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(MessageSendDto messageDto, CancellationToken cancellation)
        {
            if (messageDto.ToUserId <= 0 || string.IsNullOrWhiteSpace(messageDto.message))
                return BadRequest();

            var message = new Message
            {
                FromId = UserId,
                ToId = messageDto.ToUserId,
                Content = messageDto.message,
                SentOn = DateTime.Now,
            };

            await chatContext.Messages.AddAsync(message, cancellation);
            if (await chatContext.SaveChangesAsync(cancellation) > 0)
            {
                var responseMessageDto = new MessageDto(message.ToId, message.FromId, message.Content, message.SentOn);
                await hubContext.Clients.User(messageDto.ToUserId.ToString())
                    .MessageReceived(responseMessageDto);
                return Ok();
            }
            else
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{otherUserId:int}")]
        public async Task<IEnumerable<MessageDto>> GetMessages(int otherUserId, CancellationToken cancellationToken)
        {
            var messages = await chatContext.Messages
                .AsNoTracking()
                .Where(m => (m.FromId == otherUserId && m.ToId == UserId) || (m.ToId == otherUserId && m.FromId == UserId))
                .Select(m => new MessageDto(m.ToId, m.FromId, m.Content, m.SentOn))
                .ToListAsync(cancellationToken);

            return messages;
        }

    }
}
