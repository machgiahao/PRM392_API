
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SalesAppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("SalesAppDB")));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SalesAppDbContext>();
                db.Database.Migrate(); // apply migrations automatically
            }

            app.MapControllers();

            app.Run();
        }
    }
}
