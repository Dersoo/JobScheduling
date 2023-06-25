namespace JobSchedulingApi.Models
{
    public class JobSchedule
    {
        public Type Type { get; }
        public string Expression { get; }

        public JobSchedule(Type type, string expression)
        {
            Type = type;
            Expression = expression;
        }
    }
}
