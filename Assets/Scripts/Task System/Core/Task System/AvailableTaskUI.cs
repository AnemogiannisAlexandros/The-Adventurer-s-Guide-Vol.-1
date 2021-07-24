using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvailableTaskUI : MonoBehaviour
{
    public TextMeshProUGUI taskTitleText;
    public TextMeshProUGUI taskDescriptionText;
    public GameObject acceptTaskRoot;
    public GameObject activeTaskRoot;
    public GameObject completedTaskRoot;
    public GameObject failedTaskRoot;
    public GameObject illegibleTaskRoot;
    public Button acceptTaskButton;
    private Task thisTask;
    private TaskManager taskManager;

    public void Init(Task task)
    {
        taskManager = FindObjectOfType<TaskManager>();
        thisTask = task;
        taskTitleText.text = task.TaskTitle;
        taskDescriptionText.text = task.TaskDescription;
        EnableAppropriateGroup(task.TaskState);
        task.OnTaskBecameEligible += () => EnableAppropriateGroup(task.TaskState);
        task.OnTaskCompleted += () => EnableAppropriateGroup(task.TaskState);
        task.OnTaskUndertaken += () => EnableAppropriateGroup(task.TaskState);
        task.OnTaskFailed += () => EnableAppropriateGroup(task.TaskState);
        acceptTaskButton.onClick.AddListener(()=>taskManager.AddTask(task));
    }

    void EnableAppropriateGroup(TaskState state) 
    {
        acceptTaskRoot.SetActive(false);
        activeTaskRoot.SetActive(false);
        completedTaskRoot.SetActive(false);
        failedTaskRoot.SetActive(false);
        illegibleTaskRoot.SetActive(false);
        switch (state) 
        {
            case TaskState.Eligible:
                acceptTaskRoot.SetActive(true);
                break;
            case TaskState.Active:
                activeTaskRoot.SetActive(true);
                break;
            case TaskState.Ineligible:
                illegibleTaskRoot.SetActive(true);
                break;
            case TaskState.Completed:
                completedTaskRoot.SetActive(true);
                break;
            case TaskState.Failed:
                failedTaskRoot.SetActive(true);
                break;
        }
    }
    private void OnDestroy()
    {
        thisTask.OnTaskBecameEligible -= () => EnableAppropriateGroup(thisTask.TaskState);
        thisTask.OnTaskCompleted -= () => EnableAppropriateGroup(thisTask.TaskState);
        thisTask.OnTaskUndertaken -= () => EnableAppropriateGroup(thisTask.TaskState);
        thisTask.OnTaskFailed -= () => EnableAppropriateGroup(thisTask.TaskState);
    }
}
