﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Models
{
    public class Order : BaseEntity
    {
       
        public int OrderStatus { get; set; }
        public int UserId { get; set; }
        public string PaymentMethodType { get; set; }
        public string DeliveryAddress { get; set; }
        public int TotalAmount { get; set; }
        public int DeliveryCharges { get; set; }

        [ForeignKey("UserId")]
        public virtual User ObjUser { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company ObjCompany { get; set; }
    }
}