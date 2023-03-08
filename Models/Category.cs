using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string  Description { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }

    }
}
