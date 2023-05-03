namespace WebApplication5.Models
{
    public class Reports
    {
        public Reports(string data, string type)
        {
            Data = data;
            Type = type;
        }
        public Reports()
        {
         
        }


        public String Data { get; set; }
        public String Type { get; set; }
        public object RawData { get; set; }
    }
}
