using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameState gameState;

    public int level = 1;
    public int lives = 3;

    public Sprite emptyHeart;

    public TextMeshProUGUI levelUi;

    public Image[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Running;
    }

    // Update is called once per frame
    void Update()
    {
        levelUi.SetText("Level: " + level.ToString());
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
