using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToObjectRequirement : TaskRequirement
{
    public GameObject gameObject;

    private Player p;

    public override void TrackRequirements()
    {
        base.TrackRequirements();
        if (IsComplete) return;
        OnRequirementProgressed?.Invoke();
        gameObject.GetComponent<Renderer>().material.color = CanTrackRequirement ? Color.blue : Color.red;
        if (RequirementMet()) 
        {
            IsComplete = true;
            gameObject.GetComponent<Renderer>().material.color = IsComplete ? Color.green : Color.red;
            OnRequirementMet?.Invoke();
        }
    }
    public override object[] GetTrackedValues()
    {
        return new object[] { gameObject.name, Vector3.Distance(p.transform.position, gameObject.transform.position) };
    }
    public override object[] GetDescriptionValues()
    {
        return new object[] { gameObject.name };
    }
    public override bool RequirementMet()
    {
        return Vector3.Distance(p.transform.position, gameObject.transform.position) < .5f;
    }
    public override void Setup()
    {
        base.Setup();
        p = Object.FindObjectOfType<Player>();
        gameObject.GetComponent<Renderer>().material.color = CanTrackRequirement ? Color.blue : Color.red;
    }

}
