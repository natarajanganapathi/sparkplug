
## Sample Connection String
```string
Server=sssssss.postgres.database.azure.com;Database=ddddddd;Port=5432;User Id=uuuuuuu;Password=ppppppp;Ssl Mode=Require;Trust Server Certificate=true;Search Path=tenant_1,public
```

Note: `Search Path` denotes that tenants' schema if the the tenant is not using defalt schema. `Search Path` will containe comma separated schema names like example connection string `Search Path=tenant_1,public`. 

## TenantOptions

"DbConfig:ConnectionString"

## Create new Schema
```sql
CREATE SCHEMA tenant_1;
```

## List Schema
```sql
SELECT schema_name FROM information_schema.schemata;
```

## Switch Schema
```sql
SET search_path TO tenant_1;
```

## Converty the type of a column from one type to another
```sql
ALTER TABLE tenant_1."MenuItem" ALTER COLUMN "Properties" TYPE json USING "Properties"::json;
```

## Get the data type for a column
```sql
SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'MenuItem' AND column_name = 'Properties';
```

## JSON in postgresql
[.Net Reference](https://www.npgsql.org/efcore/mapping/json.html?tabs=data-annotations%2Cpoco)