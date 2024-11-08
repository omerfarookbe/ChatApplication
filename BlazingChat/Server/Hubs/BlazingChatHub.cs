using BlazingChat.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazingChat.Server.Hubs
{
    [Authorize]
    public class BlazingChatHub : Hub<IBlazingChatHubClient>, IBlazingChatHubServer
    {
        private static readonly IDictionary<int, UserDto> onlineUsers = new Dictionary<int, UserDto>();

        public BlazingChatHub()
        {
             
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync(); 
        }

        public async Task SetUserOnline(UserDto user)
        {
            await Clients.Caller.OnlineUsersList(onlineUsers.Values);
            if (!onlineUsers.ContainsKey(user.Id))
            {
                onlineUsers.Add(user.Id, user);
                //await Clients.Others.UserConnected(user);
                await Clients.Others.UserIsOnline(user.Id);
            }
        }
    }
}
    