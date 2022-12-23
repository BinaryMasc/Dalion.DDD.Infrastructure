namespace Dalion.DDD.Infrastructure.Data.Attributes
{
    public class PrimaryKeyAttribute : Attribute
    {
    }

    public class ForeignKeyAttribute : Attribute
    {
        string _referencedTable;
        string _fieldPrimaryKey;
        Type? _referencedtblType;
        public ForeignKeyAttribute(string referencedTable, string fieldPrimaryKey, Type? referencedtblType = null)
        {
            _fieldPrimaryKey = fieldPrimaryKey;
            _referencedTable = referencedTable;
            _referencedtblType = referencedtblType;
        }
    }

    public class TableNameAttribute : Attribute
    {
        string tableName;
        public TableNameAttribute(string tableName)
        {
            this.tableName = tableName;
        }
    }

    public class SqlIgnoreAttribute : Attribute
    {

    }
}
