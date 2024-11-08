using BlazingChat.Shared.Dtos;

namespace BlazingChat.Shared
{
    public interface IBlazingChatHubServer
    {
        Task SetUserOnline(UserDto user);
    }
}
