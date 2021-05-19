using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField] private int score;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverPanel;


    public static GameController instance;
    private void Awake()
    {
        
        instance = this;

        if(PlayerPrefs.GetInt("score") > 0)
        {
            score = PlayerPrefs.GetInt("score");
            scoreText.text = "X " + score.ToString();
        }
    }
    private void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getCoin()
    {
        score++;
        scoreText.text = "X " +  score.ToString();

        PlayerPrefs.SetInt("score", score);
    }

    public void gameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
