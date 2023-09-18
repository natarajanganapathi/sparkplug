# Database Migration

```
dotnet ef migrations --list
```

dotnet ef migrations add InitialCreate --startup-project ./sdk/ClientSdkGenerator --project ./src/SparkPlug.DesignTimeMigration --context HomeDbMigrationContext