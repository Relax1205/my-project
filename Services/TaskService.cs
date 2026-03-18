using System.Text.Json;
using my_project.Models;

namespace my_project.Services;

public static class TaskService
{
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "tasks.json");
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    static TaskService()
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }

    public static List<TaskModel> GetAll()
    {
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new List<TaskModel>();
    }

    public static TaskModel? GetById(int id)
    {
        var tasks = GetAll();
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public static void Add(TaskModel task)
    {
        var tasks = GetAll();
        
        if (tasks.Count > 0)
        {
            task.Id = tasks.Max(t => t.Id) + 1;
        }
        else
        {
            task.Id = 1;
        }

        tasks.Add(task);
        Save(tasks);
    }

    public static void Update(TaskModel updatedTask)
    {
        var tasks = GetAll();
        var existingTask = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);

        if (existingTask != null)
        {
            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;
            Save(tasks);
        }
    }

    public static void Delete(int id)
    {
        var tasks = GetAll();
        var taskToRemove = tasks.FirstOrDefault(t => t.Id == id);

        if (taskToRemove != null)
        {
            tasks.Remove(taskToRemove);
            Save(tasks);
        }
    }

    public static List<TaskModel> GetCompleted()
    {
        var tasks = GetAll();
        return tasks.Where(t => t.IsCompleted).ToList();
    }

    private static void Save(List<TaskModel> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, Options);
        File.WriteAllText(FilePath, json);
    }
}