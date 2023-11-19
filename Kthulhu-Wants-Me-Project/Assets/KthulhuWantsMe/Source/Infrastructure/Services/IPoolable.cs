using System;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IPoolable<T>
    {
        public Action<T> Release { get; set; }
    }
}