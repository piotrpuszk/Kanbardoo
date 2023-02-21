using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kanbardoo.Application.Authorization;

public class AuthorizationRequirementContext
{
    private Dictionary<AuthorizationRequirement, bool> _requirementSucceed = new Dictionary<AuthorizationRequirement, bool>();
    private readonly ClaimsPrincipal _user;
    public ClaimsPrincipal User => _user;

    public AuthorizationRequirementContext(HttpContext context)
    {
        _user = context.User;
    }

    public bool HasSucceeded()
    {
        return _requirementSucceed.Count > 0 && !_requirementSucceed.Any(e => !e.Value);
    }

    public void Succeed(AuthorizationRequirement requirement)
    {
        _requirementSucceed[requirement] = true;
    }

    public void Fail(AuthorizationRequirement requirement)
    {
        _requirementSucceed[requirement] = false;
    }
}
