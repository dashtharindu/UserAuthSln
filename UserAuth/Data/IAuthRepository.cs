using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuth.Models;

namespace UserAuth.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User User,string Password);
        Task<User> Login(string Username, string Password);
        Task<bool> IsUserExist(string Username);
    }
}
