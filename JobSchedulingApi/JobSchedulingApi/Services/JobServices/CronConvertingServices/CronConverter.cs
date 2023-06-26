using JobSchedulingApi.Models;

namespace JobSchedulingApi.Services.JobServices.CronConvertingServices
{
    public class CronConverter : ICronConverter
    {
        //Helpers
        private string ConvertArrayOfDaysToCommaSeparetedString(string[] daysOfTheWeek) 
        {
            string commaSeparetedString = "";

            for (int i = 0; i < daysOfTheWeek.Count(); i++)
            {
                if (i > 0 && (i < daysOfTheWeek.Count()))
                {
                    commaSeparetedString += ",";
                }

                commaSeparetedString += daysOfTheWeek[i];
            }

            return commaSeparetedString;
        }
        //

        public string ConfiguredScheduleToCronExpression(ConfiguredSchedule configuredSchedule)
        {
            string cronExpression = "0/5 0/1 * 1/1 * ? *"; //Default value

            if (configuredSchedule.UnitOfTime != "")
            {
                switch (configuredSchedule.UnitOfTime)
                {
                    case "m":
                        cronExpression = $"0 0/{configuredSchedule.UnitOfTimeValue} * 1/1 * ? *";
                        break;
                    case "h":
                        cronExpression = $"0 0 0/{configuredSchedule.UnitOfTimeValue} 1/1 * ? *";
                        break;
                }
            }
            else if (configuredSchedule.DaysOfTheWeek.Count() > 0)
            {
                string daysOfTheWeek = (configuredSchedule.DaysOfTheWeek.Count() > 0) ?
                                            ConvertArrayOfDaysToCommaSeparetedString(configuredSchedule.DaysOfTheWeek) :
                                            "?";
                string hours = (configuredSchedule.Hours != "") ? configuredSchedule.Hours : "0";

                cronExpression = $"0 0 {hours} ? * {daysOfTheWeek} *";
            }

            return cronExpression;
        }

        public ConfiguredSchedule CronExpressionToConfiguredSchedule(string cronExpression)
        {
            if (cronExpression != "") {
                ConfiguredSchedule configuredSchedule = new ConfiguredSchedule();

                string minutes = cronExpression.Split(' ')[1];
                string hourse = cronExpression.Split(' ')[2];
                string days = cronExpression.Split(' ')[5];

                if (minutes != "" && minutes != "0")
                {
                    configuredSchedule.UnitOfTime = "m";
                    configuredSchedule.UnitOfTimeValue = Byte.Parse(minutes.Replace("0/", ""));
                }
                else if (hourse != "*" && days == "?")
                {
                    configuredSchedule.UnitOfTime = "h";
                    configuredSchedule.UnitOfTimeValue = Byte.Parse(hourse.Replace("0/", ""));
                }
                else if (days != "?")
                {
                    configuredSchedule.Hours = hourse;
                    configuredSchedule.DaysOfTheWeek = days.Split(',');
                }

                return configuredSchedule;
            }

            return null;
        }
    }
}
