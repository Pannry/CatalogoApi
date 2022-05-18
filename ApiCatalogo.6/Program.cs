using Microsoft.EntityFrameworkCore;
using ApiCatalogo._6.Context;
using System.Text.Json.Serialization;
using ApiCatalogo._6.Filters;
using ApiCatalogo._6.Repository;
using AutoMapper;
using ApiCatalogo._6.DTOs.Mappings;

/*
    Model Bidings: 
    [BindRequired]: Força a entrada de parametro. 
        Ex: public ActionResult<Produto> Get(int id, [BindRequired] string nome);
    [BindNever]: Não vincula a info ao parametro. É utilizado nas propriedades das classes models.

    Fonte de dados dos parametros, o uso é semelhante ao BindRequired:
    [FromForm],
    [FromRoute],
    [FromQuery],
    [FromHeader],
    [FromBody],
    [FromServices]

 */

/*
    ~~ Configurações ~~
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do banco de dados
string SqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(SqlConnection);

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(SqlConnection)
);

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

/*
    ~~ Middlewares ~~
 */

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-local-development");
}
else
{
    app.UseExceptionHandler("/error");
}

// Zera o banco de dados ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
