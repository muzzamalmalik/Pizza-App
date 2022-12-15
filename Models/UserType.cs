using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PizzaOrder.Models;

namespace PizzaOrder.Models
{
    public class UserType : BaseEntity
    {
        [Required]
        [StringLength(30, ErrorMessage = "User Type cannot be longer then 30 characters")]
        public string Label { get; set; }
  
    }
}
