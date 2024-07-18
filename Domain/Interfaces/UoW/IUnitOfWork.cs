namespace Domain.Interfaces.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        ICatalogRepository CatalogRepository { get; }

        ICitiesRepository CitiesRepository { get; }

        ICitiesToTelephoneCostRepository CitiesToTelephoneCost {  get; }

        ITelephoneRepository TelephoneRepository { get; }

        Task SaveAsync(CancellationToken cancellationToken);
    }
}
