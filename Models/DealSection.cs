using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class DealSection : BaseEntity
    {
        public int DealId { get; set; }
        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }

        [ForeignKey("DealId")]
        public virtual Deal ObjDeal { get; set; }

    }
}
