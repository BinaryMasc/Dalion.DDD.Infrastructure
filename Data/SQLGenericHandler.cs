using Dalion.DDD.Commons;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dalion.DDD.Infrastructure.Data
{
    public class SQLGenericHandler : IDatabaseGenericHandler
    {

        public SQLGenericHandler() { }



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T">Type of model that coincide with TableName</typeparam>
        ///// <param name="modelT">For nothing</param>
        ///// <param name="pFilter">filters applied to query. Ex: id=1</param>
        ///// <returns>The mapped object</returns>
        ///// <exception cref="ArgumentNullException">modelT can't be null</exception>y
        //public async Task<T> GetObjectAsync<T>(T modelT, string pFilter = "")
        //{
        //    if (modelT == null) throw new ArgumentNullException("modelT");



        //    var type = modelT.GetType();
        //    var tableName = type.Name;
        //    var properties = type.GetProperties();


        //    var fields = "";

        //    for (int i = 0; i < properties.Length; i++)
        //    {
        //        fields += (i > 0 ? ",\n" : "") + $"[{properties[i].Name}]";
        //    }


        //    var query =
        //        $"SELECT {fields} \n" +
        //        $"FROM [{tableName}]\n" +
        //        $"{(string.IsNullOrEmpty(pFilter) ? "" : $"WHERE {pFilter} ")}";

        //    using (var adapter = await GetDataFromSqlQuery(query))
        //    {
        //        GenericMapper mapper = new();
        //        await mapper.MapObjectFromQueryAsync(modelT, adapter, tableName, properties);
        //    }

        //    return modelT;
        //}




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
