using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Task : MonoBehaviour, ITask
{
    public void Init(Task task) 
    {
        UId = task.UId;
        taskTitle = task.taskTitle;
        taskDescription = task.taskDescription;
        taskProgress = task.taskProgress;
        TaskState = task.TaskState;
        taskCheck = task.taskCheck;
        OnTaskUndertaken = task.OnTaskBecameEligible;
        OnTaskCompleted = task.OnTaskCompleted;
        OnTaskFailed = task.OnTaskFailed;
        OnTaskBecameEligible = task.OnTaskBecameEligible;
    }
    public string UID => UId;
    [ReadOnly] [SerializeField] private string UId;

#if UNITY_EDITOR
    void OnValidate() 
    {
        if (GetComponent<GuidComponent>() != null) 
        {
            UId = GetComponent<GuidComponent>().GetGuid().ToString();
        }
    }
#endif


    [SerializeField] private string taskTitle;
    public string TaskTitle => taskTitle;

    [Multiline(5)][SerializeField] private string taskDescription;
    public string TaskDescription 
    { 
        get 
        {
            List<string> interestingValues = new List<string>();
            for (int i = 0; i < taskRequirements.Length; i++)
            {
                object[] values = taskRequirements[i].GetDescriptionValues();
                for (int j = 0; j < values.Length; j++)
                {
                    interestingValues.Add(values[j].ToString());
                }
            }
            return string.Format(taskDescription,interestingValues.ToArray()); 
        }
        private set 
        {
            taskDescription = value; 
        }
    }

    public bool trackOneAtATime = false;

    [Multiline(5)] [SerializeField] private string taskProgress;
    public string TaskProgress
    {
        get
        {
            List<string> interestingValues = new List<string>();
            for (int i = 0; i < taskRequirements.Length; i++)
            {
                if (!trackOneAtATime)
                {
                    object[] values = taskRequirements[i].GetTrackedValues();
                    for (int j = 0; j < values.Length; j++)
                    {
                        interestingValues.Add(values[j].ToString());
                    }
                }
                else 
                {
                    if (taskRequirements[i].IsComplete) continue;
                    object[] values = taskRequirements[i].GetTrackedValues();
                    for (int j = 0; j < values.Length; j++)
                    {
                        interestingValues.Add(values[j].ToString());
                    }
                    break;
                }
              
            }
            
            return interestingValues.Count > 0 ? string.Format(taskProgress, interestingValues.ToArray()) : "Completed";
        }
        private set
        {
            taskDescription = value;
        }
    }

    public TaskState TaskState { get; private set; } = TaskState.Setup;

    [SerializeField] private TaskCheckMethod taskCheck = TaskCheckMethod.Parallel;
    public TaskCheckMethod TaskCheck => taskCheck;

    private TaskRequirement[] taskRequirements;
    public TaskRequirement[] TaskRequirements => taskRequirements;

    public Action OnTaskUndertaken { get; set; }

    public Action OnTaskCompleted { get; set; }

    public Action OnTaskFailed { get; set; }

    public Action OnTaskBecameEligible { get; set; }

    public Action<bool> OnTaskPaused { get; set; }

    /// <summary>
    /// We run this when the game is first initialized
    /// to setup our task data.
    /// </summary>
    public virtual void SetupTask() 
    {
        OnTaskBecameEligible += () => TaskState = TaskState.Eligible;
        OnTaskCompleted += () => TaskState = TaskState.Completed;
        OnTaskFailed += () => TaskState = TaskState.Failed;
        OnTaskUndertaken += () => TaskState = TaskState.Active;
        OnTaskPaused += (t) => TaskState = TaskState.Inactive;
        taskRequirements = GetComponents<TaskRequirement>();
        for (int i = 0; i < taskRequirements.Length; i++)
        {
            taskRequirements[i].Setup();
        }
        if (taskCheck == TaskCheckMethod.InSequence)
        {
            for (int i = 0; i < taskRequirements.Length; i++)
            {
                if (taskRequirements[i].IsComplete) continue;
                taskRequirements[i].CanTrackRequirement = true;
                break;
            }
        }
        else 
        {
            for (int i = 0; i < taskRequirements.Length; i++)
            {
                taskRequirements[i].CanTrackRequirement = true;
            }
        }
        TaskState = CheckEligibility()? TaskState.Eligible : TaskState.Ineligible;
    }

    /// <summary>
    /// Pause/Unpause a task from tracking
    /// </summary>
    /// <param name="pause"></param>
    public void PauseTask(bool pause) 
    {
        for (int i = 0; i < taskRequirements.Length; i++)
        {
            taskRequirements[i].CanTrackRequirement = !pause;
        }
        OnTaskPaused?.Invoke(pause);
    }

    /// <summary>
    /// Subscribe to a relative event to track for eligibility when that value changes.
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckEligibility() 
    {
        return true;
    }

    /// <summary>
    /// This runs when you first undertake a task.
    /// </summary>
    public virtual void UndertakeTask()
    {
        for (int i = 0; i < taskRequirements.Length; i++)
        {
            taskRequirements[i].Setup();
        }
        OnTaskUndertaken?.Invoke();
    }

    /// <summary>
    /// This method will update the task at runtime,
    /// but only if the task is Active
    /// </summary>
    /// <returns></returns>
    public virtual void UpdateTask()
    {
        if(TaskState != TaskState.Active)return;
        bool allRequirementsCleared = true;
        for (int i = 0; i < taskRequirements.Length; i++)
        {
            if (taskRequirements[i].IsComplete) continue;
            else
            {
                allRequirementsCleared = false;
                if (taskCheck == TaskCheckMethod.Parallel)
                {
                    taskRequirements[i].TrackRequirements();
                }
                else 
                {
                    if (i > 0 && !taskRequirements[i - 1].IsComplete)
                    {
                        taskRequirements[i].CanTrackRequirement = false;
                        return;
                    }
                    taskRequirements[i].CanTrackRequirement = true;
                    taskRequirements[i].TrackRequirements();
                }

            }
        }
        if (allRequirementsCleared) Complete();
    }
    
    /// <summary>
    /// This will Run once the task is completed. It will mark the task as complete
    /// and sent a message that it is now complete
    /// </summary>
    public virtual void Complete()
    {
        OnTaskCompleted?.Invoke();
    }

    /// <summary>
    /// This will Run once the task is failed. It will mark the task as failed
    /// and sent a message that it is now failed
    /// </summary>
    public virtual void Fail()
    {
        OnTaskFailed?.Invoke();
    }
}
