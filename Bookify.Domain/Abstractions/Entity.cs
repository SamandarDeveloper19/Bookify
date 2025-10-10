namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvent> domainEvents = new();

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}
