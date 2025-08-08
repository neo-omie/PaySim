using PaySlip.Persistence;
using PaySlip.Application;

namespace PaySim.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowRazorPages",
                builder =>
                {
                    builder.WithOrigins("https://localhost:7065").AllowAnyHeader().AllowAnyMethod(); // Razor Page URL
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowRazorPages");

            app.MapControllers();

            app.Run();
        }
    }
}
