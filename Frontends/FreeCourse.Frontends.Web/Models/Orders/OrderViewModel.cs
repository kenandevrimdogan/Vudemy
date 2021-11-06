using System;
using System.Collections.Generic;

namespace FreeCourse.Frontends.Web.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public AddressViewModel Address { get; set; }

        public string BuyerId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
