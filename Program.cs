using Microsoft.EntityFrameworkCore;
using TestAPI.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});*/

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
/*app.UseCors("AllowAllHeaders");*/


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Middleware to handle OPTIONS requests
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return;
    }

    await next();
});
app.UseAuthorization();

app.Run();

