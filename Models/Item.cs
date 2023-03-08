using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Item : BaseEntity
    {
        [Required]
        [StringLength(30, ErrorMessage = "Name cannot be longer then 30 characters")]
        public string  Name { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Description cannot be longer then 250 characters")]
        public string  Description { get; set; }
        public int Sku { get; set; } // Stock Keeping Unit
        [Required]
        [StringLength(250, ErrorMessage = "File Name cannot be longer then 250 characters")]
        public string  FileName { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "File Path cannot be longer then 250 characters")]
        public string FilePath { get; set; }

        public int CategoryId { get; set; }
        public int CompanyId { get; set; }

        public bool ActiveQueue { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category ObjCategory { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }
    }
}
