using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Data;
using ProductCatalog.Repositoreis;
using ZNetCS.AspNetCore.Compression.DependencyInjection;

namespace ProductCatalog
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Objetos de escopo (AddScoped) s�o os mesmos em uma solicita��o, 
            //mas diferentes entre solicita��es diferentes.;
            services.AddScoped<StoreDataContext, StoreDataContext>();

            //Objetos transientes (AddTransient) s�o sempre diferentes. 
            //uma nova inst�ncia � fornecida para todos os controladores e todos os servi�os.
            services.AddTransient<ProductRepository, ProductRepository>();
            services.AddTransient<CategoryRepository, CategoryRepository>();

            //adicionando o middleware para o servi�o de compress�o
            services.AddResponseCompression();
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


        }
    }
}
