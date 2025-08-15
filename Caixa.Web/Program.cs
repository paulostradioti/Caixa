using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;

namespace Caixa.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<CaixaDbContext>(option =>
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbPath = Path.Combine(localAppData, "Caixa.db");


                option.UseSqlite($"Data Source={dbPath}")
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            });

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CaixaDbContext>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            context.SeedDatabase();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseStatusCodePagesWithReExecute("/Erros/Status/{0}");
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Turmas}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
