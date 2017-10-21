namespace OpenMVVM.Samples.Basic.WebView.DotNetCore
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseWebSockets();
            app.UseSignalR();
        }
    }
}