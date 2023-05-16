using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Crust : BaseEntity
    {
        [Required]
        [StringLength(250, ErrorMessage = "Description cannot be longer then 250 characters")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Description cannot be longer then 250 characters")]
        public string Description { get; set; }
        public int Price { get; set; }
        public int? ItemId { get; set; }
        public int ItemSizeId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }


        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }
        //[ForeignKey("ItemId")]
        //public virtual Item ObjItem { get; set; }

    }
}
