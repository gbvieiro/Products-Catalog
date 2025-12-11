namespace Shared.Entities;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now.ToUniversalTime();
        UpdateAt = DateTime.Now.ToUniversalTime();
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdateAt { get; private set; }
}