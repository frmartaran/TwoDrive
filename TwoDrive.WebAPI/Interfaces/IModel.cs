namespace TwoDrive.WebApi.Interfaces
{
    public interface IModel<E, M>
    where E : class
    where M : IModel<E, M>, new()
    {
        E ToDomain();

        M FromDomain(E entity);
    }
}
