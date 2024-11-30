namespace LAB_3.Models
{
    public class Batch
    {
        public int StoreId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }   
        public decimal Price { get; set; }   
    }
}
