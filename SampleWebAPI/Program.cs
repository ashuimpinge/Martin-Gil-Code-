
namespace MartinGilDemoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5270",
                                            "https://localhost:7011");
                        policy.AllowAnyMethod();
                        policy.AllowAnyHeader();
                    });
            });

            // Add services to the container.

            builder.Services.AddControllers();
             
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<AppDb>(_ => new AppDb(builder.Configuration["ConnectionStrings:DefaultConnection"]));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors();
            app.MapControllers();

            app.Run();
        }
    }
}