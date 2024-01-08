using Microsoft.EntityFrameworkCore;
using WebApplication1.DbContexts;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<MyImageDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlserverConnection")));

            var app = builder.Build();
            app.MapControllerRoute(
                "Default", "{controller=User}/{action=GetMainPage}/{id?}");

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}