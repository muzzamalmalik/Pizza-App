using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Branches : BaseEntity
    {

        [Required]
        [StringLength(30, ErrorMessage = "Title cannot be longer then 30 characters")]
        public string Title { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Contract Person cannot be longer then 25 characters")]
        public string ContactPerson { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "Phone Number cannot be longer then 11 characters")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Address cannot be longer then 250 characters")]
        public string Address { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }
    }
}
