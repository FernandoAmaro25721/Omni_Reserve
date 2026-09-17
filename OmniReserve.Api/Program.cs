var builder = WebApplication.CreateBuilder(args);

// 1. Agregar el servicio generador de Swagger (DI)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Agregar el Middleware en el Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. Middlewares restantes
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();