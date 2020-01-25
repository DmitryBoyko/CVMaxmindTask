using GeoLocator.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoLocator
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options)
				: base(options)
		{
		}

		public DbSet<CountryBlockIPv4> CountryBlockIPv4 { get; set; }

		public DbSet<CountryLocation> CountryLocation { get; set; }
	}
}
