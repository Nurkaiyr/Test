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
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = connection.CreateCommand();

                    SqlParameter balanceParameter = command.CreateParameter();
                    balanceParameter.DbType = System.Data.DbType.Decimal;
                    balanceParameter.IsNullable = false;
                    balanceParameter.ParameterName = "@balance";
                    balanceParameter.Value = wallet.Balance;
                    command.Parameters.Add(balanceParameter);

                    SqlParameter idParameter = command.CreateParameter();
                    idParameter.DbType = System.Data.DbType.Int32;
                    idParameter.IsNullable = false;
                    idParameter.ParameterName = "@Id";
                    idParameter.Value = wallet.Id;
                    command.Parameters.Add(idParameter);

                    command.CommandText = @"INSERT INTO [dbo].[wallets]
                                            ([balance],[id]) VALUES (@balance,@id)";

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.StackTrace);// вызовы перед вознинаванием ошибки по очередно
                }
            }
        }

        public void UpdateWallet(Wallet wallet)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = connection.CreateCommand();

                    SqlParameter balanceParameter = command.CreateParameter();
                    balanceParameter.DbType = System.Data.DbType.Decimal;
                    balanceParameter.IsNullable = false;
                    balanceParameter.ParameterName = "@balance";
                    balanceParameter.Value = wallet.Balance;
                    command.Parameters.Add(balanceParameter);

                    SqlParameter idParameter = command.CreateParameter();
                    idParameter.DbType = System.Data.DbType.Int32;
                    idParameter.IsNullable = false;
                    idParameter.ParameterName = "@Id";
                    idParameter.Value = wallet.Id;
                    command.Parameters.Add(idParameter);

                    command.CommandText = @"Update [dbo].[wallets]
                                            Set balance = @balance where id = @id";

                    //string sql = string.Format("Update Wallet " +
                    //                           "Set balance = '{ 0 }' " +
                    //                           "Where id = '{ 1 }'", balance, id);

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


        public List<Wallet> SelectAllWallet()
        {
            List<Wallet> wallets = new List<Wallet>();
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "select * from Wallets";

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        wallets.Add(
                            new Wallet
                            {
                                Id = (int)reader["Id"],
                                Balance = (decimal)reader["Balance"],
                            }
                            );
                    }
                    transaction.Commit();
                }
                catch(SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return wallets;
        }
    }
}
