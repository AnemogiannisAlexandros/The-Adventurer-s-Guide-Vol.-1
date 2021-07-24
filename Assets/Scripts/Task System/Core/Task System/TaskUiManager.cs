using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(90)]
public class TaskUiManager : MonoBehaviour
{
    TaskManager taskManager;

    [SerializeField] private RectTransform availableTasksContainer;
    [SerializeField] private GameObject availableTaskUiItem;
    [SerializeField] private RectTransform collectedTasksContainer;
    [SerializeField] private GameObject activeTaskItem;

    public void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
        for (int i = 0; i < taskManager.TaskDatabase.Count; i++)
        {
            Instantiate(availableTaskUiItem, availableTasksContainer).GetComponent<AvailableTaskUI>().Init(taskManager.TaskDatabase[i]);
        }
        taskManager.OnTaskAdded += AddTaskToLog;
    }

    public void AddTaskToLog(Task task) 
    {
        Instantiate(activeTaskItem, collectedTasksContainer).GetComponent<ActiveTaskUI>().Init(task);
    }
}
