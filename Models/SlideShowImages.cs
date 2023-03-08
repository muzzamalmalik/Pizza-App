using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PizzaOrder.Models;

namespace PizzaOrder.Models
{
    public class SlideShowImages : BaseEntity
    {
        
        public int SlideShowId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        [ForeignKey("SlideShowId")]
        public virtual SlideShow ObjSlideShow { get; set; }

    }
}
