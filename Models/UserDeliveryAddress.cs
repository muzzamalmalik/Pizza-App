using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class UserDeliveryAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SecoundaryAddress { get; set; }
    }
}
