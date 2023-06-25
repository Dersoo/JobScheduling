using Quartz.Spi;
using Quartz;
using JobSchedulingApi.Models;

namespace JobSchedulingApi.Services.JobServices
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobs;
        public IScheduler Scheduler { get; set; }

        public QuartzHostedService(IConfiguration configuration,
                                   ISchedulerFactory schedulerFactory,
                                   IJobFactory jobFactory,
                                   IEnumerable<JobSchedule> jobs)
        {
            _configuration = configuration;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobs = jobs;
        }
        //Helpers
        private static IJobDetail CreateJob(JobSchedule job, string jobName = "defaultJobName")
        {
            var type = job.Type;

            return JobBuilder.Create(type)
                //.WithIdentity(type.FullName)
                .WithIdentity(new JobKey(jobName))
                .WithDescription(type.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(JobSchedule job)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"{job.Type.FullName}.trigger")
                //.WithSimpleSchedule(
                //    s => s.WithMisfireHandlingInstructionNextWithExistingCount()
                //)
                .WithCronSchedule(job.Expression)
                .WithDescription(job.Expression)
                .Build();
        }
        //

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var job in _jobs)
            {
                var newJob = CreateJob(job, _configuration.GetValue<string>("JobsNames:Emailing"));
                var trigger = CreateTrigger(job);

                await Scheduler.ScheduleJob(newJob, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }
    }
}
