using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for keeping track of all tasks
/// Allows the Addition, Removal and Reference
/// </summary>
public class TaskManager : MonoBehaviour,ITaskManager
{
    public Action<Task> OnTaskAdded { get; set; }
    public Action<Task> OnTaskRemoved { get; set; }

    /// <summary>
    /// We only use the list to manipulate the task database on the editor
    /// and debug it at runtime
    /// </summary>
    [SerializeField] private List<Task> taskDatabase;
    public List<Task> TaskDatabase => taskDatabase;
    private Dictionary<string, Task> taskDictionaryDatabase = new Dictionary<string, Task>();

    [SerializeField] private List<Task> currentActiveTaskPool;
    public List<Task> ActiveTasks => currentActiveTaskPool;
    private Dictionary<string, Task> currentTaskPoolDatabase = new Dictionary<string, Task>();


    public Task GetDatabaseTask(Task task)
    {
        return taskDictionaryDatabase[task.UID];
    }

    public Task GetDatabaseTask(string UID)
    {
        return taskDictionaryDatabase[UID];
    }

    // Add a task to the current active tasks
    public void AddTask(Task task)
    {
        currentActiveTaskPool.Add(task);
        currentTaskPoolDatabase.Add(task.UID, task);
        currentTaskPoolDatabase[task.UID].UndertakeTask();
        OnTaskAdded?.Invoke(task);
    }

    // Remove a task from the current active tasks
    public Task RemoveTask(Task task)
    {
        if (!currentTaskPoolDatabase.ContainsKey(task.UID)) return null;
        else
        {
            currentActiveTaskPool.Remove(task);
            Task returnTask = currentTaskPoolDatabase[task.UID];
            currentTaskPoolDatabase.Remove(task.UID);
            OnTaskRemoved?.Invoke(task);
            return returnTask;
        }
    }

    // Get a task from the current active Tasks
    public Task GetActiveTask(Task task)
    {
        if (!currentTaskPoolDatabase.ContainsKey(task.UID))
        {
            Debug.LogWarning("Requested task doesn't exist in the dictionary.");
            return null;
        }
        else return currentTaskPoolDatabase[task.UID];
    }

    // Setup all tasks
    private void Start()
    {
        for (int i = 0; i < taskDatabase.Count; i++)
        {
            taskDatabase[i].SetupTask();
        }
        List<Task> tempList = new List<Task>(currentActiveTaskPool);
        currentActiveTaskPool.Clear();
        for (int i = 0; i < tempList.Count; i++)
        {
            AddTask(tempList[i]);
        }

    }

    public bool CheckEligibility(Task task)
    {
       return GetActiveTask(task).CheckEligibility();
    }

    public void UndertakeTask(Task task)
    {
        task.UndertakeTask();
    }

    public void UpdateTask(Task task)
    {
        GetActiveTask(task).UpdateTask();
    }

    public void Complete(Task task)
    {
        GetActiveTask(task).Complete();
    }

    public void Fail(Task task)
    {
        task.Fail();
    }

    void Update() 
    {
        foreach (var item in currentTaskPoolDatabase.Values)
        {
            item.UpdateTask();
        }
    }

    public void PauseTask(Task task, bool pause)
    {
        GetActiveTask(task).PauseTask(pause);
    }
}