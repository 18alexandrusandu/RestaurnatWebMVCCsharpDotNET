using WebApplication5.Models;

namespace WebApplication5.Services
{
    public interface IMenuService
    {
        public Task<MenuItem> GetMenuItem(String nume);
        public Task<MenuItem> GetMenuItem(int Id);
        public Task<List<MenuItem>> List();







        public  Task<bool> Add(MenuItem item);

        public Task<bool> Delete(MenuItem item);

        public Task<bool> Update(MenuItem item);

       



    }
}
