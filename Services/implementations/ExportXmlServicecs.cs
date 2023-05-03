using ServiceStack.Text;
using System.Xml.Serialization;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace WebApplication5.Services.implementations
{
    public class ExportXmlServicecs : IExportService
    {
        public void deserialize<T>()
        {
            throw new NotImplementedException();
        }

        public void export(string content, string filename)
        {
            throw new NotImplementedException();
        }



        public string serialize<T>(object o)
        {
                 XmlSerializer serializer = new XmlSerializer(typeof(T));

            StringWriter textWriter = new StringWriter();
            serializer.Serialize(textWriter, o);
           return textWriter.ToString();
               
        }
    }
}
