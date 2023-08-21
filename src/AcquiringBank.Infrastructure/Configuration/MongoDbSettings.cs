namespace AcquiringBank.Infrastructure.Configuration;

public record MongoDbSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
}