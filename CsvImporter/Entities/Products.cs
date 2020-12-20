using System;

namespace CsvImporter.Entities
{
	/// <summary>
	/// Entity representing the products
	/// </summary>
	public class Products
	{
		public string Product { get; set; }
		public string PointOfSale { get; set; }
		public DateTime Date { get; set; }
		public int Stock { get; set; }
	}
}