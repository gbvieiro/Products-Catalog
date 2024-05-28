namespace Products.Catalog.Application.DTOs.Common
{
    public abstract class EntityDTO
    {
        /// <summary>
        /// A Product unique identificator.
        /// </summary>
        public Guid Id { get; }
    }
}