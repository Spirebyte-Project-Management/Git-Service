using System;
using System.Collections.Concurrent;
using System.Net;
using Convey;
using Convey.WebApi.Exceptions;
using Spirebyte.Services.Git.Application.Exceptions.Base;
using Spirebyte.Services.Git.Core.Exceptions.Base;

namespace Spirebyte.Services.Git.Infrastructure.Exceptions;

internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ExceptionResponse Map(Exception exception)
    {
        return exception switch
        {
            DomainException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                HttpStatusCode.BadRequest),
            AppException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                HttpStatusCode.BadRequest),
            AuthorizationException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                HttpStatusCode.Forbidden),
            _ => new ExceptionResponse(new { code = "error", reason = "There was an error." },
                HttpStatusCode.BadRequest)
        };
    }

    private static string GetCode(Exception exception)
    {
        var type = exception.GetType();
        if (Codes.TryGetValue(type, out var code)) return code;

        var exceptionCode = exception switch
        {
            DomainException domainException when !string.IsNullOrWhiteSpace(domainException.Code) => domainException
                .Code,
            AppException appException when !string.IsNullOrWhiteSpace(appException.Code) => appException.Code,
            AuthorizationException authorizationException when !string.IsNullOrWhiteSpace(authorizationException.Code)
                => authorizationException.Code,
            _ => exception.GetType().Name.Underscore().Replace("_exception", string.Empty)
        };

        Codes.TryAdd(type, exceptionCode);

        return exceptionCode;
    }
}