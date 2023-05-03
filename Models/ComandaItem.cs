namespace WebApplication5.Models
{
    public class ComandaItem : IModel
    {
        public ComandaItem(int id, int item, int comandaId, int quantity)
        {
            Id = id;
            ItemId = item;
            ComandaId = comandaId;
            Quantity = quantity;
        }
        public ComandaItem()
        {
           
        }
        public int Id { get; set; }
        public int ItemId { get; set; }


        public int  ComandaId { get; set; }
       

        public int Quantity { get; set; }

      
    }
}
