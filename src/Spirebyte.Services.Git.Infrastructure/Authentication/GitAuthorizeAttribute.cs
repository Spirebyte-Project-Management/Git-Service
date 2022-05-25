using Microsoft.AspNetCore.Authorization;

namespace Spirebyte.Services.Git.Infrastructure.Authentication;

public class GitAuthorizeAttribute : AuthorizeAttribute
{
    public GitAuthorizeAttribute()
    {
        Policy = nameof(GitAuthorizeAttribute);
    }
}