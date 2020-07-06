using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalog.Data;
using ProductCatalog.Repositoreis;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.Swagger;

namespace ProductCatalog
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //adicionando o middleware para o serviço de compressão
            services.AddResponseCompression();

            //Objetos de escopo (AddScoped) são os mesmos em uma solicitação, 
            //mas diferentes entre solicitações diferentes.;
            services.AddScoped<StoreDataContext, StoreDataContext>();

            //Objetos transientes (AddTransient) são sempre diferentes. 
            //uma nova instância é fornecida para todos os controladores e todos os serviços.
            services.AddTransient<ProductRepository, ProductRepository>();
            services.AddTransient<CategoryRepository, CategoryRepository>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api Product Category", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "my api - v1");
            });


        }
    }
}
