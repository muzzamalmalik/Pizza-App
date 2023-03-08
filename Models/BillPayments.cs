using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class BillPayments : BaseEntity
    {
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public int PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransactionReference { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool Active { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order ObjOrder { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }

    }
}
