using Dalion.DDD.Commons;
using Dalion.DDD.Infrastructure.Exceptions;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Dalion.DDD.Infrastructure.Data
{
    public class GenericMapper
    {
        public async Task<T> MapObjectFromQueryAsync<T>(T modelT, SqlDataAdapter adapter, string tableName, PropertyInfo[] properties)
        {
            if (modelT == null)
                throw new ArgumentNullException("modelT");

            using (var ds = new DataSet())
            {
                await Task.Run(() =>
                {
                    adapter.Fill(ds, tableName);
                });

                if (ds.Tables[0].Rows.Count < 1)
                    throw new DataNotFoundException("The query didn't return data.");


                foreach (var propertie in properties)
                {
                    var memberName = propertie.Name;

                    if (ds.Tables[0].Columns.Contains(memberName))
                        Helpers.SetProperty(modelT, memberName, ds.Tables[0].Rows[0][memberName]);

                }
            }

            return modelT;
        }



#pragma warning disable CS8604 // Posible argumento de referencia nulo

        public async Task<T[]> MapObjectsFromQueryAsync<T>(T[] modelT, SqlDataAdapter adapter, string tableName, PropertyInfo[] properties)
            where T : new()
        {
            if (modelT == null)
                throw new ArgumentNullException("modelT");

            using (var ds = new DataSet())
            {
                await Task.Run(() =>
                {
                    adapter.Fill(ds, tableName);
                });

                if (ds.Tables[0].Rows.Count < 1)
                    throw new DataNotFoundException("The query didn't return data.");


                modelT = new T[ds.Tables[0].Rows.Count];


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var tempT = new T();
                    foreach (var propertie in properties)
                    {
                        var memberName = propertie.Name;

                        if (ds.Tables[0].Columns.Contains(memberName))
                        {
                            Helpers.SetProperty(tempT, memberName, ds.Tables[0].Rows[i][memberName]);
                        }
                    }
                    modelT[i] = tempT;
                }
                

            }

            return modelT;
        }

#pragma warning restore CS8604 // Posible argumento de referencia nulo
    }
}
