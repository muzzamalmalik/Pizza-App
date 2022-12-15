using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class ToppingTransactionLog : BaseEntity
    {
        public int ToppingId { get; set; }

        public int OldPrice { get; set; }

        public int NewPrice { get; set; }
    }
}
