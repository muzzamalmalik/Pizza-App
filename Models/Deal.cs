using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Deal : BaseEntity
    {

        [Required]
        [StringLength(250, ErrorMessage = "Title cannot be longer then 250 characters")]
        public string Title { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Description cannot be longer then 250 characters")]
        public string  Description { get; set; }
        public int Price { get; set; }
        public int? Percentage { get; set; }
        public int? DiscountAmount { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "File Name cannot be longer then 250 characters")]
        public string FileName { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "File Path cannot be longer then 250 characters")]
        public string FilePath { get; set; }
        public bool ActiveQueue { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }

    }
}
