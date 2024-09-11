using Microsoft.EntityFrameworkCore;

namespace SQL_OLA_A2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        // Add the ApplicationDbContext with SQL Server configuration
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register TaskFacade as a scoped service
        builder.Services.AddScoped<TaskFacade>();

        // Add controllers to the service container
        builder.Services.AddControllers();

        // Add authorization service
        builder.Services.AddAuthorization();

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

        // Map controllers to handle API requests
        app.MapControllers();

        app.Run();
    }
}