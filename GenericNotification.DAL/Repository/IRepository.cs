namespace GenericNotification.DAL.Repository;

public interface IRepository<T>
{
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    IEnumerable<T> GetAll();
}