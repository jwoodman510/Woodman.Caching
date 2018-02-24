using System;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Redis;
using Woodman.Caching;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, RedisCacheConfiguration config)
        {
            return services
                .AddRedisCache(config)
                .AddInMemoryCache();
        }

        public static IServiceCollection AddCaching(this IServiceCollection services, Action<RedisCacheConfiguration> configure)
        {
            return services
                .AddRedisCache(configure)
                .AddInMemoryCache();
        }

        public static IServiceCollection AddCaching<TDefaultCache>(this IServiceCollection services, RedisCacheConfiguration config)
            where TDefaultCache : ICache
        {
            var defaultCacheType = typeof(TDefaultCache);

            return services
                .AddRedisCache(config, defaultCacheType == typeof(IRedisCache) || defaultCacheType == typeof(Woodman.Caching.RedisCache))
                .AddInMemoryCache(defaultCacheType == typeof(IInMemoryCache) || defaultCacheType == typeof(InMemoryCache));
        }

        public static IServiceCollection AddCaching<TDefaultCache>(this IServiceCollection services, Action<RedisCacheConfiguration> configure)
            where TDefaultCache : ICache
        {
            return services
                .AddRedisCache(configure)
                .AddInMemoryCache();
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, RedisCacheConfiguration config, bool useAsDefault = false)
        {
            return services.AddRedisCache(c =>
            {
                c.Endpoints = config.Endpoints;
                c.Username = config.Username;
                c.Password = config.Password;
            }, useAsDefault);
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheConfiguration> configure, bool useAsDefault = false)
        {
            var config = new RedisCacheConfiguration();

            configure?.Invoke(config);

            services.AddOptions();

            services.AddSingleton<IDistributedCache>(p =>
            {
                var options = p.GetRequiredService<IOptions<RedisCacheOptions>>();

                options.Value.Configuration = config.ConnectionString;

                return new Caching.Redis.RedisCache(options);
            });

            services.AddSingleton<IRedisCache, Woodman.Caching.RedisCache>();

            if (useAsDefault)
            {
                services.AddSingleton<ICache, Woodman.Caching.RedisCache>();
            }

            MessagePackSerializer.SetDefaultResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            return services;
        }

        public static IServiceCollection AddInMemoryCache(this IServiceCollection services, bool useAsDefault = false)
        {
            services.AddSingleton<IInMemoryCache, InMemoryCache>();

            if (useAsDefault)
            {
                services.AddSingleton<ICache, InMemoryCache>();
            }

            return services;
        }
    }
}
