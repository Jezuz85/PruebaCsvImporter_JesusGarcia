using CsvImporter.Domain;
using CsvImporter.Interfaces.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net;
using static System.DateTime;

namespace CsvImporter.Facade.Implementation
{
	/// <summary>
	/// Class that contains the logic of the import of the csv file
	/// </summary>
	public class Importer : IImporter
	{
		private readonly ILogger<Importer> _logger;
		private readonly ProductContext _context;
		private readonly AppSettings _appSettings;

		/// <summary>
		/// Class Constructor
		/// </summary>
		public Importer(ProductContext context, ILogger<Importer> logger, IOptions<AppSettings> appSettings)
		{
			_logger = logger;
			_context = context;
			_appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
		}

		/// <summary>
		/// Method that performs an import of product data to the database
		/// </summary>
		public bool ImportCsv()
		{
			Console.WriteLine("*******************************************");
			Console.WriteLine("*              CsvImporter                *");
			Console.WriteLine("*             Jesus - Garcia              *");
			Console.WriteLine("*                                         *");
			Console.WriteLine("*******************************************");

			try
			{
				var urlfileLocal = _appSettings.UrlfileLocal + "_" + Now.ToString("yyyy'_'MM'_'dd'T'HH'_'mm'_'ss") + ".csv";
				var urlfileRemote = _appSettings.UrlfileRemote;
				var rowsInserted = 0;

#if DEBUG
				Process[] pr = Process.GetProcessesByName("vstest.console");//vstest.console VsDebugConsole
				PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", pr[0].ProcessName);
				PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
#else
				Process[] pr = Process.GetProcessesByName("CsvImporter");
				PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set", pr[0].ProcessName);
				PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
#endif

				if (DownloadFile(urlfileLocal, urlfileRemote))
				{
					PrintUsageCpuRam(ramCounter, cpuCounter);
					PrintLogConsole($"Start Uploading at {Now}");

					rowsInserted = _context.Database.ExecuteSqlRaw("EXEC [dbo].[InsertCsvProducts] @UrlFile = '" + urlfileLocal + "'");

					PrintLogConsole($"Total Rows Inserted {rowsInserted:n0}");
					PrintLogConsole($"Uploading Finished at {Now}");
					PrintUsageCpuRam(ramCounter, cpuCounter);
				}

				//Call Garbage Collector
				GC.Collect();
				GC.WaitForPendingFinalizers();

				return rowsInserted > 0;
			}
			catch (Exception ex)
			{
				MlsConsoleException(ex);
				return false;
			}
		}

		/// <summary>
		/// Method that obtains the CPU and RAM values and displays them on the screen and in the log
		/// </summary>
		/// <param name="ramCounter">Value RAM</param>
		/// <param name="cpuCounter">Value CPU</param>
		private void PrintUsageCpuRam(PerformanceCounter ramCounter, PerformanceCounter cpuCounter)
		{
			var ramUso = ramCounter.NextValue() / 1024 / 1024;
			var cpuUso = cpuCounter.NextValue();
			PrintLogConsole($"^ RAM: {ramUso} MB; CPU: {cpuUso} %");
		}

		/// <summary>
		/// Method that gets CSV file from remote url and downloads it to local url
		/// </summary>
		/// <param name="urlfileLocal">url remote file</param>
		/// <param name="urlfileRemote">url local file</param>
		/// <returns></returns>
		private bool DownloadFile(string urlfileLocal, string urlfileRemote)
		{
			if (!string.IsNullOrEmpty(urlfileLocal) && !string.IsNullOrEmpty(urlfileRemote))
			{
				try
				{
					PrintLogConsole($"Start Downloading at {Now}");

					WebClient webClient = new WebClient();
					webClient.DownloadFile(urlfileRemote, urlfileLocal);

					PrintLogConsole($"Downloading Finished at {Now}");

					return true;
				}
				catch (Exception ex)
				{
					MlsConsoleException(ex);
					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Method that prints a message on the screen and in the log
		/// </summary>
		/// <param name="message">message to print</param>
		private void PrintLogConsole(string message)
		{
			_logger.LogInformation(message);
			Console.WriteLine(message);
		}

		/// <summary>
		/// Method that prints on screen and in the log when an exception occurs
		/// </summary>
		/// <param name="ex">exception to show</param>
		public void MlsConsoleException(Exception ex)
		{
			_logger.LogInformation($"\n\r.Exception Message= :{ex.Message}:.\n\r");
			_logger.LogInformation($"\n\r.Exception InnerException= :{ex.InnerException}:.\n\r");
			_logger.LogInformation($"\n\r.Exception StackTrace= :{ex.StackTrace}:.\n\r");
		}
	}
}