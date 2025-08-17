namespace YNAER.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; private set; }

    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; private set; }

    public BankAccount(Guid id, Guid userId, string name, DateTime createdOn, DateTime? updatedOn)
    {
        Id = id;
        UserId = userId;
        Name = name;
        CreatedOn = createdOn;
        UpdatedOn = updatedOn;
    }

    private BankAccount(Guid id, Guid userId, string name, DateTime createdOn)
    {
        Id = id;
        UserId = userId;
        Name = name;
        CreatedOn = createdOn;
    }

    public static BankAccount Create(ApplicationUser user, string name)
    {
        var id = Guid.CreateVersion7();
        var createdOn = DateTime.UtcNow;

        return new BankAccount(id, user.Id, name, createdOn);
    }
}