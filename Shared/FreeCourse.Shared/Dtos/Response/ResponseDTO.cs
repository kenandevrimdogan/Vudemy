using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos.Response
{
    public class ResponseDTO<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; set; }

        public static ResponseDTO<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T>
            {
                Data = data,
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static ResponseDTO<T> Success(HttpStatusCode statusCode)
        {
            return new ResponseDTO<T>
            {
                Data = default(T),
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }

        public static ResponseDTO<T> Fail(List<string> errors, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T>
            {
                StatusCode = statusCode,
                IsSuccessful = false,
                Errors = errors
            };
        }

        public static ResponseDTO<T> Fail(string error, HttpStatusCode statusCode)
        {
            return new ResponseDTO<T>
            {
                StatusCode = statusCode,
                IsSuccessful = false,
                Errors = new List<string> { error }
            };
        }
    }
}
