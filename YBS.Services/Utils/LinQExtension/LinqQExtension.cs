using System.Linq.Expressions;

namespace Pnl.Util.Common.Extensions
{
    public static class LinqQExtension
    {

        public static IQueryable<TEntity> SortDesc<TEntity>(this IQueryable<TEntity> source, string orderByProperty, string? direction)
        {
            var type = typeof(TEntity);
            var property = type.GetProperties().FirstOrDefault(q => q.Name.ToLower() == orderByProperty?.ToLower());
            if (property == null)
            {
                return source;
            }
            string command;
            switch (direction)
            {
                case "asc":
                    command = "OrderBy";
                    break;
                default:
                    command = "OrderByDescending";
                    break;
            }
            var parameter = Expression.Parameter(type, "entity"); //entity
            var propertyAccess = Expression.MakeMemberAccess(parameter, property); //entity.property
            var orderByExpression = Expression.Lambda(propertyAccess, parameter); //entity => entity.property
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[]
            {
                type,property.PropertyType
            }, source.Expression, Expression.Quote(orderByExpression)); //OrderBy/OrderByDescending(entity => entity.property)
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
