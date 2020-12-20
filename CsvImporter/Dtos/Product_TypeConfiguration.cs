using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsvImporter.Domain
{
	using Entities;

	public class Product_TypeConfiguration : IEntityTypeConfiguration<Products>
	{
		public void Configure(EntityTypeBuilder<Products> builder)
		{
			builder.ToTable("Product", "dbo");

			builder.Property(e => e.Product)
				.HasColumnName("Product");
			builder.Property(e => e.PointOfSale)
				.HasColumnName("PointOfSale");
			builder.Property(e => e.Stock)
				.HasColumnName("Stock");
			builder.Property(e => e.Date)
				.HasColumnName("Date");
		}
	}
}