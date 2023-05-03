using WebApplication5.Data;
using WebApplication5.Models;
namespace WebApplication5.Repositories
{
    public class MenuRepository : GenericRepository<MenuItem>
    {
        public MenuRepository(RestaurantDbContext context) : base(context)
        {
           
        }
        public Models.MenuItem GetByName(String name)
        {
            return _context.MenuItem.Single(a => a.numePreparat == name);

        }
    }
}
