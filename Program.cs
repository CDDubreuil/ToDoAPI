using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using ToDoAPIProject.Models;

namespace ToDoAPIProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("OriginPolicy", "http://localhost:7104").AllowAnyMethod().AllowAnyHeader();
                });
            });



            // Add services to the container.
            builder.Services.AddDbContext<ToDoAPIProject.Models.ToDoContext>(
                        options =>
                        {
                            options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoDB"));
                            //The string passed to GetConnectionString() should match the name in appsettings.json
                        }
                        );


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ToDoContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoDB"));
                }
                );

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

            app.UseCors();

            app.Run();
        }
    }
}