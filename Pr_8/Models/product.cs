using System.ComponentModel.DataAnnotations;

namespace Pr_8.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0", "999999999")]
        public decimal Price { get; set; }
    }
}