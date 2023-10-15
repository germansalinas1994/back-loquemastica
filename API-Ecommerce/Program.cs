using BussinessLogic.Services;
using DataAccess.Repository;
using DataAccess.Entities;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.

// ADD Entity framework con mysql

builder.Services.AddDbContext<EcommercedbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("dbConnection")));

//agrego la inyeccion de dependencia de los repositorios y el UnitOfWork

//El AddScoped es para que se cree un nuevo contexto cada vez que se haga un request
//El addTransient es para que se cree un nuevo contexto cada vez que se llame a la clase
//La diferencia entre AddScoped y AddTransient es que el AddScoped crea un contexto por cada request y el AddTransient crea un contexto por cada vez que se lo llama
//El AddSingleton es para que se cree un contexto por una unica vez y se reutilice en todos los request

builder.Services.AddScoped<ServiceCategoria>();
builder.Services.AddScoped<ServiceProducto>();
builder.Services.AddScoped<ServicePublicacion>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("politica", app =>
    {
        app.AllowAnyOrigin();
        app.AllowAnyHeader();
        app.AllowAnyMethod();
    });
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EcommercedbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//habilito los cors

app.UseCors("politica");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

