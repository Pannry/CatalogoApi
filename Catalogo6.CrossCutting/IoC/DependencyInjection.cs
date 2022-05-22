using AutoMapper;
using Catalogo6.Application.Interfaces;
using Catalogo6.Application.Mappings;
using Catalogo6.Application.Services;
using Catalogo6.Domain.Interfaces;
using Catalogo6.Infrastructure.Context;
using Catalogo6.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo6.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {

            string SqlConnection = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(SqlConnection);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(SqlConnection));

            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<ICategoriaService, CategoriaService>();


            // Automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToDTOMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
