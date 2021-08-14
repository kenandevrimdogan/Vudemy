using System;
using FreeCourse.Services.Catalog.Dtos.Future;
using FreeCourse.Services.Catalog.Dtos.Category;

namespace FreeCourse.Services.Catalog.Dtos.Course
{
    public class CourseDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        public DateTime CreatedTime { get; set; }

        public FutureDTO Feature { get; set; }

        public string CategoryId { get; set; }

        public CategoryDTO Category { get; set; }
    }
}
