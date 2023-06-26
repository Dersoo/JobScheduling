namespace JobSchedulingApi.Models
{
    public class ConfiguredSchedule
    {
        public string UnitOfTime { get; set; } = String.Empty;
        public byte UnitOfTimeValue { get; set; }
        public string Hours { get; set; } = String.Empty;
        public string[] DaysOfTheWeek { get; set; } = Array.Empty<string>();
        public bool IsActive { get; set; }
    }
}
