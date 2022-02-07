# DrSproc Dependency Injection Extensions
Dr Sproc is a syntactically simple way to call SQL stored procedures!

## Dependency Injection Extensions
This project has a simple extension for registering DrSproc and it's dependency injection interfaces within an IServiceCollection.

### Installation

You can install Dr Sproc with Dependency Injection from Nuget Package Manager, or Nuget CLI:

```cli
nuget install DrSproc.DependencyInjection
```

Or you can use the dotnet cli:

```cli
dotnet add package DrSproc.DependencyInjection
```

### Usage

Start by registering DrSproc in your startup class:

```cs

using DrSproc.Registration;
// using...

namespace MyProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.RegisterDrSproc();
            
            //...
        }
        
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
