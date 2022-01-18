using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCheckpointCollider : MonoBehaviour
{
    private GameManager gameManager;

    public int taskNumber = 0;
    public float yRotation = 0;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Car"))
        {
            bool collided = gameManager.CollideWithGoalCheckpoint(other.gameObject, this);

            if(collided)
            {
                Destroy(gameObject, 1);
            }
        }
    }
}
