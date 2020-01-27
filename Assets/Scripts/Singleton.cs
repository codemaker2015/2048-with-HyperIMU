using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:Singleton<T> {
    protected static T instance = null;
    public static T Instance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            if (FindObjectsOfType<T>().Length > 1)
            {
                return instance;
            }
            if (instance == null)
            {
                string name = typeof(T).Name;
                GameObject go = GameObject.Find(name);
                if (go == null)
                {
                    go = new GameObject(name);
                }
                instance = go.AddComponent<T>();
                //DontDestroyOnLoad(go);
            }
        }
        return instance;
    }

    protected void OnDestroy()
    {
        instance = null;
    }
}
