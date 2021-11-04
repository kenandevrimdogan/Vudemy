using System.Collections.Generic;

namespace FreeCourse.Frontends.Web.Models.Orders
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItems = new List<OrderItemCreateInput>();
        }

        public string BuyerId { get; set; }

        public List<OrderItemCreateInput> OrderItems { get; set; }

        public AddressViewModel Address { get; set; }
    }
}
