namespace WebApplication5.Models
{
    public class MenuItem:IModel
    {
        public int Id { get; set; }
        public string numePreparat { get; set; }
        public Double pret { get; set; }
        public int stoc { get; set; }

        private object p { get; set; }
        private int newStoc { get; set; }

        public MenuItem()
        {
        }

        public MenuItem(string menuItemname, object p, int newStoc)
        {
            this.numePreparat = menuItemname;
            this.pret = (Double)p;
            this.newStoc = newStoc;
        }

        public MenuItem(String numePreparat, Double pret, int stoc, int id = 1)
        {
            this.numePreparat = numePreparat;
            this.pret = pret;
            this.stoc = stoc;
            this.Id= id;
        }

        public int CompareTo(object obj)
        {
            if (this.numePreparat == ((MenuItem)obj).numePreparat
                && this.Id == ((MenuItem)obj).Id
                && this.pret == ((MenuItem)obj).pret
                && this.stoc == ((MenuItem)obj).stoc)
                return 0;
            return -1;

        }

        override public string ToString()
        {
            return numePreparat;
        }

    }
}
