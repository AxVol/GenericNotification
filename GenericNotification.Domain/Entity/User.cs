using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Entity;

public class User : IEntityId<long>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}