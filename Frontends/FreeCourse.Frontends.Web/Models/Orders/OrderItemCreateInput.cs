namespace FreeCourse.Frontends.Web.Models.Orders
{
    public class OrderItemCreateInput
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureURL { get; set; }

        public decimal Price { get; set; }
    }
}
