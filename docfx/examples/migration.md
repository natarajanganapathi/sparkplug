# Database Migration

```
dotnet ef migrations --list
```

dotnet ef migrations add InitialCreate --project ./src/SparkPlug.Persistence.EntityFramework --startup-project ./samples/Api.Module.Sample --context TenantDbContext