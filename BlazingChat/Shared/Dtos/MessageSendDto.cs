namespace BlazingChat.Shared.Dtos
{
    public record MessageSendDto(int ToUserId, string message);
    public record MessageDto(int ToUserId, int FromUserId, string message, DateTime SentOn);
}
