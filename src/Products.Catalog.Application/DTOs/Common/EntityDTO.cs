namespace Products.Catalog.Application.DTOs.Common
{
    /// <summary>
    /// Represents a entity structure.
    /// </summary>
    public abstract class EntityDTO
    {
        /// <summary>
        /// A Product unique identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Generates a Id when not defined.
        /// </summary>
        public void GenerateId()
        {
            if(Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }
    }
}