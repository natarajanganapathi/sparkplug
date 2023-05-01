namespace SparkPlug.Persistence.EntityFramework;

public static class QueryExtentions
{
    // TODO: Optimization required
    private static readonly MethodInfo? _selectMethodInfo = Array.Find(typeof(Enumerable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static),
                m => m.Name == nameof(Enumerable.Select) && m.GetParameters().Length == 2 &&
                m.GetParameters()[1].ParameterType.GetGenericArguments()[1] == m.ReturnType.GetGenericArguments()[0]
    );

    #region Includes
    public static IQueryable<TEntity> ApplyIncludes<TEntity>(this IQueryable<TEntity> query, Include[]? includes, string? parent = default) where TEntity : class
    {
        return includes?.Length > 0 ? includes.Aggregate(query, (current, include) => current.ApplyInclude(include, parent)) : query;
    }
    public static IQueryable<TEntity> ApplyInclude<TEntity>(this IQueryable<TEntity> query, Include include, string? parent = default) where TEntity : class
    {
        if (include == null || string.IsNullOrEmpty(include.Name)) return query;
        query = string.IsNullOrWhiteSpace(parent)
                ? query.Include(include.Name)
                : query.Include(new StringBuilder(parent).Append('.').Append(include.Name).ToString());
        return include.Includes?.Length > 0 ? query.ApplyIncludes(include.Includes, include.Name) : query;
    }
    #endregion

    #region Where
    public static IQueryable<TEntity> ApplyWhere<TEntity>(this IQueryable<TEntity> query, Filter? filter)
    {
        if (filter == null) return query;
        var where = filter.GetFilterExpression<TEntity>();
        return where == null ? query : query.Where(where);
    }
    #endregion

