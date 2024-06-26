using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Base
{
    public abstract class Product : IEntity<Guid>
    {
        public Guid Id { get; private set;  }

        public double Price { get; protected set; }

        protected void ValidateProductDomain() 
        {
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            DomainExceptionValidation.When(Price < 0, "Price invalid, could not be less than 0.");
        }

        protected Product(Guid id, double price)
        {
            Id = id;
            Price = price;
            
            ValidateProductDomain();
        }

        public bool Equals(IEntity<Guid>? other)
        {
            return other is Product && other.Id == Id;
        }
    }
}