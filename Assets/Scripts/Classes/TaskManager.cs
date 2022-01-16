using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager
{
    private int level = 1;

    private TextMeshProUGUI levelUi;

    public TaskManager(TextMeshProUGUI levelUi)
    {
        this.levelUi = levelUi;
    }

    /**
     * 
     */
    public bool CollideWithGoalIndicator(GameObject gameObject, int taksNumber)
    {
        if(taksNumber != level)
        {
            return false;
        }

        level++;
        levelUi.SetText("Level: " + level.ToString());
        
        return true;
    }
}
