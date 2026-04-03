using CampusRouteLab.Services.Interfaces;
using CampusRouteLab.Models;

namespace CampusRouteLab.Handlers;

public static class EndpointHandlers
{
    public static IResult GetStudentsList(IStudentCatalogService catalog)
    {
        var groups = catalog.GetAllGroups()
            .Select(g => new
            {
                Group = g.Name,
                StudentCount = g.StudentCount,
                HasStudents = g.Students.Count > 0
            });
        
        return Results.Json(groups);
    }
    
    public static Task<IResult> GetStudentById(
        string group,
        int id,
        IStudentCatalogService catalog,
        ITransientMarkerService transient)
    {
        var student = catalog.GetStudent(group, id);
        
        if (student is null)
        {
            return Task.FromResult<IResult>(Results.NotFound(new 
            { 
                Error = "Student not found", 
                Group = group, 
                Id = id,
                SearchedAt = DateTime.Now
            }));
        }
        
        return Task.FromResult<IResult>(Results.Json(new
        {
            Student = student,
            TransientMarker = transient.MarkerId,
            RetrievedAt = DateTime.Now
        }));
    }
}