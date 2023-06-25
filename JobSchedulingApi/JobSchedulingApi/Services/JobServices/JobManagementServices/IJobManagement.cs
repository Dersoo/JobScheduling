using JobSchedulingApi.Models;

namespace JobSchedulingApi.Services.JobServices.JobManagementServices
{
    public interface IJobManagement
    {
        Task PauseJob();
        Task ResumeJob();
        Task RescheduleJob(ConfiguredSchedule configuredSchedule);
        ConfiguredSchedule GetReschedule();
    }
}
