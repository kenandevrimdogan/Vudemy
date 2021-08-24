using System;
using System.Collections.Generic;

namespace FreeCourse.Services.Order.Application.Dtos.Orders
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public AddressDTO Address { get; set; }

        public string BuyerId { get; set; }

        private List<OrderItemDTO> OrderItems { get; set; }

    }
}
