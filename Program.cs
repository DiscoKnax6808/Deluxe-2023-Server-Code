using DeluxeNET.Data;
using DeluxeNET.Hubs;
using DeluxeNET.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeluxeNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            });

            builder.Services.AddSingleton<jwt>();
            builder.Services.AddSingleton<NotificationHub>();
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.MapHub<NotificationHub>("/hub/v1");

            app.Use(async (ctx, next) =>
            {
                await next();

                var status = ctx.Response.StatusCode;
                var ct = ctx.Request.ContentType?.ToLower() ?? "";

                var type = ctx.Request.Method == "POST"
                    ? (ct.Contains("application/json") ? "JSON"
                    : ct.Contains("application/x-www-form-urlencoded") || ct.Contains("multipart/form-data") ? "FORM"
                    : ct.StartsWith("image/") ? "IMAGE"
                    : "BODY")
                    : "N/A";
                var url = ctx.Request.Path + ctx.Request.QueryString.Value;

                ConsoleColor color =
                    status >= 200 && status < 300 ? ConsoleColor.Green :
                    status >= 300 && status < 400 ? ConsoleColor.Yellow :
                    status >= 400 && status < 500 ? ConsoleColor.Red :
                    status >= 500 ? ConsoleColor.DarkRed :
                    ConsoleColor.Gray;

                Console.ForegroundColor = color;
                Console.WriteLine($"{status} {ctx.Request.Method} {url} [{type}]");
                Console.ResetColor();
            });

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
