
using FeedbackSystem.Data;
using FeedbackSystem.Mappers;
using FeedbackSystem.Services;
using FeedbackSystem.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddLogging();



            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<ISpecificationService, SpecificationService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback API V1");
            });

            app.UseHttpsRedirection();

            app.MapControllers();


            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        logger.LogError(exceptionHandlerPathFeature.Error, "Unhandled exception occurred.");
                    }
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected error occurred.");
                });
            });

            app.Run();
        }
    }
}
