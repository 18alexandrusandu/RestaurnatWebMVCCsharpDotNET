namespace WebApplication5.Services
{
    public interface IExportService
    {
        public void export(String content, String filename);
        public String serialize<T>(Object o);

        public void deserialize<T>();


    }
}
