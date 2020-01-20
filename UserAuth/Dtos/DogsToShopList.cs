using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuth.Models;

namespace UserAuth.Dtos
{
    public class DogsToShopList
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime DOB { set; private get; }
        
        public int Age
        {
            get
            {
                var ageGetter = DateTime.Today.Year - DOB.Year;
                if (DOB.AddYears(ageGetter) > DateTime.Today)
                {
                    ageGetter--;
                }
                return ageGetter;
            }
        }
        public string photoUrl { get; set; }
    }
}
