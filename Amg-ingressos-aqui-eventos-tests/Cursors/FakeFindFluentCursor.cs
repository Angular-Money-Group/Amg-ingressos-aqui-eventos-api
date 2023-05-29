using MongoDB.Driver;

public class FakeFindFluentCursor<T> : IFindFluent<T, T>
{
    private readonly IEnumerable<T> _documents;

    public FakeFindFluentCursor(IEnumerable<T> documents)
    {
        _documents = documents;
    }

    public FindOptions<T, T> Options => throw new NotImplementedException();

    FilterDefinition<T> IFindFluent<T, T>.Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IFindFluent<T, TResult> As<TResult>(MongoDB.Bson.Serialization.IBsonSerializer<TResult> resultSerializer = null)
    {
        throw new NotImplementedException();
    }

    public long Count(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public long CountDocuments(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> CountDocumentsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IFindFluent<T, T> Filter(FilterDefinition<T> filter)
    {
        return this;
    }

    public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_documents.FirstOrDefault());
    }

    public IFindFluent<T, T> Limit(int? limit)
    {
        throw new NotImplementedException();
    }

    public IFindFluent<T, TNewProjection> Project<TNewProjection>(ProjectionDefinition<T, TNewProjection> projection)
    {
        throw new NotImplementedException();
    }

    public IFindFluent<T, T> Skip(int? skip)
    {
        throw new NotImplementedException();
    }

    public IFindFluent<T, T> Sort(SortDefinition<T> sort)
    {
        throw new NotImplementedException();
    }

    public IAsyncCursor<T> ToCursor(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IAsyncCursor<T>> ToCursorAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    // Outros métodos da interface IFindFluent<T, T> podem ser implementados conforme necessário
}
