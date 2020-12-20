# Test Jesús García

Este es el código fuente de la prueba técnica para el proceso de entrevista del candidato Jesús García y la empresa Selecta Digital para el cliente **Analytic Always**, que contiene lo siguiente:

  - Una aplicación de consola para la importación de un archivo .csv a la base de datos.
  - Un Api Gateway 
  - Un proyecto de Base de Datos.
 
## 1. Funcionalidades a evaluar
  - Tienes que desarrollar un programa de consola .NET Core en C#, que lea un fichero .csv almacenado en una cuenta de almacenamiento de Azure e inserte su contenido en un BD SQL Server local.
  - La empresa que quiere el programa se llama Acme Corporation (un clásico) y tiene decidido que el nombre del programa sea CsvImporter. Además, le gustaría que el código esté subido a un repositorio de github con el nombre PruebaCsvImporter_<Autor>.
  - El fichero .csv está disponible en https://storage10082020.blob.core.windows.net/y9ne9ilzmfld/Stock.CSV
  - Antes de insertar en la BD, tendrás que eliminar el contenido de una posible previa importación.
  - Además de un código bien escrito, siguiendo las mejoras prácticas, nos importa el tiempo de proceso y el consumo de recursos (RAM, CPU, etc.) ¡tenlo en cuenta!
  - En la inserción puedes asumir que no es necesaria una transacción.
  - Si usas sabiamente el framework, te ayudará con la configuración, registro de dependencias, logging, etc.
  - Por supuesto, si acompañas tu código de testing, serás nuestro mejor amigo.
  - Podrás usar las librerías que creas oportuno.
  - También nos gustaría saber el porqué de las decisiones que tomaste (y también de las que descartaste).
  - Si acompañas el proyecto con un buen README.md, ¡nos harías muy felices!
  - Por último, si consideras necesario agregar algo de testing automatizado para ganar más confianza, ¡nunca viene mal!

## 2. Patrón de Diseño

Para este proyecto utilizamos la inyección de dependencias para el consumo de los servicios que se utiliza en la carga del archivo csv, se escogio por ser uno de los principios SOLID en donde se busca disminuir el acoplamiento en un software, y tambien .NetCore esta orientado al desarrollo en este patron.

Según la definición de Wikipedia:
> Inyección de dependencias (en inglés Dependency Injection, DI) es un patrón de diseño orientado a objetos, en el que se suministran objetos a una clase en lugar de ser la propia clase la que cree dichos objetos. Esos objetos cumplen contratos que necesitan nuestras clases para poder funcionar (de ahí el concepto de dependencia). Nuestras clases no crean los objetos que necesitan, sino que se los suministra otra clase 'contenedora' que inyectará la implementación deseada a nuestro contrato.

## 4. Base de Datos

La base de datos a la cual se accede esta en un repositorio en Azure, a continuación se deja el script para la creación de las tablas que se usaron en este proyecto.

```sh
/****** Object:  Table [dbo].[Product] ******/
CREATE TABLE [dbo].[Product] (
    [Product]     VARCHAR (50) NULL,
    [PointOfSale] VARCHAR (20) NULL,
    [Date]        DATE         NULL,
    [Stock]       INT          NULL
);

CREATE PROCEDURE [dbo].[InsertCsvProducts]
	@UrlFile NVARCHAR(300)
AS
BEGIN 
	declare  @UrlFile2 NVARCHAR(4000)
	set @UrlFile2 =  @UrlFile

	TRUNCATE TABLE [CsvImporter].[dbo].[Product]
	DECLARE @sql NVARCHAR(4000) = 'BULK INSERT [CsvImporter].[dbo].[Product] FROM ''' + @UrlFile2 + ''' WITH ( FIRSTROW = 2,FIELDTERMINATOR= '';'',ROWTERMINATOR = ''\n'' )';
	EXEC(@sql);

END

```

## 5. Tecnologías Utilizadas
* [.Net Core](https://docs.microsoft.com/en-us/dotnet/core/)
* [Nlog](https://nlog-project.org/)
* [NUnit](https://docs.nunit.org/)

## 6. Ejecución de la aplicación.

Al ejecutar la aplicación hará lo siguiente:

- Descargará el archivo Csv desde la Url remota a la Url Local de nuestra maquina
- Ejecuta el Procedimiento Almacenado que realiza el Bulk en la base de datos ya que es la manera más eficiente de subir gran cantidad de datos a la base de datos,
- Muestra pro pantalla el uso de memoria y RAM y uso de CPU de la aplicación
- se registran los logs del resultado de la operación.
- el archivo descargado, queda guardado con su fecha de descarga.
 
Datos de Configuración
```sh
"UrlfileRemote" : "https://storage10082020.blob.core.windows.net/y9ne9ilzmfld/Stock.CSV",

"UrlfileLocal": "C:\\Users\\User\\Documents\\TestJesusGarcia\\CsvImporter\\Files\\Stock",

"ConectionDB": "Server=DESKTOP-CRH25O6\\SQL_PRUEBAS;Database=CsvImporter;Integrated Security=True" 
```
