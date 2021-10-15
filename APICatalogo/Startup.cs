using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using APICatalogo.Filters;
using APICatalogo.Repository;

namespace APICatalogo
{
    /*
        Model Bidings: 
        [BindRequired]: For�a a entrada de parametro. 
            Ex: public ActionResult<Produto> Get(int id, [BindRequired] string nome);
        [BindNever]: N�o vincula a info ao parametro. � utilizado nas propriedades das classes models.

        Fonte de dados dos parametros, o uso � semelhante ao BindRequired:
        [FromForm],
        [FromRoute],
        [FromQuery],
        [FromHeader],
        [FromBody],
        [FromServices]
        
     */

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["ConexaoSqlite:SqliteConnectionString"];

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connection)
            );
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });
            });

            // dotnet core 6+
            // services.AddControllers().AddJsonOptions(x =>
            //     x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // dotnet core 5
            services.AddControllers().AddJsonOptions(x =>
               x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddScoped<ApiLoggingFilter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // pipeline de Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICatalogo v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
            }            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
