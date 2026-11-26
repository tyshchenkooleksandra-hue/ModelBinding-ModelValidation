using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    public class Product : IValidatableObject
    {
        public enum Category { Toy, Technique, Clothes, Transport }

        public int Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [EnumDataType(typeof(Category), ErrorMessage = "Unknown product type")]
        public Category Type { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "Description should be longer than 2 symbols")]
        public string Description { get; set; }

        [Range(typeof(decimal), "0", "100000", ErrorMessage = "Price should be between 0 and 100000")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Name))
            {
                yield break;
            }

            if (Description == Name)
            {
                yield return new ValidationResult(
                    "Description should not be equal to Name",
                    new[] { nameof(Description) });
            }
            else if (!Description.StartsWith(Name, StringComparison.Ordinal))
            {
                yield return new ValidationResult(
                    "Description should start with the Name of the product",
                    new[] { nameof(Description) });
            }
        }
    }
}
