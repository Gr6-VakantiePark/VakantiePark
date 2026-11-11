
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
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
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "VakantiePark API",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });

            string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? ["https://localhost:7158"];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("frontend", policy =>
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
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

            app.UseExceptionHandler("/error");

            app.UseCors("frontend");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
