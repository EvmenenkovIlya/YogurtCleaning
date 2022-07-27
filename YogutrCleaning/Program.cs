using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using YogurtCleaning.API;
using YogurtCleaning.Business.Infrastrucure;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new BadRequestObjectResult(context.ModelState);
            result.StatusCode = StatusCodes.Status422UnprocessableEntity;

            return result;
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "YogurtCleaning", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorization: Bearer JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
               });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            SaveSigninToken = true,
        };
    });
builder.Services.AddDbContext<YogurtCleaningContext>(o =>
{
    o.UseSqlServer(ServerOptions.ConnectionOption);
});

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<ICleanersRepository, CleanersRepository>();
builder.Services.AddScoped<ICleaningObjectsRepository, CleaningObjectsRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
builder.Services.AddScoped<IBundlesRepository, BundlesRepository>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IBundlesService, BundlesService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IAdminsRepository, AdminsRepository>();

builder.Services.AddAutoMapper(typeof(MapperConfigStorage));

builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<ICleanersService, CleanersService>();
builder.Services.AddScoped<ICleaningObjectsService, CleaningObjectsService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();