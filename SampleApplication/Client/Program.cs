using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MartinGilDemo.Client;
using MartinGilDemo.Client.Services;

namespace MartinGilDemo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            

            builder.RootComponents.Add<MartinGilDemo.Client.App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IHttpService, HttpService>();

            // configure http client
            builder.Services.AddScoped(x =>
            {
                var apiUrl = new Uri("http://localhost:5270");

                // use fake backend if "fakeBackend" is "true" in appsettings.json
                //if (builder.Configuration["fakeBackend"] == "true")
                //    return new HttpClient(new FakeBackendHandler()) { BaseAddress = apiUrl };

                return new HttpClient() { BaseAddress = apiUrl };
            });

            builder.Services.AddScoped<IUserService, UserService>();

            await builder.Build().RunAsync();
        }
    }
}