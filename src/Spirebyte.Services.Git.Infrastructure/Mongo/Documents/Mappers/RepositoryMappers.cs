

using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Infrastructure.Mongo.Documents.Mappers;

internal static class RepositoryMappers
{
    public static Repository AsEntity(this RepositoryDocument document)
    {
        return new Repository(document.Id, document.Title, document.Description, document.ProjectId, document.ReferenceId, document.Branches, document.CreatedAt);
    }

    public static RepositoryDocument AsDocument(this Repository entity)
    {
        return new RepositoryDocument
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ProjectId = entity.ProjectId,
            ReferenceId = entity.ReferenceId,
            Branches = entity.Branches,
            CreatedAt = entity.CreatedAt
        };
    }
}