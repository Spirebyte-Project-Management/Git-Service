using System.IO;
using Spirebyte.Services.Git.Application.Interfaces;

namespace Spirebyte.Services.Git.Application.Git.Commands;

public record GetInfoRefs
    (string ProjectId, string RepositoryId, string Service, Stream InputStream) : IStreamableCommand
{
    public Stream OutputStream { get; set; }
}