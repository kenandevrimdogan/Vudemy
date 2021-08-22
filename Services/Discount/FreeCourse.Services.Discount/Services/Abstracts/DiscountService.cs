using AutoMapper;
using Dapper;
using FreeCourse.Services.Discount.Dtos.Discounts;
using FreeCourse.Services.Discount.Services.Interfaces;
using FreeCourse.Shared.Dtos.NoContent;
using FreeCourse.Shared.Dtos.Response;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services.Abstracts
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        private readonly IMapper _mapper;


        public DiscountService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;

            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSQL"));
        }

        public async Task<ResponseDTO<NoContent>> DeleteAsync(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });

            return status > 0 ? ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent) : ResponseDTO<NoContent>.Fail("Discount not found", HttpStatusCode.NotFound);
        }

        public async Task<ResponseDTO<List<DiscountDTO>>> GetAllAsync()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discounts.Discount>("Select * from discount");

            return ResponseDTO<List<DiscountDTO>>.Success(_mapper.Map<List<Models.Discounts.Discount>, List<DiscountDTO>>(discounts.ToList()), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<DiscountDTO>> GetByCodeAndUserIdAsync(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discounts.Discount>("select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = code });

            var hasDiscount = discounts.FirstOrDefault();

            if (hasDiscount == null)
            {
                return ResponseDTO<DiscountDTO>.Fail("Discount not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<DiscountDTO>.Success(_mapper.Map<Models.Discounts.Discount, DiscountDTO>(hasDiscount), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<DiscountDTO>> GetByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discounts.Discount>("select * from discount where id=@Id", new { Id = id })).SingleOrDefault();

            if (discount == null)
            {
                return ResponseDTO<DiscountDTO>.Fail("Discount not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<DiscountDTO>.Success(_mapper.Map<Models.Discounts.Discount, DiscountDTO>(discount), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<NoContent>> SaveAsync(CreateDiscountDTO discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);

            if (saveStatus > 0)
            {
                return ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent);
            }

            return ResponseDTO<NoContent>.Fail("an error occurred while adding", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDTO<NoContent>> UpdateAsync(UpdateDiscountDTO discount)
        {
            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id", new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });

            if (status > 0)
            {
                return ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent);
            }

            return ResponseDTO<NoContent>.Fail("Discount not found", HttpStatusCode.NotFound);
        }
    }
}
