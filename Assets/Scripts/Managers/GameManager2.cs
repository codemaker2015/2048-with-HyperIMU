using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 :MonoBehaviourSingleton<GameManager2>
{
    public GameObject PacManPrefab;
    public Transform playerSpawnPoint;
    public PacMan Avatar;
    public int maxLives = 3;
    public int lives;
    public int score;
    float timeScale;

    // Start is called before the first frame update
    void Start()
    {
        timeScale = Time.timeScale;
        lives = maxLives;
        Avatar = GameObject.Instantiate(PacManPrefab).GetComponent<PacMan>();
        Avatar.Respawn(MapManager.Get().playerStartPos);
        Avatar.OnDeathAnimationFinished += PlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    public void UpdateLives(int v)
    {
        lives = v;
        UIManager.Get().UpdateLives(lives);
    }

    public void UpdateScore(int scoreGain)
    {
        score += scoreGain;
        UIManager.Get().UpdateScore(score);
    }

    public void HandleInput()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public PacMan GetPlayer()
    {
        return Avatar;
    }
    public void GhostDestroyed()
    {
        UpdateScore(50);
    }
    public void PlayerDeath(PacMan p)
    {
        UpdateLives(lives - 1);
        if(lives > 0)
        {            
            Avatar.Respawn(MapManager.Get().playerStartPos);
            Avatar.Reset();
            EnemyManager.Get().ResetGhosts();
        }
        else
        {
            PauseGame(true);
            GameOver();
            return;
        }
    }
    public void PauseGame(bool val)
    {
        Time.timeScale = val ? 0 : timeScale;
    }
    public void Win()
    {
        PauseGame(true);
        UIManager.Get().ActivatePostGame(true);
    }
    private void GameOver()
    {
        PauseGame(true);
        UIManager.Get().ActivatePostGame(false);
    }
    public void Restart()
    {
        PauseGame(false);
        UpdateScore(-score);
        UpdateLives(maxLives);
        MapManager.Get().ResetMap();
        Avatar.Respawn(MapManager.Get().playerStartPos);
        Avatar.Reset();
        EnemyManager.Get().ResetGhosts();
        
    }
}