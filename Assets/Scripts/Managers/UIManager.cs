using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager :MonoBehaviourSingleton<UIManager>
{
    public GameObject UIPostGame;
    public GameObject UIGameplay;
    public Text pointsText;
    public Text postGameText;
    public Image[] livesImages;

    public void UpdateScore(int points)
    {
        pointsText.text = "Points: " + points;
    }
    public void ActivatePostGame(bool win)
    {
        if(win)
            postGameText.text = win ? "WINNER" : "GAMEOVER";
        UIPostGame.SetActive(true);
    }
    public void ResetUI()
    {
        UIPostGame.SetActive(false);
    }
    public void UpdateLives(int lives)
    {
        foreach(Image liveUI in livesImages)
            liveUI.color = new Color(0, 0, 0, 0);
        for(int i = 0; i < lives; i++)
            livesImages[i].color = Color.white;
    }
    public void RestartButtonPressed()
    {
        ResetUI();
        GameManager2.Get().Restart();
    }

	public void Menu(){
		SceneManager.LoadScene ("Additionals");
	}

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
