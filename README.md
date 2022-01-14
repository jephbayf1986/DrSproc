# DrSproc
Dr Sproc is a syntactically simple way to call SQL stored procedures!

The idea behind this library is to enable developers to write stored procedure calls in a syntatically similar way to how they are called in SQL server. Using a chained builder-like pattern it ensures you don't have to worry about SQL Connections and Commands. This should allow quick understanding of what's being called. It also aims to give clear error handling to ensure you don't have to spend too much time debugging through code to know which stored procedure, parameter or field is causing the problem.

## How to Use
Before using *Dr Sproc* to call stored procedures the minimum setup required is to declare any Databases you may use, inheriting from the ```IDatabase``` interface:
```cs
// Declare the Databases ensuring they can fetch the connection string*
public class ContosoDb : IDatabase
{
    public string GetConnectionString()
    {
      return ConfigurationManager.ConnectionStrings["DrSprocTest"].ConnectionString;
    }
}
```

Now you can use this database to create a target database and build up a stored procedure call:
```cs
var db = DoctorSproc.Use<ContosoDb>();

db.Execute("sp_LogEvent")
            .Go();
```

### Parameters

After calling the execute method, you can declare the parameters you need using **WithParam** and **WithParamIfNotNull** methods.

```cs
var db = DoctorSproc.Use<ContosoDb>();

db.Execute("sp_UpdateDepartment")
            .WithParam("@DepartmentId", department.Id)
            .WithParam("@DepartmentName", department.Name)
            .WithParamIfNotNull("@AlternativeName", department.AltName)
                .Go();
```
*Note: the @ sign is not required in the declaration of the parameter name, so either the above or below would result in an identical procedure call:*

```cs
db.Execute("sp_UpdateDepartment")
            .WithParam("DepartmentId", department.Id)
            .WithParam("DepartmentName", department.Name)
            .WithParamIfNotNull("AlternativeName", department.AltName)
                .Go();
```
### Return Types

### Mapping

### Asynchronous Calls


### Transactions


### Exceptions

See [this](https://github.com/jephbayf1986/DrSprocExampleProject) project for several examples of how to implement DrSproc.

## License, Copyright etc
Dr Sproc is created by Jeph & Georgina Bayfield and is licensed under the MIT license.
