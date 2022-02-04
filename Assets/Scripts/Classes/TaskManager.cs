using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager
{
    private int level = 1;

    private TextMeshProUGUI levelUi;
    private Vector3 lastCheckpoint = new Vector3(97, 0.2f, 391);
    private float lastYRotation = 180f;

    public TaskManager(TextMeshProUGUI levelUi)
    {
        this.levelUi = levelUi;
    }

    public void SetToLastCheckpoint(Transform transform)
    {
        transform.position = lastCheckpoint;
        Debug.Log("Last Y " + lastYRotation);
        transform.rotation = Quaternion.Euler(0, lastYRotation, 0);
        Debug.Log(transform.rotation);
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
        lastCheckpoint.y = 0.2f;
        lastYRotation = goalCheckpointCollider.yRotation;

        levelUi.SetText("Level: " + level.ToString());
        
        return true;
    }
}
