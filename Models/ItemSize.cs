using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class ItemSize : BaseEntity
    {

        [Required]
        [StringLength(250, ErrorMessage = "Description cannot be longer then 250 characters")]
        public string  SizeDescription { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item ObjItem { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }
    }
}
