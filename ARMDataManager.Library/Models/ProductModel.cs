namespace ARMDataManager.Library.Models
{
    public class ProductModel
    {
        /// <summary>
        /// Unique identifier for the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The product's name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The product's detailed description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The base retail cost of the product
        /// </summary>
        public decimal RetailPrice { get; set; }

        /// <summary>
        /// The current quantity of the product in inventory
        /// </summary>
        public int QuantityInStock { get; set; }

        /// <summary>
        /// A flag indicating if this product is tax-exempt
        /// </summary>
        public bool IsTaxable { get; set; }
    }
}