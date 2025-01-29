using Identity.DTOs.ConfigSettings;
using Identity.Models;
using Identity.Models.ConfigSettings;
using Identity.Models.Mapper;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SitoDeiSiti.DAL.Models;
using SitoDeiSitiService.Models.Mapper;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyHeader();
                          builder.AllowAnyMethod();
                          //builder.AllowAnyOrigin();
                          builder.WithOrigins("http://localhost:4200");
                          builder.WithOrigins("http://localhost:81");
                      });
});

builder.Services.Configure<Token>(builder.Configuration.GetSection("Token"));
builder.Services.Configure<Cache>(builder.Configuration.GetSection("Cache"));

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheManager>();

builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<AbbonamentoManager>();
builder.Services.AddScoped<DocumentoManager>();
builder.Services.AddScoped<EventiManager>();

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

builder.Services.AddDbContext<SitoDeiSitiInsitoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SitoDeiSitiInsitoDatabase"));
}, ServiceLifetime.Transient);

builder.Services.AddAutoMapper(typeof(AutoMapperUtenteProfile), typeof(AutoMapperAbbonamentoProfile),
    typeof(AutoMapperDocumento), typeof(AutoMapperEvento));

builder.Services.AddAuthorization();

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
