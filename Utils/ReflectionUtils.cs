using Dalion.DDD.Infrastructure.Data.Attributes;
using System.Linq.Expressions;

namespace Dalion.DDD.Infrastructure.Utils
{
    public class ReflectionUtils
    {
        public static void ExpressionToString<T>(Expression<Func<T, bool>> expression, ref string strExpression)
        {
            dynamic body = expression.Body;
            Type typeBody = expression.Body.GetType();

            ;

            switch (typeBody.Name)
            {
                case "MethodBinaryExpression":
                case "BinaryExpression":
                    strExpression += "(" + body.Left.Member.Name + " ";
                    strExpression += GetNodeTypeString(body.Method.Name);  //  Operator
                    strExpression += " " + GetRightOperandingExpression(body.Right).ToString();
                    strExpression += ")";
                    break;
                case "LogicalBinaryExpression":
                    Type typeLeft = body.Left.GetType();
                    Type typeRight = body.Right.GetType();
                    if (typeLeft.Name == "PropertyExpression")
                        strExpression += body.Left.Member.Name;
                    else if (typeLeft.Name == "LogicalBinaryExpression")
                        LogicalBinaryExpressionToString(expression, ref strExpression);
                    else ExpressionToString(body.Left, ref strExpression);
                    //if (typeRight.Name == "PropertyExpression") break;
                    strExpression += GetNodeTypeString(body.NodeType.ToString());
                    if (typeRight.Name == "ConstantExpression")
                        strExpression += body.Right;
                    else ExpressionToString(body.Right, ref strExpression);
                    break;
                case "PropertyExpression":
                    //  Assuming the property is bool in C# and bit in sql
                    strExpression += body.Member.Name + "=1 ";
                    break;
            }
        }

        private static string GetNodeTypeString(string nodeType)
        {
            switch (nodeType)
            {
                case "op_Equality": return "=";
                case "Equal": return "=";
                case "NotEqual": return "<>";
                case "AndAlso": return " AND ";
                case "LessThan": return "<";
                case "GreaterThan": return ">";
                case "LessThanOrEqual": return "<=";
                case "GreaterThanOrEqual": return ">=";
                case "OrElse": return " OR ";
                default: return "";
            };
        }

        private static void ExpressionToString(BinaryExpression expression, ref string strExpression)
        {
            dynamic body = expression;
            strExpression += "(" + body.Left.Member.Name + " ";
            strExpression += GetNodeTypeString(body.Method?.Name ?? expression.NodeType.ToString());  //  Operator
            strExpression += " " + GetRightOperandingExpression(body.Right).ToString();
            strExpression += ")";
        }

        //  LogicalBinaryExpression
        private static void LogicalBinaryExpressionToString(dynamic expression, ref string strExpression)
        {
            dynamic body;
            try { body = expression.Body; } catch { body = expression; }
            Type typeLeft = body.Left.GetType();
            Type typeRight = body.Right.GetType();

            if (typeLeft.Name == "PropertyExpression")
                strExpression += "(" + body.Left.Member.Name;
            else
            {
                if (body.Left.GetType().Name == "LogicalBinaryExpression")
                    LogicalBinaryExpressionToString(body.Left, ref strExpression);
                
                else ExpressionToString(body.Left, ref strExpression);
            }
            strExpression += GetNodeTypeString(body.NodeType.ToString());
            if (typeRight.Name == "ConstantExpression")
                strExpression += body.Right + ")";
            else ExpressionToString(body.Right, ref strExpression);
        }

        private static void ExpressionToString(dynamic expression, ref string strExpression)
        {


            dynamic body = expression;
            try { strExpression += "(" + body.Left.Member.Name + " "; }
            catch (Exception) { strExpression += "(" + body.Member.Name + " "; }
            ;
            if (body.GetType().Name != "PropertyExpression")
            {
                strExpression += GetNodeTypeString(body.Method.Name);  //  Operator
                strExpression += " " + GetRightOperandingExpression(body.Right).ToString();
                strExpression += ")";
            }
            else strExpression += "=1) ";
            
            
        }


        private static object GetRightOperandingExpression(MemberExpression member)
        {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            ConstantExpression din3 = (ConstantExpression)member.Expression;
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            var members = din3.Value?.GetType().GetFields() ?? throw new NullReferenceException("The expression has no fields.");
            var dictionary = members.ToDictionary(property => property.Name, property => property.GetValue(din3.Value));


            if(members[0].FieldType == typeof(string))
                return "'" + dictionary[members[0].Name] + "'";

            else return dictionary[members[0].Name];
        }

        private static object GetRightOperandingExpression(ConstantExpression member)
        {
            if (member.Value.GetType() == typeof(string))
                return "'" + member.Value + "'";

            else return member.Value;
        }

        public static void GetNamesAndValuesFromObject<T>(T model, out IEnumerable<string> fieldNames, out IEnumerable<string?> fieldValues)
        {
            var modelReflection = model.GetType();
            var properties = modelReflection.GetProperties().Where(p => !p.CustomAttributes.Where(a => a.AttributeType == typeof(SqlIgnoreAttribute)).Any());

            fieldNames = properties.Select(p => p.Name);
            fieldValues = properties
                .Select(p => p.GetValue(model))
                .Select(v => v.GetType() == typeof(string) ? $"'{v}'" : (v.GetType() == typeof(bool) ? ((bool)v == true ? "1" : "0") : v.ToString()));

        }

        public static ExpressionData DEPRECATEDGetDiscomposedExpression<T>(Expression < Func<T, bool> > expression)
        {
            object? rightValue = new();
            string leftFieldName = "";
            string Operator = "";

            dynamic body = expression.Body;
            dynamic LeftType = body.Left.GetType();
            dynamic RightType = body.Right.GetType();
            Type typeBody = expression.Body.GetType();

            ;
            //  boolean
            if (typeBody.Name == "PropertyExpression")
                return new ExpressionData { Left = body.Member.Name, LinqExpressionType = expression.Body.GetType() };

            if (typeBody.Name == "LogicalBinaryExpression")
                throw new NotImplementedException("LogicalBinaryExpression Not supported for this method.");


            try
            {
                //  If is a constant value
                rightValue = body.Right.Value;
            }
            // if not...
            catch (Exception)
            {
                ConstantExpression din3 = (ConstantExpression)body.Right.Expression;
                var members = din3.Value?.GetType().GetFields() ?? throw new NullReferenceException("The expression has no fields.");
                var dictionary = members.ToDictionary(property => property.Name, property => property.GetValue(din3.Value));
                rightValue = dictionary[members[0].Name];
            }

            leftFieldName = body.Left.Member.Name;
            Operator = body.Method.Name;

            return new ExpressionData
            {
                Left = leftFieldName,
                Right = rightValue,
                Operator = Operator,
                LinqExpressionType = expression.Body.GetType()
            };
        }
    }
}
