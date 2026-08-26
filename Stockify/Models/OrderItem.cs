namespace Stockify.Models
{
    public class OrderItem
    {
        public int Id { get; set; }     // PK
        public int OrderId { get; set; }     // FK to Order
        public int ProductId { get; set; }     // FK to Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
