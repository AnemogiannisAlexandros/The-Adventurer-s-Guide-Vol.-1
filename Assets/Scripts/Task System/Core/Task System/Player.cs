using System;
using UnityEngine;

public enum PlayerActions 
{
    Jump,
    Dodge,
    Run,
    Attack
}

public class Player : MonoBehaviour
{
    public Action OnPlayerJump; 
    public Action OnPlayerDodge; 
    public Action OnPlayerRun; 
    public Action OnPlayerAttack;
    
    public void Jump() 
    {
        OnPlayerJump?.Invoke();
    }

    public void Dodge() 
    {
        OnPlayerDodge?.Invoke();
    }

    public void Run() 
    {
        OnPlayerRun?.Invoke();
    }

    public void Attack() 
    {
        OnPlayerAttack?.Invoke();
    }
}
