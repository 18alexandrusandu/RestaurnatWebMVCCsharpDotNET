using NuGet.Versioning;
using WebApplication5.Data;

namespace WebApplication5.Repositories
{
    public class UserRepository : GenericRepository<Models.User>
    {
        public UserRepository(RestaurantDbContext context) : base(context)
        {
        }


        public Models.User GetByUsername(String username)
        {
            return _context.User.Single(a => a.username == username);

        }
    }



}
