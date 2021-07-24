using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Reward Base", menuName = "Assets/Data/Quest Rewards/Quest Reward Base")]
public class QuestReward : ScriptableObject
{
    [SerializeField] private int xp;
    public int XP => xp;
    [SerializeField] private int gold;
    public int Gold => gold;
}
