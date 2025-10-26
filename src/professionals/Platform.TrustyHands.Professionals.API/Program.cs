using Platform.TrustyHands.Professionals.API;

namespace Platform.TrustyHands.Professionals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers()
                .AddDapr(); // Dapr: enables Dapr pub/sub binding attributes

            AppConfig.ConfigureDependencies(builder.Services, builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseCloudEvents(); // Dapr: Required for pub/sub

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapSubscribeHandler(); // Dapr: Required for pub/sub

            app.Run();
        }
    }
}
