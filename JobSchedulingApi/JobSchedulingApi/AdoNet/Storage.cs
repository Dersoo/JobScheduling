using JobSchedulingApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace JobSchedulingApi.AdoNet
{
    public class Storage
    {
        private readonly string _connectionString;
        private SqlConnection _sqlConnection = null;
        private SqlParameter _sqlParameter = null;

        public Storage(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection { ConnectionString = _connectionString };
            _sqlConnection.Open();
        }

        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }

        public JobProperties GetByName(string name)
        {
            OpenConnection();

            JobProperties jobProperties = null;

            string sql = $"SELECT ID, NAME, CRON_EXPRESSION, IS_ACTIVE" +
                         $"FROM QRTZ_JOB_PROPERTIES" +
                         $"WHERE NAME = @Name";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                _sqlParameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Direction = ParameterDirection.Input,
                    Value = name,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 25
                };
                command.CommandType = CommandType.Text;
                command.Parameters.Add(_sqlParameter);

                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    jobProperties = new JobProperties
                    {
                        Id = (int)dataReader["ID"],
                        Name = (string)dataReader["NAME"],
                        CronExpression = (string)dataReader["CRON_EXPRESSION"],
                        IsActive = (bool)dataReader["IS_ACTIVE"]
                    };
                }

                dataReader.Close();
            }

            CloseConnection();

            return jobProperties;
        }

        public void UpdateCronExpression(string name, string cronExpression)
        {
            OpenConnection();

            string sql = $"UPDATE QRTZ_JOB_PROPERTIES SET CRON_EXPRESSION = @CronExpression " +
                         $"WHERE NAME = @Name";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                _sqlParameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Direction = ParameterDirection.Input,
                    Value = name,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 25
                };
                command.Parameters.Add(_sqlParameter);

                _sqlParameter = new SqlParameter
                {
                    ParameterName = "@CronExpression",
                    Direction = ParameterDirection.Input,
                    Value = cronExpression,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 120
                };
                command.Parameters.Add(_sqlParameter);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public void UpdateState(string name, bool state)
        {
            OpenConnection();

            string sql = $"UPDATE QRTZ_JOB_PROPERTIES SET IS_ACTIVE = @IS_ACTIVE " +
                         $"WHERE NAME = @Name";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                _sqlParameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Direction = ParameterDirection.Input,
                    Value = name,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 25
                };
                command.Parameters.Add(_sqlParameter);

                _sqlParameter = new SqlParameter
                {
                    ParameterName = "@IS_ACTIVE",
                    Direction = ParameterDirection.Input,
                    Value = state,
                    SqlDbType = SqlDbType.Bit
                };
                command.Parameters.Add(_sqlParameter);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }
    }
}
