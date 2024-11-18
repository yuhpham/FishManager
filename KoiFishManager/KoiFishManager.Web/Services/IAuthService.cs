using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishManager.Models;

namespace KoiFishManager.Web.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task Logout();
    }
}