    #region Sort
    // TODO: Optimization required
    public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> query, Order[]? sort)
    {
        if (sort == null || sort.Length == 0) return query;

        var parameterExp = Expression.Parameter(typeof(TEntity), "sort");
        IOrderedQueryable<TEntity>? orderedQuery = null;
        foreach (var order in sort)
        {
            var propertyExp = Expression.Property(parameterExp, order.Field);
            var lambdaExp = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(propertyExp, typeof(object)), parameterExp);
            orderedQuery = orderedQuery == null
                    ? order.Direction == Direction.Ascending ? query.OrderBy(lambdaExp) : query.OrderByDescending(lambdaExp)
                    : order.Direction == Direction.Ascending ? orderedQuery.ThenBy(lambdaExp) : orderedQuery.ThenByDescending(lambdaExp);
        }
        return orderedQuery ?? query;
    }
    #endregion

    #region Page Context
    public static IQueryable<TEntity> ApplyPageContext<TEntity>(this IQueryable<TEntity> query, PageContext? pageContext)
    {
        if (pageContext == null) return query;
        return query.Skip(pageContext.Skip).Take(pageContext.PageSize);
    }
    #endregion

    #region Projection
    public static IQueryable<JObject> ApplyProjection<TEntity>(this IQueryable<TEntity> query, string[]? select, Include[]? includes)
    {
        var elementType = typeof(TEntity);
        select = select == null || select.Length == 0 ? GetProperties(elementType).ToArray() : select;
        var sourceParameter = Expression.Parameter(elementType, elementType.Name);
        var jObjectInitExpression = GetJObjectValueExpression(elementType, sourceParameter, select, includes);
        var lambda = Expression.Lambda<Func<TEntity, JObject>>(jObjectInitExpression, sourceParameter);
        return query.Select(lambda);
    }

    public static IEnumerable<string> GetProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any())
            .Select(prop => prop.Name);
    }

    public static Expression GetValueExpression(MemberExpression propAccessExpression)
    {
        return Expression.Convert(propAccessExpression, typeof(JToken));
    }

    public static Expression GetEnumValueExpression(MemberExpression propAccessExpression)
    {
        var intValueExpression = Expression.Convert(propAccessExpression, typeof(int));
        return Expression.Convert(intValueExpression, typeof(JToken));
    }

    public static Expression GetJObjectValueExpression(Type elementType, Expression sourceParameter, string[]? select, Include[]? includes)
    {
        select = select == null || select.Length == 0 ? GetProperties(elementType).ToArray() : select;
        var jObjectExpression = Expression.New(typeof(JObject));
        var addMethodInfo = typeof(JObject).GetMethod(nameof(JObject.Add), new[] { typeof(string), typeof(JToken) });
        var jPropertiesExpressions = select.Select(property =>
        {
            var properties = property.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            if (properties.Length > 2) throw new QueryEntityException(new StringBuilder().Append(property).Append(" should not have more than 2 keywords").ToString());
            string source = properties[0];
            var propInfo = elementType.GetProperty(source, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        ?? throw new ArgumentException(new StringBuilder().Append('\'').Append(source).Append("' is not a valid property of type ").Append(elementType).ToString());
            var propAccessExpression = Expression.Property(sourceParameter, propInfo);
            var type = propInfo.PropertyType;
            var include = includes?.FirstOrDefault(i => i.Name.Equals(source, StringComparison.CurrentCultureIgnoreCase));
            // Expression? value = type switch
            // {
            //     { IsEnum: true } => GetEnumValueExpression(propAccessExpression),
            //     { FullName: "System.String" } => GetValueExpression(propAccessExpression),
            //     { IsArray: true } or { IsGenericType: true } => GetCollectionValueExpression(type.GenericTypeArguments[0], propAccessExpression, include?.Select, include?.Includes),
            //     { IsClass: true } => GetJObjectValueExpression(type, propAccessExpression, include?.Select, include?.Includes),
            //     _ => GetValueExpression(propAccessExpression)
            // };
            Expression? value = GetExpression(propAccessExpression, type, include);

            var target = properties.Length == 2 ? properties[1] : propInfo.Name;
            return Expression.ElementInit(addMethodInfo!, Expression.Constant(target.ToCamelCase()), value!);
        });
        return Expression.ListInit(jObjectExpression, jPropertiesExpressions);
    }

    private static Expression? GetExpression(MemberExpression propAccessExpression, Type type, Include? include)
    {
        if (type.IsEnum)
        {
            return GetEnumValueExpression(propAccessExpression);
        }
        else if(type == typeof(string))
        {
            return GetValueExpression(propAccessExpression);
        }
        else if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
        {
            return GetCollectionValueExpression(type.GenericTypeArguments[0], propAccessExpression, include?.Select, include?.Includes);
        }
        else if (type.IsClass)
        {
            return GetJObjectValueExpression(type, propAccessExpression, include?.Select, include?.Includes);
        }
        else
        {
            return GetValueExpression(propAccessExpression);
        }
    }

    public static Expression GetCollectionValueExpression(Type elementType, MemberExpression propAccessExpression, string[]? select, Include[]? includes)
    {
        MethodCallExpression? toArrayCall = Type.GetTypeCode(elementType) switch
        {
            TypeCode.Object when elementType.IsClass => GetObjectMethodCallException(elementType, propAccessExpression, select, includes),
            _ => GetPrimitiveMethodCallExpression(elementType, propAccessExpression)
        };
        var jArrayCtor = typeof(JArray).GetConstructor(new[] { typeof(IEnumerable) });
        return Expression.New(jArrayCtor!, toArrayCall);
    }

    public static MethodCallExpression GetPrimitiveMethodCallExpression(Type elementType, MemberExpression propAccessExpression)
    {
        var toArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray))?.MakeGenericMethod(elementType);
        return Expression.Call(toArrayMethod!, propAccessExpression);
    }

    public static MethodCallExpression GetObjectMethodCallException(Type elementType, MemberExpression propAccessExpression, string[]? select, Include[]? includes)
    {
        var elementParam = Expression.Parameter(elementType, elementType.Name);
        var aryObj = GetJObjectValueExpression(elementType, elementParam, select, includes);
        var selectLambda = Expression.Lambda(
            Expression.Lambda(aryObj, elementParam),
            Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(elementType))
        ).Body;
        MethodInfo? selectMethod = _selectMethodInfo?.MakeGenericMethod(elementType, typeof(JObject));
        var toArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray))?.MakeGenericMethod(typeof(JObject));
        return Expression.Call(toArrayMethod!, Expression.Call(selectMethod!, propAccessExpression, selectLambda));
    }

    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        var builder = new StringBuilder(str.Length);
        var words = str.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        builder.Append(char.ToLower(words[0][0]));
        builder.Append(words[0][1..]);
        for (int i = 1; i < words.Length; i++)
        {
            builder.Append(char.ToUpper(words[i][0]));
            builder.Append(words[i][1..]);
        }
        return builder.ToString();
    }
    #endregion

    #region Selector
    public static IQueryable<TEntity> ApplySelector<TEntity>(this IQueryable<TEntity> query, string[]? select, Include[]? includes)
    {
        if (select == null || select.Length == 0) return query;
        var type = typeof(TEntity);
        var parameter = Expression.Parameter(type, type.Name);
        var expression = GetMemberInitExpression(type, parameter, select, includes);
        var selector = Expression.Lambda<Func<TEntity, TEntity>>(expression, parameter);
        return query.Select(selector);
    }
    private static IEnumerable<MemberBinding> GetBindings(Type type, Expression parameter, string[] propertyNames, Include[]? includes)
    {
        foreach (var propertyName in propertyNames)
        {
            var propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException($"property {propertyName} not available in type {type}", propertyName);
            if (propertyInfo.PropertyType.IsClass && includes?.Any(i => i.Name == propertyInfo.Name) == true)
            {
                var include = includes.First(i => i.Name == propertyInfo.Name);
                var selects = include.Select?.Length > 0 ? include.Select : GetProperties(propertyInfo.PropertyType).ToArray();
                MemberExpression inProperty = Expression.Property(parameter, propertyInfo.Name);
                var inExpression = GetMemberInitExpression(propertyInfo.PropertyType, inProperty, selects, include.Includes);
                yield return GetMemberBinding(propertyInfo, inExpression);
            }
            else
            {
                yield return GetMemberBinding(propertyInfo, parameter);
            }
        }
    }
    private static MemberBinding GetMemberBinding(PropertyInfo propertyInfo, Expression parameter)
    {
        return Expression.Bind(propertyInfo, Expression.Property(parameter, propertyInfo.Name));
    }
    private static MemberBinding GetMemberBinding(PropertyInfo propertyInfo, MemberInitExpression initExpression)
    {
        return Expression.Bind(propertyInfo, initExpression);
    }
    private static MemberInitExpression GetMemberInitExpression(Type type, Expression parameter, string[] select, Include[]? includes)
    {
        var bindings = GetBindings(type, parameter, select, includes);
        return Expression.MemberInit(Expression.New(type), bindings);
    }
    #endregion
}