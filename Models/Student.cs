namespace CampusRouteLab.Models;

public record Student(int Id, string Name, string GroupName);
public record GroupInfo(string Name, int StudentCount, List<Student> Students);