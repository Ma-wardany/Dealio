using Dealio.Domain.Enums;

namespace Dealio.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderNumber { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string BuyerId { get; set; }
        public UserProfile Buyer { get; set; }

        public string? DeliveryId { get; set; }
        public DeliveryProfile Delivery { get; set; }
        public Payment Payment { get; set; }
        public Commission Commission { get; set; }
        public SellerTransaction SellerTransaction { get; set; }

    }
}
