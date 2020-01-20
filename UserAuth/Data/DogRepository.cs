using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserAuth.Dtos;
using UserAuth.Models;

namespace UserAuth.Data
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;
        public DogRepository(IConfiguration config)
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

        //============================================

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Dog> GetDog(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string dogQuery = @"SELECT * FROM Dogs WHERE id=@Id";
                string photoQuery = @"SELECT * FROM Photos where Did=@Id";
                dbConnection.Open();
                Dog dog = await dbConnection.QueryFirstOrDefaultAsync<Dog>(dogQuery, new { Id = id });
                var photos = await dbConnection.QueryAsync<Photo>(photoQuery, new { Id = id });

                dog.Photos = photos.ToList();

                return dog;
            }
        }

        public async Task<List<DogsToShopList>> GetDogs()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string dogQuery = @"SELECT d.id, d.name, d.DOB, p.photoUrl FROM Dogs d left join Photos p on d.id=p.Did WHERE p.IsMain='true'";
                dbConnection.Open();
                var dog = await dbConnection.QueryAsync<DogsToShopList>(dogQuery);

                return dog.ToList();
            }
        }

        public async Task<Dog> PutDog(Dog dog)
        {
            throw new NotImplementedException();
        }

        public async Task Update(int id)
        {
            throw new NotImplementedException();
        }
    }
}
