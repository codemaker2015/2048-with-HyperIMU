using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager insta;

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
}
