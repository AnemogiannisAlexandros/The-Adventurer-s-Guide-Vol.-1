using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveTaskUI : MonoBehaviour
{
    private Task task;
    public TextMeshProUGUI taskTitleText;
    public TextMeshProUGUI taskProgress;

    public void Init(Task task) 
    {
        this.task = task;
        UpdateText();
        for (int i = 0; i < task.TaskRequirements.Length; i++)
        {
            task.TaskRequirements[i].OnRequirementProgressed += UpdateText;
            task.TaskRequirements[i].OnRequirementMet += UpdateText;
        }
    }

    void UpdateText() 
    {
        taskTitleText.text = this.task.TaskTitle;
        taskProgress.text = this.task.TaskProgress;
    }
}
