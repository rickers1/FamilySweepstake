// Ignore Spelling: Initialize

using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class FamilyMemberCache : CacheBase<FamilyMemberModel>
{
    public async Task InitializeAsync(ISupabaseService service)
        => Load(await service.GetFamilyMembersAsync(), m => m.Id);

    public FamilyMemberModel GetOrDefault(Guid? id)
        => base.Get(id) ?? new(Guid.Empty);
}
