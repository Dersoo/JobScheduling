using JobSchedulingApi.Models;
using Quartz;

namespace JobSchedulingApi.Services.JobServices.JobManagementServices
{
    public class JobManagement : IJobManagement
    {
        ISchedulerFactory _schedulerFactory;
        private readonly IConfiguration _configuration;
        private readonly JobKey _jobKey;

        public JobManagement(ISchedulerFactory schedulerFactory, JobWrapper jobs, IConfiguration configuration)
        {
            _schedulerFactory = schedulerFactory;
            _configuration = configuration;
            _jobKey = new JobKey(_configuration.GetValue<string>("JobsNames:Emailing"));
        }

        public async Task PauseJob()
        {
            IScheduler Scheduler = await _schedulerFactory.GetScheduler();

            await Scheduler.PauseJob(_jobKey);
        }

        public async Task ResumeJob()
        {
            IScheduler Scheduler = await _schedulerFactory.GetScheduler();

            await Scheduler.ResumeJob(_jobKey);
        }

        public async Task RescheduleJob(string cronExpression)
        {
            IScheduler Scheduler = await _schedulerFactory.GetScheduler();

            var jobTriggers = await Scheduler.GetTriggersOfJob(_jobKey);

            ITrigger oldTrigger = jobTriggers.First();

            if (oldTrigger != null)
            {
                ITrigger newTrigger = TriggerBuilder.Create()
                                        .WithIdentity($"{_configuration.GetValue<string>("JobsNames:Emailing")}.trigger-{DateTime.Now}")
                                        .WithCronSchedule(cronExpression)
                                        .WithDescription(cronExpression)
                                        .Build();

                await Scheduler.RescheduleJob(oldTrigger.Key, newTrigger);
            }
        }
    }
}
