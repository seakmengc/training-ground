using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalIndicatorCollider : MonoBehaviour
{
    private GameManager gameManager;

    public int taskNumber = 0;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Car"))
        {
            bool collided = gameManager.CollideWithGoalIndicator(other.gameObject, taskNumber);

            if(collided)
            {
                Destroy(gameObject, 1);
            }
        }
    }
}
