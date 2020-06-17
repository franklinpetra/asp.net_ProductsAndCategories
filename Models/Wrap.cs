using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Models
{
    public class Wrap
    {
       
        public List<Product> AllProducts { get; set; }
        public List<Category> AllCategories { get; set; }
        public Association Form { get; set; }
    }
}