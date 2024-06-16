namespace GenericNotification.DAL.Repository.Interfaces;

public interface IRepository<T> : IBrokerRepository<T>, IDbRepository<T>
{
   
}