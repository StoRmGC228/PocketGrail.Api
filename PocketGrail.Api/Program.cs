using PocketGrail.Api.Hubs;
using PocketGrail.Infrastructure.InfConfiguration;

namespace PocketGrail.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .SetIsOriginAllowed(origin =>
                        {
                            var host = new Uri(origin).Host;
                            return host == "localhost"
                                || host.EndsWith(".ngrok-free.app")
                                || host.EndsWith(".ngrok.io");
                        })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("FrontendPolicy");

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<SessionHub>("/hubs/session");

            app.Run();
        }
    }
}