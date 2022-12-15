using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public int CretedById { get; set; }

        public int CompanyId { get; set; }

        public DateTime DateCreated { get; set; }

        public int? UpdateById { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
