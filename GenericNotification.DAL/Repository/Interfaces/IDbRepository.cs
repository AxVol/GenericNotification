namespace GenericNotification.DAL.Repository.Interfaces;

public interface IDbRepository<T>
{
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    IEnumerable<T> GetAll();
}