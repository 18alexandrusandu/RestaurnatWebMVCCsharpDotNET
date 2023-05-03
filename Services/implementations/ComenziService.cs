using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Protocol;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Text.RegularExpressions;
using WebApplication5.Models;
using WebApplication5.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication5.Services.implementations
{
    public class ComenziService : IComenziService
    {
        public readonly IUnitOfWork unitOfWork;

        public ComenziService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Add(Comanda item)
        {


              item.dateAndHour = DateTime.Now;
              await this.unitOfWork.ComenziRepository.Add(item);    
              await unitOfWork.Save();
              return true;

        }
        public async Task<bool> FinalizeComanda(int id )
        {
            double price = await ComputeTotalPrice(id);
            Debug.Assert(price > 0);
            var comanda = await unitOfWork.ComenziRepository.Get(id);


              comanda.pretTotal = price;
            comanda.status = "Finished";
            unitOfWork.ComenziRepository.Update(comanda);
            await unitOfWork.Save();
            return true;
        }


        public async Task<bool> AddProduct(int comanda_id,int product_id ,int quantity)
        {

            var comanda= await unitOfWork.ComenziRepository.Get(comanda_id);

            if (comanda.status.Contains("New"))
            {
                var product = await unitOfWork.MenuRepository.Get(product_id);


                ComandaItem comandaItem = new ComandaItem();

                comandaItem.ItemId = product.Id;
                Debug.Write(product);



                comandaItem.Quantity = quantity;
                comanda.produseList.Add(comandaItem);
                unitOfWork.ComenziRepository.Update(comanda);
                await unitOfWork.Save();
                return true;
            }
            return false;
        }
        
        public async Task<object> RaportStatisiticiProduseComanda()
        {

            var comenzi =  await unitOfWork.ComenziRepository.GetAll();
            Dictionary<MenuItem,int> produseComandate= new Dictionary<MenuItem,int>();

            foreach (Comanda comanda1 in comenzi)
            {
                var comanda = await GetComanda(comanda1.Id);
                Debug.WriteLine("comanda:" + comanda.ToJson());

                foreach (ComandaItemData comandaItem in comanda.produseList)
                {
                    if (produseComandate.ContainsKey(comandaItem.Item))
                    {
                        produseComandate[comandaItem.Item] += comandaItem.Quantity;
                    }
                    else
                        produseComandate.Add(comandaItem.Item , comandaItem.Quantity);           
                       }
            

            }
            Debug.WriteLine("comandate"+produseComandate.ToJson());
            var order = from entry in produseComandate orderby entry.Value descending select entry.Key+" X "+entry.Value;

            var dict = order.ToList();
            Debug.WriteLine(dict.Count);
            return dict;



        }



        public async Task<bool> Delete(ComandaData item)
        {
            var comanda = await unitOfWork.ComenziRepository.Get(item.Id);
             unitOfWork.ComenziRepository.Delete(comanda);
             await unitOfWork.Save();
            
            return true;

        }

        public async Task<bool> DeleteProduct(int id, MenuItem item)
        {
            var comanda = await unitOfWork.ComenziRepository.Get(id);


               int index=comanda.produseList.FindIndex(a =>a.ItemId==item.Id);
           
            if(index>=0)
            {
                comanda.produseList.RemoveAt(index);
                unitOfWork.ComenziRepository.Update(comanda);
            }
            return true;
        }


        public async Task<List<Comanda>>List()
        {
            return (List<Comanda>)await unitOfWork.ComenziRepository.GetAll();
        }


        public async Task<ComandaData> GetComanda(int id)
        {
            var comanda = await unitOfWork.ComenziRepository.Get(id);
            ComandaData comandaData = new ComandaData();
            comandaData.Id = comanda.Id;
            comandaData.dateAndHour = comanda.dateAndHour;
            comandaData.status = comanda.status;
            comandaData.produseList = new List<ComandaItemData>();
            comandaData.pretTotal = comanda.pretTotal;
            foreach(var comanda1 in comanda.produseList)
            {
                ComandaItemData comandaData1 = new ComandaItemData();
                comandaData1.Quantity = comanda1.Quantity;
                comandaData1.Id = comanda1.Id;

                var menuItem = await unitOfWork.MenuRepository.Get(comanda1.ItemId);
                comandaData1.Item = menuItem;


                comandaData.produseList.Add(comandaData1);
            }




            return comandaData;
        }


        public async Task<bool> Update(Comanda comanda)
        {
          var comanda2 = await unitOfWork.ComenziRepository.Get(comanda.Id);
            comanda2.pretTotal = comanda.pretTotal;
            comanda2.status = comanda.status;
            comanda2.dateAndHour = comanda.dateAndHour;
            unitOfWork.ComenziRepository.Update(comanda2);
            await unitOfWork.Save();
            return true;

        }
        public async Task<bool> UpdateStatus(int id ,String newStatus)
        {
            var comanda2 = await unitOfWork.ComenziRepository.Get(id);
            comanda2.status = newStatus;
            unitOfWork.ComenziRepository.Update(comanda2);
            await unitOfWork.Save();
            return true;
        }

        public async Task<object> RaportComenzi(DateTime start, DateTime end)
        {
            List<Comanda> produse = unitOfWork.ComenziRepository.GetComenziByInterval(start, end);

            return produse;
        }


    

        public async  Task<double> ComputeTotalPrice(int id)
        {
            var comanda = await GetComanda(id);
            double price = 0;
            foreach (ComandaItemData comandaItem in comanda.produseList)
            {
                price += comandaItem.Item.pret * comandaItem.Quantity;

            }
            
         
            return price;


        }

        public async Task<bool> Exists(int id)
        {
            return await unitOfWork.ComenziRepository.Any(id);
        }
        public async Task<bool> CheckMenuForComanda(int id,int? compare)
        {

            MenuItem item = await unitOfWork.MenuRepository.Get(id);

            if (compare!=null)
            {
                if (item.stoc < compare)
                    return false;
            }
            if (item.stoc <= 0)
                return false;

                return true;

        }
    }
}
