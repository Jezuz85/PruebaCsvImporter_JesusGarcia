using CsvImporter;
using CsvImporter.Domain;
using CsvImporter.Facade.Implementation;
using CsvImporter.Interfaces.Abstractions;
using CsvImporter.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NUnit.Framework;
using System;
using System.IO;

namespace CsvImporterTest
{
	public class ImporterCsvTest
	{
		private static IServiceProvider _serviceProvider;

		[SetUp]
		public void Setup()
		{
			RegisterServices();
		}

		private void RegisterServices()
		{
			var builder = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
			   .AddEnvironmentVariables();
			IConfiguration config = builder.Build();

			var services = new ServiceCollection();
			services.AddScoped<IImporter, Importer>();
			services.AddScoped<ImporterService>();
			services.AddLogging();
			services.Configure<AppSettings>(config.GetSection("AppSettings"));
			services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(config.GetSection("AppSettings:ConectionDB").Value));
			_serviceProvider = services.BuildServiceProvider(true);

			var factory = _serviceProvider.GetService<ILoggerFactory>();
			factory.AddNLog();
			factory.ConfigureNLog("nlog.config");
		}

		private void DisposeServices()
		{
			if (_serviceProvider == null)
			{
				return;
			}
			if (_serviceProvider is IDisposable)
			{
				((IDisposable)_serviceProvider).Dispose();
			}
		}

		[Test]
		public void ImportCsvTest()
		{
			try
			{
				IServiceScope scope = _serviceProvider.CreateScope();
				bool result = scope.ServiceProvider.GetRequiredService<ImporterService>().Import();
				DisposeServices();

				Assert.IsTrue(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}