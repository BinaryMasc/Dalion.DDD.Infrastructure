namespace Dalion.DDD.Infrastructure.Utils
{
    public class ExpressionData
    {
        public string Left { get; set; }
        public object? Right { get; set; }
        public string Operator { get; set; }
        public string LogicOperator { get; set; }
        public Type LinqExpressionType { get; set; }

    }
}
