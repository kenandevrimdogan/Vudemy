using FreeCourse.Frontends.Web.Models;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
