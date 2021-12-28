using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Position
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal MarketValue { get; set; }

        public override bool Equals(object obj) =>
            obj is Position other
            && other.Symbol == Symbol
            && other.Name == Name
            && other.Quantity == Quantity
            && other.MarketValue == MarketValue;
    }
}
