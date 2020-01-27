using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> {

    int score;
    Text scoreText;
    Text highScoreText;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = score.ToString();
            if (PlayerPrefs.GetInt("HighScore", 0) < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
                highScoreText.text = score.ToString();
            }
        }
    }

	void Start () {
        scoreText = GameObject.Find("score").GetComponent<Text>();
        highScoreText = GameObject.Find("bestScore").GetComponent<Text>();
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);
        scoreText.text = "0";
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
	
	void Update () {
		
	}
}
