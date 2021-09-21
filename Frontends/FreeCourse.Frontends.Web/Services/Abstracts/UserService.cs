using FreeCourse.Frontends.Web.Models;
using FreeCourse.Frontends.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Frontends.Web.Services.Abstracts
{
    public class UserService : IUserService
    {

        public Task<UserViewModel> GetUser()
        {
            throw new NotImplementedException();
        }
    }
}
