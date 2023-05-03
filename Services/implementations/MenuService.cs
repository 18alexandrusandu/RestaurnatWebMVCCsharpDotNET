using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services.implementations
{
    public class MenuService : IMenuService
    {
        
       public readonly IUnitOfWork unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<MenuItem> GetMenuItem(String nume)
        {

            return unitOfWork.MenuRepository.GetByName(nume);
           

        }
        public async Task<MenuItem> GetMenuItem(int Id)
        {

            return await unitOfWork.MenuRepository.Get(Id);


        }





        async Task<List<MenuItem>> IMenuService.List()
        {
            return (List<MenuItem>)await unitOfWork.MenuRepository.GetAll();
        }

        public async Task<bool> Add(MenuItem item)
        {
            await unitOfWork.MenuRepository.Add(item);
            await unitOfWork.Save();
            return true;
        }

        public async Task<bool> Delete(MenuItem item)
        {
              unitOfWork.MenuRepository.Delete(item);
              await unitOfWork.Save();
            return true;
        }

        public async Task<bool> Update(MenuItem item)
        {
            unitOfWork.MenuRepository.Update(item);
            await unitOfWork.Save();
            return true;
        }

        public  async void EditStoc(string nume, int newStoc)
        {
            var menuI=unitOfWork.MenuRepository.GetByName(nume);
            menuI.stoc = newStoc;

            unitOfWork.MenuRepository.Update(menuI);
            await unitOfWork.Save();
          
        }
    }
}
