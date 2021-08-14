using FreeCourse.Services.Catalog.Dtos.Future;

namespace FreeCourse.Services.Catalog.Dtos.Course
{
    public class CourseCreateDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public FutureDTO Feature { get; set; }

        public string CategoryId { get; set; }
    }
}
