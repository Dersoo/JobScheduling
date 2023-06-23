namespace JobSchedulingApi.Models
{
    public class JobWrapper
    {
        public Type Type { get; }
        public string Expression { get; }

        public JobWrapper(Type type, string expression)
        {
            Type = type;
            Expression = expression;
        }
    }
}
