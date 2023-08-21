namespace AcquiringBank.Infrastructure.Configuration;

public record StorageSettings
{
    public MongoDbSettings MongoDb { get; set; }
}