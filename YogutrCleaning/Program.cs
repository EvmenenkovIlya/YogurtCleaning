using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YogurtCleaning;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new BadRequestObjectResult(context.ModelState);
            result.StatusCode = StatusCodes.Status422UnprocessableEntity;

            return result;
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddDbContext<YogurtCleaningContext>(o =>
{
    o.UseSqlServer(ServerOptions.ConnectionOption);
});

//builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
builder.Services.AddScoped<IBundlesRepository, BundlesRepository>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IBundlesService, BundlesService>();


builder.Services.AddAutoMapper(typeof(MapperConfigStorage));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
