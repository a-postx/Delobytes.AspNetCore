using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Delobytes.AspNetCore;

public static class QueryableExtensions
{
    private static readonly MethodInfo OrderByMethod = typeof(Queryable)
        .GetMethods().Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);

    private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable)
        .GetMethods().Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

    /// <summary>
    /// Применить сортировку по имени одного свойства объекта.
    /// </summary>
    /// <typeparam name="TSource">Тип объекта.</typeparam>
    /// <param name="query">Запрос к которому необходимо добавить сортировку.</param>
    /// <param name="propertyName">Имя свойства объекта, по которому нужно произвести сортироваку.</param>
    /// <param name="desc">Признак сортировки по убыванию.</param>
    /// <returns>Выражение с сортировкой по указанному свойству.</returns>
    public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, bool desc) where TSource : class
    {
        ArgumentNullException.ThrowIfNull(query);

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException(nameof(propertyName));
        }

        Type entityType = typeof(TSource);

        PropertyInfo? propertyInfo = entityType.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"{propertyName} does not exists");
        }

        ParameterExpression arg = Expression.Parameter(entityType, "x");
        MemberExpression property = Expression.Property(arg, propertyName);
        LambdaExpression selector = Expression.Lambda(property, new ParameterExpression[] { arg });

        MethodInfo method = desc ? OrderByDescendingMethod : OrderByMethod;
        MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

        object? rawMethod = genericMethod.Invoke(genericMethod, new object[] { query, selector });

        if (rawMethod == null)
        {
            throw new InvalidOperationException("Error invoking generic method");
        }

        IOrderedQueryable<TSource> newQuery = (IOrderedQueryable<TSource>)rawMethod;

        return newQuery;
    }
}
