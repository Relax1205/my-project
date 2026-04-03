namespace CampusRouteLab.Services.Implementations;

using CampusRouteLab.Models;
using CampusRouteLab.Services.Interfaces;

public class StudentCatalogService : IStudentCatalogService
{
    private readonly List<GroupInfo> _groups;
    
    public StudentCatalogService()
    {
        _groups = new List<GroupInfo>
        {
            new GroupInfo("IKBO-01-22", 3, new List<Student> {
                new Student(1, "Иванов", "IKBO-01-22"),
                new Student(2, "Петров", "IKBO-01-22"),
                new Student(3, "Сидоров", "IKBO-01-22")
            }),
            new GroupInfo("IKBO-02-22", 2, new List<Student> {
                new Student(1, "Смирнов", "IKBO-02-22"),
                new Student(2, "Кузнецов", "IKBO-02-22")
            })
        };
    }
    
    public List<GroupInfo> GetAllGroups() => _groups;
    
    public GroupInfo? GetGroupByName(string name) => 
        _groups.FirstOrDefault(g => g.Name == name);
    
    public Student? GetStudent(string group, int id) =>
        _groups.FirstOrDefault(g => g.Name == group)
            ?.Students.FirstOrDefault(s => s.Id == id);
}