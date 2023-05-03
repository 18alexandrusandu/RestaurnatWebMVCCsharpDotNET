using Microsoft.Extensions.Options;
using ServiceStack;
using ServiceStack.Text;
using System.IO;

namespace WebApplication5.Services.implementations
{
    public class ExportCsvService : IExportService
    {
        public void deserialize<T>()
        {
            throw new NotImplementedException();
        }

        public void export(string content, string filename)
        {

                 FileStream fs= new FileStream(filename,
                FileMode.Create,
                FileAccess.ReadWrite);
                fs.Write(content.ToUtf8Bytes());
                fs.Flush(true);
                fs.Close();
        }

        public String serialize<T>(Object o)
        {
            var csv = CsvSerializer.SerializeToCsv(new[]{
                o
            });
          return  csv.ToString();

           
         
        }
    }
}
