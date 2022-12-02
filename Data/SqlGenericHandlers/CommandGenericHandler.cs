using Dalion.DDD.Infrastructure.Data.Attributes;
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

        public static int RunInsertSync(T model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Build an command using the class and its properties and then execute the command
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> RunInsert(T model)
        {

            GetNamesAndValuesFromObject(model, out IEnumerable<string> fieldNames, out IEnumerable<string?> fieldValues);

            var query =
                $"INSERT INTO [{_tableName}](\n" +
                $"{string.Join(",\n", fieldNames.Select(f => $"[{f}]"))})\n\n" +
                $"VALUES ({string.Join(",\n", fieldValues)})\n" +
                $"{""};";

            var sql = new SQLGenericHandler();

            return await sql.ExecuteSqlCommand(query);

        }

        private void GetNamesAndValuesFromObject(T model, out IEnumerable<string> fieldNames, out IEnumerable<string?> fieldValues)
        {
            var modelReflection = model.GetType();
            var properties = modelReflection.GetProperties().Where(p => !p.CustomAttributes.Where(a => a.AttributeType == typeof(SqlIgnoreAttribute)).Any());

            fieldNames = properties.Select(p => p.Name);
            fieldValues = properties
                .Select(p => p.GetValue(model))
                .Select(v => v.GetType() == typeof(string) ? $"'{v}'" : (v.GetType() == typeof(bool) ? ((bool)v == true ? "1" : "0") : v.ToString()));

        }
    }
}
