using System.IO;
using Convey.CQRS.Commands;
using Spirebyte.Services.Git.Application.Interfaces;

namespace Spirebyte.Services.Git.Application.Git.Commands;

public record ExecuteUploadPack(string ProjectId, string RepositoryId, Stream InputStream) : IStreamableCommand
{
    public Stream OutputStream { get; set; }
}