using System;
using System.Globalization;
using RYoshiga.Demo.Domain;
using Shouldly;

namespace RYoshiga.Demo.Specs
{
    public class DeliveryOptionResponseExample
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string DeliveryDate { get; set; }

        public void AssertAgainst(DeliveryOptionResponse deliveryOption)
        {
            deliveryOption.Name.ShouldBe(Name);
            deliveryOption.Price.ShouldBe(Price);
            var expectedDate = DateTime.ParseExact(DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            deliveryOption.DeliveryDate.ShouldBe(expectedDate);
        }
    }
}