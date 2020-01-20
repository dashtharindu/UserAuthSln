using System;
using System.Collections.Generic;
using System.Linq;
using UserAuth.Models;
using System.Threading.Tasks;
using UserAuth.Dtos;

namespace UserAuth.Data
{
    public interface IDogRepository
    {
        Task<List<DogsToShopList>> GetDogs();
        Task<Dog> GetDog(int id);
        Task<Dog> PutDog(Dog dog);
        Task Update(int id);
        Task Delete(int id);
    }
}
