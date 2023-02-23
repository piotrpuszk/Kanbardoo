using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kanbardoo.Infrastructure.Repositories;

public class ResourceIdConverterRepository : IResourceIdConverterRepository
{
    private readonly DBContext _dBContext;

    public ResourceIdConverterRepository(DBContext dBContext)
    {
        _dBContext = dBContext;
    }

    public async Task<int> FromTableIDToBoardID(int tableID)
    {
        var result = await _dBContext.Tables.FirstOrDefaultAsync(e => e.ID == tableID);
        return result?.BoardID ?? 0;
    }

    public async Task<int> FromTaskIDToBoardID(int taskID)
    {
        var result = await _dBContext.Tables.Where(e => e.Tasks.Select(e => e.ID).Contains(taskID)).FirstOrDefaultAsync();
        return result?.BoardID ?? 0;
    }
}