using System;

namespace RYoshiga.Demo.Domain
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }

    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
