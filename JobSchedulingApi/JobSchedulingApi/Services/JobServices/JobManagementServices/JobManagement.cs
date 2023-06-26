using JobSchedulingApi.AdoNet;
using JobSchedulingApi.Models;
using JobSchedulingApi.Services.JobServices.CronConvertingServices;
using Quartz;

namespace JobSchedulingApi.Services.JobServices.JobManagementServices
{
    public class JobManagement : IJobManagement
    {
        private ISchedulerFactory _schedulerFactory;
        private readonly IConfiguration _configuration;
        private readonly JobKey _jobKey;
        private Storage _storage = null;
        private ICronConverter _cronConverter;

        public JobManagement(ISchedulerFactory schedulerFactory, IConfiguration configuration, ICronConverter cronConverter)
        {
            _schedulerFactory = schedulerFactory;
            _configuration = configuration;
            _jobKey = new JobKey(_configuration.GetValue<string>("JobsNames:Emailing"));
            _storage = new Storage(_configuration.GetConnectionString("JobConfigurations"));
            _cronConverter = cronConverter;
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

        public async Task RescheduleJob(ConfiguredSchedule configuredSchedule)
        {
            //TODO: Require additional checking if job executing 
            if (configuredSchedule.IsActive)
            {
                await ResumeJob();
            }
            else
            {
                await PauseJob();
            }
            //

            string cronExpression = _cronConverter.ConfiguredScheduleToCronExpression(configuredSchedule);

            _storage.UpdateCronExpression(_configuration.GetValue<string>("JobsNames:Emailing"), cronExpression);
            _storage.UpdateState(_configuration.GetValue<string>("JobsNames:Emailing"), configuredSchedule.IsActive);

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

        public ConfiguredSchedule GetReschedule()
        {
            JobProperties jobProperties = _storage.GetByName(_configuration.GetValue<string>("JobsNames:Emailing"));

            ConfiguredSchedule configuredSchedule = _cronConverter.CronExpressionToConfiguredSchedule(jobProperties.CronExpression);

            configuredSchedule.IsActive = jobProperties.IsActive;

            return configuredSchedule;
        }
    }
}
