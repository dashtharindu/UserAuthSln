using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserAuth.Models;

namespace UserAuth.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;
        public AuthRepository(IConfiguration config)
        {
            _config = config;
        }

        //property
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("MyConnectionString"));
            }
        }

        //FOR IsUserExist==================================================================================================

        public async Task<bool> IsUserExist(string username)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var user = await GetUserByUsernameFromDb(username);


                if (user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            
        }

        //=================================================================================================================


        //FOR LOGIN========================================================================================================

        public async Task<User> Login(string username, string Password)
        {
            var user =await GetUserByUsernameFromDb(username);

            if (user == null)
            {
                return null;
            }

            if (!IsValidPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        private async Task<User> GetUserByUsernameFromDb(string username)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"SELECT * FROM userinfobytes WHERE Username=@Username";
                dbConnection.Open();
                var result = await dbConnection.QueryFirstOrDefaultAsync<User>(sQuery, new { Username = username });

                return result;
            }
        }

        private bool IsValidPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        //=================================================================================================================


        //FOR REGISTRATION=================================================================================================
        public async Task<User> Register(User User, string Password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(Password,out passwordHash,out passwordSalt);

            User.PasswordHash = passwordHash;
            User.PasswordSalt = passwordSalt;

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = @"INSERT INTO userinfobytes (Username,PasswordHash,PasswordSalt) VALUES (@Username,@PasswordHash,@PasswordSalt)";
                dbConnection.Open();
                await dbConnection.ExecuteAsync(sQuery, User);
            }

            return User;
        }

        //this CreatePasswordHash() method used in above register() method
        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        //=================================================================================================================
    }
}
