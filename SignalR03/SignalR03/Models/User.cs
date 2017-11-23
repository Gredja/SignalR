
namespace SignalR03.Models
{
    public class User
    {
        private User(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }

        public static User CreateUser(string id, string name)
        {
            return new User(id, name);
        }
    }
}
