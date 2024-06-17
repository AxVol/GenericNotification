namespace NotificationSender.DAL.Repository.Interfaces;

public interface IRepository<T>
{
    public Task AddAsync(T entity);
    public Task<T> GetAsync(T entity);
    public Task UpdateAsync(T entity);
}