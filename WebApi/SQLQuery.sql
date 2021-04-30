CREATE TABLE Users (
	Id int not null identity(1,1) primary key,
	FirstName nvarchar(50) not null,
	LastName nvarchar(50) not null,
	Email varchar(320) not null,
	UserHash varbinary(max) not null,
	UserSalt varbinary(max) not null
)
GO

CREATE TABLE Cases (
	Id int not null identity(1,1) primary key,
	HandlerId int not null references Users(Id),
	Client nvarchar(50) not null,
	Created date not null,
	Edited date,
	CurrentStatus nvarchar(50) not null
)
GO

Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Lovisa\source\repos\ASP.NET-WebApi\ASP.NET-WebApi-Assignment1\database_webapi.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -Context SqlDbContext -ContextDir Data -OutputDir Models
