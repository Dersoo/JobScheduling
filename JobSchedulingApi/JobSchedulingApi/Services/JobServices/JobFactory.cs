using Quartz;
using Quartz.Spi;

namespace JobSchedulingApi.Services.JobServices
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_serviceProvider.GetRequiredService(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {

        }
    }
}
