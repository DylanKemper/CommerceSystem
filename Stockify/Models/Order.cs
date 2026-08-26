namespace Stockify.Models
{
    public class Order
    {
        public int Id { get; set; }     // PK
        public int CustomerId { get; set; }     // FK to Customer
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}