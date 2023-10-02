using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using YBS.Data.Context;
using YBS.Data.Repositories.Implements;
using YBS.Data.Repositories.Interfaces;
using YBS.Services.AutoMapper;
using YBS.Services.Implements;
using YBS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<YBSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YBSContext")));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
