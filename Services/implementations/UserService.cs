using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services.implementations
{
    public class UserService : IUserService
    {

        public readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserAccount(int id)
        {

            return await unitOfWork.UserRepository.Get(id);


        }
        public async Task<bool> DeleteUserAccount(User user)
        {
            unitOfWork.UserRepository.Delete(user);
            await unitOfWork.Save();
            return true;

        }

        public Task<User> login(string username, string password)
        {
            String password2 = getMd5Hash(password);
            User user = unitOfWork.UserRepository.GetByUsername(username);
            if (user != null && user.password == password2)
            {

               
                return Task.FromResult(user);

            }


            return Task.FromResult<User>(null);



        }
        public Task<bool> changePassword(string username,string email)
        {
          
            User user = unitOfWork.UserRepository.GetByUsername(username);
        
            if (user!=null)

            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sandu.andercou@gmail.com", "000sandu2123"),
                    EnableSsl = true,
                };
                String body = "try to connect to change yout pssword to:" + "http://localhost:7080/Users/ChangePasswordSecond/" + user.Id;
                smtpClient.Send("sandu.andercou@gmail.com", email, "Reset Password Oldies", body);

            }
            return Task.FromResult(true);
             
           
        }
        public async Task<bool> resetPassword(string username, string newPassword)
        {
            User user = unitOfWork.UserRepository.GetByUsername(username);
            user.password=getMd5Hash(newPassword);
            unitOfWork.UserRepository.Update(user);
            await unitOfWork.Save();
            return true;
        }
       

        

        public async Task<bool> logout()
        {
            return true;
        }
        static string getMd5Hash(string input)

        {

            // Create a new instance of the MD5CryptoServiceProvider object. 

            MD5 md5Hasher = MD5.Create();



            // Convert the input string to a byte array and compute the hash. 

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));



            // Create a new Stringbuilder to collect the bytes 

            // and create a string. 

            StringBuilder sBuilder = new StringBuilder();



            // Loop through each byte of the hashed data  

            // and format each one as a hexadecimal string. 

            for (int i = 0; i < data.Length; i++)

            {

                sBuilder.Append(data[i].ToString("x2"));

            }



            // Return the hexadecimal string. 

            return sBuilder.ToString();

        }
        public async Task<bool> SignInUserAccount(User user)
        {
            user.password = getMd5Hash(user.password);
            await this.unitOfWork.UserRepository.Add(user);
            await this.unitOfWork.Save();
            return true;
        }

        public  async Task<bool> UpdateUserAccount(User user)
        {
           this.unitOfWork.UserRepository.Update(user);
            await unitOfWork.Save();
            return true;
        }

        public async Task<List<User>> List()
        {
            return (List<User>) await  unitOfWork.UserRepository.GetAll();
        }
        public async Task<bool> Exist(int? id)
        {
            return unitOfWork.UserRepository.Get((int)id) != null;
        }

    }
}
