using BlazingChat.Shared.Dtos;

namespace BlazingChat.Shared
{
    public interface IBlazingChatHubClient
    {
        Task UserConnected(UserDto user);
        
        Task OnlineUsersList(IEnumerable<UserDto> users);

        Task UserIsOnline(int userId);
        
        Task MessageReceived(MessageDto message);
    }
}
