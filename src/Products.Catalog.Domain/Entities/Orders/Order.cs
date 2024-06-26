using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Orders
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; private set; }

        public Guid CustomerId { get; set; }

        public DateTime CreationData { get; private set; }

        public OrderStatusEnum Status { get; private set; }

        public List<OrderItem> Items { get; private set; }

        public double TotalAmount { get; private set; }

        public Order(
            Guid id,
            Guid customerId,
            DateTime creationData,
            OrderStatusEnum status,
            List<OrderItem> items,
            double totalAmount
        )
        {
            Id = id;
            CustomerId = customerId;
            CreationData = creationData;
            Status = status;
            Items = items;
            TotalAmount = totalAmount;

            ValidateBookDomain();
        }

        private void ValidateBookDomain()
        {
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            DomainExceptionValidation.When(CustomerId == Guid.Empty, "Custumer ID is required");

            DomainExceptionValidation.When(Items == null || !Items.Any(), "A order must have order items.");
        }

        public void SetStatus(OrderStatusEnum status)
        {
            this.Status = status;
        }

        public void UpdateTotalAmount()
        {
            TotalAmount = Items.Sum(x => x.Amount);
        }

        public bool Equals(IEntity<Guid>? other)
        {
            return other is Order && other.Id == Id;
        }
    }
}