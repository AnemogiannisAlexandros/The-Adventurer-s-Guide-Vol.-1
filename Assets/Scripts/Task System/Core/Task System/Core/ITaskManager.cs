using System;

public interface ITaskManager
{
    public Action<Task> OnTaskAdded { get; }
    public Action<Task> OnTaskRemoved { get; }
    public void AddTask(Task task);
    public Task RemoveTask(Task task);
    public Task GetActiveTask(Task task);
    public Task GetDatabaseTask(Task task);
    public Task GetDatabaseTask(string UID);
    public bool CheckEligibility(Task task);
    public void UndertakeTask(Task task);
    public void UpdateTask(Task task);
    public void Complete(Task task);
    public void Fail(Task task);
    public void PauseTask(Task task, bool pause);
}
