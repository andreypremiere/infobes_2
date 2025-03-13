
using WebApplication.Session;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IRSASession, RSASession>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")  // ��������� ������ � ����� ������
                          .AllowAnyMethod()  // ��������� ��� HTTP ������ (GET, POST, PUT, DELETE � �. �.)
                          .AllowAnyHeader();  // ��������� ��� ���������
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

            app.UseCors("AllowMyOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
