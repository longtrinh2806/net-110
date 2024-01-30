using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MongoCollections
{
    public class Delivery
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid DeliveryId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid OrderId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ShipperId { get; set; }
        public string DeliveryCode { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
