using System;

namespace RYoshiga.Demo.Domain
{
    public class DeliveryOptionResponse
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}