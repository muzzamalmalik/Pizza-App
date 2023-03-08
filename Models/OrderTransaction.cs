using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class OrderTransaction 
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OrderStatusOld { get; set; }
        public int CurrentStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

    }
}
