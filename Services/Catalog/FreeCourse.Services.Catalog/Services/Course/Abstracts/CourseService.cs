﻿using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Services.Course.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using FreeCourse.Shared.Messages;
using MassTransit;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper maper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Models.Courses.Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Models.Categories.Category>(databaseSettings.CategoryCollectionName);
            _maper = maper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ResponseDTO<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any() && courses.Count > default(int))
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.FindSync(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Models.Courses.Course>();
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

            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

            var courseDTO = _maper.Map<Models.Courses.Course, CourseDTO>(course);

            return ResponseDTO<CourseDTO>.Success(courseDTO, HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<List<CourseDTO>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = _categoryCollection.Find(x => x.Id == item.CategoryId).FirstOrDefault();
                }

            }
            else
            {
                courses = new List<Models.Courses.Course>();
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

            await _publishEndpoint.Publish<CourseNameChanedEvent>(new CourseNameChanedEvent
            {
                CourseId = course.Id,
                UpdatedName = courseUpdate.Name
            });

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
