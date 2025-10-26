using Platform.TrustyHands.Professionals.API;

namespace Platform.TrustyHands.Professionals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AppConfig.ConfigureDependencies(builder.Services, builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers()
                .AddDapr(); // Dapr: enables Dapr pub/sub binding attributes
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCloudEvents();
            app.MapSubscribeHandler();

            app.UseAuthorization();

            app.MapControllers(); // Map controller endpoints

            app.Run();
        }
    }
}
