using WebApplication5.Models;

namespace WebApplication5.Services
{
    public interface IComenziService
    {
        public Task<ComandaData> GetComanda(int id);
        public Task<List<Comanda>> List();


        public Task<bool> Add(Comanda item);

        public Task<bool> AddProduct(int comanda_id,int product_id, int quantity);

        public Task<object> RaportStatisiticiProduseComanda();
        public Task<object> RaportComenzi(DateTime start, DateTime end);
      
        public Task<bool> DeleteProduct(int id,MenuItem item);
        
        public Task<bool> UpdateStatus(int id, String newStatus);

        public Task<bool> Delete(ComandaData item);

        public Task<bool> Update(Comanda item);

        public Task<double> ComputeTotalPrice(int id);

        public Task<bool> FinalizeComanda(int id);

        public Task<bool> Exists(int id);
        public  Task<bool> CheckMenuForComanda(int id, int? compare);
    }
}
