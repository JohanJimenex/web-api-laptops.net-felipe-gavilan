using Microsoft.EntityFrameworkCore;
using MiWebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configuracion de CORS
builder.Services.AddCors(opciones => {
    opciones.AddDefaultPolicy(politica => {
        //obtenemos los valores de el appsettings.json si hay varios dibidiospor coma
        var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");
        politica.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
});

// inyectamos el contexto de la base de datos para poder usarlo en los controladores de la siguiente forma: 
// private readonly ApplicationDbContext _context;
// public MiNombreController(ApplicationDbContext context)
// {
//     _context = context;
// }
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { app.MapOpenApi(); }
app.UseHttpsRedirection();
app.UseCors();//agregamos el cors
app.UseAuthorization();
app.MapControllers();
app.Run();
