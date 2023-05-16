using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaOrder.Models
{
    public class ItemSizeTransection : BaseEntity
    {
        public int ItemSizeId { get; set; }
        public int ItemId { get; set; }
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
        [ForeignKey("ItemSizeId")]
        public virtual ItemSize ObjItemSize { get; set; }
    }
}
