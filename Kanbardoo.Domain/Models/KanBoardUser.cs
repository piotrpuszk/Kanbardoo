using Kanbardoo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Domain.Models;
public class KanBoardUser : KanUser
{
    public string RoleName { get; set; } = string.Empty;
}
