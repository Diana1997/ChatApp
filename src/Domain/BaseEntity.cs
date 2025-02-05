using System;
using MongoDB.Bson;

namespace Domain
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; }
        public DateTime CreationTime { get; set; }
    }
}