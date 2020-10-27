using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public string GetConnectionString(string connectionName = "AzsunaBOT")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new MySqlConnection(GetConnectionString()))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(sql, parameters);

                return rows.ToList();
            }
        }

        public Task SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new MySqlConnection(GetConnectionString()))
            {
                return connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
