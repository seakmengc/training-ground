using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameState gameState;

    public int lives = 3;

    public Sprite emptyHeart;
    public Sprite fullHeart;

    public TextMeshProUGUI levelUi;
    public TextMeshProUGUI velocityUi;

    public Image[] hearts;

    public GameObject brakeUi;
    public GameObject car;

    private Rigidbody carRigidbody;
    private Transform carTransform;
    private TaskManager taskManager;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Running;
        taskManager = new TaskManager(levelUi);

        carRigidbody = car.GetComponent<Rigidbody>();
        carTransform = car.GetComponent<Transform>();

        StartCoroutine(CalcVelocity());
    }

    /**
     * If the car collided with Goal Indicator from wrong level, this will return false
     */
    public bool CollideWithGoalCheckpoint(GameObject collidedWith, GoalCheckpointCollider goalCheckpointCollider)
    {
        return taskManager.CollideWithGoalCheckpoint(collidedWith, goalCheckpointCollider);
    }

    IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForFixedUpdate();
            float moveSpeed = Mathf.RoundToInt(carRigidbody.velocity.magnitude * 3.6f);
            velocityUi.SetText(moveSpeed.ToString() + " km/h");
        }
    }

    public bool isNotRunning()
    {
        return gameState != GameState.Running;
    }

    public void SetBraking(bool isBraking)
    {
        if(isBraking)
        {
            brakeUi.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        } else
        {
            brakeUi.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public async void ReduceOneLive()
    {
        if(isNotRunning())
        {
            return;
        }

        taskManager.SetToLastCheckpoint(carTransform);
        gameState = GameState.Restoring;
        carRigidbody.velocity = Vector3.zero;
        carTransform.rotation = Quaternion.identity;

        lives--;
        //Set heart to empty
        int i = 0;
        while(i < 5)
        {
            hearts[lives].sprite = i % 2 == 0 ? emptyHeart : fullHeart;
            i++;
            await Task.Delay(TimeSpan.FromSeconds(0.3f));
        }

        if (lives == 0)
        {
            gameState = GameState.GameOver;
            Debug.Log("Game Over.");
            return;
        }

        gameState = GameState.Running;
    }
}
