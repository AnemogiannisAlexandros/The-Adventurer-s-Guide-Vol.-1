using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : Task
{
    public Action<Task> OnTaskDelivered { get; set; }

    [SerializeField] private bool delivered = false;
    public bool Delivered => delivered;

    [SerializeField] private QuestReward reward;
    public QuestReward Reward => reward;

    public void DeliverTask() 
    {
        delivered = true;
        OnTaskDelivered?.Invoke(this);
    }

    public override void SetupTask()
    {
        base.SetupTask();
    }
    public override bool CheckEligibility()
    {
        return base.CheckEligibility();
    }
    public override void UndertakeTask()
    {
        base.UndertakeTask();
    }
    public override void UpdateTask()
    {
        base.UpdateTask();
    }
    public override void Complete()
    {
        base.Complete();
    }
    public override void Fail()
    {
        base.Fail();
    }
}
