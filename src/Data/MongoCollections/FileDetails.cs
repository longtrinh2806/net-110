using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MongoCollections
{
    public class FileDetails
    {
        public string OrderCode { get; set; }
        public int TotalPrice { get; set; }
        public string DeliveryCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public string ShipperCode { get; set; }
        public string ShipperName { get; set; }
    }
}
