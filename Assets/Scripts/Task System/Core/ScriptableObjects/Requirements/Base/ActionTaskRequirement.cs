using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTaskRequirement : TaskRequirement
{
    [SerializeField]private PlayerActions action;
    public PlayerActions Action => action;
    [SerializeField] private int desiredPressTimes;
    public int DesiredPressTimes => desiredPressTimes;

    private int currentPressTimes;

    public override void Setup()
    {
        Player player = Object.FindObjectOfType<Player>();
        player.OnPlayerAttack -= AddPress;
        player.OnPlayerJump -= AddPress;
        player.OnPlayerDodge -= AddPress;
        player.OnPlayerRun -= AddPress;

        currentPressTimes = 0;
        IsComplete = false;
        switch (action) 
        {
            case PlayerActions.Attack:
                player.OnPlayerAttack += AddPress;
                break;
            case PlayerActions.Jump:
                player.OnPlayerJump += AddPress;
                break;
            case PlayerActions.Dodge:
                player.OnPlayerDodge += AddPress;
                break;
            case PlayerActions.Run:
                player.OnPlayerRun += AddPress;
                break;
        }
    }

    public void AddPress() 
    {
        if (!CanTrackRequirement) return;
        if (IsComplete) return;
        currentPressTimes++;
        OnRequirementProgressed?.Invoke();
        if (RequirementMet())
        {
            IsComplete = true;
            OnRequirementMet?.Invoke();
        }
    }

    public override void TrackRequirements()
    {
        base.TrackRequirements();
    }

    public override bool RequirementMet()
    {
        return currentPressTimes >= desiredPressTimes;
    }

    public override object[] GetDescriptionValues()
    {
        return new object[] { action, desiredPressTimes};
    }

    public override object[] GetTrackedValues()
    {
        return new object[] { action, currentPressTimes, desiredPressTimes };
    }
}
