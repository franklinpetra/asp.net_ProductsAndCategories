using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [Display(Name="Name: ")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Description: ")]
        public string Description { get; set; }

        [Required]
        [Display(Name="Price: ")]
        public decimal Price { get; set; }

        // ManyToMany nav property //
        public List<Association> CategoryProductIsPartOf { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

