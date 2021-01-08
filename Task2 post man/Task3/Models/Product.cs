using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task3.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[A-Z][a-zA-Z0-9]*$", ErrorMessage = "Must be uppercase ")]
        [MaxLength(30,ErrorMessage = "Must be Maximum 30 characters")]
        public string Name { get; set; }
        public string Category { get; set; }
        [Required]
        [Range(0,100)]
        public decimal Price { get; set; }

    }
}