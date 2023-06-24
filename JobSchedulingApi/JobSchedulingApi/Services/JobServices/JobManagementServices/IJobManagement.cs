namespace JobSchedulingApi.Services.JobServices.JobManagementServices
{
    public interface IJobManagement
    {
        Task PauseJob();
        Task ResumeJob();
        Task RescheduleJob(string cronExpression);
    }
}
