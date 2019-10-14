using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Diagnostics.CodeAnalysis;

namespace FetchExtensions
{
    public static class QueryableExtensions
    {
        internal static readonly MethodInfo FetchByQueryInfo
            = typeof(QueryableExtensions)
                .GetTypeInfo().GetDeclaredMethod(nameof(FetchByQuery));

        public static IQueryable<TEntity> FetchByQuery<TEntity, TQuery>(
            [NotNull] this IQueryable<TEntity> source, TQuery query) where TEntity : class where TQuery : Query
        {
            return source.Provider.CreateQuery<TEntity>(
                        Expression.Call(
                            instance: null,
                            method: FetchByQueryInfo.MakeGenericMethod(typeof(TEntity), typeof(TQuery)),
                            source.Expression, Expression.Parameter(typeof(TQuery)))).VisitFetchByQuery(query);
        }

        private static IQueryable<TEntity> VisitFetchByQuery<TEntity, TQuery>(this IQueryable<TEntity> source, TQuery query) where TEntity : class where TQuery : Query {
            var visitor = new FetchByQueryInfoExpressionVisitor<TQuery>(query);
            var lambda = visitor.Visit(source.Expression);
            var call = Expression
                .Call(
                    typeof(Queryable),
                    nameof(Queryable.Where),
                    new[] { source.ElementType },
                    Expression.Constant(source),
                    Expression.Quote(lambda));
            return source.Provider.CreateQuery<TEntity>(call);
        }
    }
}
