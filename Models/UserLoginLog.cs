using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class UserLoginLog : BaseEntity
    {
        public int LoginStatus { get; set; }
        public DateTime LogOutDateTime { get; set; }
        public bool  ActiveQueue { get; set; }


        [ForeignKey("CretedById")]
        public virtual User ObjUser { get; set; }

    }
}
