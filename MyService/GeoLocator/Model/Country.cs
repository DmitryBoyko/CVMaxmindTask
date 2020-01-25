using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoLocator.Model
{
    public sealed class Country
    {
        public string Name { get; set; }
        public string LocaleCode { get; set; }
        public string Continent { get; set; }
    }
}
