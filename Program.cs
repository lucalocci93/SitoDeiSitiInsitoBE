using Identity.Interfaces;
using Identity.Models.Mapper;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SitoDeiSiti.DAL;
using SitoDeiSiti.DAL.Interface;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.DTOs.ConfigSettings;
using SitoDeiSiti.DTOs.Mapper;
using SitoDeiSiti.External.SumUp;
using SitoDeiSiti.External.SumUp.Interfaces;
using SitoDeiSiti.Interfaces;
using SitoDeiSiti.Models.ConfigSettings;
using SitoDeiSiti.Services;
using SitoDeiSiti.Utils.HTTPHandlers;
using SitoDeiSitiService.Models.Mapper;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//SWAGGER
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    setup =>
    {
        // Include 'SecurityScheme' to use JWT Authentication
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        { jwtSecurityScheme, Array.Empty<string>() }
        });

    }
);

//CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
string[] origins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          //builder.AllowAnyOrigin();
                          policy.WithOrigins(origins);
                      });
});

//CONFIG
builder.Services.Configure<Token>(builder.Configuration.GetSection("Token"));
builder.Services.Configure<Cache>(builder.Configuration.GetSection("Cache"));
builder.Services.Configure<SumUp>(builder.Configuration.GetSection("SumUp"));
builder.Services.Configure<CORS>(builder.Configuration.GetSection("CORS"));

//CACHE
//builder.Services.AddMemoryCache();
//builder.Services.AddSingleton<ICache, CacheManager>();
builder.Services.AddHybridCache();

//SERVICES
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<AbbonamentoManager>();
builder.Services.AddScoped<DocumentoManager>();
builder.Services.AddScoped<EventiManager>();
builder.Services.AddScoped<SumUpManager>();
builder.Services.AddScoped<SitoManager>();

//AUTH
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Token:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Token:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Token:SecretKey")))
        };
    });

builder.Services.AddAuthorization();

//EF
builder.Services.AddDbContext<SitoDeiSitiInsitoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SitoDeiSitiInsitoDatabase"));
}, ServiceLifetime.Scoped);

//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(AutoMapperUtenteProfile), typeof(AutoMapperAbbonamentoProfile),
    typeof(AutoMapperDocumento), typeof(AutoMapperEvento), typeof(AutoMapperSito));

//HTTP
builder.Services.AddHttpClient<SumUpManager>(options =>
{
    options.DefaultRequestHeaders.Clear();
    options.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
.AddHttpMessageHandler(() => new OAuth2HttpHandler(builder.Configuration.GetValue<string>("SumUp:SumUpAuthUrl"),
    builder.Configuration.GetValue<string>("SumUp:GrantType"), builder.Configuration.GetValue<string>("SumUp:ClientId"),
    builder.Configuration.GetValue<string>("SumUp:ClientSecret")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
