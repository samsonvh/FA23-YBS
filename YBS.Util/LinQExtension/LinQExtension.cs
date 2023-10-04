using System.Linq.Expressions;

public static class LinqQExtension
    {

        public static IQueryable<TEntity> SortDesc<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            var type = typeof(TEntity);
            var property = type.GetProperties().FirstOrDefault(q => q.Name.ToLower() == orderByProperty?.ToLower());
            if (property == null)
            {
                return source;
            }
            if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                desc = !desc;
            }
            string command = desc ? "OrderByDescending" : "OrderBy";
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[]
            {
                type,property.PropertyType
            }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }