dotnet ef dbcontext scaffold "Data Source=localhost\MSSQLSERVER0;Initial Catalog=databaseName;User Id=my_user;Password=my_password;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --no-onconfiguring -d -o Model --context-dir Context -f --no-build