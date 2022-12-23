using System;
using System.Data;
using System.Data.SqlClient;

namespace Dalion.DDD.Infrastructure.Data
{
    public static class SQLConnection
    {
        private static SqlConnection _connection
            = new SqlConnection();  //  Supress warning
        private static string _connectionString
            = "";   //  Supress warning

        public static void SetConnectionString(string pConString)
        {
            _connectionString = pConString;
            _connection = new SqlConnection(pConString);
        }

        public static SqlConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }

        public static SqlConnection GetNewOpenConnection()
        {
            var con = new SqlConnection(_connectionString);
            con.Open();
            

            return con;
        }
    }
}
