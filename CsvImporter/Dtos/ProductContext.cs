using Microsoft.EntityFrameworkCore;

namespace CsvImporter.Domain
{
	using Entities;

	public class ProductContext : DbContext
	{
		public DbSet<Products> Products { get; set; }

		public ProductContext(DbContextOptions<ProductContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new Product_TypeConfiguration());
		}
	}
}