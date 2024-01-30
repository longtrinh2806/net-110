using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class ShipperResponseDto
    {
        [BsonRepresentation(BsonType.String)]
        public Guid ShipperId { get; set; }
        public string ShipperName { get; set;}

        [BsonRepresentation(BsonType.String)]
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; }
    }
}
