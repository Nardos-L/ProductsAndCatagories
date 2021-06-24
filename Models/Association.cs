using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCatagories.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /* Foreign Keys and Navigation Properties for Relationships */
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CatagoryId { get; set; }
        public Catagory Catagory { get; set; }
    }
}