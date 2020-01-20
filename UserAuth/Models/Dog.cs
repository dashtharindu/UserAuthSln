using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuth.Models
{
    public class Dog
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime DOB { get; set; }

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
        public DateTime DateCreated { get; set; } //filled by DB auto
        public List<Photo> Photos { get; set; }
    }
}
