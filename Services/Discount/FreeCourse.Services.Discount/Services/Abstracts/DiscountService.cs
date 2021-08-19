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
            var deleteStatus = await _dbConnection.ExecuteAsync($"DELETE FROM discount WHERE id = {id}");

            if (deleteStatus < 0)
            {
                return ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent);
            }

            return ResponseDTO<NoContent>.Fail("Discount Not Found", HttpStatusCode.NotFound);
        }

        public async Task<ResponseDTO<List<DiscountDTO>>> GetAllAsync()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discounts.Discount>("SELECT * FROM discount");


            return ResponseDTO<List<DiscountDTO>>.Success(_mapper.Map<List<DiscountDTO>>(discounts), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<DiscountDTO>> GetByCodeAndUserIdAsync(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discounts.Discount>($"SELECT * FROM discount where id={userId} AND code = {code}")).FirstOrDefault();

            if (discount == null)
            {
                return ResponseDTO<DiscountDTO>.Fail("Discount not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<DiscountDTO>.Success(_mapper.Map<DiscountDTO>(discount), HttpStatusCode.OK);
        }

        public async Task<ResponseDTO<DiscountDTO>> GetByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discounts.Discount>($"SELECT * FROM discount where id={id}")).SingleOrDefault();

            if(discount == null)
            {
                return ResponseDTO<DiscountDTO>.Fail("Discount not found", HttpStatusCode.NotFound);
            }

            return ResponseDTO<DiscountDTO>.Success(_mapper.Map<DiscountDTO>(discount), HttpStatusCode.OK);

        }

        public async Task<ResponseDTO<NoContent>> SaveAsync(CreateDiscountDTO createDiscount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync($"INSERT INTO discount (userid, rate, code) VALUES ({createDiscount.UserId}, {createDiscount.Rate}, {createDiscount.Code})");

            if(saveStatus < 0)
            {
                return ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent);
            }

            return ResponseDTO<NoContent>.Fail("An error occurred while adding",HttpStatusCode.NoContent);
        }

        public async Task<ResponseDTO<NoContent>> UpdateAsync(UpdateDiscountDTO updateDiscount)
        {
            var updateStatus = await _dbConnection.ExecuteAsync($"UPDATE discount set userId = {updateDiscount.UserId}, code = {updateDiscount.Code}, rate = {updateDiscount.Rate}");

            if (updateStatus < 0)
            {
                return ResponseDTO<NoContent>.Success(HttpStatusCode.NoContent);
            }

            return ResponseDTO<NoContent>.Fail("Discount Not Found", HttpStatusCode.NotFound);
        }
    }
}
