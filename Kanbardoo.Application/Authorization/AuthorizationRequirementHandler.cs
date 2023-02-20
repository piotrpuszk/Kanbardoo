namespace Kanbardoo.Application.Authorization;

public abstract class AuthorizationRequirementHandler<T> where T : AuthorizationRequirement
{ 
    public abstract Task HandleAsync(AuthorizationRequirementContext context, T requirement);
}
