using GeoLocator.Entities;
using GeoLocator.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoLocator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CountryController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all countries for all languages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            List<Country> result = null;

            var countries = _context.CountryLocation.ToList();
            if (countries.Any())
            {
                result = new List<Country>();
                foreach (var country in countries)
                {
                    result.Add(new Country
                    {
                        Name = country.country_name,
                        Continent = country.continent_name,
                        LocaleCode = country.locale_code
                    });
                }
            }

            return Ok(result);
        }


        /*
          Notice to get country  for the defined language use
          https://localhost:5001/api/country/185.194.88.0%2F22/en
         
          and result will be like  [{"name":"United Kingdom","localeCode":"en","continent":"Europe"}]

          if you want to get countries for all languages use
          https://localhost:5001/api/country/185.194.88.0%2F22/all

          and result will be like [{"name":"Reino Unido","localeCode":"es","continent":"Europa"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"英国","localeCode":"zh-CN","continent":"欧洲"},{"name":"英国","localeCode":"zh-CN","continent":"欧洲"},{"name":"英国","localeCode":"zh-CN","continent":"欧洲"},{"name":"Royaume-Uni","localeCode":"fr","continent":"Europe"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"Vereinigtes Königreich","localeCode":"de","continent":"Europa"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"英国","localeCode":"zh-CN","continent":"欧洲"},{"name":"Великобритания","localeCode":"ru","continent":"Европа"},{"name":"イギリス","localeCode":"ja","continent":"ヨーロッパ"},{"name":"Reino Unido","localeCode":"pt-BR","continent":"Europa"},{"name":"United Kingdom","localeCode":"en","continent":"Europe"}]
         */

        /// <summary>       
        /// Returns selected country by IP on country for all languages' names
        /// </summary>    
        /// <param name="ip4">encoded query string parameter like 185.194.88.0%2F22</param>
        /// <param name="locale">en, de, fr and etc including all</param>
        /// <returns></returns>
        [HttpGet("{ip4}/{locale}", Name = "Countries_Names")]
        public IActionResult GetCountryByIPv4(string ip4, string locale)
        {
            // In the database we keep 185.194.88.0/22  so query string parameter should be encoded like 185.194.88.0%2F22
            // and here we decode it
            var decodedIP4 = System.Uri.UnescapeDataString(ip4);
            List<Country> result = null;
            //TODO parse decodedIP4 and locale )))

            var item = _context.CountryBlockIPv4.FirstOrDefault(x => x.network.StartsWith(decodedIP4));
            if (item != null)
            {                
                var query = _context.CountryLocation.AsQueryable();
                if (!string.IsNullOrEmpty(ip4) && !string.IsNullOrEmpty(locale) && locale != "all")
                {
                    query = query.Where(x => x.geoname_id == item.geoname_id && x.locale_code == locale);
                }
                if (!string.IsNullOrEmpty(ip4) && !string.IsNullOrEmpty(locale) && locale == "all")
                {
                    query = query.Where(x => x.geoname_id == item.geoname_id);
                }

                var countries = query.ToList();
                if (countries.Any())
                {
                    result = new List<Country>();
                    foreach (var country in countries)
                    {
                        result.Add(new Country
                        {
                            Name = country.country_name,
                            Continent = country.continent_name,
                            LocaleCode = country.locale_code
                        });
                    }
                }
            }

            return Ok(result);
        }
    }
}
