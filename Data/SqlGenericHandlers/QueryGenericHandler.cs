using Dalion.DDD.Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Linq.Expressions;


namespace Dalion.DDD.Infrastructure.Data
{
    public class QueryGenericHandler<T> : GenericHandler<T>
        where T : class, new()
    {

        public QueryGenericHandler(T model)
        {
            base.Initialize(model);
        }

        public async Task<T[]> RunQuery(int rows = 100)
        {

            var modelList = new T[]{ };

            if (wheres.Count > 0)
                _where = BuildWhere(wheres);

            var query =
                $"SELECT TOP {rows} \n" +
                $"{_fields} \n" +
                $"FROM [{_tableName}]\n" +
                $"{(string.IsNullOrEmpty(_where) ? "" : $"WHERE {_where} ")}";



            using (var adapter = await new SQLGenericHandler().GetDataFromSqlQueryAsync(query))
            {
                GenericMapper mapper = new();
                modelList = await mapper.MapObjectsFromQueryAsync<T>(modelList, adapter, _tableName, _type.GetProperties());
            }

            return modelList;

        }

        

    }
}
