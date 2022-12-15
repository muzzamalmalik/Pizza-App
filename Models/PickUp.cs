using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class PickUp : BaseEntity
    {
        public int BranchId { get; set; }
        public int OrderId { get; set; }
        public int Option { get; set; } // Deliver / PickUp
        [Required]
        [StringLength(50, ErrorMessage = "Order To cannot be longer then 50 characters")]
        public string OrderTo { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "Phone Name cannot be longer then 11 characters")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Pick Up Time cannot be longer then 100 characters")]
        public string PickUpTime { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Instruction cannot be longer then 250 characters")]
        public string Instructions { get; set; }
        public int PaymentMode { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order ObjOrder { get; set; }

    }
}
