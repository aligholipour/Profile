using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profile.Models.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
    }
}
