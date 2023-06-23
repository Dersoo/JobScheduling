using Quartz.Spi;
using Quartz;
using JobSchedulingApi.Models;

namespace JobSchedulingApi.Services.JobServices
{
    public class QuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobWrapper> _jobs;
        public IScheduler Scheduler { get; set; }

        public QuartzHostedService(ISchedulerFactory schedulerFactory,
                                   IJobFactory jobFactory,
                                   IEnumerable<JobWrapper> jobs)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobs = jobs;
        }

        private static IJobDetail CreateJob(JobWrapper job)
        {
            var type = job.Type;

            return JobBuilder.Create(type)
                .WithIdentity(type.FullName)
                .WithDescription(type.Name).Build();
        }

        private static ITrigger CreateTrigger(JobWrapper job)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"{job.Type.FullName}.trigger")
                .WithCronSchedule(job.Expression)
                .WithDescription(job.Expression)
                .Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var job in _jobs)
            {
                var newJob = CreateJob(job);
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
