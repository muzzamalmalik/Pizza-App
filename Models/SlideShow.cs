using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PizzaOrder.Models;


namespace PizzaOrder.Models
{
    public class SlideShow : BaseEntity
    {
        public string ImageDescription { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }
    }
}
