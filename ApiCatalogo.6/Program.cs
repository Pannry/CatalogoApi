using ApiCatalogo._6.Context;
using ApiCatalogo._6.DTOs.Mappings;
using ApiCatalogo._6.Filters;
using ApiCatalogo._6.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

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

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
        ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    });

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });

    var jwtConfig = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Adicione o 'Bearer ' e o token para autenticar",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition("Bearer", jwtConfig);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{ { jwtConfig, Array.Empty<string>() } });
});

// Automapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Cors
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirApiRequest", opt => 
        opt.WithOrigins("https://www.exemplo.com/")
           .WithMethods("GET"));

    options.AddPolicy("PermitirApiPOST", opt => 
        opt.WithOrigins("https://www.exemplo.com/")
           .WithMethods("POST"));

    // Para utilizar as politicas, vá nos controllers e adicione o atributo:
    // [EnableCors("PermitirApiRequest")] ou [EnableCors("PermitirApiPOST")]
    // Pode ser adicionado a classe ou método
});
*/

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

// app.UseCors();
// app.UseCors(opt => opt.AllowAnyOrigin()); // Permite qualquer host de navegador acessar

app.Run();
