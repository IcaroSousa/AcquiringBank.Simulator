using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MongoDB.Driver;
using Xunit;

namespace Application.Services.IntegrationTests.Fixtures;

public class MongoDbFixture : IAsyncLifetime
{
    private const int MongoDbPublicPort = 27017;
    private const string MongoDbDockerImage = "mongo:latest";
    private const string MongoDbRootUserName = "admin";
    private const string MongoDbRootPassword = "Sup3rP4ss";
    
    private IContainer _mongodbContainer;
    private IMongoClient _mongoClient;
    
    public IMongoDatabase MongoDatabase;
    

    public async Task InitializeAsync()
    {
        _mongodbContainer = new ContainerBuilder()
            .WithImage(MongoDbDockerImage)
            .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", MongoDbRootUserName)
            .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", MongoDbRootPassword)
            .WithPortBinding(MongoDbPublicPort, true)
            .WithCleanUp(true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(MongoDbPublicPort))
            .Build();

        await _mongodbContainer.StartAsync();
        _mongoClient = new MongoClient($"mongodb://{MongoDbRootUserName}:{MongoDbRootPassword}@{_mongodbContainer.Hostname}:{_mongodbContainer.GetMappedPublicPort(MongoDbPublicPort)}");
        MongoDatabase = _mongoClient.GetDatabase(Guid.NewGuid().ToString("D"));
    }

    public async Task DisposeAsync()
    {
        await _mongodbContainer.StopAsync();
        await _mongodbContainer.DisposeAsync();
    }
}