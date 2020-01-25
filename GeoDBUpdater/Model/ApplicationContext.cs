using Microsoft.EntityFrameworkCore;

namespace GeoDBUpdater.Model
{
	public class ApplicationContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseNpgsql(Properties.Settings.Default.ConnectionStrings); 

		public DbSet<CountryBlockIPv4> CountryBlockIPv4 { get; set; }

		public DbSet<CountryLocation> CountryLocation { get; set; }
	}
}
