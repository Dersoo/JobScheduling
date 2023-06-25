using JobSchedulingApi.AdoNet;
using JobSchedulingApi.Models;
using Quartz;

namespace JobSchedulingApi.Services.JobServices.JobManagementServices
{
    public class JobManagement : IJobManagement
    {
        private ISchedulerFactory _schedulerFactory;
        private readonly IConfiguration _configuration;
        private readonly JobKey _jobKey;
        private Storage _storage = null;

        public JobManagement(ISchedulerFactory schedulerFactory, JobSchedule jobs, IConfiguration configuration)
        {
            _schedulerFactory = schedulerFactory;
            _configuration = configuration;
            _jobKey = new JobKey(_configuration.GetValue<string>("JobsNames:Emailing"));
            _storage = new Storage(_configuration.GetConnectionString("JobConfigurations"));
        }

        public async Task PauseJob()
        {
            _storage.UpdateState(_configuration.GetValue<string>("JobsNames:Emailing"), false);

            IScheduler Scheduler = await _schedulerFactory.GetScheduler();

            await Scheduler.PauseJob(_jobKey);
        }

        public async Task ResumeJob()
        {
            _storage.UpdateState(_configuration.GetValue<string>("JobsNames:Emailing"), true);

            IScheduler Scheduler = await _schedulerFactory.GetScheduler();

            await Scheduler.ResumeJob(_jobKey);
        }

        public async Task RescheduleJob(string cronExpression)
        {
            _storage.UpdateCronExpression(_configuration.GetValue<string>("JobsNames:Emailing"), cronExpression);

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
