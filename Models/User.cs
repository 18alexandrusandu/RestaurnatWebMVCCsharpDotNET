namespace WebApplication5.Models
{
    public class User: IModel
    {
        public String username { get; set; }
        public String password { get; set; }
        public String name { get; set; }
        public String role { get; set; }
        public int Id { get; set; }

        public User(string username, string password, string name, string role)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.role = role;

        }

        public User(string username, string password, string name, string role, int id) : this(username, password, name, role)
        {
            this.Id = id;
        }
        public User()
        {

        }
    }
}
