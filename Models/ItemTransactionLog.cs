using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class ItemTransactionLog : BaseEntity
    {
        public int ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }

    }
}
