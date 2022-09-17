﻿using System.IO;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Git.Application.Interfaces;

namespace Spirebyte.Services.Git.Application.Git.Commands;

[Message("git", "execute_upload_pack", "git.execute_upload_pack")]
public record ExecuteUploadPack(string ProjectId, string RepositoryId, Stream InputStream) : IStreamableCommand
{
    public Stream OutputStream { get; set; }
}