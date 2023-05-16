using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public int? DealId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSizeId { get; set; }
        public int? CrustId { get; set; }
        public int Quantity { get; set; }
        public int BillGroup { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order ObjOrder { get; set; }

        [ForeignKey("DealId")]
        public virtual Deal ObjDeal { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item ObjItem { get; set; }

        [ForeignKey("ItemSizeId")]
        public virtual ItemSize ObjItemSize { get; set; }

        [ForeignKey("CrustId")]
        public virtual Crust ObjCrust { get; set; }
        [ForeignKey("CretedById")]
        public virtual User ObjUser { get; set; }

    }
}
