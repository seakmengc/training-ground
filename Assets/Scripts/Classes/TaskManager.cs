using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager
{
    private int level = 1;

    private TextMeshProUGUI levelUi;
    private Vector3 lastCheckpoint = new Vector3(104, 1, 391);
    private float lastYRotation = 180f;

    public TaskManager(TextMeshProUGUI levelUi)
    {
        this.levelUi = levelUi;
    }

    public void SetToLastCheckpoint(Transform transform)
    {
        transform.position = lastCheckpoint;
    }

    /**
     * 
     */
    public bool CollideWithGoalCheckpoint(GameObject collidedWith, GoalCheckpointCollider goalCheckpointCollider)
    {
        if(goalCheckpointCollider.taskNumber != level)
        {
            return false;
        }

        level++;
        lastCheckpoint = collidedWith.transform.position;
        lastCheckpoint.y = 1;
        lastYRotation = goalCheckpointCollider.yRotation;

        levelUi.SetText("Level: " + level.ToString());
        
        return true;
    }
}
