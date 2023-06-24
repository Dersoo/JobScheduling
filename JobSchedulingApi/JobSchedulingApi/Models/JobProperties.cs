namespace JobSchedulingApi.Models
{
    public class JobProperties
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string CronExpression { get; set; } = String.Empty;
        public bool IsActive { get; set; }
    }
}
