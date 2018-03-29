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
                SqlCommand command = connection.CreateCommand();
                command.CommandText = $@"INSERT INTO [dbo].[developers]
           ([Name],[WebSite]) VALUES ('{developer.Name}','{developer.WebSite}')";

                command.ExecuteNonQuery();
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
