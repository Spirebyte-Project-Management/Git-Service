using Spirebyte.Services.Git.Application.Exceptions.Base;

namespace Spirebyte.Services.Git.Application.Exceptions;

public class ActionNotAllowedException : AuthorizationException
{
    public ActionNotAllowedException()
        : base("You do not have the permissions to perform this action")
    {
    }

    public override string Code { get; } = "action_not_allowed";
}