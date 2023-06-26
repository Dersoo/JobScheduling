using JobSchedulingApi.Services.JobServices;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using JobSchedulingApi.Services.EmailServices;
using JobSchedulingApi.Jobs;
using JobSchedulingApi.Models;
using JobSchedulingApi.Services.JobServices.JobManagementServices;
using JobSchedulingApi.Services.JobServices.CronConvertingServices;

namespace JobSchedulingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHostedService<QuartzHostedService>();
            builder.Services.AddSingleton<IJobFactory, JobFactory>();
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            builder.Services.AddSingleton<EmailingJob>();
            //builder.Services.AddSingleton(new JobSchedule(type: typeof(EmailingJob), expression: "0/5 0/1 * 1/1 * ? *")); //Initial JobSchedule
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddTransient<IJobManagement, JobManagement>();
            builder.Services.AddTransient<ICronConverter, CronConverter>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors((setup) =>
            {
                setup.AddPolicy("default", (options) =>
                {
                    options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}