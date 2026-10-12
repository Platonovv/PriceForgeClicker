using System;

namespace PriceForgeClicker.Services
{
    public interface IReactiveObject
    {
        void Subscribe(Action listener);
        void Unsubscribe(Action listener);
    }
}
