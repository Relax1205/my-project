namespace CampusRouteLab.Services.Interfaces;

using CampusRouteLab.Models;

public interface IStudentCatalogService
{
    List<GroupInfo> GetAllGroups();
    GroupInfo? GetGroupByName(string name);
    Student? GetStudent(string group, int id);
}