namespace CsvImporter.Interfaces.Abstractions
{
	/// <summary>
	/// service importer interface
	/// </summary>
	public interface IImporter
	{
		/// <summary>
		/// Method that performs an import of product data to the database
		/// </summary>
		bool ImportCsv();
	}
}