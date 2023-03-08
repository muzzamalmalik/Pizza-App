using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class OrderDetailAdditionalDetails : BaseEntity
    {
        public int ReferenceId { get; set; }
        public int OrderDetailId { get; set; }
        public int ReferenceTypeId { get; set; }

        [ForeignKey("OrderDetailId")]
        public virtual OrderDetail ObjOrderDetail { get; set; }
    }
}
