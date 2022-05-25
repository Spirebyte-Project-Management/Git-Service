﻿using System.IO;
using Spirebyte.Services.Git.Application.Interfaces;

namespace Spirebyte.Services.Git.Application.Git.Commands;

public record ExecuteReceivePack(string ProjectId, string RepositoryId, Stream InputStream) : IStreamableCommand
{
    public Stream OutputStream { get; set; }
}