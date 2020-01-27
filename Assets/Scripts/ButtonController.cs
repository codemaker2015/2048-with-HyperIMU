using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    public List<bool> unlocked;
    public List<Button> LevelButton;
    private int numberoflevels = 12;
    public int currentValue = 0;

    private void Start()
    {
        currentValue = PlayerPrefs.GetInt("Current");
        if (currentValue > 0)
        {
            Unlock(currentValue);
        }
    }

    public void Unlock(int level)
    {
        for(int i = 0;i < level;i++)
        {
            unlocked[i] = true;
        }
        updateButtons();
    }

    public void updateButtons()
    {
        for (int i = 0; i < numberoflevels; i++)
        {
            if (unlocked[i] == false)
            {
                LevelButton[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                LevelButton[i].GetComponent<Button>().interactable = true;
            }
        }
    }
}
