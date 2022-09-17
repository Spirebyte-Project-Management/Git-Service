using System.IO;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Git.Application.Interfaces;

namespace Spirebyte.Services.Git.Application.Git.Commands;

[Message("git", "get_info_refs", "git.get_info_refs")]
public record GetInfoRefs
    (string ProjectId, string RepositoryId, string Service, Stream InputStream) : IStreamableCommand
{
    public Stream OutputStream { get; set; }
}