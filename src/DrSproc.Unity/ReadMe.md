# DrSproc Unity Extensions
Dr Sproc is a syntactically simple way to call SQL stored procedures!

## Unity Extensions
This project has a simple extension for registering DrSproc and it's dependency injection interfaces within a Unit Container.

### Installation

You can install Dr Sproc with Unity from Nuget Package Manager, or Nuget CLI:

```cli
nuget install DrSproc.Unity
```

Or you can use the dotnet cli:

```cli
dotnet add package DrSproc.Unity
```

### Usage
Start by registering dotValidate in your `UnityConfig` class:

```cs
public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        // register all your components with the container here
        // it is NOT necessary to register your controllers

        // e.g. container.RegisterType<ITestService, TestService>();

        container.RegisterDrSproc();

        //...
    }
}
```
Then you can inject the interface `ISqlConnector` and use *DrSproc* as per the instructions [here](https://github.com/jephbayf1986/DrSproc)

```cs

public class MyRepository : IMyRepository
{
    private readonly ISqlConnector _sqlConnector;

    public MyRepository(ISqlConnector sqlConnector)
    {
        _sqlConnector = sqlConnector;
    }
    
    ///...
}

```

## License, Copyright etc
Dr Sproc and all associated packages are created by Jeph & Georgina Bayfield and is licensed under the MIT license.
