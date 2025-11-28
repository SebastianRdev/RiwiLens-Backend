// File: RiwiLens.Infrastructure/Data/ApplicationDbContextFactory.cs

using System.IO;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RiwiLens.Infrastructure.Data;

namespace RiwiLens.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Load .env manually (EF Core design-time does NOT load Program.cs)
            var basePath = Directory.GetCurrentDirectory();

            // Busca el .env en Infrastructure
            var envPath = Path.Combine(basePath, ".env");

            if (File.Exists(envPath))
            {
                Env.Load(envPath);
            }

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("DB_CONNECTION_STRING is missing in .env for design time.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Provider: PostgreSQL
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
