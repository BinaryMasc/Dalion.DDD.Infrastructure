using Dalion.DDD.Commons;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dalion.DDD.Infrastructure.Data
{
    public class SQLGenericHandler
    {

        public SQLGenericHandler() { }


        public async Task<int> ExecuteSqlCommand(string command)
        {
            SqlCommand sqlCommand = new SqlCommand(command, SQLConnection.GetOpenConnection());

            int rowsAffected = 0;

            await Task.Run(() =>
            {
                rowsAffected = sqlCommand.ExecuteNonQuery();
            });

            return rowsAffected;
        }




        public async Task<SqlDataAdapter> GetDataFromSqlQuery(string query)
        {
            SqlCommand sqlCommand = new SqlCommand(query, SQLConnection.GetOpenConnection());

            SqlDataAdapter adapter = new();
            await Task.Run(() =>
            {
                adapter = new SqlDataAdapter(sqlCommand);
            });

            return adapter;
        }



    }
}
