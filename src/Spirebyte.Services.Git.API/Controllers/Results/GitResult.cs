using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Git.Application.Interfaces;

namespace Spirebyte.Services.Git.API.Controllers.Results;

public class GitResult<T> : ActionResult where T : class, IStreamableCommand
{
    private readonly T _command;
    private readonly string _contentType;
    private readonly string _preStreamOutput;

    public GitResult(string contentType, T command, string preStreamOutput = null)
    {
        _contentType = contentType;
        _command = command;
        _preStreamOutput = preStreamOutput;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var dispatcher = context.HttpContext.RequestServices.GetService<IDispatcher>();
        if (dispatcher is null) throw new NoNullAllowedException("dispatcher");

        var response = context.HttpContext.Response;

        response.StatusCode = StatusCodes.Status200OK;
        response.ContentType = _contentType;

        response.Headers.Add("Expires", "Fri, 01 Jan 1980 00:00:00 GMT");
        response.Headers.Add("Pragma", "no-cache");
        response.Headers.Add("Cache-Control", "no-cache, max-age=0, must-revalidate");

        if (!string.IsNullOrWhiteSpace(_preStreamOutput))
            await response.Body.WriteAsync(Encoding.ASCII.GetBytes(_preStreamOutput));

        _command.SetOutputStream(response.Body);

        await dispatcher.SendAsync(_command);
    }
}