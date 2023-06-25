using Quartz.Spi;
using Quartz;
using JobSchedulingApi.Models;
using static Quartz.Logging.OperationName;
using System.Threading;
using JobSchedulingApi.AdoNet;
using JobSchedulingApi.Jobs;

namespace JobSchedulingApi.Services.JobServices
{
    public class QuartzHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<JobSchedule> _jobs;
        public IScheduler Scheduler { get; set; }
        private Storage _storage = null;

        public QuartzHostedService(IConfiguration configuration,
                                   ISchedulerFactory schedulerFactory,
                                   IJobFactory jobFactory,
                                   IEnumerable<JobSchedule> jobs)
        {
            _configuration = configuration;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobs = jobs;
            _storage = new Storage(_configuration.GetConnectionString("JobConfigurations"));
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

            //Try to get job properties from DB
            JobProperties jobProperties = _storage.GetByName(_configuration.GetValue<string>("JobsNames:Emailing"));

            if (jobProperties != null && jobProperties.IsActive)
            {
                JobSchedule jobSchedule = new JobSchedule(
                        type: typeof(EmailingJob), 
                        expression: jobProperties.CronExpression
                    );

                var newJob = CreateJob(jobSchedule, _configuration.GetValue<string>("JobsNames:Emailing"));
                var trigger = CreateTrigger(jobSchedule);

                await Scheduler.ScheduleJob(newJob, trigger, cancellationToken);
            }
            else
            {
                //Initial(default) scheduling of jobs from Program.cs
                foreach (var job in _jobs)
                {
                    var newJob = CreateJob(job, _configuration.GetValue<string>("JobsNames:Emailing"));
                    var trigger = CreateTrigger(job);

                    await Scheduler.ScheduleJob(newJob, trigger, cancellationToken);
                }
                //
            }

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }
    }
}
