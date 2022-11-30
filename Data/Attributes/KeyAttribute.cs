namespace Dalion.DDD.Infrastructure.Data.Attributes
{
    public class PrimaryKeyAttribute : Attribute
    {
    }

    public class ForeignKeyAttribute : Attribute
    {
        string _referencedTable;
        string _fieldPrimaryKey;
        public ForeignKeyAttribute(string referencedTable, string fieldPrimaryKey)
        {
            _fieldPrimaryKey = fieldPrimaryKey;
            _referencedTable = referencedTable;
        }
    }
}
