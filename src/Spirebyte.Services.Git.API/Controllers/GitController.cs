using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spirebyte.Framework.API;
using Spirebyte.Services.Git.API.Controllers.Results;
using Spirebyte.Services.Git.Application.Git.Commands;
using Spirebyte.Services.Git.Core.Constants;

namespace Spirebyte.Services.Git.API.Controllers;

[Route("{projectId}/development/{repoId}.git/")]
public class GitController : ApiController
{
    private const string FlushMessage = "0000";

    private const string UploadPackString = "git-upload-pack";
    private const string ReceivePackString = "git-receive-pack";
    private static readonly string[] PermittedServiceNames = { UploadPackString, ReceivePackString };

    [HttpGet("info/refs")]
    [Produces("application/x-git-upload-pack-advertisement", "application/x-git-receive-pack-advertisement")]
    [Authorize(ApiScopes.RepositoriesRead)]
    public IActionResult InfoRefs(string projectId, string repoId, string service)
    {
        if (string.IsNullOrWhiteSpace(service))
            return BadRequest("Only Git SMART Protocol is supported please provide service name");
        if (!PermittedServiceNames.Contains(service)) return BadRequest($"{service} is not a permitted service");

        var serviceName = service[4..];
        var advertiseRefsContent = FormatMessage($"# service={service}\n") + FlushMessage;

        var command = new GetInfoRefs(projectId, repoId, serviceName, GetInputStream());

        return new GitResult<GetInfoRefs>($"application/x-{service}-advertisement", command, advertiseRefsContent);
    }


    [HttpPost("git-upload-pack")]
    [Produces("application/x-git-upload-pack-result")]
    [Authorize(ApiScopes.RepositoriesRead)]
    public IActionResult UploadPack(string projectId, string repoId)
    {
        var command = new ExecuteUploadPack(projectId, repoId, GetInputStream());

        return new GitResult<ExecuteUploadPack>("application/x-git-upload-pack-result", command);
    }

    [HttpPost("git-receive-pack")]
    [Produces("application/x-git-receive-pack-result")]
    [Authorize(ApiScopes.RepositoriesCommit)]
    public IActionResult ReceivePack(string projectId, string repoId)
    {
        var command = new ExecuteReceivePack(projectId, repoId, GetInputStream());

        return new GitResult<ExecuteReceivePack>("application/x-git-receive-pack-result", command);
    }

    private static string FormatMessage(string input)
    {
        return (input.Length + 4).ToString("X").PadLeft(4, '0') + input;
    }

    private Stream GetInputStream()
    {
        return Request.Headers["Content-Encoding"] == "gzip"
            ? new GZipStream(Request.Body, CompressionMode.Decompress)
            : Request.Body;
    }
}