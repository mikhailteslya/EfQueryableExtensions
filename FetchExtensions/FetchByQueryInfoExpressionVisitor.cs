using System.Linq.Expressions;

namespace FetchExtensions
{
    public class FetchByQueryInfoExpressionVisitor<TQuery> : ExpressionVisitor where TQuery : Query
    {

        private TQuery _query;
        public FetchByQueryInfoExpressionVisitor(TQuery query)
        {
            _query = query;
        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == nameof(QueryableExtensions.FetchByQuery))
            {
                var param = Expression.Parameter(node.Type.GetGenericArguments()[0], "item");
                var member = param.Type.GetMember(_query.Field)[0];
                var left = Expression.MakeMemberAccess(param, member);
                var right = Expression.Constant(_query.Value);
                var equals = Expression.Equal(left, right);

                return Expression.Lambda(equals, param);
            }
            return base.VisitMethodCall(node);
        }
    }
}
