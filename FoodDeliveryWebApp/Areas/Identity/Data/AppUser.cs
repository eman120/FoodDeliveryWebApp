﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FoodDeliveryWebApp.Models;

namespace FoodDeliveryWebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public virtual List<Address> Addresses { get; set; } = new();

    public virtual FoodDeliveryWebApp.Models.Customer? Customer { get; set; }
    public virtual FoodDeliveryWebApp.Models.Seller? Seller { get; set; }

}

