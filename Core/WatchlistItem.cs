using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class WatchlistItem
    {
        public string Symbol { get; set; }

        public override string ToString()
        {
            return $"{{{Symbol}}}";
        }

        public override bool Equals(object obj) =>
            obj is WatchlistItem item &&
                   Symbol == item.Symbol;
    }
}
