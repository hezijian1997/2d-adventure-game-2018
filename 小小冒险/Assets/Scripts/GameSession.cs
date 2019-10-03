using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // 初始玩家生命次数
    [SerializeField] int playerLife = 3;
    [SerializeField] int score = 0;
    [SerializeField] Text LifeText;
    [SerializeField] Text ScoreText;
    // make a singleton
    private void Awake()
    {
        // create number of game sessions
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        // 判断gamesessions是否大于1,
        if (numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LifeText.text = playerLife.ToString();
        ScoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        // 如果游戏角色生命值大于1，继续游戏场景, 如果小于1,重载游戏场景
        if (playerLife > 1)
        {
            ContinueLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ContinueLife()
    {
        playerLife--;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        LifeText.text = playerLife.ToString();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddToScore(int CoinScore)
    {
        score += CoinScore;
        ScoreText.text = score.ToString();
    }
}
