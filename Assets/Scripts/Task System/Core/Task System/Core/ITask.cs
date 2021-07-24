using System;
using System.Collections.Generic;

public interface ITask
{
    public string TaskTitle { get; }
    public string TaskDescription { get; }
    TaskState TaskState { get; }
    TaskRequirement[] TaskRequirements { get; }
    Action OnTaskUndertaken { get; }
    Action OnTaskCompleted { get; }
    Action OnTaskFailed { get; }
    Action OnTaskBecameEligible { get; }
    Action<bool> OnTaskPaused { get; }

    void SetupTask();
    bool CheckEligibility();
    void UndertakeTask();
    void UpdateTask();
    void Complete();
    void Fail();
    void PauseTask(bool pause);
}