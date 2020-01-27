using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefManager : MonoBehaviour {

    public static PrefManager insta;

    private void Awake()
    {
        if (insta)
        {
            Destroy(gameObject);
        }
        else
        {
            insta = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start () {
        PlayerPrefs.SetInt("Current", 1);
    }

    private void Update()
    {
        //Debug.Log(PlayerPrefs.GetInt("Current"));
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Current", 1);
    }
}
