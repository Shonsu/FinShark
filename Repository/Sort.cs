using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace api.Repository
{
    public static class Sort
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, string direction)
        {

            ParameterExpression param = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.PropertyOrField(param, orderByMember);
            LambdaExpression keySelector = Expression.Lambda(property, param);
            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == "ASC" ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(keySelector)
            );
            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}