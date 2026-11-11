using Microsoft.EntityFrameworkCore;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Application;
using VakantiePark.Infrastructure;

namespace VakantiePark.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterInfrastructureServices(builder.Configuration, builder.Environment.ContentRootPath);
            builder.Services.RegisterApplicationServices();
            builder.Services.RegisterAPIServices(builder.Configuration, builder.Environment);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ApplicationDBContext>().Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                    options.RouteTemplate = "{documentName}/specifications.json");

                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "api";
                    options.SwaggerEndpoint("/v1/specifications.json", "api");
                });
            }

            app.UseExceptionHandler();

            app.UseCors(CorsSettings.PolicyName);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
    }
}
