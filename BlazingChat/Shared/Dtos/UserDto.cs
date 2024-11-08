namespace BlazingChat.Shared.Dtos
{
    public class UserDto
    {
        public UserDto(int id, string name, bool isonline = false)
        {
            Id = id;
            Name = name;
            IsOnline = isonline;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public bool IsSelected { get; set; }
    }
}
 