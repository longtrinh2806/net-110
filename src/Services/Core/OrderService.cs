using Data.DataAccess;
using Data.Dtos;
using Data.MongoCollections;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core
{
    public interface IOrderService
    {
        Task<List<ShipperResponseDto>> GetShippers(DateTime from, DateTime to);
        Task PostFileAsync(List<IFormFile> file);
    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;

        public OrderService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ShipperResponseDto>> GetShippers(DateTime from, DateTime to)
        {
            List<ShipperResponseDto> shipperResponseDtos = new();

            var orderFilter = Builders<Order>.Filter.Eq(o => o.Status, "Delivered");

            var orderDelivery = await _appDbContext.Orders.Find(orderFilter).ToListAsync();

            if (orderDelivery.Count < 1) throw new Exception();
            Console.WriteLine();
            foreach (var order in orderDelivery)
            {
                var deliveryFilter = Builders<Delivery>.Filter.And(
                    Builders<Delivery>.Filter.Gte(d => d.DeliveryDate, from),
                    Builders<Delivery>.Filter.Lte(d => d.DeliveryDate, to),
                    Builders<Delivery>.Filter.Eq(x => x.OrderId, order.OrderId)
                );
                var delivery = _appDbContext.Deliveries.Find(deliveryFilter).FirstOrDefault();
                if (delivery == null) continue;
                var shipper = _appDbContext.Shippers.Find(x => x.ShipperId == delivery.ShipperId).FirstOrDefault();
                if(shipper == null) continue;

                ShipperResponseDto shipperResponseDto = new()
                {
                    ShipperId = shipper.ShipperId,
                    ShipperName = shipper.ShipperName,
                    OrderId = order.OrderId,
                    OrderStatus = order.Status
                };
                shipperResponseDtos.Add(shipperResponseDto);
            }
            return shipperResponseDtos;
        }

        public async Task PostFileAsync(List<IFormFile> file)
        {
            foreach (var formFile in file)
            {

                if (formFile.Length > 0)
                {

                    var templateUrl = formFile.FileName;
                    string filePath = Path.Combine(@"D:\Learning IT\Learning ASP NET\.net dev 110\Net110\uploads\", templateUrl);
                    Console.WriteLine(filePath);
                    string fileName = Path.GetFileName(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    ProcessExcelFile(filePath);
                }
            }
        }
        private void ProcessExcelFile(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new XSSFWorkbook(fileStream);
                var sheet = workbook.GetSheetAt(0);

                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++) 
                {
                    var row = sheet.GetRow(i);

                    var fileDetails = new FileDetails();

                    fileDetails.OrderCode = row.GetCell(0)?.ToString(); 
                    fileDetails.TotalPrice = Convert.ToInt32(row.GetCell(1)?.ToString()); 
                    fileDetails.DeliveryCode = row.GetCell(2)?.ToString(); 
                    fileDetails.DeliveryDate = Convert.ToDateTime(row.GetCell(3)?.ToString()); 
                    fileDetails.Status = row.GetCell(4)?.ToString(); 
                    fileDetails.ShipperCode = row.GetCell(5)?.ToString(); 
                    fileDetails.ShipperName = row.GetCell(6)?.ToString();

                    Order order = new()
                    {
                        OrderCode = fileDetails.OrderCode,
                        TotalPrice = fileDetails.TotalPrice,
                        Status = fileDetails.Status
                    };
                    _appDbContext.Orders.InsertOne(order);

                    Shipper shipper = new()
                    {
                        ShipperCode = fileDetails.ShipperCode,
                        ShipperName = fileDetails.ShipperName
                    };
                    _appDbContext.Shippers.InsertOne(shipper);

                    Delivery delivery = new()
                    {
                        OrderId = order.OrderId,
                        ShipperId = shipper.ShipperId,
                        DeliveryCode = fileDetails.DeliveryCode,
                        DeliveryDate = fileDetails.DeliveryDate
                    };
                    _appDbContext.Deliveries.InsertOne(delivery);
                }

               
            }
        }


    }
}
