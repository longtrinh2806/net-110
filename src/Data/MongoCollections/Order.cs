using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MongoCollections
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("orderId")]
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
