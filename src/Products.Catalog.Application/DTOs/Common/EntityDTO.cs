namespace Products.Catalog.Application.DTOs.Common
{
    public abstract class EntityDTO
    {
        public Guid Id { get; set; }

        public void GenerateId()
        {
            if(Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }
    }
}