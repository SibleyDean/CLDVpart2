using Azure.Storage.Blobs;
using eventEasefour;
using eventEasefour.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ? Configure Services (Database)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();

            // ? Blob Storage Configuration
            string? blobConnectionString = builder.Configuration["AzureBlobStorage:ConnectionString"];
            if (string.IsNullOrWhiteSpace(blobConnectionString))
            {
                throw new InvalidOperationException("Azure Blob Storage connection string is not configured");
            }

            string? containerName = builder.Configuration["AzureBlobStorage:ContainerName"] ?? "cldvimages";

            // ? Register BlobContainerClient
            builder.Services.AddSingleton(provider =>
            {
                try
                {
                    return new BlobContainerClient(blobConnectionString, containerName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to initialize BlobContainerClient", ex);
                }
            });

            // ? Register BlobStorageService
            builder.Services.AddScoped<BlobStorageService>();

            // ? Build Application
            var app = builder.Build();

            // ? Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // ? Detailed Error Page in Development
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // ? User-Friendly Error Page in Production
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // ? Enhanced Error Handling (Error Page for Status Codes)
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            // ? Custom Route for Booking Details View
            app.MapControllerRoute(
                name: "BookingDetailsView",
                pattern: "{controller=Bookings}/{action=DetailsView}/{id?}");

            // ? Default Route Configuration
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
