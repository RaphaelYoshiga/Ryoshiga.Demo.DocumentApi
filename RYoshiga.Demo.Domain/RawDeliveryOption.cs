namespace RYoshiga.Demo.Domain
{
    public class RawDeliveryOption
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DaysToDispatch { get; set; }
        public int DaysToDeliver { get; set; }
    }
}