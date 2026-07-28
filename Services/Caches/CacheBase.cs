// Ignore Spelling: Initialize

namespace FamilySweepstake.Services;

public abstract class CacheBase<T>
{
    protected Dictionary<Guid, T> Cache = [];

    public IReadOnlyDictionary<Guid, T> All => Cache;

    public T? Get(Guid? id)
        => id is null ? default : Cache.TryGetValue(id.Value, out var value) ? value : default;

    public void Clear() => Cache.Clear();

    protected void Load(IEnumerable<T> items, Func<T, Guid> keySelector)
        => Cache = items.ToDictionary(keySelector);
}
