using my_project.Models;
using my_project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/tasks", () => Results.Ok(TaskService.GetAll()))
    .WithName("GetAllTasks");

app.MapGet("/api/tasks/{id}", (int id) =>
{
    var task = TaskService.GetById(id);
    return task == null ? Results.NotFound() : Results.Ok(task);
})
.WithName("GetTaskById");

app.MapPost("/api/tasks", (TaskModel task) =>
{
    if (string.IsNullOrWhiteSpace(task.Title))
        return Results.BadRequest("Title is required");
    
    TaskService.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
})
.WithName("CreateTask");

app.MapPut("/api/tasks/{id}", (int id, TaskModel updatedTask) =>
{
    if (TaskService.GetById(id) == null)
        return Results.NotFound();
    
    updatedTask.Id = id;
    TaskService.Update(updatedTask);
    return Results.NoContent();
})
.WithName("UpdateTask");

app.MapDelete("/api/tasks/{id}", (int id) =>
{
    if (TaskService.GetById(id) == null)
        return Results.NotFound();
    
    TaskService.Delete(id);
    return Results.NoContent();
})
.WithName("DeleteTask");

app.MapGet("/api/tasks/completed", () => 
    Results.Ok(TaskService.GetCompleted()))
    .WithName("GetCompletedTasks");

app.Run();