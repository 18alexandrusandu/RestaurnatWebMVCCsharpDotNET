namespace WebApplication5.Repositories
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; set; }
        public ComenziRepository ComenziRepository { get; set; }
        public MenuRepository MenuRepository { get; set; }

        Task<int> Save();

        void SaveAsync();


        void Dispose();

    }
}
