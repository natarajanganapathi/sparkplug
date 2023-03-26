# Entity Framework Entity Relationship 

## One-to-One Relationship:

Suppose you have two entities named `Person` and `Address`, where a `Person` can have one `Address` and an `Address` belongs to one `Person`. The `Person` entity has a foreign key `AddressId` referencing the `Address` entity's primary key `Id`.

```c#
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    public Person Person { get; set; }
}
```

You can configure this relationship in the `OnModelCreating` method like this:

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Person>()
        .HasOne(p => p.Address)
        .WithOne(a => a.Person)
        .HasForeignKey<Person>(p => p.AddressId);
}
```

In this example, we use the `HasOne` method to specify that a `Person` has one `Address`, and the `WithOne` method to specify that an `Address` belongs to one `Person`. Finally, we use the `HasForeignKey` method to specify that the `Person` entity's `AddressId` property should be used as the foreign key for this relationship.


## One-to-Many Relationship:

```c#
public class Order
{
    public int Id { get; set; }
    public ICollection<OrderItem> Items { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}

// In the DbContext's OnModelCreating method:
modelBuilder.Entity<Order>()
    .HasMany(o => o.Items)
    .WithOne(i => i.Order)
    .HasForeignKey(i => i.OrderId);

```

## Many-to-Many Relationship:

```c#
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Course> Courses { get; set; }
}

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Student> Students { get; set; }
}

// In the DbContext's OnModelCreating method:
modelBuilder.Entity<Student>()
    .HasMany(s => s.Courses)
    .WithMany(c => c.Students)
    .UsingEntity(j => j.ToTable("StudentCourses"));

```

## Many-to-One Relationship:

```C#
public class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public ICollection<OrderItem> Items { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}

// In the DbContext's OnModelCreating method:
modelBuilder.Entity<Order>()
    .HasOne(o => o.Customer)
    .WithMany()
    .HasForeignKey(o => o.CustomerId);

modelBuilder.Entity<Order>()
    .HasMany(o => o.Items)
    .WithOne(i => i.Order)
    .HasForeignKey(i => i.OrderId);

```
