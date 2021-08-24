using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class OrderItem : Entity
    {
        public OrderItem(string productId, string productName, string pictureURL, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureURL = pictureURL;
            Price = price;
        }

        public string ProductId { get; private set; }

        public string ProductName { get; private set; }

        public string PictureURL { get; private set; }

        public decimal Price { get; private set; }

        public void UpdateOrderItem(string productName, string pictureURL, decimal price)
        {
            ProductName = productName;
            PictureURL = pictureURL;
            Price = price;
        }
    }
}
