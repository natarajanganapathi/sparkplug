
## sample Query Request
```json
{
  "select": [
      "PersonName", "Department", "Salary", "Id"
  ],
  "where": {
     "op": 0,
     "filterType": 0,
     "filters": [{
        "filterType": 1,
        "field": "PersonName",
        "op": 0,
        "value": "Demo"
      }]
  },
  "sort": [
    {
      "field": "PersonName",
      "direction": 1
    }, {
      "field": "Salary",
      "direction": 1
    }
  ],
  "page": {
    "pageNo": 2,
    "pageSize": 0
  }
}

## Sample request with Includes options.

{
  "select": [
      "PersonName", "Department", "Salary", "Id", "Address"
  ],
  "includes": [
    {
      "name": "Address",
      "select": ["Id", "Street"]
    }
  ],
  "where": {
     "op": 1,
     "filterType": 0,
     "filters": [{
        "filterType": 1,
        "field": "PersonName",
        "op": 0,
        "value": "Demo1"
      }, {
        "filterType": 1,
        "field": "PersonName",
        "op": 0,
        "value": "Natarajan Ganapathi"
      }]
  },
  "sort": [
    {
      "field": "PersonName",
      "direction": 1
    }, {
      "field": "Salary",
      "direction": 1
    }
  ],
  "page": {
    "pageNo": 1,
    "pageSize": 10
  }
}
```

## Sample Request with ENUM as String 

```json
{
  "select": [
      "personName", "Department", "Salary", "id", "address"
  ],
  "includes": [
    {
      "name": "Address",
      "select": ["Id", "Street"]
    }
  ],
  "where": {
     "filterType": "COMPOSITE",
     "op": "OR",
     "filters": [{
        "filterType": "FIELD",
        "op": "EQUAL",
        "field": "department",
        "value": "IT"
      }, {
        "filterType": "FIELD",
        "op": "EQUAL",
        "field": "PersonName",
        "value": "Demo User"
      }]
  },
  "sort": [
    {
      "field": "PersonName",
      "direction": "DESC"
    }, {
      "field": "Salary",
      "direction": "ASC"
    }
  ],
  "page": {
    "pageNo": 1,
    "pageSize": 10
  }
}
```

### C# Include child tables

```c#
var users = context.Users
    .Include(u => u.Address)
    .Include(u => u.Profile)
    .Select(u => new {
        u.Id,
        u.Name,
        Address = new {
            u.Address.Street,
            u.Address.City
        },
        Profile = new {
            u.Profile.Bio,
            u.Profile.Age
        }
    })
    .ToList();

```



```c#
var users = context.Users
    .Include(u => u.Address)
    .Select(u => new {
        u.Id,
        u.PersonName,
        u.MobileNo,
        Address = new {
            u.Address.Id,
            u.Address.FlatNo
        }
    })
    .ToList();

```