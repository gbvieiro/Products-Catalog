using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Base
{
    /// <summary>
    /// Represents a product in the product catalog.
    /// </summary>
    public abstract class Product : IEntity<Guid>
    {
        /// <summary>
        /// A Product unique identificator.
        /// </summary>
        public Guid Id { get; private set;  }

        /// <remarks>
        /// A product price.
        /// </remarks>
        public double Price { get; protected set; }

        /// <summary>
        /// This method must be called every time a new product is created.
        /// </summary>
        protected void ValidateProductDomain() 
        {
            // Id
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            // Price
            DomainExceptionValidation.When(Price < 0, "Price invalid, could not be less than 0.");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Product unique identification.</param>
        /// <param name="price">Product price.</param>
        /// <param name="stockQuantity">Current stock quantity.</param>
        protected Product(Guid id, double price)
        {
            Id = id;
            Price = price;
            
            ValidateProductDomain();
        }

        /// <summary>
        /// All entities must be able to verify equality. 
        /// Entities must be unique items.
        /// </summary>
        /// <param name="other">Other entity to compare.</param>
        /// <returns>True if objects are the same entity.</returns>
        public bool Equals(IEntity<Guid>? other)
        {
            // A product with the same ID must the treated as the same object.
            return other is Product && other.Id == Id;
        }
    }
}