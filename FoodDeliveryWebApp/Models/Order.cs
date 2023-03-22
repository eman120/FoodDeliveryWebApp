﻿using FoodDeliveryWebApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryWebApp.Models
{
    public class Order:BaseModel
    {
        public decimal TotalPrice { get; set;}
        
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set;}
        
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set;}

        [ForeignKey("User")]
        public string CustomerId { get; set; } = string.Empty;

        public virtual AppUser User { get; set; } = new();

        public virtual Review Review { get; set; } = new();

        public virtual ICollection<CustomerOrderProduct> CustomerOrderProducts { get; set; } = new List<CustomerOrderProduct>();
    }
}
