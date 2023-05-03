using WebApplication5.Models;

namespace WebApplication5.Services
{
    public interface IUserService
    {
        Task<bool> SignInUserAccount(Models.User user);
        Task<bool> DeleteUserAccount(Models.User user);
        Task<bool> UpdateUserAccount(Models.User user);

        Task<User> login(String username, String password);
        Task<bool> logout();
        public Task<User> GetUserAccount(int id);
        public Task<List<User>> List();

        public Task<bool> changePassword(string username, string email);
        public Task<bool> resetPassword(string username,string newPassword);

        public  Task<bool> Exist(int? id);

    }
}
