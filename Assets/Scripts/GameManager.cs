using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameState gameState;

    public int lives = 3;

    public Sprite emptyHeart;

    public TextMeshProUGUI levelUi;
    public TextMeshProUGUI velocityUi;

    public Image[] hearts;

    public GameObject brakeUi;
    public Rigidbody carRigidbody;

    private TaskManager taskManager;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Running;
        taskManager = new TaskManager(levelUi);

        StartCoroutine(CalcVelocity());
    }

    /**
     * If the car collided with Goal Indicator from wrong level, this will return false
     */
    public bool CollideWithGoalIndicator(GameObject gameObject, int taksNumber)
    {
        return taskManager.CollideWithGoalIndicator(gameObject, taksNumber);
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

    public void ReduceOneLive()
    {
        if(gameState != GameState.Running)
        {
            return;
        }

        lives--;
        //Set heart to empty
        hearts[lives].sprite = emptyHeart;
        
        if(lives == 0)
        {
            gameState = GameState.GameOver;
            Debug.Log("Game Over.");
        }
    }
}
