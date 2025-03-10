 install-Package Microsoft.EntityFrameworkCore.SqlServer

  install Microsoft.EntityFrameworkCore.SqlServer        

  Install-Package BCrypt.Net-Next   

  Install-Package Microsoft.EntityFrameworkCore.Tools    
  Install-Package EntityFrameworkCore                    


  Download--

   dotnet tool install --global dotnet-ef         
   dotnet ef migrations add Mig1         

   Change Directory--
   cd ECommerceApp  

DTOs--
   For Migration 
   dotnet ef migrations add Mig1  

   Install-Package Microsoft.EntityFrameworkCore.Design   

   dotnet add package Microsoft.EntityFrameworkCore.Design

   For Update migratioon into Database--

    dotnet ef database update

Connection string
    "EFCoreDBConnection": "Data Source=TEP049SOFT136;Initial Catalog=ECommerceDB;Persist Security Info=False;User ID=sa;Password=Admin@123;TrustServerCertificate=True"