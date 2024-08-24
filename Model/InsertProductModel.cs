namespace TestAPI.Model
{
    public class InsertProductModel
    {
        public string? productName { get; set; }
        public double price { get; set; }

        public int sold { get; set; }
        public int stocks { get; set; }
        public string? imageString { get; set; }
    }
}
