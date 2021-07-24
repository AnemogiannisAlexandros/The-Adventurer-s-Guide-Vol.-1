using System;
using UnityEngine;

[Serializable]
public class TaskRequirement : MonoBehaviour
{

    public void Init(TaskRequirement requirement) 
    {
        OnRequirementMet = requirement.OnRequirementMet;
        OnRequirementProgressed = requirement.OnRequirementProgressed;
        CanTrackRequirement = requirement.CanTrackRequirement;
    } 

    public Action OnRequirementMet;
    public Action OnRequirementProgressed;

    public bool CanTrackRequirement { get; set; } = false;

    //Mark our requirement as Complete or Incomplete
    public bool IsComplete { get; protected set; }

    //Setup the initial data of our requirements
    public virtual void Setup() 
    {

    }

    //Check at if the requirements have been met
    public virtual bool RequirementMet() 
    {
        return false;
    }

    //Track requirements at Runtime;
    public virtual void TrackRequirements() 
    {
        if (IsComplete) return;
    }

    public virtual object[] GetDescriptionValues() 
    {
        return null;
    }
    public virtual object[] GetTrackedValues()
    {
        return null;
    }
}
