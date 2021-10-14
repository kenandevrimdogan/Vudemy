using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Services.Course.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.Course.Abstracts
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Models.Courses.Course> _courseCollection;
        private readonly IMongoCollection<Models.Categories.Category> _categoryCollection;
        private readonly IMapper _maper;

        public CourseService(IMapper maper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Models.Courses.Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Models.Categories.Category>(databaseSettings.CategoryCollectionName);
            _maper = maper;
        }

        public async Task<ResponseDTO<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.FindSync(x => x.Id == course.Id).FirstOrDefaultAsync();
                }
            }

            return ResponseDTO<List<CourseDTO>>.Success(_maper.Map<List<CourseDTO>>(courses), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return ResponseDTO<CourseDTO>.Success(new CourseDTO(), HttpStatusCode.OK);
            }

            var courseDTO = _maper.Map<Models.Courses.Course, CourseDTO>(course);

            return ResponseDTO<CourseDTO>.Success(courseDTO, HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (!courses.Any())
            {
                return ResponseDTO<List<CourseDTO>>.Success(new List<CourseDTO>(), HttpStatusCode.OK);
            }

            var courseDTOs = _maper.Map<List<Models.Courses.Course>, List<CourseDTO>>(courses);

            return ResponseDTO<List<CourseDTO>>.Success(courseDTOs, HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<CourseDTO>> CreateAsync(CourseCreateDTO createcourse)
        {
            var course = _maper.Map<CourseCreateDTO, Models.Courses.Course>(createcourse);
            course.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(course);

            var courseDTO = _maper.Map<Models.Courses.Course, CourseDTO>(course);


            return ResponseDTO<CourseDTO>.Success(courseDTO, HttpStatusCode.Created);
        }

        public async Task<ResponseDTO<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdate)
        {
            var course = _maper.Map<CourseUpdateDTO, Models.Courses.Course>(courseUpdate);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdate.Id, course);

            if (result == null)
            {
                return ResponseDTO<NoContent>.Fail("Course not found", HttpStatusCode.NotFound);
            }

            var courseDTO = _maper.Map<Models.Courses.Course, CourseDTO>(course);

            return ResponseDTO<NoContent>.Success(HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount < 0)
            {
                return ResponseDTO<NoContent>.Fail("Course not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<NoContent>.Success(HttpStatusCode.OK);
        }

    }
}
