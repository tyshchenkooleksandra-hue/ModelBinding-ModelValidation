using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    /// <summary>
    /// One row of the "edit prices by category" form: only Price is editable.
    /// </summary>
    public class ProductPrice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Range(typeof(decimal), "0", "100000", ErrorMessage = "Price should be between 0 and 100000")]
        public decimal Price { get; set; }
    }
}
