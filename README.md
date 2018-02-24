# Woodman.Caching
.NET Standard 2.0 Caching layer wrapping Microsoft.Extensions.Caching using MessagePack serialization

## Installation ##
````
Install-Package Woodman.Caching
````

## Usage ##
````
// Configure the services
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    // useAsDefault will set the ICache service to resolve as the RedisCache Instance
    services.AddRedisCache(RedisCacheConfiguration.Localhost, useAsDefault: true);
	
    services.AddInMemoryCache(useAsDefault: false);
	
    // OR add both services in one line - the generic will be used to resolve ICache
    services.AddCaching<IRedisCache>(RedisCacheConfiguration.Localhost);
  }
}

// Inject the services into your class
public class Foo
{
  public Food(
    IRedisCache redisCache,
    IInMemoryCache inMemCache,
    ICache cache
  )
  {
    // ICache will be the same instance as IRedisCache in this case
  }
}
````
