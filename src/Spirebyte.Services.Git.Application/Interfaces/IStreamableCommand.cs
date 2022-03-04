using System.IO;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Git.Application.Interfaces;

public interface IStreamableCommand : ICommand
{
    public Stream OutputStream { get; set; }

    public void SetOutputStream(Stream outputStream)
    {
        OutputStream = outputStream;
    }
}