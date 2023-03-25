```c#

   public static IQueryable<JObject> ApplySelector<TEntity>(this IQueryable<TEntity> query, string[]? select, Include[]? includes)
    {
        if (select == null || select.Length == 0) return query.Select(e => e == null ? new JObject() : JObject.FromObject(e));
        var type = typeof(TEntity);
        var parameter = Expression.Parameter(type, "select");
        var properties = type.GetProperties().Where(prop => select.Contains(prop.Name));
        var bindings = GetBindings(properties, parameter, includes);

        var memberInitExp = Expression.MemberInit(Expression.New(typeof(TEntity)), bindings);
        var selector = Expression.Lambda<Func<TEntity, object>>(memberInitExp, parameter);
         
         var demo = query.Select(selector);
         Console.WriteLine(query.ToQueryString());
         
         return query.Select(x => JObject.FromObject(x!));
    }

private static IEnumerable<MemberBinding> GetBindings(IEnumerable<PropertyInfo> properties, Expression parameter, Include[]? includes)
{
    foreach (var property in properties)
    {
        var expression = (Expression)Expression.Property(parameter, property);
        if (property.PropertyType.IsClass && includes?.Any(i => i.Name == property.Name) == true)
        {
            var include = includes.First(i => i.Name == property.Name);
            var inType = property.PropertyType;
            var inProperties = inType.GetProperties().Where(prop => include.Select.Contains(prop.Name));
            var inBindings = GetBindings(inProperties, expression, include.Includes);
            expression = Expression.MemberInit(Expression.New(inType), inBindings);
        }
        yield return Expression.Bind(property, expression);
    }
}




        // var memberInitExp = Expression.MemberInit(Expression.New(typeof(TEntity)), bindings);
        // var selector = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExp, parameter);

        // var initExp = Expression.ListInit(Expression.New(typeof(JObject)), bindings);
        // var selector = Expression.Lambda<Func<TEntity, JObject>>(initExp, parameter);

        // var projectedBindings = bindings.Select(b => Expression.Bind(b.Member, Expression.Property(parameter, b.Member.Name)));
        // var projectedInitExp = Expression.MemberInit(Expression.New(typeof(ExpandoObject)), projectedBindings);
        // var projectedSelector = Expression.Lambda<Func<TEntity, ExpandoObject>>(projectedInitExp, parameter);
        var result = query.Select(projectedSelector);
        Console.WriteLine(result.ToQueryString());
        // return query.Select(projectedSelector);
        return query.Select(x => JObject.FromObject(x!));

        
```

```c#
ParameterExpression personParameter = Expression.Parameter(typeof(Person), "u");

var addressType = typeof(Address);
MemberExpression streetProperty = Expression.Property(
    Expression.Property(
        personParameter,
        "Address"
    ),
    "Street"
);
MemberExpression flatNoProperty = Expression.Property(
    Expression.Property(
        personParameter,
        "Address"
    ),
    "FlatNo"
);
NewExpression addressExpression = Expression.New(
    addressType.GetConstructor(new[] { typeof(string), typeof(string) }),
    streetProperty,
    flatNoProperty
);
MemberBinding idBinding = Expression.Bind(
    typeof(Person).GetProperty("Id"),
    Expression.Property(personParameter, "Id")
);
MemberBinding personNameBinding = Expression.Bind(
    typeof(Person).GetProperty("PersonName"),
    Expression.Property(personParameter, "PersonName")
);
MemberBinding addressBinding = Expression.Bind(
    typeof(Person).GetProperty("Address"),
    addressExpression
);
MemberInitExpression personExpression = Expression.MemberInit(
    Expression.New(typeof(Person)),
    idBinding,
    personNameBinding,
    addressBinding
);
Expression<Func<Person, object>> personProperties = Expression.Lambda<Func<Person, object>>(
    personExpression,
    personParameter
);

```

{
    new Person() 
    {
        Id = select.Id, 
        Address = select.Address,
        Address = new Address() 
        {
            Id = select.Address.Id, 
            FlatNo = select.Address.FlatNo
        }, 
        
    }
}

{
    new Person() 
    {
        Id = select.Id, 
        Address = new Address() 
        {
            Id = select.Address.Id, 
            FlatNo = select.Address.FlatNo
        }
    }
}