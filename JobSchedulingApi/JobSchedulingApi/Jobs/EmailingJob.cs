using JobSchedulingApi.Services.EmailServices;
using Quartz;

namespace JobSchedulingApi.Jobs
{
    public class EmailingJob : IJob
    {
        private readonly IEmailSender _emailSender;
        public EmailingJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            //await _emailSender.SendEmailAsync("mikaiel.gold.harakian@gmail.com", "Test", "Hello", source.Token);
            Console.WriteLine($"Message sent! [{DateTime.Now}]");
        }
    }
}
