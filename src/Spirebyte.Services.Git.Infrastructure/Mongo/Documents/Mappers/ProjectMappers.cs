using Spirebyte.Services.Git.Core.Entities;

namespace Spirebyte.Services.Git.Infrastructure.Mongo.Documents.Mappers;

internal static class ProjectMappers
{
    public static Project AsEntity(this ProjectDocument document)
    {
        return new Project(document.Id);
    }

    public static ProjectDocument AsDocument(this Project entity)
    {
        return new ProjectDocument
        {
            Id = entity.Id
        };
    }
}