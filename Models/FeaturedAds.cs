using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaOrder.Models
{
    public class FeaturedAds : BaseEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActivated { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }


    }
}
