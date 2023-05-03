using WebApplication5.Data;

namespace WebApplication5.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
  

        public UserRepository UserRepository { get; set; }
        public ComenziRepository ComenziRepository { get; set; }
        public MenuRepository MenuRepository { get; set; }

        public RestaurantDbContext _context;

        public UnitOfWork(UserRepository userRepository, ComenziRepository comenziRepository, MenuRepository menuRepository, RestaurantDbContext context) 
        {
            UserRepository = userRepository;
            ComenziRepository = comenziRepository;
            MenuRepository = menuRepository;
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<int>   Save()
        {
           return _context.SaveChangesAsync();
        }

        public void SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
