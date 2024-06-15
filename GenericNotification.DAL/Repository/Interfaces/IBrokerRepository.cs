namespace GenericNotification.DAL.Repository.Interfaces;

public interface IBrokerRepository<T>
{
    public Task AddToBrokerAsync(T entity, string routingKey);
}