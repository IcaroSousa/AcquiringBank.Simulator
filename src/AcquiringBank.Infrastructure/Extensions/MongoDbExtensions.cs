using AcquiringBank.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AcquiringBank.Infrastructure.Extensions;

public static class MongoDbExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Storage:MongoDb").Get<MongoDbSettings>();
        var mongoClient =
            new MongoClient($"mongodb://{settings.UserName}:{settings.Password}@{settings.Host}:{settings.Port}");

        serviceCollection.AddSingleton<IMongoClient>(_ => mongoClient);
        serviceCollection.AddSingleton<IMongoDatabase>(_ => mongoClient.GetDatabase(settings.Database));
        
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        return serviceCollection;
    }
}