namespace CsvImporter
{
	/// <summary>
	/// Class that handles the configuration of a Importer Program
	/// </summary>
	public class AppSettings
	{
		/// <summary>
		/// string that represents the connection to the database
		/// </summary>
		public string ConectionDB { get; set; }

		/// <summary>
		/// string representing the URL of the remote CSV file
		/// </summary>
		public string UrlfileRemote { get; set; }

		/// <summary>
		/// string representing the URL of the local CSV file
		/// </summary>
		public string UrlfileLocal { get; set; }
	}
}