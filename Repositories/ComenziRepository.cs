

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.DependencyResolver;
using ServiceStack;
using System.Diagnostics;
using WebApplication5.Data;
using WebApplication5.Models;
namespace WebApplication5.Repositories
{
    public class ComenziRepository : GenericRepository<Comanda>
    {
        public ComenziRepository(RestaurantDbContext context) : base(context)
        {
        }

        public new async Task<Comanda> Get(int id)
        {

            var comanda=  await _context.Set<Comanda>().FindAsync(id);



            var comenziItems = await (from e in _context.ComenziItems
                                      where e.ComandaId == comanda.Id
                                      select e).ToListAsync();

            comanda.produseList.RemoveAll(x => x.Id == x.Id);


            foreach (var item in comenziItems)
            {
                comanda.produseList.Add(item);

            }
         
            return comanda;

           
        }


        public async Task<bool> Any(int id)
        {
            return  _context.Set<Comanda>().Any(e => e.Id == id);
        }


        public List<Comanda> GetComenziByInterval(DateTime start,DateTime end)
        {


            return (List<Comanda>)(_context.Comanda.Where(a=>a.dateAndHour>=start && a.dateAndHour<=end).ToList());

        }
    }
}
