namespace Api.Utils
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DataUtils
    {

        public const string CONNECTION_STRING_KEY = "DefaultConnection";

        public const string CONTENT_ROOT_PLACE_HOLDER = "%CONTENTROOTPATH%";

        public static string GetDbConnectionString(IConfiguration Configuration, string contentRootPath, string connectionStringKey = CONNECTION_STRING_KEY)
        {
            contentRootPath = contentRootPath.Replace("Api", "Data");
            var connectionString = Configuration.GetConnectionString(connectionStringKey);

            if (connectionString is not null && connectionString.Contains(CONTENT_ROOT_PLACE_HOLDER))
            {
                connectionString = connectionString.Replace(CONTENT_ROOT_PLACE_HOLDER, contentRootPath);
            }
            return connectionString;
        }
        public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<T>();
            context.Database.EnsureCreated();
        }
    }
}
