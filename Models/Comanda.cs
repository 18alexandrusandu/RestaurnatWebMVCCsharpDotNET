namespace WebApplication5.Models
{
    public class Comanda: IModel
    {
        public List<ComandaItem> produseList { get; set; }
        public Double pretTotal { get; set; }
        public String status { get; set; }
        public int Id { get; set; }
        public DateTime dateAndHour { get; set; }

        public Comanda(double pretTotal, string status,int id, DateTime dateAndHour)
        {
            produseList = new List<ComandaItem>();
            this.pretTotal = pretTotal;
            this.status = status;
            this.Id = id;
            this.dateAndHour = dateAndHour;
        }
        public Comanda()
        {
            produseList = new List<ComandaItem>();
        }
        public Comanda(List<ComandaItem> produseList, double pretTotal, string status, DateTime dateAndHour)
        {
            this.produseList = produseList;
            this.pretTotal = pretTotal;
            this.status = status;
            this.dateAndHour = dateAndHour;
        }

     
    }
}
