namespace Products.Catalog.Domain.Entities.Base
{
    public interface IEntity<IdentificationType> :  IEquatable<IEntity<IdentificationType>>
    {
        IdentificationType Id { get; }
    }
}