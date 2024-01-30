using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MongoCollections
{
    public class Shipper
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("shipperId")]
        public Guid ShipperId { get; set; }
        public string ShipperCode { get; set; }
        public string ShipperName { get; set;}
    }
}
