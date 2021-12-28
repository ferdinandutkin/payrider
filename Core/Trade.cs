using System;

namespace Core
{
    public class Trade
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Side Side { get; set; }

        public override string ToString()
        {
            return $"{{{Name}, {Price}, {Side}}}";
        }

        public override bool Equals(object obj) => obj is Trade trade &&
                                                   Name == trade.Name &&
                                                   Price == trade.Price &&
                                                   Side == trade.Side;
    }

}
