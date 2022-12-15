using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Topping : BaseEntity
    {
        
        [StringLength(50, ErrorMessage = "Name cannot be longer then 50 characters")]
        public string Name { get; set; }
        public int Price { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemSizeId { get; set; }
        public int? OrderDetailId { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item ObjItem { get; set; }

        [ForeignKey("ItemSizeId")]
        public virtual ItemSize ObjItemSize { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category ObjCategory { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }


        [ForeignKey("OrderDetailId")]
        public virtual OrderDetail ObjOrderDetailId { get; set; }


    }
}
