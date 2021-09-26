namespace ELEKSUNI
{
    public class Item
    {
        public string Name { get; }
        public int Price { get; }
        public double Weight { get; set; }
        public Item(string name, int price, double weight)
        {
            this.Name = name;
            this.Price = price;
            this.Weight = weight;
        }
    }
}