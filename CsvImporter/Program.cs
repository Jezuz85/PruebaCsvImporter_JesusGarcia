using CsvImporter.Domain;
using CsvImporter.Facade.Implementation;
using CsvImporter.Interfaces.Abstractions;
using CsvImporter.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace CsvImporter
{
	internal class Program
	{
		private static IServiceProvider _serviceProvider;

		/// <summary>
		/// starting point of the program
		/// </summary> 
		private static void Main(string[] args)
		{
			RegisterServices();

			IServiceScope scope = _serviceProvider.CreateScope();
			scope.ServiceProvider.GetRequiredService<ImporterService>().Import();

			DisposeServices();
		}

		/// <summary>
		/// method that registers the services in the dependency container
		/// </summary>
		private static void RegisterServices()
		{
			//Creamos los valores de configuracion de la aplicacion
			IConfigurationBuilder builder = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
			   .AddEnvironmentVariables();
			IConfiguration config = builder.Build();

			//Registramos los servicios que usaremos en la aplicacion
			ServiceCollection services = new ServiceCollection();
			services.AddScoped<IImporter, Importer>();
			services.AddScoped<ImporterService>();
			services.AddLogging();
			services.Configure<AppSettings>(config.GetSection("AppSettings"));
			services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(config.GetSection("AppSettings:ConectionDB").Value));
			_serviceProvider = services.BuildServiceProvider(true);

			// se configura el sistema de registro y crear instancias de ILogger a partir de los ILoggerProvider registrados en este caso Nlog
			ILoggerFactory factory = _serviceProvider.GetService<ILoggerFactory>();
			factory.AddNLog();
			factory.ConfigureNLog("nlog.config");
		}

		/// <summary>
		/// Method that frees resources from ServiceProvider
		/// </summary>
		private static void DisposeServices()
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
	}
}