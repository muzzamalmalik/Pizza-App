using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PizzaOrder.Models;

namespace PizzaOrder.Models
{
    public class Company : BaseEntity
    {

        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer then 50 characters")]
        public String Name { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Address cannot be longer then 50 characters")]
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "ContactPerson cannot be longer then 50 characters")]
        public string ContactPerson { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Cell Number cannot be longer then 50 characters")]
        public string CellNumber { get; set; }
        
        [StringLength(30, ErrorMessage = "Contract Person cannot be longer then 50 characters")]
        public string SecondaryContactPerson { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Cell Nubmer cannot be longer then 50 characters")]
        public string  SecondaryCellNumber { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }

    }
}
