namespace FreeCourse.Services.Order.Application.Dtos.Orders
{
    public class OrderItemDTO
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureURL { get; set; }

        public decimal Price { get; set; }
    }
}
