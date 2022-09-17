using System.IO;
using Spirebyte.Framework.Shared.Abstractions;

namespace Spirebyte.Services.Git.Application.Interfaces;

public interface IStreamableCommand : ICommand
{
    public Stream OutputStream { get; set; }

    public void SetOutputStream(Stream outputStream)
    {
        OutputStream = outputStream;
    }
}