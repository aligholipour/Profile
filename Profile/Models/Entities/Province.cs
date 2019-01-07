using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profile.Models.Entities
{
    public class Province
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
