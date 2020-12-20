-- =============================================
-- Author:		<Author,,Jesus Garcia>
-- Create date: <Create Date,17/12/2020,>
-- Description:	<Description,,Metodo que carga un archivo csv a la tabla products>
-- =============================================
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
