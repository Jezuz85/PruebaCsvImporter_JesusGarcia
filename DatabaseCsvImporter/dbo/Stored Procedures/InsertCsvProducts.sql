-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE InsertCsvProducts
	@UrlFile NVARCHAR(300)
AS
BEGIN 
	declare  @UrlFile2 NVARCHAR(4000)
	set @UrlFile2 =  @UrlFile

	--BULK INSERT 
	--	[CsvImporter].[dbo].[Product]
	--FROM 
	--	@filename
	--WITH (
	--	FIRSTROW = 2,FIELDTERMINATOR= ';',ROWTERMINATOR = '\n'
	--	)

	DECLARE @sql NVARCHAR(4000) = 'BULK INSERT [CsvImporter].[dbo].[Product] FROM ''' + @UrlFile2 + ''' WITH ( FIRSTROW = 2,FIELDTERMINATOR= '';'',ROWTERMINATOR = ''\n'' )';
	EXEC(@sql);

END
