namespace WebApplication5.Models
{
    public class ComandaData
    {

        public List<ComandaItemData> produseList { get; set; }
        public Double pretTotal { get; set; }
        public String status { get; set; }
        public int Id { get; set; }
        public DateTime dateAndHour { get; set; }

        public ComandaData(double pretTotal, string status, int id, DateTime dateAndHour)
        {
            produseList = new List<ComandaItemData>();
            this.pretTotal = pretTotal;
            this.status = status;
            this.Id = id;
            this.dateAndHour = dateAndHour;
        }
        public ComandaData()
        {
            produseList = new List<ComandaItemData>();
        }
        public ComandaData(List<ComandaItemData> produseList, double pretTotal, string status, DateTime dateAndHour)
        {
            this.produseList = produseList;
            this.pretTotal = pretTotal;
            this.status = status;
            this.dateAndHour = dateAndHour;
        }


    }
}
    

