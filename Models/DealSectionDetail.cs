﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class DealSectionDetail : BaseEntity
    {
        
        public int DealSectionId { get; set; }
        public int ItemId { get; set; }
        public bool IsActive { get; set; }
        public int? ItemSizeid { get; set; }

        [ForeignKey("DealSectionId")]
        public virtual DealSection ObjDealSection { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item ObjItem { get; set; }

    }
}
