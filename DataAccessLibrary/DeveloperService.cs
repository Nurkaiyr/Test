using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DomainModel;

namespace DataAccessLibrary
{
    public class DeveloperService
    {
        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection();
            string connectionString = "Data Source=A-305-03;" + // источник 
                "Initial Catalog=DB_171;"+ // имя самой БД 
                "Integrated Security=SSPI;"+ // проверка подлинности ( WINDOWS = SSPI ) // False = для логина и пароля вместо SSPI
                "MultipleActiveResultSets=True"; // несколько активных select 

            connection.ConnectionString = connectionString;

            return connection;
        }

        public void CreateDeveloper(Developer developer)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = connection.CreateCommand();

                    //SqlParameter nameParameter = new SqlParameter();
                    SqlParameter nameParameter = command.CreateParameter();
                    nameParameter.DbType = System.Data.DbType.String;
                    nameParameter.IsNullable = false;
                    nameParameter.ParameterName = "@Name";
                    nameParameter.Value = developer.Name;
                    command.Parameters.Add(nameParameter);

                    SqlParameter webSiteParameter = command.CreateParameter();
                    webSiteParameter.DbType = System.Data.DbType.String;
                    webSiteParameter.IsNullable = true;
                    webSiteParameter.ParameterName = "@WebSite";
                    webSiteParameter.Value = developer.WebSite;
                    command.Parameters.Add(nameParameter);

                    //     command.CommandText = $@"INSERT INTO [dbo].[developers]
                    //([Name],[WebSite]) VALUES ('{developer.Name}','{developer.WebSite}')";

                    command.CommandText = @"INSERT INTO [dbo].[developers]
           ([Name],[WebSite]) VALUES (@Name, @WebSite)";

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch(SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
        public List<Developer> SelectAllDevelopers()
        {
            List<Developer> developers = new List<Developer>();
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from Developers";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    developers.Add(
                        new Developer
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            WebSite = reader["WebSite"].ToString()
                        }
                        );
                }
            }
            return developers;
        }
    }
}
