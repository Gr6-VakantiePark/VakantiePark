using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VakantiePark.Shared.Common;
using VakantiePark.UI.Authentication;
using VakantiePark.UI.Interfaces;
using VakantiePark.UI.Services;

namespace VakantiePark.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddAuthorizationCore(options =>
            {
                foreach (string permission in Permissions.All)
                    options.AddPolicy(permission, policy => policy.RequireClaim(Permissions.ClaimType, permission));
                options.AddPolicy(Permissions.Owner, policy => policy.RequireClaim(Permissions.OwnerClaimType, "true"));
            });
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<SessionStorageService>();
            builder.Services.AddScoped<AuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>());
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped<ISearchService, ApiSearchService>();
            builder.Services.AddScoped<IBookingService, ApiBookingService>();
            builder.Services.AddScoped<BookingState>();
            builder.Services.AddScoped<IAccountService, ApiAccountService>();
            builder.Services.AddTransient<AuthHandler>();

            string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7037/";
            var apiBaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), apiBaseUrl);

            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<AuthHandler>();
                handler.InnerHandler = new HttpClientHandler();
                return new HttpClient(handler) { BaseAddress = apiBaseAddress };
            });

            await builder.Build().RunAsync();
        }
    }
}
