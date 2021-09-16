using FreeCourse.Frontends.Web.Models;
using FreeCourse.Shared.Dtos.Response;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ResponseDTO<bool>> SignIn(SignInInput signInInput);

        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefreshToken();

    }
}
