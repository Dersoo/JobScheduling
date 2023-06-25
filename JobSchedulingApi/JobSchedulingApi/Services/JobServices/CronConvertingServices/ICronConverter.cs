using JobSchedulingApi.Models;

namespace JobSchedulingApi.Services.JobServices.CronConvertingServices
{
    public interface ICronConverter
    {
        string ConfiguredScheduleToCronExpression(ConfiguredSchedule configuredSchedule);
        ConfiguredSchedule CronExpressionToConfiguredSchedule(string cronExpression);
    }
}
