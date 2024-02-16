public class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
