using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class DealCartDetails : BaseEntity
    {
        public int? DealId { get; set; }
        public int? DealSectionId { get; set; }
        public int? OrderId { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public string FlavourName { get; set; }
        public int? ItemId { get; set; }

        [ForeignKey("DealId")]
        public virtual Deal ObjDeal { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order ObjOrder { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category ObjCategory { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item ObjItem { get; set; }
    }
}
