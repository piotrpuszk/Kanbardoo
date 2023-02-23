using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Domain.Repositories;
public interface IResourceIdConverterRepository
{
    Task<int> FromTaskIDToBoardID(int taskID);
    Task<int> FromTableIDToBoardID(int tableID);
}
