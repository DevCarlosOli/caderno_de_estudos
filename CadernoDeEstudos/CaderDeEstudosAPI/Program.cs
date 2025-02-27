using CaderDeEstudosAPI.Application.Services;
using CaderDeEstudosAPI.Application.Services.Interfaces;
using CaderDeEstudosAPI.Infra.DbContext;
using CaderDeEstudosAPI.Infra.Mappers;
using CaderDeEstudosAPI.Infra.Repositories;
using CaderDeEstudosAPI.Infra.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add infra datas
builder.Services.AddSingleton<DbPostegreSql>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddScoped<ICadernoRepository, CadernoRepository>();
builder.Services.AddScoped<ICadernoService, CadernoService>();

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
