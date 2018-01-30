using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TravoryContainers.Services.Flickr.API.Connector;
using TravoryContainers.Services.Flickr.API.Connector.OAuthParameterHandling;
using TravoryContainers.Services.Flickr.API.Helpers;

namespace TravoryContainers.Services.Flickr.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddScoped<IOAuthParameterHandlerFactory, OAuthParameterHandlerFactory>();
            services.AddScoped<IFlickrConnector, FlickrConnector>();


            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Flickr.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flickr.API V1");
            });

            app.UseMvc();
        }
    }
}
