using Dalion.DDD.Infrastructure.Data.Attributes;
using Dalion.DDD.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dalion.DDD.Infrastructure.Data
{
    public class CommandGenericHandler<T> : GenericHandler<T>
        where T : class, new()
    {
        public CommandGenericHandler(T model)
        {
            base.Initialize(model);
        }

        public static int RunInsertSync<R>(R model) where R : notnull
        {
            ReflectionUtils.GetNamesAndValuesFromObject<R>(model, out IEnumerable<string> fieldNames, out IEnumerable<string?> fieldValues);

            var query =
                $"INSERT INTO [{model.GetType().Name}](\n" +
                $"{string.Join(",\n", fieldNames.Select(f => $"[{f}]"))})\n\n" +
                $"VALUES ({string.Join(",\n", fieldValues)})\n" +
                $"{""};";

            var sql = new SQLGenericHandler();

            return sql.ExecuteSqlCommand(query);
        }

        /// <summary>
        /// Build an command using the class and its properties and then execute the command
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> RunInsert(T model)
        {

            ReflectionUtils.GetNamesAndValuesFromObject<T>(model, out IEnumerable<string> fieldNames, out IEnumerable<string?> fieldValues);

            var query =
                $"INSERT INTO [{_tableName}](\n" +
                $"{string.Join(",\n", fieldNames.Select(f => $"[{f}]"))})\n\n" +
                $"VALUES ({string.Join(",\n", fieldValues)})\n" +
                $"{""};";

            var sql = new SQLGenericHandler();

            return await sql.ExecuteSqlCommandAsync(query);

        }

        
    }
}
