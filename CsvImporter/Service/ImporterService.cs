using CsvImporter.Interfaces.Abstractions;

namespace CsvImporter.Service
{
	public class ImporterService
	{
		private readonly IImporter _importer;

		/// <summary>
		/// class constructor
		/// </summary>
		/// <param name="customer"></param>
		public ImporterService(IImporter customer)
		{
			_importer = customer;
		}

		/// <summary>
		/// method that invokes the csv import
		/// </summary>
		/// <returns>True if the Upload was successful, false otherwise</returns>
		public bool Import()
		{
			return _importer.ImportCsv();
		}
	}
}