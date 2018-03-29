using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DomainModel;
namespace DataAccessLibrary
{
    public class WalletService
    {
        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection();
            string connectionString = "Data Source=A-305-03;" + // источник 
                "Initial Catalog=DB_171;" + // имя самой БД 
                "Integrated Security=SSPI;" + // проверка подлинности ( WINDOWS = SSPI ) // False = для логина и пароля вместо SSPI
                "MultipleActiveResultSets=True"; // несколько активных select 

            connection.ConnectionString = connectionString;

            return connection;
        }

        public void CreateWallet(Wallet wallet)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = $@"INSERT INTO [dbo].[wallets]
           ([balance],[id]) VALUES ('{wallet.Balance}','{wallet.Id}')";

                command.ExecuteNonQuery();
            }
        }

        public List<Wallet> SelectAllWallet()
        {
            List<Wallet> wallets = new List<Wallet>();
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from Wallets";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    wallets.Add(
                        new Wallet
                        {
                            Id = (int)reader["Id"],
                            Balance = (decimal)["Balance"],
                        }
                        );
                }
            }
            return wallets;
        }
    }
}
