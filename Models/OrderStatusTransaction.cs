using PizzaOrder.Dtos;

namespace PizzaOrder.Models
{
    public class OrderStatusTransaction : BaseEntity
    {
        public int OrderId { get; set; }
        public string RiderId { get; set; }
        public int OrderStatus { get; set; }
        public bool ActiveQueue { get; set; }
    }
}
