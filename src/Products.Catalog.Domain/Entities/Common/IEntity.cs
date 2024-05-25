namespace Products.Catalog.Domain.Entities.Base
{
    /// <summary>
    /// Defines a valid Entity object.
    /// </summary>
    public interface IEntity<IdentificationType> :  IEquatable<IEntity<IdentificationType>>
    {
        /// <summary>
        /// A entity unique identificator.
        /// </summary>
        IdentificationType Id { get; }
    }
}