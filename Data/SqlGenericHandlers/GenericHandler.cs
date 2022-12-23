using Dalion.DDD.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dalion.DDD.Infrastructure.Data
{

#pragma warning disable CS8604 // Posible argumento de referencia nulo
    /// <summary>
    /// Generic abstract class that provide wheres methods for manage sql commands and querys
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericHandler<T> where T : class, new()
    {
        protected string _tableName;
        protected string _fields;

        protected Type _type;
        protected string _where;

        //protected Dictionary<string, object?> _dataFields;
        protected List<WhereClause> wheres = new List<WhereClause>();

        protected void Initialize(T model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _type = model.GetType();
            _tableName = _type.Name;
            _where = "";


            var properties = _type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                _fields += (i > 0 ? ",\n" : "") + $"[{properties[i].Name}]";
            }
        }

        public string BuildWhere(List<WhereClause> WhereList)
        {
            string whereClause = _where + " ";

            foreach (var where in WhereList)
            {
                if (string.IsNullOrEmpty(whereClause))
                    whereClause += where.FieldName + " " + where.Operator + " " + (where.Type == typeof(string) ? $"'{where.Right.ToString()}'" : where.Right.ToString());

                else if (where.InnerWhere != null)
                    whereClause +=
                        (string.IsNullOrEmpty(whereClause) ? "" : where.LogicOperator) + " (" + BuildWhere(where.InnerWhere) + ") ";

                else
                    whereClause += " " + where.LogicOperator + " " + where.FieldName + " " + where.Operator + " " + (where.Type == typeof(string) ? $"'{where.Right.ToString()}'" : where.Right.ToString());
            }

            return whereClause;
        }

        public GenericHandler<T> Where(Expression<Func<T, bool>> expression)
        {
            string sqlWhere = "";
            ReflectionUtils.ExpressionToString<T>(expression, ref sqlWhere);

            _where += sqlWhere;

            return this;
        }



        public GenericHandler<T> DeprecatedWhere(Expression<Func<T, bool>> expression)
        {
            object? rightValue = new();
            string leftFieldName = "";
            string Operator = "";

            dynamic din = expression.Body;

            try
            {
                //  If is a constant value
                rightValue = din.Right.Value;
            }
            catch (Exception)
            {
                // if not...
                ConstantExpression din3 = (ConstantExpression)din.Right.Expression;
                var members = din3.Value?.GetType().GetFields() ?? throw new NullReferenceException("The expression has no fields.");
                var dictionary = members.ToDictionary(property => property.Name, property => property.GetValue(din3.Value));
                rightValue = dictionary[members[0].Name];
            }

            leftFieldName = din.Left.Member.Name;
            Operator = din.Method.Name;

            ;


            throw new NotImplementedException();
        }

        public GenericHandler<T> WhereEq<TParam>(Expression<Func<T, string>> property, TParam value)
        {
            dynamic? bodyExpr = property?.Body;

            if ((bodyExpr?.Member?.Name ?? null) == null)
                throw new NullReferenceException("Where clause building by a null expression.");

            return WhereEq(bodyExpr?.Member.Name, value);
        }

        public GenericHandler<T> WhereEq<TParam>(string fieldName, TParam b)
            where TParam : notnull
        {
            ValidateFieldName(fieldName);
            if (typeof(TParam) == typeof(string))
            {
                string rightal = ValidateStringSecurity(b.ToString());
                wheres.Add(new WhereClause(fieldName, b, b.GetType(), "="));
                return this;
            }

            wheres.Add(new WhereClause(fieldName, b, b.GetType(), "="));
            return this;
        }

        public GenericHandler<T> WhereNotEq<TParam>(string fieldName, TParam b)
            where TParam : notnull
        {
            ValidateFieldName(fieldName);
            if (typeof(TParam) == typeof(string))
            {
                string rightal = ValidateStringSecurity(b.ToString());
                wheres.Add(new WhereClause(fieldName, b, b.GetType(), "<>"));
                return this;
            }

            wheres.Add(new WhereClause(fieldName, b, b.GetType(), "<>"));
            return this;
        }

        public GenericHandler<T> WhereNotLike<TParam>(string fieldName, string b)
            where TParam : struct
        {
            ValidateFieldName(fieldName);
            b = ValidateStringSecurity(b);
            wheres.Add(new WhereClause(fieldName, b, b.GetType(), "NOT LIKE"));
            return this;
        }

        public GenericHandler<T> WhereLike<TParam>(string fieldName, string b)
            where TParam : notnull
        {
            ValidateFieldName(fieldName);
            b = ValidateStringSecurity(b);
            wheres.Add(new WhereClause(fieldName, b, b.GetType(), "LIKE"));
            return this;
        }

        public GenericHandler<T> WhereGreaterThan<TParam>(string fieldName, TParam b)
            where TParam : struct
        {
            if (typeof(TParam) == typeof(string))
                throw new InvalidOperationException("Operator '>' for strings.");
            ValidateFieldName(fieldName);
            wheres.Add(new WhereClause(fieldName, b, b.GetType(), ">"));
            return this;
        }

        public GenericHandler<T> WhereLessThan<TParam>(string fieldName, TParam b)
            where TParam : struct
        {
            if (typeof(TParam) == typeof(string))
                throw new InvalidOperationException("Operator '<' for strings.");
            ValidateFieldName(fieldName);
            wheres.Add(new WhereClause(fieldName, b, b.GetType(), "<"));
            return this;
        }

        public GenericHandler<T> SetOr()
        {
            wheres.Last().LogicOperator = "OR";
            return this;
        }
        public GenericHandler<T> SetAnd()
        {
            wheres.Last().LogicOperator = "AND";
            return this;
        }

        public class WhereClause
        {
            public WhereClause(string fieldname, object right, Type type, string @operator)
            {
                FieldName = fieldname;
                Right = right;
                Type = type;
                Operator = @operator;
            }

            public List<WhereClause> InnerWhere { get; set; }

            public object Left { get; private set; }
            public string FieldName { get; private set; }
            public object Right { get; private set; }
            public Type Type { get; private set; }
            public string Operator { get; private set; }
            public string LogicOperator { get; set; } = "AND";

        }

        protected void ValidateFieldName(string name)
        {
            const string chars = "abcdefghijklmnoprstuvwxyz1234567890.[]";

            foreach (var nameChar in name)
            {
                if (!chars.Contains(nameChar, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"Column name '{name}' is invalid.");
            }
        }

        protected string ValidateStringSecurity(string value)
        {
            if (!value.Contains('\'')) return value;

            else return value.Replace("'", "''");
        }

    }

#pragma warning disable CS8604 // Posible argumento de referencia nulo
}
