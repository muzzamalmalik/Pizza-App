using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class DealSection : BaseEntity
    {
        public int DealId { get; set; }
        [StringLength(50, ErrorMessage = "Title cannot be longer then 50 characters")]
        [Required]
        public string Title  { get; set; }
        [StringLength(50, ErrorMessage = "Description cannot be longer then 50 characters")]
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public int ChooseQuantity { get; set; }

        [ForeignKey("DealId")]
        public virtual Deal ObjDeal { get; set; }

    }
}
