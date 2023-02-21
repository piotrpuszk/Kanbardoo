using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Application.Authorization.Requirements;
public class BoardMembershipRequirement : AuthorizationRequirement
{
    public int BoardID { get; init; }
}
