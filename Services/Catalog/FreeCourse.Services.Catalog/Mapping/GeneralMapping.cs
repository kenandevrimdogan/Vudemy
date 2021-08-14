using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Dtos.Future;
using FreeCourse.Services.Catalog.Models.Category;
using FreeCourse.Services.Catalog.Models.Course;
using FreeCourse.Services.Catalog.Models.Feature;

namespace FreeCourse.Services.Catalog.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Feature, FeatureDTO>().ReverseMap();

            CreateMap<Course, CourseCreateDTO>().ReverseMap();
            CreateMap<Course, CourseUpdateDTO>().ReverseMap();

        }
    }
}
