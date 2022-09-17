using Minio.Exceptions;

namespace Spirebyte.Services.Git.Application.Exceptions;

public class ActionNotAllowedException : AuthorizationException
{
    public ActionNotAllowedException()
        : base("You do not have the permissions to perform this action")
    {
    }

    public string Code { get; } = "action_not_allowed";
}