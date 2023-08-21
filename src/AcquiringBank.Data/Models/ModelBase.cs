using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AcquiringBank.Data.Models;

public class ModelBase
{
    [BsonId] 
    [BsonIgnoreIfDefault] 
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}